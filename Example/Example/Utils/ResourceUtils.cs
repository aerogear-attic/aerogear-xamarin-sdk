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

    }
}
