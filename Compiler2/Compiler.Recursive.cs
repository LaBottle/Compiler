using Compiler2;

namespace Compiler2;

internal static partial class Compiler {
    private static int index = 0;
    private static List<Word> words = new List<Word>();
    private static Exception? err;
    public static Exception? Recursive(List<Word> w) {
        index = 0;
        words = w;

        try {
            Program();
        }
        catch (Exception e) {
            return e;
        }

        if (index != words.Count) {
            return err;
        }
        return null;
    }

    private static void Program() {
        while (Try(Method)) { }
    }

    private static void Method() {
        ResultType();
        Terminator(WordType.Identifier);
        Terminator(WordType.Separator, "(");
        ParamList();
        Terminator(WordType.Separator, ")");
        Block();
    }

    private static void ResultType() {
        Terminator(WordType.Keyword, new[] { "integer", "float", "char", "string", "void" });
    }

    private static void ParamList() {
        if (Try(Type)) {
            Terminator(WordType.Identifier);
            while (Try(Terminator, WordType.Separator, ",")) {
                Type();
                Terminator(WordType.Identifier);
            }
        }
    }

    private static void Type() {
        Terminator(WordType.Keyword, new[] { "integer", "float", "char", "string" });
    }

    private static void Block() {
        Terminator(WordType.Separator, "{");
        while (Try(Statement)) { }
        Terminator(WordType.Separator, "}");
    }

    private static void Statement() {
        if (Try(ConditionalStatement) ||
            Try(LoopStatement) ||
            Try(CallStatement, true) ||
            Try(AssignmentStatement) ||
            Try(ReturnStatement) ||
            Try(BreakStatement) ||
            Try(ContinueStatement) ||
            Try(LocalVariableDeclaration) ||
            Try(Block)
        ) { } else {
            Terminator(WordType.Separator, ";");
        }
    }

    private static void LocalVariableDeclaration() {
        Type();
        Terminator(WordType.Identifier);

        while (Try(Terminator, WordType.Separator, ",")) {
            Terminator(WordType.Identifier);
        }

        Terminator(WordType.Separator, ";");
    }

    private static void CallStatement() {
        Terminator(WordType.Identifier);
        Terminator(WordType.Separator, "(");
        ActParamList();
        Terminator(WordType.Separator, ")");
        Terminator(WordType.Separator, ";");
    }

    private static void ActParamList() {
        if (Try(Exp)) {
            while (Try(Terminator, WordType.Separator, ",")) {
                Exp();
            }
        }
    }

    private static void AssignmentStatement() {
        Terminator(WordType.Identifier);
        Terminator(WordType.Operator, "=");
        Exp();
        Terminator(WordType.Separator, ";");
    }

    private static void ConditionalStatement() {
        Terminator(WordType.Keyword, "if");
        ConditionalExp();
        Terminator(WordType.Keyword, "then");
        Statement();
        if (Try(Terminator, WordType.Keyword, "else")) {
            Statement();
        }
    }

    private static void LoopStatement() {
        Terminator(WordType.Keyword, "while");
        ConditionalExp();
        Terminator(WordType.Keyword, "do");
        Statement();
    }

    private static void ReturnStatement() {
        Terminator(WordType.Keyword, "return");
        Try(Exp);
        Terminator(WordType.Separator, ";");
    }

    private static void BreakStatement() {
        Terminator(WordType.Keyword, "break");
        Terminator(WordType.Separator, ";");
    }

    private static void ContinueStatement() {
        Terminator(WordType.Keyword, "continue");
        Terminator(WordType.Separator, ";");
    }

    private static void Exp() {
        Term();
        while (Try(Terminator, WordType.Operator, new[] { "+", "-" })) {
            Term();
        }
    }

    private static void Term() {
        Factor();
        while (Try(Terminator, WordType.Operator, new[] { "*", "/" })) {
            Factor();
        }
    }

    private static void Factor() {
        if (Try(Terminator, WordType.Identifier) ||
            Try(Terminator, WordType.Integer) ||
            Try(Terminator, WordType.FloatPoint)
        ) { } else if(Try(Terminator,WordType.Separator, "(")) {
            Exp();
            Terminator(WordType.Separator, ")");
        } else {
            throw new Exception("Error `Factor`.");
        }
    }

    private static void ConditionalExp() {
        RelationExp();
        while (Try(Terminator, WordType.Keyword, "or")) {
            RelationExp();
        }
    }

    private static void RelationExp() {
        CompExp();
        while (Try(Terminator, WordType.Keyword, "and")) {
            CompExp();
        }
    }

    private static void CompExp() {
        Exp();
        CmpOp();
        Exp();
    }

    private static void CmpOp() {
        Terminator(WordType.Operator, new[] { "<", "<=", ">", ">=", "==", "<>" });
    }

    private static void Terminator(WordType ty) {
        if (index == words.Count) {
            throw new Exception(
                $"Analysis has ended in `line {words[index].Row}, column {words[index].Col}`.But there are statements that have not been analyzed.Please check for syntax errors.");
        }

        if (words[index].Ty != ty) {
            throw new Exception(
                $"line {words[index].Row}, column {words[index].Col}.\n" +
                $"expected `{ty}`, found `{words[index].Val}`."
            );
        }

        index += 1;
    }


    private static void Terminator(WordType ty, string val) {
        if (index == words.Count) {
            throw new Exception(
                "Analysis has ended in `line {}, column {}`.But there are statements that have not been analyzed.Please check for syntax errors.");
        }

        if (!(words[index].Ty == ty && words[index].Val == val)) {
            throw new Exception(
                $"line {words[index].Row}, column {words[index].Col}.\n" +
                $"expected `{val}`, found `{words[index].Val}`."
            );
        }

        index += 1;
    }

    private static void Terminator(WordType ty, string[] values) {
        if (index == words.Count) {
            throw new Exception("End where it should not end");
        }

        if (!(words[index].Ty == ty && values.Contains(words[index].Val))) {
            throw new Exception(
                $"line {words[index].Row}, column {words[index].Col}.\n" +
                $"expected `{ty}`, found `{words[index].Val}`."
            );
        }

        index += 1;
    }
}