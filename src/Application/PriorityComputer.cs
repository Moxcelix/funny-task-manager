using TaskManager.Models;

namespace TaskManager.Application
{
    public class PriorityComputer : IPriorityComputer
    {
        public double ComputePriority(Models.Task task, double time)
        {
            return task.PrimaryPriority + task.Coefficient * Math.Sqrt(time);
        }
    }
}
