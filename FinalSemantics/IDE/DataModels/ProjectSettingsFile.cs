namespace IDE.DataModels
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Xml.Serialization;

    /// <summary>
    /// Holds information about a project settings file.
    /// </summary>
    public class ProjectSettingsFile
    {
        /// <summary>
        /// The folder containing the project sounds.
        /// </summary>
        private ProjectFolder soundsFolder;

        /// <summary>
        /// The folder containing the project images.
        /// </summary>
        private ProjectFolder imagesFolder;

        /// <summary>
        /// The folder containing the project code files.
        /// </summary>
        private ProjectFolder codeFolder;

        /// <summary>
        /// Initializes a new instance of the ProjectSettingsFile class.
        /// </summary>
        public ProjectSettingsFile()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ProjectSettingsFile class.
        /// </summary>
        /// <param name="projectName">The project name.</param>
        /// <param name="projectLocation">The project location.</param>
        public ProjectSettingsFile(string projectName, string projectLocation)
        {
            this.ProjectName = projectName;
            this.ProjectLocation = projectLocation;

            this.soundsFolder = new ProjectFolder("Sounds", this.ProjectLocation);
            this.codeFolder = new ProjectFolder("Code", this.ProjectLocation);
            this.imagesFolder = new ProjectFolder("Images", this.ProjectLocation);
        }

        /// <summary>
        /// Gets or sets the project name.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Gets or sets the project location.
        /// </summary>
        public string ProjectLocation { get; set; }
        
        /// <summary>
        /// Gets the exact full location of this project.
        /// </summary>
        public string ProjectFullPath
        {
            get { return string.Format("{0}\\{1}.xproject", this.ProjectLocation, this.ProjectName); }
        }

        /// <summary>
        /// Gets or sets the folder containing the project sounds.
        /// </summary>
        public ProjectFolder SoundsFolder
        {
            get { return this.soundsFolder; }
            set { this.soundsFolder = value; }
        }

        /// <summary>
        /// Gets or sets the folder containing the project images.
        /// </summary>
        public ProjectFolder ImagesFolder
        {
            get { return this.imagesFolder; }
            set { this.imagesFolder = value; }
        }

        /// <summary>
        /// Gets or sets the folder containing the project code files.
        /// </summary>
        public ProjectFolder CodeFolder
        {
            get { return this.codeFolder; }
            set { this.codeFolder = value; }
        }

        /// <summary>
        /// Gets the parent folder of a given child file.
        /// </summary>
        /// <param name="child">Child file to use.</param>
        /// <returns>A ProjectFolder object.</returns>
        public ProjectFolder GetParentFile(ProjectFile child)
        {
            ProjectFolder temp = this.codeFolder.GetParentFile(child);
            if (temp != null)
            {
                return temp;
            }

            temp = this.soundsFolder.GetParentFile(child);
            if (temp != null)
            {
                return temp;
            }

            return this.imagesFolder.GetParentFile(child);
        }

        /// <summary>
        /// Gets the parent folder of a given child folder.
        /// </summary>
        /// <param name="child">Child folder to use.</param>
        /// <returns>A ProjectFolder object.</returns>
        public ProjectFolder GetParentFolder(ProjectFolder child)
        {
            if (child == this.codeFolder || child == this.soundsFolder || child == this.imagesFolder)
            {
                return null;
            }

            ProjectFolder temp = this.codeFolder.GetParentFolder(child);
            if (temp != null)
            {
                return temp;
            }

            temp = this.soundsFolder.GetParentFolder(child);
            if (temp != null)
            {
                return temp;
            }

            return this.imagesFolder.GetParentFolder(child);
        }

        /// <summary>
        /// Saves the project to the hard disk.
        /// </summary>
        public void SaveProject()
        {
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(ProjectSettingsFile));
                FileStream fileStream = new FileStream(this.ProjectFullPath, FileMode.Create);
                xml.Serialize(fileStream, this);
                fileStream.Close();
            }
            catch
            {
                MessageBox.Show("Error saving the project settings.", "Project Saving Error");
                return;
            }

            try
            {
                this.soundsFolder.CheckFiles();
                this.codeFolder.CheckFiles();
                this.imagesFolder.CheckFiles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("The Item " + ex.Message + " cannot be found.", "Project Saving Error");
            }
        }
    }
}
