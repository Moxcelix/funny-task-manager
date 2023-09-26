using System.Collections.Generic;

namespace TaskManager.Models
{
    /// <summary>
    /// Класс диспетчера задач.
    /// </summary>
    public class TaskManager
    {
        /// <summary>
        /// Список задач.
        /// </summary>
        private readonly List<Task> _tasks = new List<Task>();

        /// <summary>
        /// Метод добавления задачи.
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(Task task)
        {
            _tasks.Add(task);
        }

        /// <summary>
        /// Метод обновления (один такт).
        /// </summary>
        public void Update()
        {

        }
    }
}
