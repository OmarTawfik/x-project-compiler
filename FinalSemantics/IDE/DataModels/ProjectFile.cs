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
        /// Initializes a new instance of the ProjectFile class.
        /// </summary>
        public ProjectFile()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ProjectFile class.
        /// </summary>
        /// <param name="type">The resource type of this file.</param>
        /// <param name="name">The name of this file.</param>
        public ProjectFile(ProjectResourceType type, string name)
        {
            this.ResourceType = type;
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets the resource type of this file.
        /// </summary>
        public ProjectResourceType ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the name of this file.
        /// </summary>
        public string Name { get; set; }
    }
}
