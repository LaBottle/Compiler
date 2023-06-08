namespace Compiler3;

internal static partial class Compiler {
    public class Variable : IEquatable<Variable> {
        public string Name { get; }
        public Hierarchy Block { get; }
        public int Row { get; }
        public int Col { get; }
        public bool HasUsed { get; set; }

        public Variable(string n, Hierarchy d, int r, int c, bool h = false) {
            Name = n;
            Block = d;
            Row = r;
            Col = c;
            HasUsed = h;
        }

        public bool Equals(Variable? other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Block == other.Block;
        }

        public override bool Equals(object? obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Variable other && Equals(other);
        }

        public override int GetHashCode() {
            return HashCode.Combine(Name, Block);
        }
    }

    public class Function : IEquatable<Function> {
        public string Name { get; }
        public string ReturnType { get; }
        public int paramNum { get; }
        public int Row { get; }
        public int Col { get; }
        public bool HasReturn { get; set; }

        public Function(string n, string t, int r, int c, int paramNum) {
            Name = n;
            ReturnType = t;
            Row = r;
            Col = c;
            this.paramNum = paramNum;
        }

        public bool Equals(Function? other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name;
        }

        public override bool Equals(object? obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Function other && Equals(other);
        }

        public override int GetHashCode() {
            return HashCode.Combine(Name, ReturnType);
        }
    }

    //public class InfoMessage {
    //    public enum Identification {
    //        Error,
    //        Warning,
    //        Info
    //    }

    //    public InfoMessage(Identification i, string m) {
    //        Id = i;
    //        Msg = m;
    //    }
    //    public Identification Id { get; set; }
    //    public string Msg { get; set; }
    //    public override string ToString() {
    //        return Id + ": " + Msg;
    //    }
    //}

    private static ulong _hierarchyNumber = 0;

    public class Hierarchy : IEquatable<Hierarchy> {
        public ulong Number { get; }
        public Hierarchy? Father { get; }

        public Hierarchy(Hierarchy? f = null) {
            Father = f;
            Number = _hierarchyNumber++;
        }

        public Hierarchy In() {
            return new Hierarchy(this);
        }

        public Hierarchy? Out() {
            return Father;
        }

        public static bool operator ==(Hierarchy? h1, Hierarchy? h2) {
            return ReferenceEquals(h1, h2);
        }

        public static bool operator !=(Hierarchy? h1, Hierarchy? h2) {
            return !(h1 == h2);
        }

        public bool Equals(Hierarchy? other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Number == other.Number;
        }

        public override bool Equals(object? obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Hierarchy other && Equals(other);
        }

        public override int GetHashCode() {
            return HashCode.Combine(Number);
        }
    }

    //public class BlockManager {
    //    private Stack<Hierarchy> s = new Stack<Hierarchy>();

    //    public void In(Hierarchy b) {
    //        s.Push(b);
    //    }

    //    public void Out() {
    //        s.Pop();
    //    }
    //}

    public class Library {
        private List<Variable> varSet = new();
        private List<Function> funcSet = new();
        private HashSet<(string id, string msg)> message = new();

        public void DefVar(string n, Hierarchy b, int r, int c) {
            var v = new Variable(n, b, r, c);
            if (varSet.IndexOf(v) != -1) {
                Log("Error", $"Function `{n}` already exists.\nAt line {r}, column {c}.");
            } else {
                varSet.Add(v);
            }
        }

        public void UseVar(string n, Hierarchy b, int r, int c) {
            List<Hierarchy> h = new List<Hierarchy>();
            var t = b;
            while (t != null) {
                h.Add(t);
                t = t.Father;
            }

            var i = varSet.FindIndex(o => h.Contains(o.Block) && n == o.Name);
            if (i == -1) {
                Log("Error", $"Variable `{n}` not defined.\nAt line {r}, column {c}.");
            } else {
                varSet[i].HasUsed = true;
            }
        }

        public Function DefFunc(string n, string t, int pn, int r, int c) {
            var f = new Function(n, t, r, c, pn);
            if (funcSet.Contains(f)) {
                Log("Error", $"Function `{n}` already exists.\nAt line {r}, column {c}.");
            } else {
                funcSet.Add(f);
            }

            return f;
        }

        public void CallFunc(string n, int pn, int r, int c) {
            var i = funcSet.FindIndex((Function o) => o.Name == n);
            if (i == -1) {
                Log("Error", $"Function `{n}` not defined.\nAt line {r}, column {c}.");
            } else if (funcSet[i].paramNum != pn) {
                Log("Error",
                    $"Function `{n}` expect {funcSet[i].paramNum} parameter(s), found {pn} parameter(s).\nAt line {r}, column {c}.");
            }
        }

        public void Return(Function? f, bool has, int r, int c) {
            if (f == null) {
                Log("Error",
                    $"Return statement is not within the function body.\nAt line {r}, column {c}.");
                return;
            }

            if (f.ReturnType == "void") {
                if (has) {
                    Log("Error",
                        $"Function `{f.Name}` has not return value, but return has a value.\nAt line {r}, column {c}.");
                }
            } else {
                if (has) {
                    f.HasReturn = true;
                } else {
                    Log("Error",
                        $"Function `{f.Name}` expect a return value, but no.\nAt line {r}, column {c}.");
                }
            }
        }

        public void Log(string id, string msg) {
            message.Add((id, msg));
        }

        public (string id, string msg)[] Output() {
            foreach (var v in varSet) {
                if (!v.HasUsed) {
                    Log("Warning", $"Variable `{v.Name}` not used.\nAt line {v.Row}, column {v.Col}.");
                }
            }

            foreach (var f in funcSet) {
                if (f.ReturnType != "void" && !f.HasReturn) {
                    Log("Warning", $"Function `{f.Name}` not return.\nAt line {f.Row}, column {f.Col}.");
                }
            }

            return message.ToArray();
        }
    }
}