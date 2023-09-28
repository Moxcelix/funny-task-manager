using TaskManagerModel.Models;
using TaskManagerModel.Application;

using Task = TaskManagerModel.Models.Task;

namespace TaskManagerModel.Infrastructure
{
    public class VirtualSystem
    {
        private readonly IdProvider _idProvider;

        private readonly TaskManager _taskManager;

        private readonly Processor _processor;

        private readonly PriorityComputer _priorityComputer;

        /// <summary>
        /// Делегат конца работы.
        /// </summary>
        public delegate void OnWorkEndDelegate();

        /// <summary>
        /// Событие конца работы.
        /// </summary>
        public event OnWorkEndDelegate? OnWorkEnd;

        public Processor Processor => _processor;

        public TaskManager TaskManager => _taskManager;

        public VirtualSystem()
        {
            _idProvider = new IdProvider();
            _processor = new Processor();
            _priorityComputer = new PriorityComputer();
            _taskManager = new TaskManager(_processor, _priorityComputer);
        }

        /// <summary>
        /// Добавить новую задачу.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="priority"></param>
        /// <param name="tactCount"></param>
        /// <param name="startTact"></param>
        public void AddTask(
            string name,
            double priority,
            double coefficient,
            int tactCount,
            int startTact)
        {
            var task = new Task(_idProvider,
                name,
                priority,
                coefficient,
                tactCount,
                startTact);

            _taskManager.AddTask(task);
        }

        public void Start()
        {
            var thread = new Thread(Update);
            thread.Start();
        }

        public void Update()
        {
            while (_taskManager.IsRunning)
            {
                _taskManager.Update();
                _processor.Update();
            }

            OnWorkEnd?.Invoke();
        }
    }
}
