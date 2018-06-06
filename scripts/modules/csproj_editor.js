'use strict';
/*
 * .csproj editing functions 
 */


const fs = require("fs")
const promisify = require("util").promisify
const readFile = promisify(fs.readFile)

/**
 * Removes project dependency in csproj XML.
 * @param {Document} doc XML document with csproj
 * @param {string} dependency dependency path to project
 */
async function removeProjectDependency(doc, dependency) {
    const itemGroups = doc.documentElement.getElementsByTagName("ItemGroup");
    for (let i = 0; i < itemGroups.length; i++) {
        const projectReferences = itemGroups.item(i).getElementsByTagName("ProjectReference");
        for (let j = 0; j < projectReferences.length; j++) {
            const include = projectReferences.item(j).getAttribute("Include");
            if (include == dependency) {
                itemGroups.item(i).removeChild(projectReferences.item(j));
                return true;
            }
        }
    }
    return false;
}

/**
 * Adds Project dependency to .csproj XML
 * @param {Document} doc 
 * @param {string} dependency 
 */
async function addProjectDependency(doc, dependency) {
    function checkAlreadyExists(doc) {
        const projRefs=doc.getElementsByTagName("ProjectReference");
        for (let i=0;i<projRefs.length;i++) {
            const element=projRefs.item(i)
            const include=element.getAttribute('Include')
            if (include==dependency) return true;
        }
        return false;
    }

    const document = doc.documentElement;
    const existingProjectReference = document.getElementsByTagName("ProjectReference")
    const addToExistingGroup = existingProjectReference.length > 0
    const itemGroup = addToExistingGroup ? existingProjectReference.item(0).parentNode : doc.createElement("ItemGroup")
    if (checkAlreadyExists(doc)) throw new Error(`Project dependency ${dependency} already exists.`)
    const projectReference = doc.createElement("ProjectReference")
    projectReference.setAttribute("Include", dependency)
    itemGroup.appendChild(projectReference)
    if (!addToExistingGroup) document.appendChild(itemGroup)
}


/**
 * Adds NuGet dependency to .csproj XML
 * @param {Document} doc 
 * @param {string} dependency 
 * @param {string} version 
 */
async function addNuGetDependency(doc, dependency, version) {
    function checkAlreadyExists(doc) {
        const packageRefs=doc.getElementsByTagName("PackageReference");
        for (let i=0;i<packageRefs.length;i++) {
            const element=packageRefs.item(i)
            const include=element.getAttribute('Include')
            if (include==dependency) return true;
        }
        return false;
    }
    
    
    const document = doc.documentElement;
    const project = document.getElementsByTagName("Project").item(0)
    const existingPackageReference = document.getElementsByTagName("PackageReference")
    const addToExistingGroup = existingPackageReference.length > 0
    const itemGroup = addToExistingGroup ? existingPackageReference.item(0).parentNode : doc.createElement("ItemGroup")
    if (checkAlreadyExists(doc)) throw new Error(`NuGet dependency ${dependency} already exists.`)
    const packageReference = doc.createElement("PackageReference")
    packageReference.setAttribute("Include", dependency)
    packageReference.setAttribute("Version", version)
    itemGroup.appendChild(packageReference)
    if (!addToExistingGroup) document.appendChild(itemGroup)
}

/**
 * Removes NuGet dependency from .csproj XML
 * @param {Document} doc 
 * @param {string} dependency 
 * @param {string} version 
 */
async function removeNuGetDependency(doc, dependency, version) {
    const document = doc.documentElement;
    const project = document.getElementsByTagName("Project").item(0)
    const itemGroups = doc.documentElement.getElementsByTagName("ItemGroup");
    for (let i = 0; i < itemGroups.length; i++) {
        const packageReferences = itemGroups.item(i).getElementsByTagName("PackageReference");
        for (let j = 0; j < packageReferences.length; j++) {
            const include = packageReferences.item(j).getAttribute("Include");
            if (include == dependency) {
                packageReferences.item(j).removeChild(packageReferences.item(j));
                return true;
            }
        }
    }
    return false;
}

module.exports = {
    "removeProjectDependency": removeProjectDependency, 
    "addNuGetDependency": addNuGetDependency, 
    "removeNuGetDependency": removeNuGetDependency,
    "addProjectDependency": addProjectDependency
}