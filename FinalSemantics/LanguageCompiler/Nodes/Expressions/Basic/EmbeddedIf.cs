namespace LanguageCompiler.Nodes.Expressions.Basic
{
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Errors;
    using LanguageCompiler.Nodes.Types;
    using LanguageCompiler.Semantics;

    /// <summary>
    /// Holds all data related to a "EmbeddedIf" rule.
    /// </summary>
    public class EmbeddedIf : BaseNode
    {
        /// <summary>
        /// Condition of this embedded if.
        /// </summary>
        private BaseNode condition;

        /// <summary>
        /// Expression to return if condition is true.
        /// </summary>
        private BaseNode trueChoice;

        /// <summary>
        /// Expression to return if condition is false.
        /// </summary>
        private BaseNode falseChoice;

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            TreeNode result = new TreeNode("Embedded If");
            result.Nodes.Add(this.condition.GetGUINode());
            result.Nodes.Add(this.trueChoice.GetGUINode());
            result.Nodes.Add(this.falseChoice.GetGUINode());
            return result;
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            this.condition = ExpressionsFactory.GetBinaryExpr(node.ChildNodes[0]);
            this.trueChoice = ExpressionsFactory.GetBaseExpr(node.ChildNodes[1].ChildNodes[1]);
            this.falseChoice = ExpressionsFactory.GetBaseExpr(node.ChildNodes[1].ChildNodes[3]);

            this.StartLocation = this.condition.StartLocation;
            this.EndLocation = node.ChildNodes[1].ChildNodes[4].Token.Location;
        }

        /// <summary>
        /// Checks for semantic errors within this node.
        /// </summary>
        /// <param name="scopeStack">The scope stack associated with this node.</param>
        /// <returns>True if errors are found, false otherwise.</returns>
        public override bool CheckSemanticErrors(ScopeStack scopeStack)
        {
            bool foundErrors = false;

            foundErrors |= this.condition.CheckSemanticErrors(scopeStack);
            foundErrors |= this.trueChoice.CheckSemanticErrors(scopeStack);
            foundErrors |= this.falseChoice.CheckSemanticErrors(scopeStack);

            if (!foundErrors)
            {
                if (this.condition.GetDataType() != Literal.Bool)
                {
                    this.AddError(ErrorType.ExpressionNotBoolean);
                    foundErrors = true;
                }

                if (this.trueChoice.GetDataType() != this.falseChoice.GetDataType())
                {
                    this.AddError(ErrorType.EmbeddedIfTypeMismatch);
                    foundErrors = true;
                }
            }

            return foundErrors;
        }

        /// <summary>
        /// Gets the type of this expression.
        /// </summary>
        /// <returns>A string representing the name of the type.</returns>
        public override string GetDataType()
        {
            return this.trueChoice.GetDataType();
        }
    }
}
