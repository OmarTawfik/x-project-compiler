namespace LanguageCompiler.Nodes.Statements.CommandStatements
{
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Errors;
    using LanguageCompiler.Semantics;

    /// <summary>
    /// Holds all data related to a "BreakStatement" rule.
    /// </summary>
    public class BreakStatement : BaseNode
    {
        /// <summary>
        /// Initializes a new instance of the BreakStatement class.
        /// </summary>
        /// <param name="start">Starting location of node.</param>
        /// <param name="end">Ending location of node.</param>
        public BreakStatement(SourceLocation start, SourceLocation end)
        {
            this.StartLocation = start;
            this.EndLocation = end;
        }

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            return new TreeNode("Break Statement");
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
        }

        /// <summary>
        /// Checks for semantic errors within this node.
        /// </summary>
        /// <param name="scopeStack">The scope stack associated with this node.</param>
        /// <returns>True if errors are found, false otherwise.</returns>
        public override bool CheckSemanticErrors(ScopeStack scopeStack)
        {
            if (scopeStack.CheckParentScopes(ScopeType.Loop) == false)
            {
                this.AddError(ErrorType.StatementMustAppearInLoop);
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}