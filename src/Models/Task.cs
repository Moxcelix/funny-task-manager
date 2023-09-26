namespace TaskManager.Models
{
    /// <summary>
    /// Класс задачи.
    /// </summary>
    public class Task
    {
        /// <summary>
        /// Свойство Id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Свойство имя.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Свойство первичного приоритета задачи.
        /// </summary>
        public double PrimaryPriority { get; }

        /// <summary>
        /// Свойство коэффициента ai.
        /// </summary>
        public double Coefficient { get; }

        /// <summary>
        /// Свойство состояния задачи.
        /// </summary>
        public TaskState State { get; private set; }

        /// <summary>
        /// Свойство приоритета задачи.
        /// </summary>
        public double Priority { get; private set; }

        /// <summary>
        /// Конструктор задачи.
        /// </summary>
        /// <param name="idProvider"></param>
        /// <param name="name"></param>
        public Task(IIdProvider idProvider, string name, double primaryPriority)
        {
            Id = idProvider.GetId();
            Name = name;
            State = TaskState.Running;
            PrimaryPriority = primaryPriority;
            Priority = PrimaryPriority;
        }

        /// <summary>
        /// Метод смены состояния.
        /// </summary>
        /// <param name="state"></param>
        public void SwitchState(TaskState state)
        {
            State = state;
        }

        /// <summary>
        /// Метод смены приоритета.
        /// </summary>
        /// <param name="priority"></param>
        public void SwitchPriority(double priority)
        {
            Priority = priority;
        }
    }
}
