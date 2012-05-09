namespace IDE.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Holds information about a folder inside a project.
    /// </summary>
    public class ProjectFolder
    {
        /// <summary>
        /// Subfolders contained in this folder.
        /// </summary>
        private List<ProjectFolder> subFolders = new List<ProjectFolder>();

        /// <summary>
        /// Subfiles contained in this folder.
        /// </summary>
        private List<ProjectFile> subFiles = new List<ProjectFile>();

        /// <summary>
        /// Initializes a new instance of the ProjectFolder class.
        /// </summary>
        public ProjectFolder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ProjectFolder class.
        /// </summary>
        /// <param name="name">The folder's name.</param>
        /// <param name="location">The folder's location.</param>
        public ProjectFolder(string name, string location)
        {
            this.Name = name;
            this.Location = location;
        }

        /// <summary>
        /// Gets or sets the folder's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the folder's location.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the subfolders contained in this folder.
        /// </summary>
        public List<ProjectFolder> SubFolders
        {
            get { return this.subFolders; }
            set { this.subFolders = value; }
        }

        /// <summary>
        /// Gets or sets the subfiles contained in this folder.
        /// </summary>
        public List<ProjectFile> SubFiles
        {
            get { return this.subFiles; }
            set { this.subFiles = value; }
        }

        /// <summary>
        /// Gets the parent folder of a given child folder.
        /// </summary>
        /// <param name="child">Child folder to use.</param>
        /// <returns>A ProjectFolder object.</returns>
        public ProjectFolder GetParent(ProjectFolder child)
        {
            if (this.SubFolders.Contains(child))
            {
                return this;
            }

            foreach (ProjectFolder subFolder in this.subFolders)
            {
                ProjectFolder temp = subFolder.GetParent(child);
                if (temp != null)
                {
                    return temp;
                }
            }

            return null;
        }

        /// <summary>
        /// Checks if all subfolder/subfiles exist on hard disk.
        /// </summary>
        public void CheckFiles()
        {
            string folderPath = this.Location + "\\" + this.Name;

            if (Directory.Exists(folderPath) == false)
            {
                Directory.CreateDirectory(folderPath);
            }

            foreach (ProjectFolder subFolder in this.subFolders)
            {
                subFolder.CheckFiles();
            }

            foreach (ProjectFile subFile in this.subFiles)
            {
                if (File.Exists(folderPath + "\\" + subFile.Name) == false)
                {
                    throw new Exception(folderPath + "\\" + subFile.Name);
                }
            }
        }
    }
}
