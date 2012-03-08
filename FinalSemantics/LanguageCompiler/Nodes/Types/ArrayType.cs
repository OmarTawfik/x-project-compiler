namespace LanguageCompiler.Nodes.Types
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Irony.Parsing;
using LanguageCompiler.Semantics;

    /// <summary>
    /// Holds all data related to a "Array Type" rule.
    /// </summary>
    public class ArrayType : BaseNode
    {
        /// <summary>
        /// The type of this array.
        /// </summary>
        private Identifier type;

        /// <summary>
        /// Indexes of this array.
        /// </summary>
        private int indexes;

        /// <summary>
        /// Gets the type of this array.
        /// </summary>
        public Identifier Type
        {
            get { return this.type; }
        }

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            return new TreeNode("Array of type" + this.type.Text + ": Indexes = " + this.indexes);
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            this.type = new Identifier();
            this.type.RecieveData(node.ChildNodes[0]);
            this.indexes = node.ChildNodes[2].ChildNodes.Count + 1;

            this.StartLocation = this.type.StartLocation;
            this.EndLocation = node.ChildNodes[node.ChildNodes.Count - 1].Token.Location;
        }

        /// <summary>
        /// Checks for semantic errors within this node.
        /// </summary>
        /// <param name="scopeStack">The scope stack associated with this node.</param>
        /// <returns>True if errors are found, false otherwise.</returns>
        public override bool CheckSemanticErrors(ScopeStack scopeStack)
        {
            return this.type.CheckTypeExists() || this.type.CheckSemanticErrors(scopeStack);
        }
    }
}
