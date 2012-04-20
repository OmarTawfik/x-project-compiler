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
        /// Gets the if bodies of the if statement.
        /// </summary>
        public List<IfBody> Bodies
        {
            get { return this.bodies; }     
        }

        /// <summary>
        /// Gets the else block for the if statement.
        /// </summary>
        public Block ElseBody
        {
            get { return this.elseBody; }
        }

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

            this.StartLocation = this.bodies[0].StartLocation;
            this.EndLocation = (this.elseBody == null)
                ? this.bodies[this.bodies.Count - 1].EndLocation
                : this.elseBody.EndLocation;
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