namespace IDE.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Project Resource type.
    /// </summary>
    public enum ProjectResourceType
    {
        /// <summary>
        /// Sound resource.
        /// </summary>
        Sounds,

        /// <summary>
        /// Image resource.
        /// </summary>
        Images,

        /// <summary>
        /// Code resource.
        /// </summary>
        Code,
    }

    /// <summary>
    /// Holds all information related to a project file.
    /// </summary>
    public class ProjectFile
    {
        /// <summary>
        /// The folder that contains this file.
        /// </summary>
        private ProjectFolder containingFolder;

        /// <summary>
        /// Initializes a new instance of the ProjectFile class.
        /// </summary>
        public ProjectFile()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ProjectFile class.
        /// </summary>
        /// <param name="containingFolder">The folder that contains this file.</param>
        public ProjectFile(ProjectFolder containingFolder)
        {
            this.containingFolder = containingFolder;
        }

        /// <summary>
        /// Gets or sets the resource type of this file.
        /// </summary>
        public ProjectResourceType ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the name of this file.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the folder that contains this file.
        /// </summary>
        public ProjectFolder ContainingFolder
        {
            get { return this.containingFolder; }
            set { this.containingFolder = value; }
        }
    }
}
