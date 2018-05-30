'use strict';
/*
 * Operations for processing nuspecs and csproj on SDK projects. 
 */

const xmlhandling = require('./xml_handling')
const chalk = require('chalk')
const promisify = require('util').promisify
const execFile = promisify(require('child_process').execFile);

/**
 * Processes project, shows information about it, and bumps version if it is required
 * @param {Document} doc not used
 * @param {string} projConfig project configuration
 * @param {string} projPath path to .csproj 
 * @param {boolean} dryrun true=no changes to nuspec,false=changes will be written
 */
async function processSDKProject(doc, projConfig, projPath, dryrun) {
    const dir = `${__dirname}/../../`;
    try {
        const doc = await xmlhandling.openXML(dir, projConfig["nuspec"])
        const metadata = doc.documentElement.getElementsByTagName("metadata").item(0)
        const versionElement = metadata.getElementsByTagName('version').item(0)
        const version = versionElement.textContent
        let addendum = ""
        if (typeof projConfig["bumpVersion"] != 'undefined') {
            splitVersion = version.split('.')
            switch (projConfig["bumpVersion"]) {
                case 'major':
                    splitVersion[0]++
                    splitVersion[1] = 0
                    splitVersion[2] = 0
                    break;
                case 'minor':
                    splitVersion[1]++
                    splitVersion[2] = 0
                    break;
                case 'patch':
                    splitVersion[2]++
                    break;
            }
            newVersion = `${splitVersion[0]}.${splitVersion[1]}.${splitVersion[2]}`
            addendum = `bumping to ${chalk.green(newVersion)}`
            projConfig["newVersion"] = newVersion
            versionElement.textContent = newVersion
            if (!dryrun) {
                await xmlhandling.saveXML(dir, projConfig["nuspec"], doc)
                addendum += chalk.yellow(" (saved)")
            } else {
                addendum += chalk.gray(" (not saved)")
            }
        }
        console.log(`Project "${projPath}" package "${projConfig['package']}" version ${version} ${addendum}`)
    } catch (e) {
        console.log(`${e}: Unable to modify nuspec "${projConfig["nuspec"]}"`)
    }
}

/**
 * Updates dependencies for new versions in nuspec files.
 * @param {*} doc not used
 * @param {*} config configuration from sdk_config.json
 * @param {*} projPath path to project
 * @param {*} dryrun true=no changes to nuspec,false=changes will be written
 */
async function updateDependencies(doc, config, projPath, dryrun) {
    const dir = `${__dirname}/../../`;
    try {
        const doc = await xmlhandling.openXML(dir, config['projects'][projPath]["nuspec"])
        const metadata = doc.documentElement.getElementsByTagName("metadata").item(0)
        const dependencies = metadata.getElementsByTagName("dependencies").item(0)
        const packages = Object.keys(config["nuGets"])
        for (let i = 0; i < dependencies.childNodes.length; i++) {
            const dependency = dependencies.childNodes[i]
            if (dependency.nodeName == 'dependency') {
                const id = dependency.getAttribute('id')
                if (packages.includes(id)) {
                    dependency.setAttribute('version', config['nuGets'][id])
                }
            }
        }
        if (!dryrun) {
            await xmlhandling.saveXML(dir, config['projects'][projPath]["nuspec"], doc)
        }
    } catch (e) {
        console.log(`${e}: Unable to modify nuspec "${config['projects'][projPath]["nuspec"]}"`)
    }
}

/**
 * Packs all NuGets for pulishing
 * @param {Document} doc not used
 * @param {string} projConfig project configuration
 * @param {string} projPath path to .csproj 
 * @param {boolean} dryrun true=no changes to nuspec,false=changes will be written
 */
async function packNuGets(doc, projConfig, projPath, dryrun) {
    const dir = `${__dirname}/../../`;
    const nuSpecPath = `${dir}/${projConfig['nuSpec']}`
    const result=await execFile("nuget", ["pack", nuSpecPath])
    console.log(result)
}


module.exports = {
    "processSDKProject": processSDKProject,
    "packNuGets":packNuGets,
    "updateDependencies": updateDependencies
}