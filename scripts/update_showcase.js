#!/usr/bin/env node
'use strict'

/*
Script for updating Showcase Template App from the SDK Example app.
*/

const yargs = require("yargs")
const fs = require("fs")
const promisify = require("util").promisify
const xmlhandling = require('./modules/xml_handling')
const showcaseop = require('./modules/showcase_operations')
const projop=require('./modules/project_operations')
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
 * Called when config is successfuly loaded.
 * @param {Object} config configuration
 */ 
function configLoaded(config) {
    const projRefs = config["proj-refs"];
    const nuGetDeps = config["nuget-deps"];

    if (argv._.includes("removeProjDeps")) {
        console.log("Removing project's project dependencies:")
        Object.keys(projRefs).forEach(async project => await projop.processProject(project, projRefs[project], showcaseop.removeProjDeps, !argv.write,true))
    } else if (argv._.includes("addNuGets")) {
        console.log("Adding project's NuGet dependencies:")
        Object.keys(nuGetDeps).forEach(async project => await projop.processProject(project, nuGetDeps[project], showcaseop.addNuGets, !argv.write,true))
    } else if (argv._.includes('removeNuGets')) {
        console.log("Removing project's NuGet dependencies:")
        Object.keys(nuGetDeps).forEach(async project => await projop.processProject(project, nuGetDeps[project], showcaseop.removeNuGets, !argv.write,true))
    } else if (argv._.includes("addProjDeps")) {
        console.log("Adding project's project dependencies:")
        Object.keys(nuGetDeps).forEach(async project => await projop.processProject(project, projRefs[project], showcaseop.addProjDeps, !argv.write,true))
    } else {
        console.log(`Invalid command "${argv._[0]}" specified\n`);
        yargs.showHelp()
    }
}