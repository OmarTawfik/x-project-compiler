namespace LanguageCompiler
{
    using System.Collections.Generic;
    using Irony.Parsing;

    /// <summary>
    /// The Grammar class for Irony Parser.
    /// </summary>
    public class LanguageGrammar : Grammar
    {
        #region Terminals
        /// <summary>
        /// The terminal object for the "Identifier" rule.
        /// </summary>
        public static readonly Terminal ID = TerminalFactory.CreateCSharpIdentifier("ID");

        /// <summary>
        /// The terminal object for the "CharLiteral" rule.
        /// </summary>
        public static readonly Terminal CharLiteral = TerminalFactory.CreateCSharpChar("Char Literal");

        /// <summary>
        /// The terminal object for the "StringLiteral" rule.
        /// </summary>
        public static readonly Terminal StringLiteral = TerminalFactory.CreateCSharpString("String Literal");

        /// <summary>
        /// The terminal object for the "NaturalLiteral" rule.
        /// </summary>
        public static readonly RegexBasedTerminal NaturalLiteral = new RegexBasedTerminal("Natural", "([0-9]+)(b|s|i|l|B|S|I|L)?");

        /// <summary>
        /// The terminal object for the "DecimalLiteral" rule.
        /// </summary>
        public static readonly RegexBasedTerminal DecimalLiteral = new RegexBasedTerminal("Decimal", "([0-9]*)\\.([0-9]+)(d|f|D|F)?");

        #endregion Terminals

        #region NonTerminals

        /// <summary>
        /// The non terminal object for the "Class Base" rule.
        /// </summary>
        public static readonly NonTerminal ClassBase = new NonTerminal("Class Base");

        /// <summary>
        /// The non terminal object for the "Plus Expressions List" rule.
        /// </summary>
        public static readonly NonTerminal PlusExpressionsList = new NonTerminal("Plus Expressions List");

        /// <summary>
        /// The non terminal object for the "Class Definition" rule.
        /// </summary>
        public static readonly NonTerminal ClassDefinition = new NonTerminal("Class Definition");

        /// <summary>
        /// The non terminal object for the "File Definition" rule.
        /// </summary>
        public static readonly NonTerminal FileDefinition = new NonTerminal("File Definition");

        /// <summary>
        /// The non terminal object for the "Class Modifier" rule.
        /// </summary>
        public static readonly NonTerminal ClassModifier = new NonTerminal("Class Modifier");

        /// <summary>
        /// The non terminal object for the "Class Label" rule.
        /// </summary>
        public static readonly NonTerminal ClassLabel = new NonTerminal("Class Label");

        /// <summary>
        /// The non terminal object for the "Class Member" rule.
        /// </summary>
        public static readonly NonTerminal ClassMember = new NonTerminal("Class Member");

        /// <summary>
        /// The non terminal object for the "Class Members List" rule.
        /// </summary>
        public static readonly NonTerminal ClassMembersList = new NonTerminal("Class Members List");

        /// <summary>
        /// The non terminal object for the "Method Definition" rule.
        /// </summary>
        public static readonly NonTerminal MethodDefinition = new NonTerminal("Method Definition");

        /// <summary>
        /// The non terminal object for the "Member Accessor" rule.
        /// </summary>
        public static readonly NonTerminal MemberAccessor = new NonTerminal("Member Accessor");

        /// <summary>
        /// The non terminal object for the "Member Modifier" rule.
        /// </summary>
        public static readonly NonTerminal MemberModifier = new NonTerminal("Member Modifier");

        /// <summary>
        /// The non terminal object for the "Member Static" rule.
        /// </summary>
        public static readonly NonTerminal MemberStatic = new NonTerminal("Member Static");

        /// <summary>
        /// The non terminal object for the "Method Type" rule.
        /// </summary>
        public static readonly NonTerminal MethodType = new NonTerminal("Method Type");

        /// <summary>
        /// The non terminal object for the "CommasList" rule.
        /// </summary>
        public static readonly NonTerminal CommasList = new NonTerminal("CommasList");

        /// <summary>
        /// The non terminal object for the "Parameters List" rule.
        /// </summary>
        public static readonly NonTerminal ParametersList = new NonTerminal("Parameters List");

        /// <summary>
        /// The non terminal object for the "Parameters Definition" rule.
        /// </summary>
        public static readonly NonTerminal ParametersDefinition = new NonTerminal("Parameters Definition");

        /// <summary>
        /// The non terminal object for the "Parameter" rule.
        /// </summary>
        public static readonly NonTerminal Parameter = new NonTerminal("Parameter");

        /// <summary>
        /// The non terminal object for the "Block" rule.
        /// </summary>
        public static readonly NonTerminal Block = new NonTerminal("Block");

        /// <summary>
        /// The non terminal object for the "Block Or Semicolon" rule.
        /// </summary>
        public static readonly NonTerminal BlockOrSemicolon = new NonTerminal("Block Or Semicolon");

        /// <summary>
        /// The non terminal object for the "Equality Operator" rule.
        /// </summary>
        public static readonly NonTerminal EqualityOperator = new NonTerminal("Equality Operator");

        /// <summary>
        /// The non terminal object for the "Relational Operator" rule.
        /// </summary>
        public static readonly NonTerminal RelationalOperator = new NonTerminal("Relational Operator");

        /// <summary>
        /// The non terminal object for the "Unary Operator" rule.
        /// </summary>
        public static readonly NonTerminal UnaryOperator = new NonTerminal("Unary Operator");

        /// <summary>
        /// The non terminal object for the "Assignment Operator" rule.
        /// </summary>
        public static readonly NonTerminal AssignmentOperator = new NonTerminal("Assignment Operator");

        /// <summary>
        /// The non terminal object for the "Overloadable Operator" rule.
        /// </summary>
        public static readonly NonTerminal OverloadableOperator = new NonTerminal("Overloadable Operator");

        /// <summary>
        /// The non terminal object for the "Operator Definition" rule.
        /// </summary>
        public static readonly NonTerminal OperatorDefinition = new NonTerminal("Operator Definition");

        /// <summary>
        /// The non terminal object for the "Field Definition" rule.
        /// </summary>
        public static readonly NonTerminal FieldDefinition = new NonTerminal("Field Definition");

        /// <summary>
        /// The non terminal object for the "Field Atom" rule.
        /// </summary>
        public static readonly NonTerminal FieldAtom = new NonTerminal("Field Atom");

        /// <summary>
        /// The non terminal object for the "Field Atoms List" rule.
        /// </summary>
        public static readonly NonTerminal FieldAtomsList = new NonTerminal("Field Atoms List");

        /// <summary>
        /// The non terminal object for the "Optional Assignment" rule.
        /// </summary>
        public static readonly NonTerminal OptionalAssignment = new NonTerminal("Optional Assignment");

        /// <summary>
        /// The non terminal object for the "Expression" rule.
        /// </summary>
        public static readonly NonTerminal Expression = new NonTerminal("Expression");

        /// <summary>
        /// The non terminal object for the "Statement" rule.
        /// </summary>
        public static readonly NonTerminal Statement = new NonTerminal("Statement");

        /// <summary>
        /// The non terminal object for the "Statements List" rule.
        /// </summary>
        public static readonly NonTerminal StatementsList = new NonTerminal("Statements List");

        /// <summary>
        /// The non terminal object for the "Command Statement" rule.
        /// </summary>
        public static readonly NonTerminal CommandStatement = new NonTerminal("Command Statement");

        /// <summary>
        /// The non terminal object for the "Break Statement" rule.
        /// </summary>
        public static readonly NonTerminal BreakStatement = new NonTerminal("Break Statement");

        /// <summary>
        /// The non terminal object for the "Continue Statement" rule.
        /// </summary>
        public static readonly NonTerminal ContinueStatement = new NonTerminal("Continue Statement");

        /// <summary>
        /// The non terminal object for the "Return Statement" rule.
        /// </summary>
        public static readonly NonTerminal ReturnStatement = new NonTerminal("Return Statement");

        /// <summary>
        /// The non terminal object for the "Declaration Statement" rule.
        /// </summary>
        public static readonly NonTerminal DeclarationStatement = new NonTerminal("Declaration Statement");

        /// <summary>
        /// The non terminal object for the "Control Statement" rule.
        /// </summary>
        public static readonly NonTerminal ControlStatement = new NonTerminal("Control Statement");

        /// <summary>
        /// The non terminal object for the "While Statement" rule.
        /// </summary>
        public static readonly NonTerminal WhileStatement = new NonTerminal("While Statement");

        /// <summary>
        /// The non terminal object for the "Do While Statement" rule.
        /// </summary>
        public static readonly NonTerminal DoWhileStatement = new NonTerminal("Do While Statement");

        /// <summary>
        /// The non terminal object for the "If Body" rule.
        /// </summary>
        public static readonly NonTerminal IfBody = new NonTerminal("If Body");

        /// <summary>
        /// The non terminal object for the "If Bodies List" rule.
        /// </summary>
        public static readonly NonTerminal IfBodiesList = new NonTerminal("If Bodies List");

        /// <summary>
        /// The non terminal object for the "If Statement" rule.
        /// </summary>
        public static readonly NonTerminal IfStatement = new NonTerminal("If Statement");

        /// <summary>
        /// The non terminal object for the "For Statement" rule.
        /// </summary>
        public static readonly NonTerminal ForStatement = new NonTerminal("For Statement");

        /// <summary>
        /// The non terminal object for the "Expressions List" rule.
        /// </summary>
        public static readonly NonTerminal ExpressionsList = new NonTerminal("Expressions List");

        /// <summary>
        /// The non terminal object for the "Embedded If" rule.
        /// </summary>
        public static readonly NonTerminal EmbeddedIf = new NonTerminal("Embedded If");

        /// <summary>
        /// The non terminal object for the "Conditional Or Expression" rule.
        /// </summary>
        public static readonly NonTerminal ConditionalOrExpression = new NonTerminal("Conditional Or Expression");

        /// <summary>
        /// The non terminal object for the "Conditional And Expression" rule.
        /// </summary>
        public static readonly NonTerminal ConditionalAndExpression = new NonTerminal("Conditional And Expression");

        /// <summary>
        /// The non terminal object for the "Equality Expression" rule.
        /// </summary>
        public static readonly NonTerminal EqualityExpression = new NonTerminal("Equality Expression");

        /// <summary>
        /// The non terminal object for the "Relational Expression" rule.
        /// </summary>
        public static readonly NonTerminal RelationalExpression = new NonTerminal("Relational Expression");

        /// <summary>
        /// The non terminal object for the "Additive Expression" rule.
        /// </summary>
        public static readonly NonTerminal AdditiveExpression = new NonTerminal("Additive Expression");

        /// <summary>
        /// The non terminal object for the "Subtractive Expression" rule.
        /// </summary>
        public static readonly NonTerminal SubtractiveExpression = new NonTerminal("Subtractive Expression");

        /// <summary>
        /// The non terminal object for the "Multiplicative Expression" rule.
        /// </summary>
        public static readonly NonTerminal MultiplicativeExpression = new NonTerminal("Multiplicative Expression");

        /// <summary>
        /// The non terminal object for the "Division Expression" rule.
        /// </summary>
        public static readonly NonTerminal DivisionExpression = new NonTerminal("Division Expression");

        /// <summary>
        /// The non terminal object for the "Modulus Expression" rule.
        /// </summary>
        public static readonly NonTerminal ModulusExpression = new NonTerminal("Modulus Expression");

        /// <summary>
        /// The non terminal object for the "Positive Expression" rule.
        /// </summary>
        public static readonly NonTerminal PositiveExpression = new NonTerminal("Positive Expression");

        /// <summary>
        /// The non terminal object for the "Negative Expression" rule.
        /// </summary>
        public static readonly NonTerminal NegativeExpression = new NonTerminal("Negative Expression");

        /// <summary>
        /// The non terminal object for the "Invert Expression" rule.
        /// </summary>
        public static readonly NonTerminal InvertExpression = new NonTerminal("Invert Expression");

        /// <summary>
        /// The non terminal object for the "PostfixIncrementExpression" rule.
        /// </summary>
        public static readonly NonTerminal PostfixIncrementExpression = new NonTerminal("PostfixIncrementExpression");

        /// <summary>
        /// The non terminal object for the "PostfixDecrementExpression" rule.
        /// </summary>
        public static readonly NonTerminal PostfixDecrementExpression = new NonTerminal("PostfixDecrementExpression");

        /// <summary>
        /// The non terminal object for the "Compound Expression" rule.
        /// </summary>
        public static readonly NonTerminal CompoundExpression = new NonTerminal("Compound Expression");

        /// <summary>
        /// The non terminal object for the "Invocation Expression" rule.
        /// </summary>
        public static readonly NonTerminal InvocationExpression = new NonTerminal("Invocation Expression");

        /// <summary>
        /// The non terminal object for the "Paren Expression" rule.
        /// </summary>
        public static readonly NonTerminal ParenExpression = new NonTerminal("Paren Expression");

        /// <summary>
        /// The non terminal object for the "Primary Expression" rule.
        /// </summary>
        public static readonly NonTerminal PrimaryExpression = new NonTerminal("Primary Expression");

        /// <summary>
        /// The non terminal object for the "Object Creation Expression" rule.
        /// </summary>
        public static readonly NonTerminal ObjectCreationExpression = new NonTerminal("Object Creation Expression");
        
        /// <summary>
        /// The non terminal object for the "Object Creation Expression" rule.
        /// </summary>
        public static readonly NonTerminal ListCreationExpression = new NonTerminal("List Creation Expression");
        
        /// <summary>
        /// The non terminal object for the "Expression Statement" rule.
        /// </summary>
        public static readonly NonTerminal ExpressionStatement = new NonTerminal("Expression Statement");

        /// <summary>
        /// The non terminal object for the "Conditional Operator" rule.
        /// </summary>
        public static readonly NonTerminal ConditionalOperator = new NonTerminal("Conditional Operator");

        /// <summary>
        /// The non terminal object for the "Assignment Expression" rule.
        /// </summary>
        public static readonly NonTerminal AssignmentExpression = new NonTerminal("Assignment Expression");

        #endregion NonTerminals

        /// <summary>
        /// List of reserved words in our language.
        /// </summary>
        private static List<string> reservedWords = new List<string>();

        /// <summary>
        /// Initializes static members of the LanguageGrammar class.
        /// </summary>
        static LanguageGrammar()
        {
            LanguageGrammar.reservedWords.AddRange(new string[]
            {
                "new", "if", "using", "abstract", "concrete", "class", "screen", "extends", "backend", "primitive",
                "public", "private", "protected", "virtual", "override", "abstract", "static",
                "operator", "continue", "break", "for", "else", "while", "do", "true", "false"
            });
        }

        /// <summary>
        /// Initializes a new instance of the LanguageGrammar class.
        /// </summary>
        public LanguageGrammar()
        {
            CommasList.Rule = MakeStarRule(CommasList, ToTerm(","));

            Parameter.Rule = ID + ID;
            ParametersList.Rule = MakeStarRule(ParametersList, ToTerm(","), Parameter);
            ParametersDefinition.Rule = "(" + ParametersList + ")";

            ClassModifier.Rule = this.Empty | "abstract" | "concrete";
            ClassLabel.Rule = ToTerm("class") | "screen";
            ClassMember.Rule = MethodDefinition | OperatorDefinition | FieldDefinition;
            ClassMembersList.Rule = MakeStarRule(ClassMembersList, ClassMember);
            ClassBase.Rule = this.Empty | ("extends" + ID);
            ClassDefinition.Rule = ClassModifier + (this.Empty | "primitive") + (this.Empty | "backend")
                + ClassLabel + ID + ClassBase + "{" + ClassMembersList + "}";

            MemberAccessor.Rule = this.Empty | "public" | "private" | "protected";
            MemberModifier.Rule = this.Empty | "virtual" | "override" | "abstract";
            MemberStatic.Rule = this.Empty | "static";

            OptionalAssignment.Rule = ("=" + Expression) | this.Empty;
            FieldAtom.Rule = ID + OptionalAssignment;
            FieldAtomsList.Rule = MakePlusRule(FieldAtomsList, ToTerm(","), FieldAtom);
            FieldDefinition.Rule = MemberAccessor + MemberModifier + MemberStatic + ID + FieldAtomsList + ";";
            MethodDefinition.Rule = MemberAccessor + MemberModifier + MemberStatic + ID + ID + ParametersDefinition + BlockOrSemicolon;
            OperatorDefinition.Rule = MemberAccessor + MemberModifier + MemberStatic + ID + "operator" + OverloadableOperator + ParametersDefinition + BlockOrSemicolon;

            StatementsList.Rule = MakeStarRule(StatementsList, Statement);
            Block.Rule = "{" + StatementsList + "}";
            BlockOrSemicolon.Rule = Block | ";";

            ContinueStatement.Rule = ToTerm("continue") + ";";
            BreakStatement.Rule = ToTerm("break") + ";";
            ReturnStatement.Rule = "return" + (Expression | this.Empty) + ";";
            CommandStatement.Rule = ReturnStatement | ContinueStatement | BreakStatement;

            ForStatement.Rule = ToTerm("for") + "(" + (FieldAtomsList | this.Empty) + ";" + (Expression | this.Empty) + ";" + ExpressionsList + ")" + BlockOrSemicolon;
            IfBody.Rule = ToTerm("if") + "(" + Expression + ")" + Block;
            IfBodiesList.Rule = MakePlusRule(IfBodiesList, this.ToTerm("else"), IfBody);
            IfStatement.Rule = IfBodiesList + (("else" + Block) | this.Empty);
            WhileStatement.Rule = ToTerm("while") + "(" + Expression + ")" + BlockOrSemicolon;
            DoWhileStatement.Rule = "do" + Block + "while" + "(" + Expression + ")" + ";";
            ControlStatement.Rule = WhileStatement | DoWhileStatement | IfStatement | ForStatement;

            ExpressionStatement.Rule = Expression + ";";
            DeclarationStatement.Rule = ID + FieldAtomsList + ";";
            Statement.Rule = CommandStatement | DeclarationStatement | ControlStatement | Block | ExpressionStatement;
            
            Expression.Rule = AssignmentExpression;
            ExpressionsList.Rule = MakeStarRule(ExpressionsList, ToTerm(","), Expression);
            PlusExpressionsList.Rule = MakePlusRule(PlusExpressionsList, ToTerm(","), Expression);
            AssignmentExpression.Rule = EmbeddedIf | EmbeddedIf + AssignmentOperator + AssignmentExpression;
            EmbeddedIf.Rule = ConditionalOrExpression + (("?" + Expression + ":" + Expression) | this.Empty);

            ConditionalOrExpression.Rule = ConditionalAndExpression | ConditionalAndExpression + "or" + ConditionalOrExpression;
            ConditionalAndExpression.Rule = EqualityExpression | EqualityExpression + "and" + ConditionalAndExpression;
            EqualityExpression.Rule = RelationalExpression | RelationalExpression + EqualityOperator + EqualityExpression;
            RelationalExpression.Rule = AdditiveExpression | AdditiveExpression + RelationalOperator + RelationalExpression;
            AdditiveExpression.Rule = SubtractiveExpression | SubtractiveExpression + "+" + AdditiveExpression;
            SubtractiveExpression.Rule = MultiplicativeExpression | MultiplicativeExpression + "-" + SubtractiveExpression;
            MultiplicativeExpression.Rule = DivisionExpression | DivisionExpression + "*" + MultiplicativeExpression;
            DivisionExpression.Rule = ModulusExpression | ModulusExpression + "/" + DivisionExpression;
            ModulusExpression.Rule = PositiveExpression | PositiveExpression + "%" + ModulusExpression;

            PositiveExpression.Rule = NegativeExpression | "+" + NegativeExpression;
            NegativeExpression.Rule = InvertExpression | "-" + InvertExpression;
            InvertExpression.Rule = PrimaryExpression | "not" + PrimaryExpression;

            PrimaryExpression.Rule = ParenExpression | CompoundExpression
                | ID | CharLiteral | StringLiteral | DecimalLiteral | NaturalLiteral | "true" | "false"
                | PostfixIncrementExpression | PostfixDecrementExpression
                | ObjectCreationExpression | InvocationExpression | ListCreationExpression;

            ParenExpression.Rule = "(" + Expression + ")";
            CompoundExpression.Rule = PrimaryExpression + "." + ID;

            PostfixIncrementExpression.Rule = PrimaryExpression + "++";
            PostfixDecrementExpression.Rule = PrimaryExpression + "--";

            ObjectCreationExpression.Rule = "new" + ID + "(" + ExpressionsList + ")";
            ListCreationExpression.Rule = "new" + ID + "list" + "(" + ExpressionsList + ")";

            InvocationExpression.Rule = PrimaryExpression + "(" + ExpressionsList + ")";

            EqualityOperator.Rule = ToTerm("==") | "!=";
            RelationalOperator.Rule = ToTerm("<") | ">" | "<=" | ">=";
            UnaryOperator.Rule = ToTerm("+") | "-" | "not" | "++" | "--";
            AssignmentOperator.Rule = ToTerm("=") | "+=" | "-=" | "*=" | "/=" | "%=";
            ConditionalOperator.Rule = ToTerm("and") | "or";
            OverloadableOperator.Rule = EqualityOperator | RelationalOperator | UnaryOperator
                | AssignmentOperator | ConditionalOperator | "*" | "/" | "%";

            this.MarkReservedWords(LanguageGrammar.reservedWords.ToArray());
            this.RegisterBracePair("(", ")");
            this.RegisterBracePair("[", "]");
            this.RegisterBracePair("{", "}");

            this.NonGrammarTerminals.Add(new CommentTerminal("Single Line Comment", "/", "\r\n", "\n", "\r"));
            this.NonGrammarTerminals.Add(new CommentTerminal("Multi Line Comment", "/*", "*/"));

            this.RegisterOperators(13, Associativity.Neutral, "=", "+=", "-=", "*=", "/=", "%=");
            this.RegisterOperators(12, Associativity.Neutral, "?");
            this.RegisterOperators(11, Associativity.Neutral, ":");
            this.RegisterOperators(10, Associativity.Neutral, "or");
            this.RegisterOperators(9, Associativity.Neutral, "==", "!=");
            this.RegisterOperators(8, Associativity.Neutral, "<", ">", "<=", ">=");
            this.RegisterOperators(7, Associativity.Neutral, "+");
            this.RegisterOperators(6, Associativity.Neutral, "-");
            this.RegisterOperators(5, Associativity.Neutral, "*");
            this.RegisterOperators(4, Associativity.Neutral, "/");
            this.RegisterOperators(3, Associativity.Neutral, "%");
            this.RegisterOperators(2, Associativity.Neutral, "not");
            this.RegisterOperators(1, Associativity.Left, "--", "++");

            this.MarkTransient(EqualityOperator, RelationalOperator, UnaryOperator, AssignmentOperator);
            this.MarkTransient(ClassLabel, ClassMember, MethodType, BlockOrSemicolon);
            this.MarkTransient(Statement, CommandStatement, ControlStatement, Expression, OverloadableOperator);

            FileDefinition.Rule = MakePlusRule(FileDefinition, ClassDefinition);
            this.Root = FileDefinition;
        }

        /// <summary>
        /// Gets the list of reserved words in our language.
        /// </summary>
        public static List<string> ReservedWords
        {
            get { return LanguageGrammar.reservedWords; }
        }
    }
}