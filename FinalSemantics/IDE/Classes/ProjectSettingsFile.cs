namespace IDE.Classes
{
    /// <summary>
    /// Holds information about a project settings file.
    /// </summary>
    public class ProjectSettingsFile
    {
        /// <summary>
        /// The folder containing the project sounds.
        /// </summary>
        private ProjectFolder soundsFolder = new ProjectFolder();

        /// <summary>
        /// The folder containing the project sprites.
        /// </summary>
        private ProjectFolder spritesFolder = new ProjectFolder();

        /// <summary>
        /// The folder containing the project code files.
        /// </summary>
        private ProjectFolder codeFolder = new ProjectFolder();

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
        /// Gets or sets the folder containing the project sprites.
        /// </summary>
        public ProjectFolder SpritesFolder
        {
            get { return this.spritesFolder; }
            set { this.spritesFolder = value; }
        }

        /// <summary>
        /// Gets or sets the folder containing the project code files.
        /// </summary>
        public ProjectFolder CodeFolder
        {
            get { return this.codeFolder; }
            set { this.codeFolder = value; }
        }
    }
}
