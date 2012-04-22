namespace LanguageCompiler.Nodes
{
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Errors;
    using LanguageCompiler.Semantics;

    /// <summary>
    /// A base for all listed compiler nodes.
    /// </summary>
    public abstract class BaseNode
    {
        /// <summary>
        /// Start location of this node in input stream.
        /// </summary>
        private SourceLocation startLocation;

        /// <summary>
        /// End location of this node in input stream.
        /// </summary>
        private SourceLocation endLocation;

        /// <summary>
        /// Gets or sets the start location of this node in input stream.
        /// </summary>
        public SourceLocation StartLocation
        {
            get { return this.startLocation; }
            set { this.startLocation = value; }
        }

        /// <summary>
        /// Gets or sets the end location of this node in input stream.
        /// </summary>
        public SourceLocation EndLocation
        {
            get { return this.endLocation; }
            set { this.endLocation = value; }
        }

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public abstract TreeNode GetGUINode();

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public abstract void RecieveData(ParseTreeNode node);

        /// <summary>
        /// Checks for semantic errors within this node.
        /// </summary>
        /// <param name="scopeStack">The scope stack associated with this node.</param>
        /// <returns>True if errors are found, false otherwise.</returns>
        public virtual bool CheckSemanticErrors(ScopeStack scopeStack)
        {
            return false;
        }

        /// <summary>
        /// Checks if a statement or block of code returns a value.
        /// </summary>
        /// <returns>True if it returns a value, false otherwise.</returns>
        public abstract bool ReturnsAValue();

        /// <summary>
        /// A short hand to be used for adding errors.
        /// </summary>
        /// <param name="type">Type of error found.</param>
        /// <param name="parameters">Parameters given for form the error string.</param>
        protected void AddError(ErrorType type, params string[] parameters)
        {
            CompilerService.Instance.Errors.Add(ErrorsFactory.SemanticError(type, this, parameters));
        }
    }
}