namespace LanguageCompiler.Nodes.Statements.ControlStatements
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Semantics;

    /// <summary>
    /// Holds all data related to a "IfStatement" rule.
    /// </summary>
    public class IfStatement : BaseNode
    {
        /// <summary>
        /// Bodies of this if statement.
        /// </summary>
        private List<IfBody> bodies = new List<IfBody>();

        /// <summary>
        /// The else body for this if statement.
        /// </summary>
        private Block elseBody;

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            TreeNode result = new TreeNode("If Statement");
            foreach (IfBody body in this.bodies)
            {
                result.Nodes.Add(body.GetGUINode());
            }

            if (this.elseBody != null)
            {
                result.Nodes.Add(this.elseBody.GetGUINode());
            }
            else
            {
                result.Nodes.Add("No Else!");
            }

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
                IfBody body = new IfBody();
                body.RecieveData(child);
                this.bodies.Add(body);
            }

            if (node.ChildNodes[1].ChildNodes.Count > 0)
            {
                this.elseBody = new Block();
                this.elseBody.RecieveData(node.ChildNodes[1].ChildNodes[1]);
            }

            this.StartLocation = node.ChildNodes[0].Token.Location;
            this.StartLocation = node.ChildNodes[node.ChildNodes.Count - 1].Token.Location;
        }

        /// <summary>
        /// Checks for semantic errors within this node.
        /// </summary>
        /// <param name="scopeStack">The scope stack associated with this node.</param>
        /// <returns>True if errors are found, false otherwise.</returns>
        public override bool CheckSemanticErrors(ScopeStack scopeStack)
        {
            bool foundErrors = false;

            foreach (IfBody body in this.bodies)
            {
                foundErrors |= body.CheckSemanticErrors(scopeStack);
            }

            if (this.elseBody != null)
            {
                foundErrors |= this.elseBody.CheckSemanticErrors(scopeStack);
            }

            return foundErrors;
        }
    }
}
