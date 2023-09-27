namespace TaskManagerModel.Models
{
    /// <summary>
    /// Интерфейс поставщика Id.
    /// </summary>
    public interface IIdProvider
    {
        /// <summary>
        /// Метод запроса id.
        /// </summary>
        /// <returns>id</returns>
        public int GetId();
    }
}
