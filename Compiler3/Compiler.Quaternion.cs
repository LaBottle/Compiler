namespace Compiler3; 

internal static partial class Compiler {
    public class TempGen {
        private int number;

        public TempGen() {
            number = 0;
        }

        public string Gen() {
            return $"${number++}";
        }

        public void Reset() {
            number = 0;
        }
    }

    public class Quaternion {
        public string op{ get; set; }
        public string arg1{ get; set; }
        public string arg2{ get; set; }
        public string res{ get; set; }

        public Quaternion(string op, string arg1, string arg2, string res) {
            this.op = op;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.res = res;
        }
    }
}