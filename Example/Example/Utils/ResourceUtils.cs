using System;
namespace Example.Utils
{
    public static class ResourceUtils
    {
        /// <summary>
        /// Gets the resource URI to be used for loading images.
        /// </summary>
        /// <returns>The resource URI.</returns>
        /// <param name="fileName">File name.</param>
        public static String GetResourceUri(String fileName) {
            return $"resource://Example.Resources.{fileName}";
        }
        /// <summary>
        /// Gets the resource URI of the SVG image stored 
        /// </summary>
        /// <returns>The svg.</returns>
        /// <param name="name">Name.</param>
        public static String GetSvg(String name) {
            return GetResourceUri($"{name}.svg");
        }

    }
}
