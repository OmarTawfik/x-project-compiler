namespace LanguageCompiler.Nodes.Expressions.Complex
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Irony.Parsing;
    using LanguageCompiler.Errors;
    using LanguageCompiler.Nodes.ClassMembers;
    using LanguageCompiler.Semantics;
    using LanguageCompiler.Semantics.ExpressionTypes;

    /// <summary>
    /// Holds all data related to a "InvocationExpression" rule.
    /// </summary>
    public class InvocationExpression : ExpressionNode
    {
        /// <summary>
        /// Expression in LHS.
        /// </summary>
        private ExpressionNode lhs;

        /// <summary>
        /// Arguments of expression.
        /// </summary>
        private List<ExpressionNode> arguments = new List<ExpressionNode>();

        /// <summary>
        /// Gets the left hand side of the expression.
        /// </summary>
        public ExpressionNode LHS
        {
            get { return this.lhs; }
        }

        /// <summary>
        /// Gets the list of arguments of invocation.
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
            TreeNode result = new TreeNode("Invocation Expression");
            result.Nodes.Add(this.lhs.GetGUINode());
            TreeNode args = new TreeNode("Arguments: Count = " + this.arguments.Count);
            foreach (BaseNode child in this.arguments)
            {
                args.Nodes.Add(child.GetGUINode());
            }

            result.Nodes.Add(args);
            return result;
        }

        /// <summary>
        /// Recieves an irony ParseTreeNode and constructs its contents.
        /// </summary>
        /// <param name="node">The irony ParseTreeNode.</param>
        public override void RecieveData(ParseTreeNode node)
        {
            this.lhs = ExpressionsFactory.GetPrimaryExpr(node.ChildNodes[0]);

            foreach (ParseTreeNode child in node.ChildNodes[2].ChildNodes)
            {
                this.arguments.Add(ExpressionsFactory.GetBaseExpr(child));
            }

            this.StartLocation = this.lhs.StartLocation;
            this.EndLocation = node.ChildNodes[3].Token.Location;
        }

        /// <summary>
        /// Checks for semantic errors within this node.
        /// </summary>
        /// <param name="scopeStack">The scope stack associated with this node.</param>
        /// <returns>True if errors are found, false otherwise.</returns>
        public override bool CheckSemanticErrors(ScopeStack scopeStack)
        {
            if (this.lhs.CheckSemanticErrors(scopeStack) == false)
            {
                foreach (ExpressionNode arg in this.arguments)
                {
                    if (arg.CheckSemanticErrors(scopeStack))
                    {
                        return true;
                    }
                }

                ExpressionType lhsType = this.lhs.GetExpressionType(scopeStack);
                if (lhsType is MethodExpressionType)
                {
                    MethodDefinition lhsMethod = (lhsType as MethodExpressionType).Method;
                    if (lhsMethod.Parameters.Count == this.arguments.Count)
                    {
                        for (int i = 0; i < this.arguments.Count; i++)
                        {
                            ExpressionType argType = this.arguments[i].GetExpressionType(scopeStack);
                            this.arguments[i].ExpressionType = argType;

                            if (argType is ObjectExpressionType)
                            {
                                ObjectExpressionType argObject = argType as ObjectExpressionType;
                                if (argObject.DataType.Name.Text != lhsMethod.Parameters[i].Type.Text)
                                {
                                    this.AddError(ErrorType.InvalidParameterType, lhsMethod.Name.Text, argObject.DataType.Name.Text);
                                    return true;
                                }
                            }
                            else
                            {
                                this.AddError(ErrorType.InvalidParameterType, lhsMethod.Name.Text, (argType as MethodExpressionType).Method.Name.Text);
                                return true;
                            }
                        }

                        return false;
                    }
                    else
                    {
                        this.AddError(ErrorType.WrongNumberOfParameters, lhsMethod.Name.Text, this.arguments.Count.ToString());
                        return true;
                    }
                }
                else
                {
                    this.AddError(ErrorType.LHSNotAFunction);
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the expression type of this node.
        /// </summary>
        /// <param name="stack">Current Scope Stack.</param>
        /// <returns>The expression type of this node.</returns>
        public override ExpressionType GetExpressionType(ScopeStack stack)
        {
            return this.ExpressionType =
                (this.lhs.GetExpressionType(stack) as MethodExpressionType)
                .Method.Type.GetExpressionType(stack);
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