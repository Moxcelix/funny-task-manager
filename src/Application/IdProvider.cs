using TaskManager.Models;

namespace TaskManager.Application
{
    /// <summary>
    /// Класс-реализация интерфейса поставщика id.
    /// </summary>
    public class IdProvider : IIdProvider
    {
        /// <summary>
        /// Счтечик id.
        /// </summary>
        private static int _id = 0;

        /// <summary>
        /// Метод запроса на получение уникального id.
        /// </summary>
        /// <returns>id</returns>
        public int GetId()
        {
            // Возврат с последующей инкрементацией.
            return _id++;
        }
    }
}
