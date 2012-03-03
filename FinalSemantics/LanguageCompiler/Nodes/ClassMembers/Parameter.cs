namespace LanguageCompiler.Nodes.ClassMembers
{
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Nodes.Types;

    /// <summary>
    /// Holds all data related to a "Parameter" rule.
    /// </summary>
    public class Parameter : MemberDefinition
    {
        /// <summary>
        /// Type of this parameter.
        /// </summary>
        private BaseNode type;

        /// <summary>
        /// Name of this parameter.
        /// </summary>
        private Identifier name;

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            TreeNode result = new TreeNode("Parameter");
            result.Nodes.Add(this.name.GetGUINode());
            result.Nodes.Add(this.type.GetGUINode());
            return result;
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            if (node.ChildNodes[0].Term.Name == LanguageGrammar.ID.Name)
            {
                this.type = new Identifier();
                this.type.RecieveData(node.ChildNodes[0]);
            }
            else if (node.ChildNodes[0].Term.Name == LanguageGrammar.ArrayType.Name)
            {
                this.type = new ArrayType();
                this.type.RecieveData(node.ChildNodes[0]);
            }

            this.name = new Identifier();
            this.name.RecieveData(node.ChildNodes[1]);

            this.StartLocation = this.type.StartLocation;
            this.EndLocation = this.name.EndLocation;
        }
    }
}
