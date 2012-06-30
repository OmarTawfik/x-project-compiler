namespace XIDE.Manager
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Media;
    using System.Windows.Media.Imaging;
    using System.Xml.Serialization;

    /// <summary>
    /// Indicates the type of a resource inside the project.
    /// </summary>
    public enum ProjectResourceType
    {
        /// <summary>
        /// Sprite: Images used in the project (.png).
        /// </summary>
        Sprite,

        /// <summary>
        /// Sound: effects used in the project (.wav).
        /// </summary>
        Sound,

        /// <summary>
        /// Code: logic described in language-x syntax (.x).
        /// </summary>
        Code,
    }

    /// <summary>
    /// Holds all information about a language-x project.
    /// </summary>
    public class ProjectSettings
    {
        #region Project Properties
        /// <summary>
        /// The title of the project (AlphaNumeric/starts with a letter).
        /// </summary>
        private string title;

        /// <summary>
        /// The physical location this project was loaded from.
        /// </summary>
        private string location;

        /// <summary>
        /// Names of project's sprite files.
        /// </summary>
        private List<string> spriteFiles = new List<string>();

        /// <summary>
        /// Names of project's sound files.
        /// </summary>
        private List<string> soundFiles = new List<string>();

        /// <summary>
        /// Names of project's code files.
        /// </summary>
        private List<string> codeFiles = new List<string>();
        #endregion

        #region Constructors
        /// <summary>
        /// Prevents a default instance of the ProjectSettings class from being created.
        /// </summary>
        public ProjectSettings()
        {
            this.spriteFiles = new List<string>();
            this.soundFiles = new List<string>();
            this.codeFiles = new List<string>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the title of the project (AlphaNumeric/starts with a letter).
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        /// <summary>
        /// Gets the physical location this project was loaded from.
        /// </summary>
        public string Location
        {
            get { return this.location; }
            set { this.location = value; }
        }

        /// <summary>
        /// Gets the names of project's sprite files.
        /// </summary>
        public List<string> SpriteFiles
        {
            get { return this.spriteFiles; }
            set { this.spriteFiles = value; }
        }

        /// <summary>
        /// Gets the names of project's sound files.
        /// </summary>
        public List<string> SoundFiles
        {
            get { return this.soundFiles; }
            set { this.soundFiles = value; }
        }

        /// <summary>
        /// Gets the names of project's code files.
        /// </summary>
        public List<string> CodeFiles
        {
            get { return this.codeFiles; }
            set { this.codeFiles = value; }
        }

        public string CodeLocation
        {
            get { return this.location + "\\" + this.Title + "\\" + ProjectResourceType.Code.ToString(); }
        }

        public string SpritesLocation
        {
            get { return this.location + "\\" + this.Title + "\\" + ProjectResourceType.Sprite.ToString(); }
        }

        public string SoundsLocation
        {
            get { return this.location + "\\" + this.Title + "\\" + ProjectResourceType.Sound.ToString(); }
        }

        #endregion

        #region Load/Create/Save
        /// <summary>
        /// Loads a new project from a location and returns it.
        /// </summary>
        /// <param name="location">Location to load from.</param>
        /// <returns>A ProjectSettings object.</returns>
        public static ProjectSettings LoadProject(string location)
        {
            FileStream fileStream = new FileStream(location, FileMode.Open);

            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(ProjectSettings));
                return (ProjectSettings)xml.Deserialize(fileStream);
            }
            catch
            {
                throw new Exception("Error loading file settings.");
            }
            finally
            {
                fileStream.Close();
            }
        }

        /// <summary>
        /// Creates a new project from scratch and returns it.
        /// </summary>
        /// <param name="location">Location to create the project into.</param>
        /// <param name="title">Title of the project.</param>
        /// <returns>A ProjectSettings object.</returns>
        public static ProjectSettings CreateProject(string location, string title)
        {
            ProjectSettings project = new ProjectSettings();

            project.title = title;
            project.location = location;

            string folder = Path.GetDirectoryName(location);

            Directory.CreateDirectory(folder + "\\" + ProjectResourceType.Code.ToString());
            Directory.CreateDirectory(folder + "\\" + ProjectResourceType.Sprite.ToString());
            Directory.CreateDirectory(folder + "\\" + ProjectResourceType.Sound.ToString());

            project.SaveProject();
            return project;
        }

        /// <summary>
        /// Saves a project in it's location on disk.
        /// </summary>
        public void SaveProject()
        {
            FileStream fileStream = new FileStream(this.location, FileMode.OpenOrCreate);
            fileStream.SetLength(0);
            fileStream.Flush();

            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(ProjectSettings));
                xml.Serialize(fileStream, this);
            }
            catch
            {
                throw new Exception("Couldn't save file settings.");
            }
            finally
            {
                fileStream.Close();
            }
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Generates the physical path for a project resource file.
        /// </summary>
        /// <param name="name">Name of the file.</param>
        /// <param name="type">Type of resource.</param>
        /// <returns>The full physical path of this file on disk</returns>
        public string GetFullPath(string name, ProjectResourceType type)
        {
            return string.Format(
                "{0}\\{1}\\{2}",
                Path.GetDirectoryName(this.location),
                type.ToString(),
                name);
        }

        /// <summary>
        /// Loads a code file from disk
        /// </summary>
        /// <param name="name">Name of the file.</param>
        /// <returns>A string object.</returns>
        public string LoadCodeFile(string name)
        {
            return File.ReadAllText(this.GetFullPath(name, ProjectResourceType.Code));
        }

        /// <summary>
        /// Loads a sprite file from disk
        /// </summary>
        /// <param name="name">Name of the file.</param>
        /// <returns>A BitmapImage object.</returns>
        public BitmapImage LoadSpriteFile(string name)
        {
            return new BitmapImage(new Uri(this.GetFullPath(name, ProjectResourceType.Sprite), UriKind.Absolute));
        }

        /// <summary>
        /// Loads a sound file from disk
        /// </summary>
        /// <param name="name">Name of the file.</param>
        /// <returns>A SoundPlayer object.</returns>
        public SoundPlayer LoadSoundFile(string name)
        {
            return new SoundPlayer(this.GetFullPath(name, ProjectResourceType.Sound));
        }
        #endregion
    }
}