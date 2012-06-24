namespace LanguageCompiler
{
    using System;
    using System.Collections.Generic;
    using Irony;
    using Irony.Parsing;
    using LanguageCompiler.Errors;
    using LanguageCompiler.Nodes.TopLevel;
    using LanguageCompiler.Semantics;

    /// <summary>
    /// The Service class for building a valid project.
    /// </summary>
    public class CompilerService
    {
        /// <summary>
        /// The instance used in this singleton.
        /// </summary>
        private static CompilerService instance;

        /// <summary>
        /// List of errors found in input.
        /// </summary>
        private List<CompilerError> errors = new List<CompilerError>();

        /// <summary>
        /// List of all templates in input.
        /// </summary>
        private List<KeyValuePair<string, string>> listTemplates = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// The name of the currently parsed file.
        /// </summary>
        private string currentFile;

        /// <summary>
        /// Irony parser object to use in parsing.
        /// </summary>
        private Parser parser = new Parser(new LanguageGrammar());

        /// <summary>
        /// Holds all class definitions parsed in project.
        /// </summary>
        private Dictionary<string, ClassDefinition> classesList = new Dictionary<string, ClassDefinition>();

        /// <summary>
        /// Prevents a default instance of the CompilerService class from being created.
        /// </summary>
        private CompilerService()
        {
        }

        /// <summary>
        /// Gets the instance used in this singleton.
        /// </summary>
        public static CompilerService Instance
        {
            get
            {
                if (CompilerService.instance == null)
                {
                    CompilerService.instance = new CompilerService();
                }

                return CompilerService.instance;
            }
        }

        /// <summary>
        /// Gets the list of errors found in input.
        /// </summary>
        public List<CompilerError> Errors
        {
            get { return this.errors; }
        }

        /// <summary>
        /// Gets the list of templates found in input.
        /// </summary>
        public List<KeyValuePair<string, string>> ListTemplates
        {
            get { return this.listTemplates; }
        }

        /// <summary>
        /// Gets all class definitions parsed in project.
        /// </summary>
        public Dictionary<string, ClassDefinition> ClassesList
        {
            get { return this.classesList; }
        }

        /// <summary>
        /// Gets the name of the currently parsed file.
        /// </summary>
        public string CurrentFile
        {
            get { return this.currentFile; }
        }

        /// <summary>
        /// Parses a stream of text for building a valid parse tree.
        /// </summary>
        /// <param name="text">Text stream to parse.</param>
        /// <param name="fileName">The name of the file this class was declared in.</param>
        public void ParseFile(string text, string fileName)
        {
            this.currentFile = fileName;
            ParseTree tree = this.parser.Parse(text);
            foreach (LogMessage message in tree.ParserMessages)
            {
                this.errors.Add(ErrorsFactory.SyntaxError(message));
            }

            if (tree.ParserMessages.Count == 0)
            {
                foreach (ParseTreeNode child in tree.Root.ChildNodes)
                {
                    ClassDefinition classDefinition = new ClassDefinition(fileName);
                    classDefinition.RecieveData(child);
                    if (this.classesList.ContainsKey(classDefinition.Name.Text))
                    {
                        this.errors.Add(ErrorsFactory.SemanticError(ErrorType.MultipleTypesWithSameName, classDefinition, classDefinition.Name.Text));
                    }
                    else
                    {
                        this.classesList.Add(classDefinition.Name.Text, classDefinition);
                    }
                }
            }
        }

        /// <summary>
        /// Checks for semantic errors after parsing is finished and adds any respective errors.
        /// </summary>
        public void CheckSemantics()
        {
            foreach (KeyValuePair<string, string> listName in this.listTemplates)
            {
                try
                {
                    this.classesList.Add(listName.Key, ClassDefinition.GenerateList(listName.Key, listName.Value));
                }
                catch (Exception)
                {
                    ////Multiple list declaration with the same type.
                }
            }

            if (this.errors.Count == 0)
            {
                foreach (KeyValuePair<string, ClassDefinition> pair in this.classesList)
                {
                    pair.Value.CheckSemanticErrors(new ScopeStack());
                }
            }
        }

        /// <summary>
        /// Clears all previously collected data in this service.
        /// </summary>
        public void Clear()
        {
            this.errors.Clear();
            this.classesList.Clear();
            this.listTemplates.Clear();
        }
    }
}