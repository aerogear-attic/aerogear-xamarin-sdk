#!/bin/bash
'use strict'
const assert = require('assert');
const fs = require('fs-extra')
const promisify = require("util").promisify
const xmlhandling = require('../modules/xml_handling')
const showcaseop = require('../modules/showcase_operations')
const projop = require('../modules/project_operations')
const readFile = promisify(fs.readFile)

describe('Showcase App updater', () => {
    describe('', () => {
        const csProjFileNuGetsOrig = `Showcase.Test.NuGets.csproj`
        const csProjFileNuGets = `${csProjFileNuGetsOrig}.copy`
        const csProjFileProjDepsOrig = `Showcase.Test.ProjDeps.csproj`
        const csProjFileProjDeps = `${csProjFileProjDepsOrig}.copy`
        beforeEach(() => {
            fs.copySync(`${__dirname}/${csProjFileNuGetsOrig}`, `${__dirname}/${csProjFileNuGets}`)
            fs.copySync(`${__dirname}/${csProjFileProjDepsOrig}`, `${__dirname}/${csProjFileProjDeps}`)
        })

        it('should remove NuGet dependencies', async () => {
            const config = {
                "AeroGear.Mobile.Core": "0.0.7",
                "AeroGear.Mobile.Auth": "0.0.7",
                "AeroGear.Mobile.Security": "0.0.8"
            }
            await projop.processProject(`scripts/test/${csProjFileNuGets}`, config, showcaseop.removeNuGets, false, true)
            const doc = await xmlhandling.openXML(`${__dirname}/${csProjFileNuGets}`, '')
            const packageRefs = doc.getElementsByTagName("PackageReference")
            for (let i = 0; i < packageRefs.length; i++) {
                const element = packageRefs.item(i)
                if (element.hasAttribute('Include')) {
                    assert.equal(Object.keys(config).includes(element.getAttribute('Include')), false, "Check that all NuGet dependencies removed properly")
                }
            }
        });

        describe("adding project dependencies", () => {
            const config = [
                "..\\..\\Core\\Core\\Core.csproj",
                "..\\..\\Auth\\Auth\\Auth.csproj",
                "..\\..\\Security\\Security\\Security.csproj"
            ]

            function getCount(doc) {
                const projectRefs = doc.getElementsByTagName("ProjectReference")
                let count = 0
                for (let i = 0; i < projectRefs.length; i++) {
                    const element = projectRefs.item(i)
                    if (element.hasAttribute('Include')) {
                        if (config.includes(element.getAttribute('Include'))) count++
                    }
                }
                return count;
            }


            it('should add project dependencies', async () => {
                await projop.processProject(`scripts/test/${csProjFileNuGets}`, config, showcaseop.addProjDeps, false, true)
                const doc = await xmlhandling.openXML(`${__dirname}/${csProjFileNuGets}`, '')

                const count = getCount(doc)
                assert.equal(count, config.length, "Check that all project dependencies were added")
            })

            it('shouldn\'t add project dependencies, if they are already there', async () => {
                await projop.processProject(`scripts/test/${csProjFileNuGets}`, config, showcaseop.addProjDeps, false, true)
                let doc = await xmlhandling.openXML(`${__dirname}/${csProjFileNuGets}`, '')
                const count = getCount(doc)
                await projop.processProject(`scripts/test/${csProjFileNuGets}`, config, showcaseop.addProjDeps, false, true)
                doc = await xmlhandling.openXML(`${__dirname}/${csProjFileNuGets}`, '')
                const newCount = getCount(doc)
                assert.equal(newCount, count, "Check that no more project dependencies were added")
            })
        })

        it('should remove project dependencies', async () => {
            const config = [
                "..\\..\\Core\\Core.Platform.Android\\Core.Platform.Android.csproj",
                "..\\..\\Core\\Core\\Core.csproj",
                "..\\..\\Security\\Security\\Security.csproj",
                "..\\..\\Auth\\Auth\\Auth.csproj"
            ]
            await projop.processProject(`scripts/test/${csProjFileProjDeps}`, config, showcaseop.removeProjDeps, false, true)
            const doc = await xmlhandling.openXML(`${__dirname}/${csProjFileProjDeps}`, '')
            const projectRefs = doc.getElementsByTagName("ProjectReference")
            for (let i = 0; i < projectRefs.length; i++) {
                const element = projectRefs.item(i)
                if (element.hasAttribute('Include')) {
                    assert.equal(config.includes(element.getAttribute('Include')), false, "Check that all project dependencies removed properly")
                }
            }
        });

        describe("adding NuGet dependencies", () => {

            function getCount(doc) {
                const packageRefs = doc.getElementsByTagName("PackageReference")
                let count = 0
                for (let i = 0; i < packageRefs.length; i++) {
                    const element = packageRefs.item(i)
                    if (element.hasAttribute('Include')) {
                        const val = element.getAttribute('Include')
                        if (Object.keys(config).includes(val) && element.getAttribute("Version") == config[val]) count++
                    }
                }
                return count
            }

            const config = {
                "AeroGear.Mobile.Core": "0.0.7",
                "AeroGear.Mobile.Auth": "0.0.7",
                "AeroGear.Mobile.Security": "0.0.8"
            }

            it('should add NuGet dependencies', async () => {
                
                await projop.processProject(`scripts/test/${csProjFileProjDeps}`, config, showcaseop.addNuGets, false, true)
                const doc = await xmlhandling.openXML(`${__dirname}/${csProjFileProjDeps}`, '')
                const count = getCount(doc)
                assert.equal(count, Object.keys(config).length, "Check that all NuGet dependencies were added")
            });

            it('shouldn\'t add NuGet dependencies more than once', async () => {
                await projop.processProject(`scripts/test/${csProjFileProjDeps}`, config, showcaseop.addNuGets, false, true)
                let doc = await xmlhandling.openXML(`${__dirname}/${csProjFileProjDeps}`, '')
                const count = getCount(doc)
                
                await projop.processProject(`scripts/test/${csProjFileProjDeps}`, config, showcaseop.addNuGets, false, true)
                doc = await xmlhandling.openXML(`${__dirname}/${csProjFileProjDeps}`, '')
                const newCount = getCount(doc)
                assert.equal(newCount, count, "Check that NuGet dependencies are added only once")
            });

        })


        afterEach(() => {
            fs.removeSync(`${__dirname}/${csProjFileNuGets}`)
            fs.removeSync(`${__dirname}/${csProjFileProjDeps}`)
        })
    });
});