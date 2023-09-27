using TaskManagerModel.Models;

namespace TaskManagerModel.Application
{
    /// <summary>
    /// Класс-реализация вычислителя приоритетов.
    /// </summary>
    public class PriorityComputer : IPriorityComputer
    {
        /// <summary>
        /// Формула приоритета.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="time"></param>
        /// <returns>приоритет</returns>
        public double ComputePriority(Models.Task task, double time)
        {
            return task.PrimaryPriority + task.Coefficient * Math.Sqrt(time);
        }
    }
}
