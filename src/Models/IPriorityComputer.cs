namespace TaskManagerModel.Models
{
    public interface IPriorityComputer
    {
        public double ComputePriority(Task task, double time);
    }
}
