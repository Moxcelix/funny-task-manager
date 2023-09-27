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
        private readonly List<Task> _tasks = new();

        /// <summary>
        /// Вычислитель приоритета.
        /// </summary>
        private readonly IPriorityComputer _priorityComputer;

        /// <summary>
        /// Процессор.
        /// </summary>
        private readonly Processor _processor;

        /// <summary>
        /// Конструктор класса диспетчера задач.
        /// </summary>
        /// <param name="priorityComputer"></param>
        public TaskManager(IPriorityComputer priorityComputer)
        {
            _priorityComputer = priorityComputer;
        }

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
            RecomputePriority();
        }

        private void RecomputePriority()
        {
            foreach (var task in _tasks)
            {
                _priorityComputer.ComputePriority(task, _processor.Tact);
            }
        }
    }
}
