using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Compiler3; 

internal static partial class Compiler {
    private static bool Try(this Action m, bool backtrack = false) {
        int i = index;
        try {
            m();
            return true;
        } catch (Exception ex) {
            if (index != i && !backtrack) {
                throw;
            } else {
                index = i;
            }
            err = ex;
            return false;
        }
    }

    private static bool Try<T>(this Action<T> m, T argv, bool backtrack = false) {
        int i = index;
        try {
            m(argv);
            return true;
        } catch (Exception ex) {
            if (index != i && !backtrack) {
                throw;
            } else {
                index = i;
            }
            err = ex;
            return false;
        }
    }

    private static bool Try<T1, T2>(this Action<T1, T2> m, T1 argv1, T2 argv2, bool backtrack = false) {
        int i = index;
        try {
            m(argv1, argv2);
            return true;
        } catch (Exception ex) {
            if (index != i && !backtrack) {
                throw;
            } else {
                index = i;
            }
            err = ex;
            return false;
        }
    }
}