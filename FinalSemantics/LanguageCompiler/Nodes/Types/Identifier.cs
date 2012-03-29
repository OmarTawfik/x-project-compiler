namespace LanguageCompiler.Nodes.Types
{
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Errors;
    using LanguageCompiler.Nodes.ClassMembers;
    using LanguageCompiler.Nodes.Expressions;
    using LanguageCompiler.Semantics;
    using LanguageCompiler.Semantics.ExpressionTypes;

    /// <summary>
    /// Holds all data related to a "Identifier" rule.
    /// </summary>
    public class Identifier : ExpressionNode
    {
        /// <summary>
        /// The text of this identifier.
        /// </summary>
        private string text;

        /// <summary>
        /// Gets the text of this identifier.
        /// </summary>
        public string Text
        {
            get { return this.text; }
        }

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            return new TreeNode("Identifier = " + this.text);
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            this.text = node.Token.Text;
            this.StartLocation = node.Token.Location;
            this.EndLocation = node.Token.Location;
        }

        /// <summary>
        /// Checks if this type exists in the parsed list.
        /// </summary>
        /// <param name="reportError">If true, the function reports an error if type isn't found.</param>
        /// <returns>True if this type exists, false otherwise.</returns>
        public bool CheckTypeExists(bool reportError = true)
        {
            if (CompilerService.Instance.ClassesList.ContainsKey(this.text))
            {
                return true;
            }
            else if (reportError)
            {
                this.AddError(ErrorType.TypeNotFound, this.text);
            }

            return false;
        }

        /// <summary>
        /// Checks for semantic errors within this node.
        /// </summary>
        /// <param name="scopeStack">The scope stack associated with this node.</param>
        /// <returns>True if errors are found, false otherwise.</returns>
        public override bool CheckSemanticErrors(ScopeStack scopeStack)
        {
            if (LanguageGrammar.ReservedWords.Contains(this.text))
            {
                this.AddError(ErrorType.IdentifierIsReservedWord, this.text);
                return true;
            }

            if (this.text.Length > 50)
            {
                this.AddError(ErrorType.IdentifierNameTooLong, this.text);
                return true;
            }

            if (this.text.Contains("_"))
            {
                this.AddError(ErrorType.UnderscoreInIdentifier, this.text);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the expression type of this node.
        /// </summary>
        /// <param name="stack">Current Scope Stack.</param>
        /// <returns>The expression type of this node.</returns>
        public override ExpressionType GetExpressionType(ScopeStack stack)
        {
            if (this.CheckTypeExists(false))
            {
                return new ObjectExpressionType(CompilerService.Instance.ClassesList[this.text], MemberStaticType.Static);
            }
            else
            {
                return stack.GetVariable(this.text).Type;
            }
        }

        /// <summary>
        /// Checks if this expression can be assigned to.
        /// </summary>
        /// <returns>True if this expression can be assigned to, false otherwise.</returns>
        public override bool IsAssignable()
        {
            return true;
        }
    }
}