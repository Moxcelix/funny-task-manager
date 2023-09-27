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

        /// <summary>
        /// Рассчитать новые значения приоритетов.
        /// </summary>
        private void RecomputePriority()
        {
            // Пройтись по всем задачам.
            foreach (var task in _tasks)
            {
                // Вычислить значение приоритета для данной задачи.
                var priority = _priorityComputer.ComputePriority(task, _processor.Tact);
                // Изменить значениени приоритета для данной задачи.
                task.SwitchPriority(priority);
            }
        }

        /// <summary>
        /// Метод находит первичную задачу для выполнения
        /// </summary>
        /// <returns>task</returns>
        private Task? GetPrimaryTask()
        {
            // Начальное значение искомой задачи null.
            var primaryTask = (Task?)null;
            // Итерируемся по задачам.
            foreach (var task in _tasks)
            {
                // Если задача не готова, пропускаем итерацию.
                if(task.State != TaskState.Ready)
                {
                    continue;
                }
                // Если задача еще не найдена, задаем ее и пропускаем итерацию.
                if (primaryTask == null)
                {
                    primaryTask = task;

                    continue;
                }
                // Если приоритет меньше или равен текущему, пропускаем итерацию.
                if(task.Priority <= primaryTask.Priority)
                {
                    continue;
                }
                // В противном случае обновляем искомую задачу на текущую.
                primaryTask = task;
            }
            // Возвращаем найденное значение.
            return primaryTask;
        }
    }
}
