namespace FileUtils
{
    /// <summary>
    /// Helper class which contains helper functions for basic file/folder dialogs.
    /// </summary>
    public static class DialogHelpers
    {
        /// <summary>
        /// Open a Folder dialog.
        /// </summary>
        /// <param name="DialogDescription">The description of the dialog.</param>
        /// <returns>The selected folder, or null if a folder was not selected.</returns>
        public static string SelectFolderDialog(string DialogDescription)
        {
            string selectedPath = null;

            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog()
            {
                Description = DialogDescription,
            };

            System.Windows.Forms.DialogResult folderBrowserResult = folderBrowserDialog.ShowDialog();

            if (folderBrowserResult == System.Windows.Forms.DialogResult.OK)
            {
                selectedPath = folderBrowserDialog.SelectedPath;
            }

            return selectedPath;
        }

        /// <summary>
        /// Open a choose file dialog with the supplied properties. Get the files the user selects.
        /// </summary>
        /// <param name="DialogTitle">The title of the dialog.</param>
        /// <param name="FileFilter">The filter to use. EG "Word Documents|*.doc"</param>
        /// <param name="AllowMultiSelect">If true, allows multiple files to be selected.</param>
        /// <returns>An array of files which were selected, or null if none were selected.</returns>
        public static string[] SelectFilesDialog(string DialogTitle, string FileFilter, bool AllowMultiSelect)
        {
            string[] selectedFiles = null;

            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog()
            {
                Title = DialogTitle,
                Filter = FileFilter,
                Multiselect = AllowMultiSelect
            };

            System.Windows.Forms.DialogResult folderBrowserResult = openFileDialog.ShowDialog();

            if (folderBrowserResult == System.Windows.Forms.DialogResult.OK)
            {
                selectedFiles = openFileDialog.FileNames;
            }

            return selectedFiles;
        }
    }
}
