'use strict';
/*
 * Operations for updating Showcase app .csproj
 */

const csprojeditor = require("./csproj_editor")

/**
 * Adds project dependencies in csproj xml.
 * @param {Document} XML document
 * @param {Object} nuGets  nugets to add
 * @param {string} projPath .csproj path
 */
async function addNuGets(doc, nuGets, projPath) {
    await Promise.all(Object.keys(nuGets).map(async dependency => {
        const dependencyVersion = nuGets[dependency]
        const result = await csprojeditor.addNuGetDependency(doc, dependency, dependencyVersion)
        console.log(`Project "${projPath}" added dependency "${dependency}" version ${dependencyVersion}.`);
    }))
    return doc;
}

/**
 * Removes project's NuGet dependencies in csproj xml.
 * @param {Document} XML document
 * @param {Object} nuGets NuGets to remove
 * @param {string} projPath .csproj path
 */
async function removeNuGets(doc, nuGets, projPath) {
    await Promise.all(Object.keys(nuGets).map(async dependency => {
        const result = await csprojeditor.removeNuGetDependency(doc, dependency)
        console.log(`NuGet "${projPath}" dependency "${dependency}" ${result ? "removed" : "not found"}.`);
    }))
    return doc;
}

/**
 * Adds project dependencies to csproj xml.
 * @param {Document} XML document
 * @param {Object} projDeps project dependencies
 * @param {string} projPath .csproj path
 */
async function removeProjDeps(doc, projDeps, projPath) {
    await Promise.all(projDeps.map(async dependency => {
        const result = await csprojeditor.removeProjectDependency(doc, dependency)
        console.log(`Project "${projPath}" dependency "${dependency}" ${result ? "removed" : "not found"}.`);
    }))
    return doc;
}


/**
 * Adds project dependencies to csproj xml.
 * @param {Document} XML document
 * @param {Object} projDeps project dependencies
 * @param {string} projPath .csproj path
 */
async function addProjDeps(doc, projDeps, projPath) {
    await Promise.all(projDeps.map(async dependency => {
        const result = await csprojeditor.addProjectDependency(doc, dependency)
        console.log(`Project "${projPath}" dependency "${dependency}" added.`);
    }))
    return doc;
}

module.exports= {
    "addProjDeps":addProjDeps,
    "removeProjDeps":removeProjDeps,
    "addNuGets":addNuGets,
    "removeNuGets":removeNuGets
}