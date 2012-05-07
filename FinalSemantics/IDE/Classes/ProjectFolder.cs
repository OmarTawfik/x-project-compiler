namespace IDE.Classes
{
    using System.Collections.Generic;

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
        private List<string> subFiles = new List<string>();

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
        public ProjectFolder(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets the folder's name.
        /// </summary>
        public string Name { get; set; }

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
        public List<string> SubFiles
        {
            get { return this.subFiles; }
            set { this.subFiles = value; }
        }
    }
}
