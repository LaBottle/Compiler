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
    private static List<Quaternion> quaternions;
    private static TempGen tempGen;


    public static Exception? Recursive(List<Word> w, out (string id, string msg)[] info, out List<Quaternion>? quats) {
        index = 0;
        words = w;
        block = new Hierarchy();
        row = 0;
        col = 0;
        inWhile = false;
        currentFunction = null;
        lib = new Library();
        quaternions = new List<Quaternion>();
        tempGen = new TempGen();
        quats = null;
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

        quats = quaternions;
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
        var arg1 = name;
        Terminator(WordType.Separator, "(");
        ActParamList();
        Terminator(WordType.Separator, ")");
        quaternions.Add(new Quaternion("call", arg1, "_", "_"));
        Terminator(WordType.Separator, ";");
        lib.CallFunc(arg1, paramNum, row, col);
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
        var res = name;
        lib.UseVar(name, block, row, col);
        Terminator(WordType.Operator, "=");
        Exp();
        var arg1 = name;
        quaternions.Add(new Quaternion("=", arg1, "_", res));
        Terminator(WordType.Separator, ";");
    }

    private static void ConditionalStatement() {
        Terminator(WordType.Keyword, "if");
        ConditionalExp();
        var arg1 = name;
        quaternions.Add(new Quaternion("jnz", arg1, "_", (quaternions.Count + 2).ToString()));
        var a1 = quaternions.Count;
        quaternions.Add(new Quaternion("j", "_", "_", ""));
        Terminator(WordType.Keyword, "then");
        Statement();
        quaternions[a1].res = (quaternions.Count + 1).ToString();
        if (Try(Terminator, WordType.Keyword, "else")) {
            var a2 = quaternions.Count;
            quaternions.Add(new Quaternion("j", "_", "_", ""));
            Statement();
            quaternions[a2].res = quaternions.Count.ToString();
        }
    }

    private static void LoopStatement() {
        var a1 = quaternions.Count;
        Terminator(WordType.Keyword, "while");
        ConditionalExp();
        var arg1 = name;
        var a2 = quaternions.Count;
        quaternions.Add(new Quaternion("jez", arg1, "_", ""));
        Terminator(WordType.Keyword, "do");
        inWhile = true;
        Statement();
        inWhile = false;
        quaternions.Add(new Quaternion("j", "_", "_", a1.ToString()));
        quaternions[a2].res = quaternions.Count.ToString();
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
        var arg1 = name;
        while (Try(Terminator, WordType.Operator, new[] {"+", "-"})) {
            var op = name;
            Term();
            var arg2 = name;
            var res = tempGen.Gen();
            quaternions.Add(new Quaternion(op, arg1, arg2, res));
            arg1 = res;
        }

        name = arg1;
    }

    private static void Term() {
        Factor();
        var arg1 = name;
        while (Try(Terminator, WordType.Operator, new[] {"*", "/"})) {
            var op = name;
            Factor();
            var arg2 = name;
            var res = tempGen.Gen();
            quaternions.Add(new Quaternion(op, arg1, arg2, res));
            arg1 = res;
        }

        name = arg1;
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
        var arg1 = name;
        while (Try(Terminator, WordType.Keyword, "or")) {
            var op = name;
            RelationExp();
            var arg2 = name;
            var res = tempGen.Gen();
            quaternions.Add(new Quaternion("or", arg1, arg2, res));
            arg1 = res;
        }

        name = arg1;
    }

    private static void RelationExp() {
        CompExp();
        var arg1 = name;
        while (Try(Terminator, WordType.Keyword, "and")) {
            var op = name;
            CompExp();
            var arg2 = name;
            var res = tempGen.Gen();
            quaternions.Add(new Quaternion("and", arg1, arg2, res));
            arg1 = res;
        }

        name = arg1;
    }

    private static void CompExp() {
        Exp();
        var arg1 = name;
        CmpOp();
        var op = name;
        Exp();
        var arg2 = name;
        var res = tempGen.Gen();
        quaternions.Add(new Quaternion(op, arg1, arg2, res));
        name = res;
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