namespace LanguageTranslator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using LanguageCompiler.Nodes.TopLevel;
    using LanguageTranslator;

    /// <summary>   
    /// the primary interface for any translator plugin.
    /// </summary>
    public abstract class TranslatorPlugin
    {
        /// <summary>
        /// Generated code.
        /// </summary>
        private StringBuilder code;

        /// <summary>
        /// Holds the tabs string.
        /// </summary>
        private string tabs;

        /// <summary>
        /// All backend classes.
        /// </summary>
        private List<BackendClass> backendClasses;

        /// <summary>
        /// Gets the targeted platform ID.
        /// </summary>
        public abstract string PlatformID
        {
            get;
        }

        /// <summary>
        /// Gets the targeted language.
        /// </summary>
        public abstract string Language
        {
            get;
        }

        /// <summary>
        /// Gets the compiler that will build to the targeted platform.
        /// </summary>
        public abstract string Compiler
        {
            get;
        }

        /// <summary>
        /// Gets the string builder for the generated code.
        /// </summary>
        public StringBuilder GeneratedCode
        {
            get { return this.code; }
        }

        /// <summary>
        /// Gets or sets the backend classes.
        /// </summary>
        protected List<BackendClass> BackendClasses
        {
            get { return this.backendClasses; }
            set { this.backendClasses = value; }
        }

        /// <summary>
        /// Translates to the targeted platform.
        /// </summary>
        /// <param name="classes">Classes to be translated.</param>
        public abstract void Translate(List<ClassDefinition> classes);

        /// <summary>
        /// Builds the generated code.
        /// </summary>
        public abstract void Build();

        /// <summary>
        /// Runs the built application.
        /// </summary>
        public abstract void Run();

        /// <summary>
        /// Resets all generated code to start traslation.
        /// </summary>
        protected void ResetGeneratedCode()
        {
            this.code = new StringBuilder();
            this.tabs = string.Empty;
        }

        /// <summary>
        /// Starts a block of code.
        /// </summary>
        protected void StartBlock()
        {
            this.code.Append(this.tabs + "{" + Environment.NewLine);
            this.tabs = this.tabs + "\t";
        }

        /// <summary>
        /// Ends a block of code.
        /// </summary>
        protected void EndBlock()
        {
            this.tabs = this.tabs.Substring(1);
            this.code.Append(this.tabs + "}" + Environment.NewLine);
        }

        /// <summary>
        /// Appened the given string to the code.
        /// </summary>
        /// <param name="str">The string to append.</param>
        protected void Append(string str)
        {
            this.code.Append(str);
        }

        /// <summary>
        /// Appends the given string in a line.
        /// </summary>
        /// <param name="str">The string to append.</param>
        protected void AppendLine(string str)
        {
            this.code.Append(this.tabs + str + Environment.NewLine);
        }

        /// <summary>
        /// Starts a new line of code.
        /// </summary>
        protected void StartLine()
        {
            this.code.Append(this.tabs);
        }

        /// <summary>
        /// Ends a line of code.
        /// </summary>
        protected void EndLine()
        {
            this.code.Append(Environment.NewLine);
        }
    }
}
