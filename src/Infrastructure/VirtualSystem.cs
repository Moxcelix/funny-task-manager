﻿using TaskManagerModel.Models;
using TaskManagerModel.Application;

using Task = TaskManagerModel.Models.Task;

namespace TaskManagerModel.Infrastructure
{
    public class VirtualSystem
    {
        private readonly IdProvider _idProvider;

        private readonly TaskManager _taskManager;

        private readonly PriorityComputer _priorityComputer;

        public VirtualSystem()
        {
            _idProvider = new IdProvider();
            _priorityComputer = new PriorityComputer();
            _taskManager = new TaskManager(_priorityComputer);
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
            int tactCount,
            int startTact)
        {
            var task = new Task(_idProvider, name, priority, tactCount, startTact);

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
            }
        }
    }
}
