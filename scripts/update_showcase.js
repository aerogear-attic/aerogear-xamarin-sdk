#!/usr/bin/env node
'use strict'

/*
Script for updating Showcase Template App from the SDK Example app.
*/

const yargs = require("yargs")
const fs = require("fs")
const xmldom = require("xmldom")
const promisify = require("util").promisify
const csprojeditor = require("./modules/csproj_editor")
const xmlop = require('./modules/xml_operations')
const readFile = promisify(fs.readFile)
const showcaseSync=require("./modules/showcase_sync");

const argv = yargs.usage("Usage: $0 <command> [options]'")
    .command("removeProjDeps", "Removes project dependencies").command("addNuGets", "Adds NuGet dependencies to project")
    .command("pullShowcase", "Pulls Showcase Template from its repo to the local project").demandCommand(1)
    .option("write", {
        default: false, desc:
            "Writes changes to .csproj files (it's a dry-run by default)"
    }).argv

readFile(`${__dirname}/showcase_config.json`).then(data =>
    configLoaded(JSON.parse(data))).catch(err => console.log(`${err}: Failed to open or read config json file.`))

/**
 * Adds project dependencies in csproj xml.
 * @param {Document} XML document
 * @param {Object} config configuration
 * @param {string} projPath .csproj path
 */
async function addNuGetDeps(doc, config, projPath) {
    const nuGetDeps = config["nuget-deps"]
    await Object.keys(nuGetDeps[projPath]).forEach(async dependency => {
        const dependencyVersion = nuGetDeps[projPath][dependency]
        const result = await csprojeditor.addNuGetDependency(doc, dependency, dependencyVersion)
        console.log(`Project "${projPath}" added dependency "${dependency}" version ${dependencyVersion}.`);
    })
    return doc;
}

/**
 * Removes project dependencies in csproj xml.
 * @param {Document} XML document
 * @param {Object} config configuration
 * @param {string} projPath .csproj path
 */
async function removeDeps(doc, config, projPath) {
    const projRefs = config["proj-refs"]
    await projRefs[projPath].forEach(async dependency => {
        const result = await csprojeditor.removeProjectDep(doc, dependency)
        console.log(`Project "${projPath}" dependency "${dependency}" ${result ? "removed" : "not found"}.`);
    })
    return doc;
}

async function processProject(config, projPath, operation, dryrun) {
    const dir = `${__dirname}/../`;
    try {
        const doc = await xmlop.openXML(dir, projPath)
        await operation(doc, config, projPath)
        if (!dryrun) {
            await xmlop.saveXML(dir, projPath, doc)
            console.log(`Saved project ${projPath}`)
        } else {
            console.log(`Dry-run for project ${projPath} completed, changes not saved.`)
        }
    } catch (e) {
        console.log(`${e}: Unable to modify project "${projPath}"`)
    }
}

function configLoaded(config) {
    if (argv._.includes("removeDeps")) {
        console.log("Removing project dependencies:")
        Object.keys(config["proj-refs-remove"]).forEach(async project => await processProject(config, project, removeDeps, !argv.write))
    } else if (argv._.includes("addNuGets")) {
        console.log("Adding project NuGet dependencies:")
        Object.keys(config["deps-add"]).forEach(async project => await processProject(config, project, addNuGetDeps, !argv.write))
    } else if (argv._.includes('pullShowcase')) {
        showcaseSync.pullShowcase(config['showcase-repo'],`${__dirname}/${config['showcase-dir']}`)
    } else {
        console.log(`Invalid command "${argv._[0]}" specified\n`);
        yargs.showHelp()
    }
}

