#!/usr/bin/env node

/*
Script for releasing the Xamarin SDK.
*/

const yargs = require("yargs")
const fs = require("fs")
const xmldom = require("xmldom")
const promisify = require("util").promisify
const csprojeditor = require("./modules/csproj_editor")
const xmlhandling = require('./modules/xml_handling')
const chalk = require('chalk')
const projop = require('./modules/project_operations')
const sdkop = require('./modules/sdk_operations')
const readFile = promisify(fs.readFile)

const argv = yargs.usage("Usage: $0 <command> [options]'")
    .command("info", "Prints out information about configuration")
    .command("bump <module|all> [level]", "Bumps SDK version (level is major,minor or patch)")
    .command("pack <module|all>", "Packages modules to NuGets")
    .command("pushLocal <dir>", "Copies NuGets to local directory")
    .command("push <module|all>", "Pushes NuGets to public repository")
    .demandCommand(1)
    .option("write", {
        default: false, desc:
            "Writes changes to .csproj and .nuspec files (it's a dry-run by default)"
    }).argv

readFile(`${__dirname}/sdk_config.json`).then(data =>
    configLoaded(JSON.parse(data))).catch(err => console.log(`${err}: Failed to open or read "sdk_config.json" file.`))

/**
 * Called when config is successfuly loaded.
 * @param {Object} config configuration
 */
function configLoaded(config) {
    if (argv._.includes("info")) {
        console.log("This is the configuration of SDK packages:")
        Object.keys(config["projects"]).forEach(async project => {
            await projop.processProject(project, config["projects"][project], sdkop.processSDKProject)
        })
    } else if (argv._.includes("bump")) {
        const bumpModule = argv.module
        Promise.all(Object.keys(config["projects"]).map(async project => {
            if (bumpModule == 'all' || bumpModule == config["projects"][project]['package']) {
                config["projects"][project]['bumpVersion'] = argv.level ? argv.level : "patch"
            }
            await projop.processProject(project, config["projects"][project], sdkop.processSDKProject, !argv.write)
        })).then(() => {
            let nuGets = {};
            Object.keys(config["projects"]).forEach(project => {
                if (config["projects"][project]["newVersion"] != undefined) {
                    nuGets[config["projects"][project]["package"]] = config["projects"][project]["newVersion"]
                }
            })
            config["nuGets"] = nuGets
        }).then(() => Promise.all(Object.keys(config["projects"])
            .map(async project => await projop.processProject(project, config, sdkop.updateDependencies, !argv.write)))).then(() =>
                console.log("Updated dependencies")).catch(fail =>
                    console.log(fail))
    } else if (argv._.includes("pack")) {
        const packModule = argv.module
        Promise.all(Object.keys(config["projects"])
            .filter(project => packModule == 'all' || packModule == config["projects"][project]['package'])
            .map(async project => await projop.processProject(project, config["projects"][project], sdkop.packNuGets)))
            .then(() => console.log("NuGet(s) packed."))
    } else if (argv._.includes("push")) {
        const pushModule = argv.module
        Promise.all(Object.keys(config["projects"])
            .filter(project => pushModule == 'all' || pushModule == config["projects"][project]['package'])
            .map(async project => await projop.processProject(project, config["projects"][project], sdkop.pushNuGets)))
            .then(() => console.log("NuGet(s) packages released."))
    } else {
        console.log(`Invalid command "${argv._[0]}" specified\n`);
        yargs.showHelp()
    }
}