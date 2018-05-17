'use strict';

const fs = require("fs")
const promisify = require("util").promisify
const readFile = promisify(fs.readFile)

/**
 * Removes project dependency in csproj XML.
 * @param {Document} doc XML document with csproj
 * @param {string} dependency dependency path to project
 */
async function removeProjectDep(doc, dependency) {
    const itemGroups = doc.documentElement.getElementsByTagName("ItemGroup");
    for (let i = 0; i < itemGroups.length; i++) {
        const projectReferences = itemGroups.item(i).getElementsByTagName("ProjectReference");
        for (let j = 0; j < projectReferences.length; j++) {
            const include = projectReferences.item(j).getAttribute("Include");
            if (include == dependency) {
                projectReferences.item(j).removeChild(projectReferences.item(j));
                return true;
            }
        }
    }
    return false;
}
/**
 * Adds NuGet dependency to .csproj XML
 * @param {Document} doc 
 * @param {string} dependency 
 * @param {string} version 
 */
async function addNuGetDependency(doc, dependency,version) {
    const document=doc.documentElement;
    const project=document.getElementsByTagName("Project").item(0)
    const existingPackageReference=document.getElementsByTagName("PackageReference")
    const addToExistingGroup=existingPackageReference.length>0
    const itemGroup=addToExistingGroup?existingPackageReference.item(0).parentNode:doc.createElement("ItemGroup")
    const packageReference=doc.createElement("PackageReference")
    packageReference.setAttribute("Include",dependency)
    packageReference.setAttribute("Version",version)
    itemGroup.appendChild(packageReference)
    if (!addToExistingGroup) project.appendChild(itemGroup)
}

module.exports = { "removeProjectDep": removeProjectDep, "addNuGetDependency": addNuGetDependency }