namespace TaskManagerModel.Presentation
{
    /// <summary>
    /// Класс таблицы в консоли.
    /// </summary>
    public class ConsoleTable
    {
        /// <summary>
        /// Ширина таблицы.
        /// </summary>
        private readonly int _tableWidth;

        /// <summary>
        /// Массив ширин столбцов.
        /// </summary>
        private readonly int[] _columnWidths;

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="columnWidths"></param>
        public ConsoleTable(int[] columnWidths)
        {
            _columnWidths = columnWidths;
            // Подсчет ширины таблицы.
            _tableWidth = 1;

            foreach (var item in _columnWidths)
            {
                _tableWidth += item + 1;
            }
        }

        /// <summary>
        /// Напечатать разделительную полосу.
        /// </summary>
        public void PrintLine()
        {
            Console.Write("+");
            foreach (var item in _columnWidths)
            {
                Console.Write(new string('-', item));
                Console.Write("+");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Напечатать ряд.
        /// </summary>
        /// <param name="columns"></param>
        public void PrintRow(params string[] columns)
        {
            string row = "|";

            for (int i = 0; i < columns.Length; i++)
            {
                row += AlignCentre(columns[i], _columnWidths[i]) + "|";
            }

            Console.WriteLine(row);
        }

        /// <summary>
        /// Выравнять по ширине.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        private static string AlignCentre(string text, int width)
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
