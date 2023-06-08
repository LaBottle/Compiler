using System.Collections;


namespace Compiler2;

internal static partial class Compiler {

    private static bool IsLetter(char c) {
        return char.IsLetter(c) || c == '_';
    }

    private static bool IsNumber(char c) {
        return char.IsDigit(c);
    }

    private static readonly char[] Separator = {'(', ')', '{', '}', ';', ',', ' '};

    private static bool IsSeparator(char c) {
        return Separator.Contains(c);
    }

    private static readonly char[] Operator = {'+', '-', '*', '/', '=', '<', '>'};

    private static bool IsOperator(char c) {
        return Operator.Contains(c);
    }

    private static readonly string[] Keywords = {
        "void", "integer", "float", "char", "string", "if", "then", "else", "while",
        "do", "and", "or", "return", "break", "continue"
    };

    public static List<Word> Analysis(string s) {
        var r = 1;
        var c = 0;
        var output = new List<Word>();
        var i = 0;
        while (i < s.Length) {
            c += 1;
            if (s[i] == '\r' || s[i] == '\t' || s[i] == ' ') {
                i += 1;
                continue;
            } else if (s[i] == '\n') {
                r += 1;
                c = 0;
            } else if (IsLetter(s[i])) {
                var ty = WordType.Identifier;
                var word = s[i].ToString();
                var j = i + 1;
                while (j != s.Length && (IsLetter(s[j]) || IsNumber(s[j]))) {
                    word += s[j];
                    j += 1;
                }

                if (Keywords.Contains(word)) {
                    ty = WordType.Keyword;
                }

                j -= 1;

                output.Add(new Word(ty, word, r, c));
                c += j - i;
                i = j;
            } else if (IsNumber(s[i])) {
                var ty = WordType.Integer;
                var word = s[i].ToString();
                var j = i + 1;
                while (j != s.Length && IsNumber(s[j])) {
                    word += s[j];
                    j += 1;
                }

                if (j != s.Length && s[j] == '.') {
                    ty = WordType.FloatPoint;
                    word += '.';
                    j += 1;
                    while (j != s.Length && IsNumber(s[j])) {
                        word += s[j];
                        j += 1;
                    }
                }

                j -= 1;

                output.Add(new Word(ty, word, r, c));
                c += j - i;
                i = j;
            } else if (s[i] == '\"') {
                var ty = WordType.String;
                var word = s[i].ToString();
                var j = i + 1;
                while (j != s.Length && s[j] != '\"') {
                    word += s[j];
                    j += 1;
                }

                word += s[j];

                output.Add(new Word(ty, word, r, c));
                c += j - i;
                i = j;
            } else if (s[i] == '\'') {
                var ty = WordType.Character;
                var word = s[i].ToString();
                var j = i + 1;
                //if s[j] == b\\'  {
                //    word.push(s[j] );
                //    j += 1;
                //}
                word += s[j];
                j += 1;
                word += s[j];

                output.Add(new Word(ty, word, r, c));
                c += j - i;
                i = j;
            }

            else if (IsSeparator(s[i])) {
                var ty = WordType.Separator;
                var word = s[i].ToString();

                output.Add(new Word(ty, word, r, c));
            } else if (IsOperator(s[i])) {
                var ty = WordType.Operator;
                var word = s[i].ToString();
                var j = i + 1;
                if (j != s.Length && s[j] ==  '=') {
                    if (s[i] == '='  || s[i] == '<' || s[i] == '>') {
                        word += s[j];
                        j += 1;
                    }
                } else if (j != s.Length && s[j] == '>') {
                    if (s[i] ==  '<') {
                        word += s[j];
                        j += 1;
                    }
                } else if (j != s.Length && s[j] == '/' ){
                    // Single-Line Comment
                    if (s[i] ==  '/') {
                        i += 2;
                        while (i < s.Length && s[i] != '\n') {
                            i += 1;
                        }

                        i += 1;
                        r += 1;
                        c = 0;
                        continue;
                    }
                } else if (j != s.Length && s[j] ==  '*') {
                    // Multi-Line Comment
                    if (s[i] == '/') {
                        i += 2;
                        while (i < s.Length && s[i] != '/' || s[i - 1] != '*') {
                            i += 1;
                            if (s[i] != '\n') {
                                c += 1;
                            } else {
                                r += 1;
                                c = 1;
                            }
                        }

                        i += 1;
                        continue;
                    }
                }

                j -= 1;
                output.Add(new Word(ty, word, r, c));
                c += j - i;
                i = j;
            } else {
                output.Add(new Word(WordType.Error, "$Error$", r, c));
            }
            i += 1;
        }

        return output;
    }
}