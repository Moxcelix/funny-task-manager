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
        /// Свойство состояния задачи.
        /// </summary>
        public TaskState State { get; private set; }

        /// <summary>
        /// Конструктор задачи.
        /// </summary>
        /// <param name="idProvider"></param>
        /// <param name="name"></param>
        public Task(IIdProvider idProvider, string name)
        {
            Id = idProvider.GetId();
            Name = name;
            State = TaskState.Running;
        }

        /// <summary>
        /// Метод смены состояния.
        /// </summary>
        /// <param name="state"></param>
        public void SwitchState(TaskState state)
        {
            State = state;
        }
    }
}
