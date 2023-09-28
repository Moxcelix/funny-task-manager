namespace TaskManagerModel.Presentation
{
    public class ConsoleTable
    {
        private readonly int _tableWidth = 73;

        private readonly int[] _columnWidths;

        public ConsoleTable(int[] columnWidths)
        {
            _columnWidths = columnWidths;

            _tableWidth = 1;

            foreach (var item in _columnWidths)
            {
                _tableWidth += item + 1;
            }
        }

        public void PrintLine()
        {
            Console.WriteLine(new string('-', _tableWidth));
        }

        public void PrintRow(params string[] columns)
        {
            string row = "|";

            for (int i = 0; i < columns.Length; i++)
            {
                row += AlignCentre(columns[i], _columnWidths[i]) + "|";
            }

            Console.WriteLine(row);
        }

        static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }
}
