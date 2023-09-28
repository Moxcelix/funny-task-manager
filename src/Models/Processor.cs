using System.Threading;

namespace TaskManagerModel.Models
{
    /// <summary>
    /// Класс процессора.
    /// </summary>
    public class Processor
    {
        /// <summary>
        /// Текущая задача.
        /// </summary>
        private Task? _task = null;
        /// <summary>
        /// Текущий такт.
        /// </summary>
        public int Tact { get; private set; }

        /// <summary>
        /// Делегат обновления.
        /// </summary>
        public delegate void OnUpdateDelegate();

        /// <summary>
        /// Событие обновления.
        /// </summary>
        public event OnUpdateDelegate OnUpdate;

        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="startTact"></param>
        public Processor(int startTact = 0)
        {
            Tact = startTact;
        }

        /// <summary>
        /// Выполнить задачу.
        /// </summary>
        /// <param name="task"></param>
        public void ExecuteTask(Task task)
        {
            _task = task;
        }

        public void Update()
        {
            OnUpdate?.Invoke();

            _task?.Execute();

            _task = null;

            Thread.Sleep(millisecondsTimeout: 1000);

            Tact++;
        }
    }
}
