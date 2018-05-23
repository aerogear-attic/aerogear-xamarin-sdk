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

const argv = yargs.usage("Usage: $0 <command> [options]'")
    .command("removeProjDeps", "Removes project dependencies").command("addNuGets", "Adds NuGet dependencies to project")
    .command("addProjDeps", "Adds project dependencies").command("removeNuGets", "Removes NuGet dependencies from project").demandCommand(1)
    .option("write", {
        default: false, desc:
            "Writes changes to .csproj files (it's a dry-run by default)"
    }).argv

readFile(`${__dirname}/showcase_config.json`).then(data =>
    configLoaded(JSON.parse(data))).catch(err => console.log(`${err}: Failed to open or read config json file.`))

/**
 * Adds project dependencies in csproj xml.
 * @param {Document} XML document
 * @param {Object} nuGets  nugets to add
 * @param {string} projPath .csproj path
 */
async function addNuGets(doc, nuGets, projPath) {
    await Object.keys(nuGets).forEach(async dependency => {
        const dependencyVersion = nuGets[dependency]
        const result = await csprojeditor.addNuGetDependency(doc, dependency, dependencyVersion)
        console.log(`Project "${projPath}" added dependency "${dependency}" version ${dependencyVersion}.`);
    })
    return doc;
}

/**
 * Removes project's NuGet dependencies in csproj xml.
 * @param {Document} XML document
 * @param {Object} nuGets NuGets to remove
 * @param {string} projPath .csproj path
 */
async function removeNuGets(doc, nuGets, projPath) {
    await Object.keys(nuGets).forEach(async dependency => {
        const result = await csprojeditor.removeNuGetDependency(doc, dependency)
        console.log(`NuGet "${projPath}" dependency "${dependency}" ${result ? "removed" : "not found"}.`);
    })
    return doc;
}

/**
 * Adds project dependencies to csproj xml.
 * @param {Document} XML document
 * @param {Object} projDeps project dependencies
 * @param {string} projPath .csproj path
 */
async function removeProjDeps(doc, projDeps, projPath) {
    await projDeps.forEach(async dependency => {
        const result = await csprojeditor.removeProjectDependency(doc, dependency)
        console.log(`Project "${projPath}" dependency "${dependency}" ${result ? "removed" : "not found"}.`);
    })
    return doc;
}


/**
 * Adds project dependencies to csproj xml.
 * @param {Document} XML document
 * @param {Object} projDeps project dependencies
 * @param {string} projPath .csproj path
 */
async function addProjDeps(doc, projDeps, projPath) {
    await projDeps.forEach(async dependency => {
        const result = await csprojeditor.addProjectDependency(doc, dependency)
        console.log(`Project "${projPath}" dependency "${dependency}" added.`);
    })
    return doc;
}



async function processProject(config, projPath, projConfig, operation, dryrun) {
    const dir = `${__dirname}/../`;
    try {
        const doc = await xmlop.openXML(dir, projPath)
        await operation(doc, projConfig, projPath)
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
    const projRefs = config["proj-refs"];
    const nuGetDeps = config["nuget-deps"];

    if (argv._.includes("removeProjDeps")) {
        console.log("Removing project's project dependencies:")
        Object.keys(projRefs).forEach(async project => await processProject(config, project, projRefs[project], removeProjDeps, !argv.write))
    } else if (argv._.includes("addNuGets")) {
        console.log("Adding project's NuGet dependencies:")
        Object.keys(nuGetDeps).forEach(async project => await processProject(config, project, nuGetDeps[project], addNuGets, !argv.write))
    } else if (argv._.includes('removeNuGets')) {
        console.log("Removing project's NuGet dependencies:")
        Object.keys(nuGetDeps).forEach(async project => await processProject(config, project, nuGetDeps[project], removeNuGets, !argv.write))
    } else if (argv._.includes("addProjDeps")) {
        console.log("Adding project's project dependencies:")
        Object.keys(nuGetDeps).forEach(async project => await processProject(config, project, projRefs[project], addProjDeps, !argv.write))
    } else {
        console.log(`Invalid command "${argv._[0]}" specified\n`);
        yargs.showHelp()
    }
}


