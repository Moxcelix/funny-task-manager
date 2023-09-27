namespace TaskManagerModel.Models
{
    /// <summary>
    /// Перечисление состояний задачи.
    /// </summary>
    public enum TaskState
    {
        Running,    // Выполнение.
        Sleeping,   // Бездействие.
        Ready,      // Готовность.
        Output      // Ожидание окончания вывода.
    }
}
