using Irony.Parsing;

// ReSharper disable InconsistentNaming

namespace SBLScripting
{
    public class SBLCompiler
    {
        public Parser Parser { get; private set; }

        public bool Parse(string source)
        {
            var tree = new Parser(new LanguageData(new SBLScriptGrammar()));
            var result = tree.Parse(source);
            Parser = tree;
            return !result.HasErrors();
        }
    }

    public class SBLScriptGrammar : Grammar
    {
        internal const string INT_LITERAL = "int_literal";
        internal const string STRING_LITERAL = "string_literal";
        internal const string BOOL_LITERAL = "bool_literal";
        internal const string DECIMAL_LITERAL = "decimal_literal";
        internal const string FLOAT_LITERAL = "float_literal";


        internal const string BASE_IDENTIFIER = "base_identifier";
        internal const string FULL_IDENTIFIER = "full_identifier";

        internal const string EXPRESSION = "expression";
        internal const string BINARY_OP = "binary_op";
        internal const string BINARY_OPERATION = "binary_opetaion";
        //
        internal const string FUNCTION_CALL_ARGUMENT_LIST = "function_call_argument_list";
        internal const string FUNCTION_CALL = "function_call";
        // 
        internal const string FUNCTION_DEFINITION = "function_def";
        internal const string FUNCTION_DEFINITION_ARGUMENT = "function_def_argument";
        //
        internal const string TYPE_NAME = "type_name";
        //
        internal const string STATEMENT = "statement";
        internal const string STATEMENT_GROUP = "statement_group";
        internal const string META_INSTRUCTION = "meta_intstruction";
        //
        internal const string VAR_DECLARATION = "var_declaration";
        internal const string ASSIGNMENT_STATEMENT = "assignment_statement";
        internal const string FOR_STATEMENT = "for_statement";
        internal const string FOREACH_STATEMENT = "foreach_statement";
        internal const string WHILE_STATEMENT = "while_statement";
        //
        internal const string PROGRAM = "program";


        public SBLScriptGrammar()
        {
            var IntegerLiteral = new NumberLiteral(INT_LITERAL,NumberOptions.IntOnly);
            var StringLiteral = new StringLiteral(STRING_LITERAL, "'",
                StringOptions.AllowsAllEscapes | StringOptions.AllowsLineBreak);
            var BoolLiteral = new RegexBasedTerminal(BOOL_LITERAL, "true|false");
            var DecimalLiteral = new RegexBasedTerminal(DECIMAL_LITERAL, "[+-]?([0-9]+M|[0-9]+\\.[0-9]+M)");
            var FloatLiteral = new RegexBasedTerminal(FLOAT_LITERAL, "[+-]?([0-9]+F|[0-9]+\\.[0-9]+F)");
            //
            var BaseIdentifier = new IdentifierTerminal(BASE_IDENTIFIER, IdOptions.NameIncludesPrefix);
            var Identifier = new NonTerminal(FULL_IDENTIFIER);
            //
            var Expression = new NonTerminal(EXPRESSION);
            var BinaryOp = new NonTerminal(BINARY_OP);
            var BinaryOperation = new NonTerminal(BINARY_OPERATION);
            //
            var FunctionCallArgumentList = new NonTerminal(FUNCTION_CALL_ARGUMENT_LIST);
            var FunctionCall = new NonTerminal(FUNCTION_CALL);

            var FunctionDefinition = new NonTerminal(FUNCTION_DEFINITION);
            var FunctionDefinitionArgument = new NonTerminal(FUNCTION_DEFINITION_ARGUMENT);

            //
            var TypeName = new NonTerminal(TYPE_NAME);
            //
            var Statement = new NonTerminal(STATEMENT);
            var StatementGroup = new NonTerminal(STATEMENT_GROUP);
            var MetaStatement = new NonTerminal(META_INSTRUCTION);

            var VarDeclaration = new NonTerminal(VAR_DECLARATION);
            var AssignmentStatement = new NonTerminal(ASSIGNMENT_STATEMENT);

            var ForStatement = new NonTerminal(FOR_STATEMENT);
            var ForEachStatement = new NonTerminal(FOREACH_STATEMENT);
            var WhileStatement = new NonTerminal(WHILE_STATEMENT);


            var Program = new NonTerminal(PROGRAM);

            /******************/

            Identifier.Rule = BaseIdentifier | BaseIdentifier + "." + Identifier;

            Expression.Rule = IntegerLiteral |
                              StringLiteral |
                              BoolLiteral |
                              DecimalLiteral |
                              FloatLiteral |
                              Identifier |
                              BinaryOperation |
                              "(" + Expression + ")";

            BinaryOp.Rule = ToTerm("+") | "-" | "*" | "/" | ">" | "<" | ">=" | "<=" | "%" | "==" | "<>" |
                            "&" | "|" | "and" | "or";

            BinaryOperation.Rule = Expression + BinaryOp + Expression;

            FunctionCallArgumentList.Rule = Expression | Expression + "," + FunctionCallArgumentList;
            FunctionCall.Rule = Identifier + "(" + ")" | Identifier + "(" + FunctionCallArgumentList + ")";
            //
            FunctionDefinition.Rule = "fn" + Identifier + "(" + ")" |
                                      "fn" + Identifier + "(" + FunctionDefinitionArgument + ")";
            //
            FunctionDefinitionArgument.Rule = TypeName + BaseIdentifier |
                                              TypeName + BaseIdentifier + "," + FunctionDefinitionArgument;
            //
            TypeName.Rule = ToTerm("bool") | "int" | "float" | "decimal" | "string" | "struct" + Identifier |
                            "class" + Identifier;

            Statement.Rule = MetaStatement | VarDeclaration | ForStatement | AssignmentStatement | ForEachStatement | WhileStatement;
            StatementGroup.Rule = Statement | StatementGroup + Statement;
            //
            MetaStatement.Rule = "@" + BaseIdentifier + BaseIdentifier |
                                 "@" + BaseIdentifier + "(" + StringLiteral + ")" |
                                 "@" + BaseIdentifier + "(" + StringLiteral + ")" + "as" + Identifier;

            VarDeclaration.Rule = "var" + Identifier + "=" + Expression + ";" | 
                                  "var" + Identifier + "=" + "new" + FunctionCall + ";";

            AssignmentStatement.Rule = Identifier + "=" + Expression + ";";


            ForStatement.Rule = ToTerm("for") + "(" + VarDeclaration + Expression + ";" + Identifier + "=" +  Expression + ")" + "{" + "}" |
                                ToTerm("for") + "(" + VarDeclaration + Expression + ";" + Identifier + "=" + Expression + ")" + "{" + StatementGroup + "}";

            ForEachStatement.Rule = ToTerm("foreach") + "(" + "var" + BaseIdentifier + "in" + Identifier + ")" + "{" + "}" |
                                     ToTerm("foreach") + "(" + "var" + BaseIdentifier + "in" + Identifier + ")" + "{" + StatementGroup + "}";

            WhileStatement.Rule = ToTerm("while") + "(" + Expression + ")" + "{" + "}" |
                                  ToTerm("while") + "(" + Expression + ")" + "{" + StatementGroup + "}";

            //
            //
            Program.Rule = StatementGroup;
            //
            Root = Program;

        }
    }
}
