namespace LanguageCompiler.Nodes.Expressions
{
    using Irony.Parsing;
    using LanguageCompiler.Nodes.Expressions.Basic;
    using LanguageCompiler.Nodes.Expressions.Complex;
    using LanguageCompiler.Nodes.Types;

    /// <summary>
    /// Contains functions to produce correct expression trees.
    /// </summary>
    public static class ExpressionsFactory
    {
        /// <summary>
        /// Resolves a base expression between assignment and embedded if.
        /// </summary>
        /// <param name="node">ParseTreeNode to resolve.</param>
        /// <returns>BaseNode Generated.</returns>
        public static BaseNode GetBaseExpr(ParseTreeNode node)
        {
            if (node.Term.Name == LanguageGrammar.AssignmentExpression.Name)
            {
                if (node.ChildNodes.Count == 1)
                {
                    return GetBaseExpr(node.ChildNodes[0]);
                }
                else
                {
                    BinaryExpression expr = new BinaryExpression();
                    expr.RecieveData(node);
                    return expr;
                }
            }
            else if (node.Term.Name == LanguageGrammar.EmbeddedIf.Name)
            {
                if (node.ChildNodes[1].ChildNodes.Count > 0)
                {
                    EmbeddedIf embeddefIf = new EmbeddedIf();
                    embeddefIf.RecieveData(node);
                    return embeddefIf;
                }
                else
                {
                    return GetBinaryExpr(node.ChildNodes[0]);
                }
            }
            else
            {
                return GetBinaryExpr(node);
            }
        }

        /// <summary>
        /// Resolves a binary expression from binary or unary expressions.
        /// </summary>
        /// <param name="node">ParseTreeNode to resolve.</param>
        /// <returns>BaseNode Generated.</returns>
        public static BaseNode GetBinaryExpr(ParseTreeNode node)
        {
            if (node.Term.Name == LanguageGrammar.PositiveExpression.Name)
            {
                return GetUnaryExpr(node);
            }
            else if (node.ChildNodes.Count == 1)
            {
                return GetBinaryExpr(node.ChildNodes[0]);
            }
            else
            {
                BinaryExpression expr = new BinaryExpression();
                expr.RecieveData(node);
                return expr;
            }
        }

        /// <summary>
        /// Resolves a unary expression from unary or prefix expressions.
        /// </summary>
        /// <param name="node">ParseTreeNode to resolve.</param>
        /// <returns>BaseNode Generated.</returns>
        public static BaseNode GetUnaryExpr(ParseTreeNode node)
        {
            if (node.Term.Name == LanguageGrammar.PrimaryExpression.Name)
            {
                return GetPrimaryExpr(node.ChildNodes[0]);
            }
            else if (node.ChildNodes.Count == 1)
            {
                return GetUnaryExpr(node.ChildNodes[0]);
            }
            else
            {
                BaseNode expr = new UnaryExpression();
                expr.RecieveData(node);
                return expr;
            }
        }

        /// <summary>
        /// Resolves an expression from primary expressions.
        /// </summary>
        /// <param name="node">ParseTreeNode to resolve.</param>
        /// <returns>BaseNode Generated.</returns>
        public static BaseNode GetPrimaryExpr(ParseTreeNode node)
        {
            BaseNode expr = null;

            if (node.Term.Name == LanguageGrammar.PrimaryExpression.Name)
            {
                return GetPrimaryExpr(node.ChildNodes[0]);
            }
            else if (node.Term.Name == LanguageGrammar.ID.Name)
            {
                expr = new Identifier();
            }
            else if (node.Term.Name == LanguageGrammar.ParenExpression.Name)
            {
                return GetBaseExpr(node.ChildNodes[1]);
            }
            else if (node.Term.Name == LanguageGrammar.CompoundExpression.Name)
            {
                expr = new CompoundExpression();
            }
            else if (node.Term.Name == LanguageGrammar.PostfixIncrementExpression.Name
                || node.Term.Name == LanguageGrammar.PostfixDecrementExpression.Name)
            {
                expr = new PostfixExpression();
            }
            else if (node.Term.Name == LanguageGrammar.ArrayCreationExpression.Name)
            {
                expr = new ArrayCreationExpression();
            }
            else if (node.Term.Name == LanguageGrammar.ObjectCreationExpression.Name)
            {
                expr = new ObjectCreationExpression();
            }
            else if (node.Term.Name == LanguageGrammar.ArrayExpression.Name)
            {
                expr = new ArrayExpression();
            }
            else if (node.Term.Name == LanguageGrammar.InvocationExpression.Name)
            {
                expr = new InvocationExpression();
            }
            else
            {
                expr = new Literal();
            }

            expr.RecieveData(node);
            return expr;
        }
    }
}
