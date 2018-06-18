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
            const splitVersion = version.split('.')
            const suffixSplit = splitVersion[2].split('-')
            if (suffixSplit.length == 2) {
                splitVersion[2] = suffixSplit[0]
                splitVersion[3] = suffixSplit[1]
            }
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
            if (Object.keys(projConfig).includes("suffix")) splitVersion[3] = projConfig['suffix']
            const newVersion = `${splitVersion[0]}.${splitVersion[1]}.${splitVersion[2]}` + `${splitVersion.length > 3 && splitVersion[3]!="" ? `-${splitVersion[3]}` : ""}`
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
    const nuSpecPath =`${dir}/${projConfig['nuspec']}`
    try {
        const result = await execFile("nuget", ["pack", nuSpecPath, '-OutputDirectory', `${dir}/nugets`,'-Prop',`Configuration=${projConfig['configuration']}`])        
        console.log(result.stdout)
        console.log(`Packed package ${chalk.green(projConfig['package'])}`)
    } catch (e) {
        console.log(e.stdout)
        console.log(e.stderr)
        console.log(chalk.red(`Failed to pack "${projConfig['package']}".`))
    }
}

/**
 * Pushes all NuGets to public repo
 * @param {Document} doc csproj XML document
 * @param {string} projConfig project configuration
 * @param {string} projPath path to .csproj 
 * @param {boolean} dryrun true=no changes to nuspec,false=changes will be written
 */
async function pushNuGets(doc, projConfig, projPath, dryrun) {
    const dir = `${__dirname}/../../`;
    try {
        const doc = await xmlhandling.openXML(dir, projConfig["nuspec"])
        const version = doc.getElementsByTagName('version').item(0).textContent
        const nuGetPath = `${dir}nugets/${projConfig['package']}.${version}.nupkg`
        const result = await execFile("nuget", ["push", nuGetPath, '-Source', 'https://api.nuget.org/v3/index.json'])
        console.log(result.stdout)
        console.log(`Released package ${chalk.green(projConfig['package'])} to public repository.`)
    } catch (e) {
        console.log(e.stdout)
        console.log(e.stderr)
        console.log(chalk.red(`Failed to release "${projConfig['package']}" to public repository.`))
    }
}



module.exports = {
    "processSDKProject": processSDKProject,
    "packNuGets": packNuGets,
    "pushNuGets": pushNuGets,
    "updateDependencies": updateDependencies
}