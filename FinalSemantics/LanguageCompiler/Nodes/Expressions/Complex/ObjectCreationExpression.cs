namespace LanguageCompiler.Nodes.Expressions.Complex
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Errors;
    using LanguageCompiler.Nodes.ClassMembers;
    using LanguageCompiler.Nodes.TopLevel;
    using LanguageCompiler.Nodes.Types;
    using LanguageCompiler.Semantics;
    using LanguageCompiler.Semantics.ExpressionTypes;

    /// <summary>
    /// Holds all data related to a "ObjectCreationExpression" rule.
    /// </summary>
    public class ObjectCreationExpression : ExpressionNode
    {
        /// <summary>
        /// Type of object.
        /// </summary>
        private Identifier type;

        /// <summary>
        /// Arguments of constructor.
        /// </summary>
        private List<ExpressionNode> arguments = new List<ExpressionNode>();

        /// <summary>
        /// Gets the type of the object to create.
        /// </summary>
        public Identifier Type
        {
            get { return this.type; }
        }

        /// <summary>
        /// Gets the list of arguments for the constructor of the object creation statement.
        /// </summary>
        public List<ExpressionNode> Arguments
        {
            get { return this.arguments; }
        }

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            TreeNode args = new TreeNode("Constructor Arguments: Count = " + this.arguments.Count);
            foreach (BaseNode child in this.arguments)
            {
                args.Nodes.Add(child.GetGUINode());
            }

            TreeNode result = new TreeNode("Object Creation Expression");
            result.Nodes.Add(this.type.GetGUINode());
            result.Nodes.Add(args);
            return result;
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            this.type = new Identifier();
            this.type.RecieveData(node.ChildNodes[1]);

            foreach (ParseTreeNode child in node.ChildNodes[3].ChildNodes)
            {
                this.arguments.Add(ExpressionsFactory.GetBaseExpr(child));
            }

            this.StartLocation = node.ChildNodes[0].Token.Location;
            this.EndLocation = node.ChildNodes[4].Token.Location;
        }

        /// <summary>
        /// Checks for semantic errors within this node.
        /// </summary>
        /// <param name="scopeStack">The scope stack associated with this node.</param>
        /// <returns>True if errors are found, false otherwise.</returns>
        public override bool CheckSemanticErrors(ScopeStack scopeStack)
        {
            if (this.type.CheckTypeExists() == false)
            {
                return true;
            }

            bool foundErrors = false;
            foreach (ExpressionNode arg in this.arguments)
            {
                foundErrors |= arg.CheckSemanticErrors(scopeStack);
            }

            if (foundErrors)
            {
                return true;
            }

            ClassDefinition classObj = CompilerService.Instance.ClassesList[this.type.Text];
            bool foundHiddenDefaultCtor = false;

            foreach (MemberDefinition member in classObj.Members)
            {
                if (member is MethodDefinition)
                {
                    MethodDefinition method = member as MethodDefinition;
                    if (method.Name.Text == "constructor")
                    {
                        if ((method.AccessorType == MemberAccessorType.Protected || method.AccessorType == MemberAccessorType.Private)
                            && this.arguments.Count == 0 && method.Parameters.Count == 0)
                        {
                            foundHiddenDefaultCtor = true;
                        }
                        else if (method.Parameters.Count == this.arguments.Count)
                        {
                            bool correctCtor = true;
                            for (int i = 0; i < method.Parameters.Count; i++)
                            {
                                if (method.Parameters[i].Type.GetExpressionType(scopeStack)
                                    .IsEqualTo(this.arguments[i].GetExpressionType(scopeStack)) == false)
                                {
                                    correctCtor = false;
                                    break;
                                }
                            }

                            if (correctCtor)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            if (foundHiddenDefaultCtor)
            {
                this.AddError(ErrorType.NoSuitableConstructor, this.type.Text);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the expression type of this node.
        /// </summary>
        /// <param name="stack">Current Scope Stack.</param>
        /// <returns>The expression type of this node.</returns>
        public override ExpressionType GetExpressionType(ScopeStack stack)
        {
            return this.ExpressionType = this.type.GetExpressionType(stack);
        }

        /// <summary>
        /// Checks if this expression can be assigned to.
        /// </summary>
        /// <returns>True if this expression can be assigned to, false otherwise.</returns>
        public override bool IsAssignable()
        {
            return false;
        }
    }
}