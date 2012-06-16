namespace LanguageTranslator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// Holds info about a backend class (YEAH! :D :D). WEEEEEEEEEEW!!! :D.
    /// </summary>
    public class BackendClass
    {
        /// <summary>
        /// Gets or sets the name of the backend class.
        /// </summary>
        public string Classname { get; set; }

        /// <summary>
        /// Gets or sets the platform this class resides on.
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// Gets or sets the language this class was implemented in.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the targeted language code definition.
        /// </summary>
        public string CodeDefinition { get; set; }

        /// <summary>
        /// Gets or sets the targeted language code declaration.
        /// </summary>
        public string CodeDeclaration { get; set; }

        /// <summary>
        /// Gets or sets the abstract Xlang class code.
        /// </summary>
        public string XlangCode { get; set; }

        /// <summary>
        /// Gets or sets the header files this class uses.
        /// </summary>
        public List<string> Headers { get; set; }

        /// <summary>
        /// Gets or sets the libraries this class uses.
        /// </summary>
        public List<string> Libraries { get; set; }

        /// <summary>
        /// Gets or sets the binaries this class uses.
        /// </summary>
        public List<string> Binaries { get; set; }

        /// <summary>
        /// Loads a backend class from a file.
        /// </summary>
        /// <param name="filepath">The fiule path of the backend class.</param>
        /// <returns>Backend class object.</returns>
        public static BackendClass LoadBackendClassFromFile(string filepath)
        {
            XmlSerializer xml = new XmlSerializer(new BackendClass().GetType());
            FileStream file = new FileStream(filepath, FileMode.Open);
            BackendClass loadedClass = (BackendClass)xml.Deserialize(file);
            file.Close();
            return loadedClass;
        }
    }
}
