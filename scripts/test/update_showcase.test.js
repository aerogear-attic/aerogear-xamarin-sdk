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
        it('should add project dependencies',async () => {
            const config = [
                "..\\..\\Core\\Core\\Core.csproj",
                "..\\..\\Auth\\Auth\\Auth.csproj",
                "..\\..\\Security\\Security\\Security.csproj"
            ]
            await projop.processProject(`scripts/test/${csProjFileNuGets}`, config, showcaseop.addProjDeps, false, true)
            const doc = await xmlhandling.openXML(`${__dirname}/${csProjFileNuGets}`, '')
            const projectRefs = doc.getElementsByTagName("ProjectReference")
            let j=0
            for (let i = 0; i < projectRefs.length; i++) {
                const element = projectRefs.item(i)
                if (element.hasAttribute('Include')) {
                    if (config.includes(element.getAttribute('Include'))) j++
                }
            }
            assert.equal(j,config.length,"Check that all project dependencies were added")
        });
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
        it('should add NuGet dependencies', async () => {
            const config = {
                "AeroGear.Mobile.Core": "0.0.7",
                "AeroGear.Mobile.Auth": "0.0.7",
                "AeroGear.Mobile.Security": "0.0.8"
            }         
            await projop.processProject(`scripts/test/${csProjFileProjDeps}`, config, showcaseop.addNuGets, false, true)
            const doc = await xmlhandling.openXML(`${__dirname}/${csProjFileProjDeps}`, '')
            const packageRefs = doc.getElementsByTagName("PackageReference")
            let j=0
            for (let i = 0; i < packageRefs.length; i++) {
                const element = packageRefs.item(i)
                if (element.hasAttribute('Include')) {
                    const val=element.getAttribute('Include')
                    if (Object.keys(config).includes(val) && element.getAttribute("Version")==config[val]) j++
                }
            }
            assert.equal(j,Object.keys(config).length,"Check that all NuGet dependencies were added")
        });
        afterEach(() => {
            fs.removeSync(`${__dirname}/${csProjFileNuGets}`)
            fs.removeSync(`${__dirname}/${csProjFileProjDeps}`)
        })
    });
});