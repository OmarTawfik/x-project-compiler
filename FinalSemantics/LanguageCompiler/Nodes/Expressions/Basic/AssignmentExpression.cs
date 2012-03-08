namespace LanguageCompiler.Nodes.Expressions.Basic
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Nodes.Types;
    using LanguageCompiler.Semantics;

    /// <summary>
    /// Holds all data related to a "AssignmentExpression" rule.
    /// </summary>
    public class AssignmentExpression : BaseNode
    {
        /// <summary>
        /// LHS of expression.
        /// </summary>
        private List<Identifier> lhs = new List<Identifier>();

        /// <summary>
        /// Operator of expression.
        /// </summary>
        private string assignmentOperator;

        /// <summary>
        /// expression assigned to LHS.
        /// </summary>
        private BaseNode expression;

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            TreeNode result = new TreeNode("Assignment Expression");
            string lhsString = this.lhs[0].Text;
            for (int i = 1; i < this.lhs.Count; i++)
            {
                lhsString += "." + this.lhs[i].Text;
            }

            result.Nodes.Add("LHS: " + lhsString);
            result.Nodes.Add(new TreeNode(this.assignmentOperator));
            result.Nodes.Add(this.expression.GetGUINode());
            return result;
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            foreach (ParseTreeNode child in node.ChildNodes[0].ChildNodes)
            {
                Identifier id = new Identifier();
                id.RecieveData(child);
                this.lhs.Add(id);
            }

            this.assignmentOperator = node.ChildNodes[1].Token.Text;
            this.expression = ExpressionsFactory.GetBaseExpr(node.ChildNodes[2]);

            this.StartLocation = node.ChildNodes[0].ChildNodes[0].Token.Location;
            this.EndLocation = this.expression.EndLocation;
        }
    }
}
