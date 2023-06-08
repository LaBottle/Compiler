namespace Compiler3;

internal static partial class Compiler {
    private static int index;
    private static List<Word> words;
    private static Exception? err;
    private static string name;
    private static Hierarchy block;
    private static int row, col, paramNum;
    private static Library lib;
    private static Function? currentFunction;
    private static bool inWhile;


    public static Exception? Recursive(List<Word> w, out (string id, string msg)[] info) {
        index = 0;
        words = w;
        block = new Hierarchy();
        row = 0;
        col = 0;
        inWhile = false;
        currentFunction = null;
        lib = new Library();

        try {
            Program();
        }
        catch (Exception e) {
            info = lib.Output();
            return e;
        }
        info = lib.Output();
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
        var resultType = name;
        Terminator(WordType.Identifier);
        var funcName = name;
        block = block.In();
        Terminator(WordType.Separator, "(");
        ParamList();
        Terminator(WordType.Separator, ")");
        currentFunction = lib.DefFunc(funcName, resultType, paramNum, row, col);
        Terminator(WordType.Separator, "{");
        while (Try(Statement)) { }
        Terminator(WordType.Separator, "}");
        block = block.Out()!;
        currentFunction = null;
    }

    private static void ResultType() {
        Terminator(WordType.Keyword, new[] {"integer", "float", "char", "string", "void"});
    }

    private static void ParamList() {
        paramNum = 0;
        if (Try(Type)) {
            Terminator(WordType.Identifier);
            lib.DefVar(name, block, row, col);
            paramNum++;
            while (Try(Terminator, WordType.Separator, ",")) {
                Type();
                Terminator(WordType.Identifier);
                lib.DefVar(name, block, row, col);
                paramNum++;
            }
        }
    }

    private static void Type() {
        Terminator(WordType.Keyword, new[] {"integer", "float", "char", "string"});
    }

    private static void Block() {
        Terminator(WordType.Separator, "{");
        block = block.In();
        while (Try(Statement)) { }
        Terminator(WordType.Separator, "}");
        block = block.Out()!;
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
        lib.DefVar(name, block, row, col);
        while (Try(Terminator, WordType.Separator, ",")) {
            Terminator(WordType.Identifier);
        }

        Terminator(WordType.Separator, ";");
    }

    private static void CallStatement() {
        Terminator(WordType.Identifier);
        var funcName = name;
        Terminator(WordType.Separator, "(");
        ActParamList();
        Terminator(WordType.Separator, ")");
        Terminator(WordType.Separator, ";");
        lib.CallFunc(funcName, paramNum, row, col);
    }

    private static void ActParamList() {
        paramNum = 0;
        if (Try(Exp)) {
            paramNum++;
            while (Try(Terminator, WordType.Separator, ",")) {
                Exp();
                paramNum++;
            }
        }
    }

    private static void AssignmentStatement() {
        Terminator(WordType.Identifier);
        lib.UseVar(name, block, row, col);
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
        inWhile = true;
        Statement();
        inWhile = false;
    }

    private static void ReturnStatement() {
        Terminator(WordType.Keyword, "return");
        if (Try(Exp)) {
            lib.Return(currentFunction, true, row, col);
        } else {
            lib.Return(currentFunction, false, row, col);
        }
        Terminator(WordType.Separator, ";");
    }

    private static void BreakStatement() {
        Terminator(WordType.Keyword, "break");
        Terminator(WordType.Separator, ";");
        if (!inWhile) {
            lib.Log("Error", "break is not in the while statement");
        }
    }

    private static void ContinueStatement() {
        Terminator(WordType.Keyword, "continue");
        Terminator(WordType.Separator, ";");
        if (!inWhile) {
            lib.Log("Error", "break is not in the while statement");
        }
    }

    private static void Exp() {
        Term();
        while (Try(Terminator, WordType.Operator, new[] {"+", "-"})) {
            Term();
        }
    }

    private static void Term() {
        Factor();
        while (Try(Terminator, WordType.Operator, new[] {"*", "/"})) {
            Factor();
        }
    }

    private static void Factor() {
        if (Try(Terminator, WordType.Identifier)) {
            lib.UseVar(name, block, row, col);
        } else if (Try(Terminator, WordType.Integer)) {
            //
        } else if (Try(Terminator, WordType.FloatPoint)) {
            //
        } else if (Try(Terminator, WordType.Separator, "(")) {
            Exp();
            Terminator(WordType.Separator, ")");
        } else {
            throw new Exception("Not a Factor.");
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
        Terminator(WordType.Operator, new[] {"<", "<=", ">", ">=", "==", "<>"});
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

        name = words[index].Val;
        row = words[index].Row;
        col = words[index].Col;
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

        name = words[index].Val;
        row = words[index].Row;
        col = words[index].Col;
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

        name = words[index].Val;
        row = words[index].Row;
        col = words[index].Col;
        index += 1;
    }
}