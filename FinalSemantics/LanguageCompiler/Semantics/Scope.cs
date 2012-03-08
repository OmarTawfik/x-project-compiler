namespace LanguageCompiler.Semantics
{
    using System.Collections.Generic;

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
        /// Variables declared in this scope.
        /// </summary>
        private List<Variable> variables = new List<Variable>();

        /// <summary>
        /// Initializes a new instance of the Scope class.
        /// </summary>
        /// <param name="type">Type of this scope.</param>
        public Scope(ScopeType type)
        {
            this.type = type;
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
    }
}
