namespace LanguageCompiler.Semantics
{
    using System.Collections.Generic;
    using LanguageCompiler.Errors;
    using LanguageCompiler.Nodes;

    /// <summary>
    /// baf asd asd.
    /// </summary>
    public class ScopeStack
    {
        /// <summary>
        /// Holds all variables defined within a stack of scopes.
        /// </summary>
        private Stack<List<Variable>> stack = new Stack<List<Variable>>();

        /// <summary>
        /// Declares a variable in this scope.
        /// </summary>
        /// <param name="v">Variable to be declared.</param>
        /// <param name="parent">Parent of this variable.</param>
        /// <returns>True if declaration is successful, false otherwise.</returns>
        public bool DeclareVariable(Variable v, BaseNode parent)
        {
            if (this.Containes(v) == false)
            {
                this.stack.Peek().Add(v);
                return true;
            }
            else
            {
                CompilerService.Instance.Errors.Add(ErrorsFactory.SemanticError(
                    ErrorType.ItemAlreadyDefined,
                    parent,
                    v.Name.Text));
                return false;
            }
        }

        /// <summary>
        /// Adds a new level to the stack.
        /// </summary>
        public void AddLevel()
        {
            this.stack.Push(new List<Variable>());
        }

        /// <summary>
        /// Deletes the last level added to the stack.
        /// </summary>
        public void DeleteLevel()
        {
            this.stack.Pop();
        }

        /// <summary>
        /// Checks if this stack already containes a variable.
        /// </summary>
        /// <param name="v">Variable to be checked.</param>
        /// <returns>True if the variable exists in the stack, false otherwise.</returns>
        public bool Containes(Variable v)
        {
            foreach (List<Variable> scope in this.stack)
            {
                if (scope.Contains(v))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
