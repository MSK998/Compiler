namespace Compiler
{
    /// <summary>
    /// A position in a file
    /// </summary>
    public class Position
    {
        public int LineNumber { get; }

        public int PositionInLine { get; }

        public Position(int lineNumber, int positioninLine)
        {
            LineNumber = lineNumber;
            PositionInLine = positioninLine;
        }

        public override string ToString()
        {
            if (this == BuiltIn)
                return "System Defined";
            else
                return $"Line {LineNumber}, Column {PositionInLine}";
        }

        public static Position BuiltIn { get; } = new Position(-1, -1);
    }
}