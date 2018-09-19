namespace FileUtils
{
    /// <summary>
    /// Helper class for getting the folder name from a file path.
    /// </summary>
    public static class FolderNameFromPath
    {
        private static string RemoveAfter(string str, char c)
        {
            return str.Substring(0, str.LastIndexOf(c));
        }

        private static string RemoveBefore(string str, char c)
        {
            var len = str.LastIndexOf(c);
            return str.Substring(len + 1, str.Length - len - 1);
        }

        /// <summary>
        /// Gets the name of a folder that a file is in.
        /// </summary>
        /// <param name="FilePath">The path to the file.</param>
        /// <returns>The folder name.</returns>
        public static string GetFolderNameFromFilePath(string FilePath)
        {
            return RemoveBefore(RemoveAfter(FilePath, '\\'), '\\');
        }
    }
}
