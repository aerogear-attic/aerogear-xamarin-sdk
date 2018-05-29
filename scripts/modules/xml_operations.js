
const promisify=require("util").promisify
const fs = require("fs")
const readFile = promisify(fs.readFile)
const writeFile = promisify(fs.writeFile)
const xmldom = require("xmldom")

/**
 * Opens XML file.
 * @param {string} path path to solution directory
 * @param {string} xmlPath location to XML relative to solution dir
 * @returns {Promise}
 */
async function openXML(path, xmlPath) {
    const fname = `${path}${xmlPath}`
    const data = await readFile(fname);
    return new xmldom.DOMParser().parseFromString(data.toString());
}
/**
 * Saves XML file
 * @param {string} path path to solution directory
 * @param {string}  xmlPath location to XML relative to solution dir
 * @param {Document} doc XML document
 * @param {string} [extension] XML filename extension (default is .csproj)
 */
async function saveXML(path, xmlPath,doc) {
    const fname = `${path}${xmlPath}`
    const xmlstring = new xmldom.XMLSerializer().serializeToString(doc)
    await writeFile(fname, xmlstring)
}

module.exports={"saveXML":saveXML,"openXML":openXML}