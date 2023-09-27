using System.Threading;

namespace TaskManager.Models
{
    /// <summary>
    /// Класс задачи.
    /// </summary>
    public class Task
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Первичный приоритет задачи.
        /// </summary>
        public double PrimaryPriority { get; }

        /// <summary>
        /// Коэффициент ai.
        /// </summary>
        public double Coefficient { get; }

        /// <summary>
        /// Состояние задачи.
        /// </summary>
        public TaskState State { get; private set; }

        /// <summary>
        /// Приоритет задачи.
        /// </summary>
        public double Priority { get; private set; }

        /// <summary>
        /// Время выполнения на процессоре.
        /// </summary>
        public int TactCount { get; private set; }

        /// <summary>
        /// Конструктор задачи.
        /// </summary>
        /// <param name="idProvider"></param>
        /// <param name="name"></param>
        public Task(
            IIdProvider idProvider,
            string name,
            double primaryPriority,
            int tactCount)
        {
            Id = idProvider.GetId();
            Name = name;
            State = TaskState.Running;
            PrimaryPriority = primaryPriority;
            Priority = PrimaryPriority;
            TactCount = tactCount;
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

        /// <summary>
        /// Выполнить задачу.
        /// </summary>
        public void Execute()
        {
            if(TactCount <= 0)
            {
                return;
            }

            SwitchState(TaskState.Running);

            Thread.Sleep(millisecondsTimeout: 1000);

            TactCount--;

            SwitchState(TaskState.Ready);
        }
    }
}
