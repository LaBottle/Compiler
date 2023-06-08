namespace Compiler3;

enum WordType {
    Error,
    Keyword = 1,
    Identifier,
    Integer,
    FloatPoint,
    String,
    Character,
    Separator,
    Operator,
}

struct Word {
    public WordType Ty { get; set; }
    public string Val { get; set; }
    public int Row { get; set; }
    public int Col { get; set; }

    public Word(WordType ty, string val, int row, int col) {
        Ty = ty;
        Val = val;
        Row = row;
        Col = col;
    }

    public override string ToString() {
        return $"({(int)Ty}, {Val}, {Row}, {Col})";
    }
}