namespace LanguageCompiler.Nodes.Expressions.Basic
{
    using System;
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Errors;
    using LanguageCompiler.Nodes.ClassMembers;
    using LanguageCompiler.Nodes.TopLevel;
    using LanguageCompiler.Semantics;
    using LanguageCompiler.Semantics.ExpressionTypes;

    /// <summary>
    /// Holds all data related to a "PostfixExpression" rule.
    /// </summary>
    public class PostfixExpression : ExpressionNode
    {
        /// <summary>
        /// Operator of expression.
        /// </summary>
        private string operatorDefined;

        /// <summary>
        /// LHS of expression.
        /// </summary>
        private ExpressionNode lhs;

        /// <summary>
        /// Gets the left hand side of the postfix expression.
        /// </summary>
        public ExpressionNode LHS
        {
            get { return this.lhs; }
        }

        /// <summary>
        /// Gets the operator of expression.
        /// </summary>
        public string OperatorDefined
        {
            get { return this.operatorDefined; }
        }

        /// <summary>
        /// Forms a valid tree node representing this object.
        /// </summary>
        /// <returns>The formed tree node.</returns>
        public override TreeNode GetGUINode()
        {
            TreeNode result = new TreeNode(this.operatorDefined + " Postfix Expression");
            result.Nodes.Add(this.lhs.GetGUINode());
            return result;
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            this.lhs = ExpressionsFactory.GetPrimaryExpr(node.ChildNodes[0]);
            this.operatorDefined = node.ChildNodes[1].Token.Text;

            this.StartLocation = this.lhs.StartLocation;
            this.EndLocation = node.ChildNodes[1].Token.Location;
        }

        /// <summary>
        /// Gets the expression type of this node.
        /// </summary>
        /// <param name="stack">Current Scope Stack.</param>
        /// <returns>The expression type of this node.</returns>
        public override ExpressionType GetExpressionType(ScopeStack stack)
        {
            ClassDefinition lhsType = (this.lhs.GetExpressionType(stack) as ObjectExpressionType).DataType;

            foreach (MemberDefinition member in lhsType.Members)
            {
                if (member is OperatorDefinition)
                {
                    OperatorDefinition op = member as OperatorDefinition;
                    if (op.OperatorDefined == this.operatorDefined)
                    {
                        return this.ExpressionType = op.Type.GetExpressionType(stack);
                    }
                }
            }

            throw new Exception("Faulty Type. Should be handled in semantics.");
        }

        /// <summary>
        /// Checks if this expression can be assigned to.
        /// </summary>
        /// <returns>True if this expression can be assigned to, false otherwise.</returns>
        public override bool IsAssignable()
        {
            return true;
        }

        /// <summary>
        /// Checks for semantic errors within this node.
        /// </summary>
        /// <param name="scopeStack">The scope stack associated with this node.</param>
        /// <returns>True if errors are found, false otherwise.</returns>
        public override bool CheckSemanticErrors(ScopeStack scopeStack)
        {
            if (this.lhs.IsAssignable() == false)
            {
                this.AddError(ErrorType.PostfixOnTemp);
                return true;
            }

            if (this.lhs.CheckSemanticErrors(scopeStack))
            {
                return true;
            }

            ClassDefinition lhsType = (this.lhs.GetExpressionType(scopeStack) as ObjectExpressionType).DataType;
            foreach (MemberDefinition member in lhsType.Members)
            {
                if (member is OperatorDefinition)
                {
                    OperatorDefinition op = member as OperatorDefinition;
                    if (op.OperatorDefined == this.operatorDefined)
                    {
                        return false;
                    }
                }
            }

            this.AddError(ErrorType.NotAValidLHS, this.operatorDefined);
            return true;
        }
    }
}