namespace Compiler3; 

internal static partial class Compiler {
    public static Result<string?> Ok(string? v) {
        return new Result<string?> {Ok = v};
    }
    public static Result<string?> Err(Exception err) {
        return new Result<string?> {Err = err};
    }
}