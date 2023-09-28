namespace TaskManagerModel.Models
{
    /// <summary>
    /// Перечисление состояний задачи.
    /// </summary>
    public enum TaskState
    {
        Sleeping,   // Бездействие.
        Running,    // Выполнение.
        Ready,      // Готовность.
        Output      // Ожидание окончания вывода.
    }
}
