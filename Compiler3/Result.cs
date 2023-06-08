using System.Runtime.InteropServices;

namespace Compiler3; 

[StructLayout(LayoutKind.Explicit)]
public struct Result<T> {
    [FieldOffset(0)]
    public Exception Err;
    [FieldOffset(0)]
    public T Ok;

    public T unwarp() {
        try {
            return (T)(object)this;
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }
}


