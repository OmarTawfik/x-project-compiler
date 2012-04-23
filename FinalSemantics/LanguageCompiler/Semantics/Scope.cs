namespace LanguageCompiler.Semantics
{
    using System.Collections.Generic;
    using LanguageCompiler.Nodes;

    /// <summary>
    /// Holds all types a scope can have.
    /// </summary>
    public enum ScopeType
    {
        /// <summary>
        /// A class scope.
        /// </summary>
        Class,

        /// <summary>
        /// A function scope.
        /// </summary>
        Function,

        /// <summary>
        /// A block scope.
        /// </summary>
        Block,

        /// <summary>
        /// A loop scope.
        /// </summary>
        Loop,

        /// <summary>
        /// An expression scope.
        /// </summary>
        Expression,
    }

    /// <summary>
    /// Represents a scope in the scope stack.
    /// </summary>
    public class Scope
    {
        /// <summary>
        /// Type of this scope.
        /// </summary>
        private ScopeType type;

        /// <summary>
        /// The node this scope was defined in.
        /// </summary>
        private BaseNode node;

        /// <summary>
        /// Variables declared in this scope.
        /// </summary>
        private List<Variable> variables = new List<Variable>();

        /// <summary>
        /// Initializes a new instance of the Scope class.
        /// </summary>
        /// <param name="type">Type of this scope.</param>
        /// <param name="node">The node this scope was defined in.</param>
        public Scope(ScopeType type, BaseNode node)
        {
            this.type = type;
            this.node = node;
        }

        /// <summary>
        /// Gets the type of this scope.
        /// </summary>
        public ScopeType Type
        {
            get { return this.type; }
        }

        /// <summary>
        /// Gets the variables declared in this scope.
        /// </summary>
        public List<Variable> Variables
        {
            get { return this.variables; }
        }

        /// <summary>
        /// Gets the node this scope was defined in.
        /// </summary>
        public BaseNode Node
        {
            get { return this.node; }
        }
    }
}