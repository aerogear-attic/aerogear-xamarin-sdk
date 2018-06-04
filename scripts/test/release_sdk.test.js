#!/bin/bash
'use strict'
const assert = require('assert');
const fs = require('fs-extra')
const promisify = require("util").promisify
const xmlhandling = require('../modules/xml_handling')
const sdkop = require('../modules/sdk_operations')
const projop = require('../modules/project_operations')
const readFile = promisify(fs.readFile)

describe('Release SDK script', () => {
    describe('', () => {
        let config = {}
        const testNuGet1Orig = `Test1.nuspec`
        const testNuGet1 = `${testNuGet1Orig}.copy`
        const testNuGet2Orig = `Test2.nuspec`
        const testNuGet2 = `${testNuGet2Orig}.copy`
        const testNuGet3Orig = `Test3.nuspec`
        const testNuGet3 = `${testNuGet3Orig}.copy`
        const test1Orig = `Test1.csproj`
        const test1 = `${test1Orig}.copy`
        const test2Orig = `Test2.csproj`
        const test2 = `${test2Orig}.copy`
        const test3Orig = `Test3.csproj`
        const test3 = `${test3Orig}.copy`

        beforeEach(() => {
            config = {
                "projects": {
                    "scripts/test/Test1.csproj.copy": {
                        "nuspec": "scripts/test/Test1.nuspec.copy",
                        "package": "Test1"
                    },
                    "scripts/test/Test2.csproj.copy": {
                        "nuspec": "scripts/test/Test2.nuspec.copy",
                        "package": "Test2"
                    },
                    "scripts/test/Test3.csproj.copy": {
                        "nuspec": "scripts/test/Test3.nuspec.copy",
                        "package": "Test3"
                    }
                },
                "nuGets": {
                    "Test1": "0.2.1",
                    "Test2": "0.2.2",
                    "Test3": "0.2.3"
                }
            }
            fs.copySync(`${__dirname}/${testNuGet1Orig}`, `${__dirname}/${testNuGet1}`)
            fs.copySync(`${__dirname}/${testNuGet2Orig}`, `${__dirname}/${testNuGet2}`)
            fs.copySync(`${__dirname}/${testNuGet3Orig}`, `${__dirname}/${testNuGet3}`)
            fs.copySync(`${__dirname}/${test1Orig}`, `${__dirname}/${test1}`)
            fs.copySync(`${__dirname}/${test2Orig}`, `${__dirname}/${test2}`)
            fs.copySync(`${__dirname}/${test3Orig}`, `${__dirname}/${test3}`)
        })
        it('bump versions in nuspec', async () => {

            async function bumpVersionType(versionPart) {
                config["projects"]["scripts/test/Test1.csproj.copy"]['bumpVersion'] = versionPart
                await projop.processProject(`scripts/test/Test1.csproj`, config["projects"]["scripts/test/Test1.csproj.copy"], sdkop.processSDKProject, false, true)
                const doc = await xmlhandling.openXML(`${__dirname}/${testNuGet1}`, '')
                const version = doc.getElementsByTagName('version').item(0).textContent
                return version
            }

            assert.equal(await bumpVersionType("patch"), '0.0.2', "check patch version bump")
            assert.equal(await bumpVersionType("minor"), '0.1.0', "check minor version bump")
            assert.equal(await bumpVersionType("patch"), '0.1.1', "check patch version bump")
            assert.equal(await bumpVersionType("major"), '1.0.0', "check major version bump")
            assert.equal(await bumpVersionType("patch"), '1.0.1', "check patch version bump")
            assert.equal(await bumpVersionType("minor"), '1.1.0', "check minor version bump")

        });
        it('update versions in projects', async () => {

            async function checkProject(projName, version) {

                const doc = await xmlhandling.openXML(`${__dirname}/${projName}`, '')
                const deps = doc.getElementsByTagName("dependency")
                let j = 0
                for (let i = 0; i < deps.length; i++) {
                    const element = deps.item(i)
                    if (element.hasAttribute('id')) {
                        const val = element.getAttribute('id')
                        if (Object.keys(config['nuGets']).includes(val)) {
                            const version = element.getAttribute('version')
                            assert.equal(version, config['nuGets'][val], "Check if version was properly updated")
                        }
                    }
                }
            }
            await Promise.all(Object.keys(config["projects"])
                .map(async project => await projop.processProject(project, config, sdkop.updateDependencies, false)))
            await checkProject(testNuGet1, config['nuGets']['Test1'])
            await checkProject(testNuGet2, config['nuGets']['Test2'])
            await checkProject(testNuGet3, config['nuGets']['Test3'])
        })

        afterEach(() => {
            fs.removeSync(`${__dirname}/${testNuGet1}`)
            fs.removeSync(`${__dirname}/${testNuGet2}`)
            fs.removeSync(`${__dirname}/${testNuGet3}`)
            fs.removeSync(`${__dirname}/${test1}`)
            fs.removeSync(`${__dirname}/${test2}`)
            fs.removeSync(`${__dirname}/${test3}`)
        })
    });
});