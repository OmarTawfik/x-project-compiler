namespace LanguageCompiler
{
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
        /// Gets all class definitions parsed in project.
        /// </summary>
        public Dictionary<string, ClassDefinition> ClassesList
        {
            get { return this.classesList; }
        }

        /// <summary>
        /// Parses a stream of text for building a valid parse tree.
        /// </summary>
        /// <param name="text">Text stream to parse.</param>
        public void ParseFile(string text)
        {
            ParseTree tree = this.parser.Parse(text);
            foreach (LogMessage message in tree.ParserMessages)
            {
                this.errors.Add(ErrorsFactory.SyntaxError(message));
            }

            if (tree.ParserMessages.Count == 0)
            {
                foreach (ParseTreeNode child in tree.Root.ChildNodes)
                {
                    ClassDefinition classDefinition = new ClassDefinition();
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
        }
    }
}
