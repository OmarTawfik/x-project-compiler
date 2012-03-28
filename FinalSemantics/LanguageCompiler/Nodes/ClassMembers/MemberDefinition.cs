namespace LanguageCompiler.Nodes.ClassMembers
{
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Nodes.Expressions;
    using LanguageCompiler.Nodes.TopLevel;
    using LanguageCompiler.Nodes.Types;

    /// <summary>
    /// The member accessor type.
    /// </summary>
    public enum MemberAccessorType
    {
        /// <summary>
        /// Public: This member is visible to outside classes.
        /// </summary>
        Public,

        /// <summary>
        /// Private: This member is visible to this class only.
        /// </summary>
        Private,

        /// <summary>
        /// Protected: This member is visible to this class and child classes only.
        /// </summary>
        Protected,
    }

    /// <summary>
    /// The member modifier type.
    /// </summary>
    public enum MemberModifierType
    {
        /// <summary>
        /// Override: this member overrides a base class member with the same name.
        /// </summary>
        Override,

        /// <summary>
        /// Virtual: this member may be overridden in a child class.
        /// </summary>
        Virtual,

        /// <summary>
        /// Abstract: this member must be overridden in a child class.
        /// </summary>
        Abstract,

        /// <summary>
        /// Normal: This member doesn't override anything, and cannot be overridden.
        /// </summary>
        Normal,
    }

    /// <summary>
    /// The member static type.
    /// </summary>
    public enum MemberStaticType
    {
        /// <summary>
        /// Static: This member has one instance visible to all objects.
        /// </summary>
        Static,

        /// <summary>
        /// Normal: This member is instantinated with every object, and visible only to this object.
        /// </summary>
        Normal,
    }

    /// <summary>
    /// Holds all data related to a "Member Definition" rule.
    /// </summary>
    public abstract class MemberDefinition : BaseNode
    {
        /// <summary>
        /// Accessor type for this member.
        /// </summary>
        private MemberAccessorType accessorType = MemberAccessorType.Private;

        /// <summary>
        /// Modifier type for this member.
        /// </summary>
        private MemberModifierType modifierType = MemberModifierType.Normal;

        /// <summary>
        /// Static type for this member.
        /// </summary>
        private MemberStaticType staticType = MemberStaticType.Normal;

        /// <summary>
        /// Data type for this member.
        /// </summary>
        private ExpressionNode type;

        /// <summary>
        /// The class where this member was defined in.
        /// </summary>
        private ClassDefinition parent;

        /// <summary>
        /// Initializes a new instance of the MemberDefinition class.
        /// </summary>
        /// <param name="parent">The class where this member was defined in.</param>
        public MemberDefinition(ClassDefinition parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Gets the base node of the member type
        /// </summary>
        public ExpressionNode Type
        {
            get { return this.type; }
        }

        /// <summary>
        /// Gets the class where this member was defined in.
        /// </summary>
        public ClassDefinition Parent
        {
            get { return this.parent; }
        }

        /// <summary>
        /// Gets the modifier type for this member.
        /// </summary>
        public MemberModifierType ModifierType
        {
            get { return this.modifierType; }
        }

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            TreeNode result = new TreeNode();
            result.Nodes.Add("Accessor = " + this.accessorType.ToString());
            result.Nodes.Add("Modifier = " + this.modifierType.ToString());
            result.Nodes.Add("Static = " + this.staticType.ToString());
            result.Nodes.Add(this.type.GetGUINode());
            return result;
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            this.StartLocation = new SourceLocation(-1, -1, -1);

            if (node.ChildNodes[0].ChildNodes.Count > 0)
            {
                ParseTreeNode accessorNode = node.ChildNodes[0].ChildNodes[0];
                if (accessorNode.Token.Text == "public")
                {
                    this.accessorType = MemberAccessorType.Public;
                }
                else if (accessorNode.Token.Text == "protected")
                {
                    this.accessorType = MemberAccessorType.Protected;
                }
                else if (accessorNode.Token.Text == "private")
                {
                    this.accessorType = MemberAccessorType.Private;
                }

                if (this.StartLocation.Line == -1)
                {
                    this.StartLocation = node.ChildNodes[0].ChildNodes[0].Token.Location;
                }
            }

            if (node.ChildNodes[1].ChildNodes.Count > 0)
            {
                ParseTreeNode modifierNode = node.ChildNodes[1].ChildNodes[0];
                if (modifierNode.Token.Text == "override")
                {
                    this.modifierType = MemberModifierType.Override;
                }
                else if (modifierNode.Token.Text == "abstract")
                {
                    this.modifierType = MemberModifierType.Abstract;
                }
                else if (modifierNode.Token.Text == "virtual")
                {
                    this.modifierType = MemberModifierType.Virtual;
                }

                if (this.StartLocation.Line == -1)
                {
                    this.StartLocation = node.ChildNodes[1].ChildNodes[0].Token.Location;
                }
            }

            if (node.ChildNodes[2].ChildNodes.Count > 0)
            {
                this.staticType = MemberStaticType.Static;
                if (this.StartLocation.Line == -1)
                {
                    this.StartLocation = node.ChildNodes[2].ChildNodes[0].Token.Location;
                }
            }

            if (node.ChildNodes[3].Term.Name == LanguageGrammar.ID.Name)
            {
                this.type = new Identifier();
                this.type.RecieveData(node.ChildNodes[3]);
            }
            else if (node.ChildNodes[3].Term.Name == LanguageGrammar.ArrayType.Name)
            {
                this.type = new ArrayType();
                this.type.RecieveData(node.ChildNodes[3]);
            }

            if (this.StartLocation.Line == -1)
            {
                this.StartLocation = this.type.StartLocation;
            }
        }
    }
}