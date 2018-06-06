'use strict';
/*
 * Operations for processing  .csproj files. 
 */

const xmlhandling = require('./xml_handling')

/**
 * Processes XML csproj file with given operation
 * @param {string} projPath path to project
 * @param {projConfig} projConfig part of configuration with operation specific configuration
 * @param {function} operation operation (see `modules/showcase_operations.js`)
 * @param {bool} [dryrun] true=changes are not saved, false=changes will be written to project files 
 * @param {bool} [displayText] true=display saved/dry-run messages
 */
async function processProject(projPath, projConfig, operation, dryrun, displayText) {
    const dir = `${__dirname}/../../`;
    try {
        const doc = await xmlhandling.openXML(dir, projPath)
        await operation(doc, projConfig, projPath, dryrun)
        if (dryrun != undefined) {
            if (!dryrun) {
                await xmlhandling.saveXML(dir, projPath, doc)
                if (displayText) console.log(`Saved project ${projPath}`)
            } else {
                if (displayText) console.log(`Dry-run for project ${projPath} completed, changes not saved.`)
            }
        }
    } catch (e) {        
        console.log(`${e}: Unable to modify project "${projPath}"`)
        return;        
    }
}

module.exports = {
    "processProject": processProject
}