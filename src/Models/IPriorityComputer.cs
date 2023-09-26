namespace TaskManager.Models
{
    public interface IPriorityComputer
    {
        public void ComputePriority(Task task, double time);
    }
}
