﻿using System.Collections.Generic;
using TaskManagerModel.Models;

using Task = TaskManagerModel.Models.Task;

namespace TaskManagerModel.Application
{
    /// <summary>
    /// Класс диспетчера задач.
    /// </summary>
    public class TaskManager
    {
        /// <summary>
        /// Список задач.
        /// </summary>
        private readonly List<Task> _tasks = new();

        /// <summary>
        /// Вычислитель приоритета.
        /// </summary>
        private readonly IPriorityComputer _priorityComputer;

        /// <summary>
        /// Процессор.
        /// </summary>
        private readonly Processor _processor;

        /// <summary>
        /// Текущая задача.
        /// </summary>
        public Task? CurrentTask { get; private set; }

        /// <summary>
        /// Пока очередь не закончилась, диспетчер задач работает.
        /// </summary>
        public bool IsRunning => _tasks.Count > 0;

        /// <summary>
        /// Конструктор класса диспетчера задач.
        /// </summary>
        /// <param name="priorityComputer"></param>
        public TaskManager(Processor processor,
            IPriorityComputer priorityComputer)
        {
            _processor = processor;
            _priorityComputer = priorityComputer;
        }

        /// <summary>
        /// Метод добавления задачи.
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(Task task)
        {
            task.SwitchState(TaskState.Ready);

            _tasks.Add(task);
        }

        /// <summary>
        /// Метод обновления (один такт).
        /// </summary>
        public void Update()
        {
            RecomputePriority();
            UpdateCurrentTask();
            ExecuteCurrentTask();
        }

        /// <summary>
        /// Обновление текущей задачи.
        /// </summary>
        private void UpdateCurrentTask()
        {
            // Получаем задачу с наивысшим приоритетом.
            var primaryTask = FindPrimaryTask();
            // Если такая задача не найдена - выходим.
            if (primaryTask == null)
            {
                return;
            }
            // Если текущая задача не определена или выполнена -
            // выбираем найденную и выходим.
            if (CurrentTask == null || CurrentTask.IsCompleted)
            {
                SetCurrentTask(primaryTask);

                return;
            }
            // Если приоритет найденной задачи меньше или равен текущей - выход.
            if (primaryTask.Priority <= CurrentTask.Priority)
            {
                return;
            }
            // В остальных случаях устанавливаем новую задачу на исполнение.
            SetCurrentTask(primaryTask);
        }

        /// <summary>
        /// Выполнение текущей задачи.
        /// </summary>
        private void ExecuteCurrentTask()
        {
            // Если текущей задачи нет - ничего не делаем.
            if (CurrentTask == null)
            {
                return;
            }
            // Выполнить текущую задачу на процессоре.
            _processor.ExecuteTask(CurrentTask);
            // Если текущая задача оказалась выполненной...
            if (CurrentTask.IsCompleted)
            {
                // Убираем из списка текущую задачу.
                _tasks.Remove(CurrentTask);
                // Текущей задачи теперь нет.
                CurrentTask = null;
            }
        }

        /// <summary>
        /// Устанавливает текущую задачу на исполнение.
        /// </summary>
        /// <param name="task"></param>
        private void SetCurrentTask(Task task)
        {
            // Старую задачу переводим в состояние готовности.
            CurrentTask?.SwitchState(TaskState.Ready);
            // Устанавливаем новую задачу.
            CurrentTask = task;
            // Устанавливаем новой задачи состояние выполнения.
            CurrentTask.SwitchState(TaskState.Running);
        }

        /// <summary>
        /// Рассчитать новые значения приоритетов.
        /// </summary>
        private void RecomputePriority()
        {
            // Пройтись по всем задачам.
            foreach (var task in _tasks)
            {
                // Если задача не готова - пропускаем итерацию.
                if (task.State != TaskState.Ready ||
                    task.StartTact > _processor.Tact)
                {
                    return;
                }
                // Вычислить значение приоритета для данной задачи.
                var priority = _priorityComputer.ComputePriority(task, _processor.Tact);
                // Изменить значениени приоритета для данной задачи.
                task.SwitchPriority(priority);
            }
        }

        /// <summary>
        /// Метод находит первичную задачу для выполнения
        /// </summary>
        /// <returns>task</returns>
        private Task? FindPrimaryTask()
        {
            // Начальное значение искомой задачи null.
            var primaryTask = (Task?)null;
            // Итерируемся по задачам.
            foreach (var task in _tasks)
            {
                // Если задача не готова, пропускаем итерацию.
                if (task.State != TaskState.Ready ||
                    task.StartTact > _processor.Tact)
                {
                    continue;
                }
                // Если задача еще не найдена, задаем ее и пропускаем итерацию.
                if (primaryTask == null)
                {
                    primaryTask = task;

                    continue;
                }
                // Если приоритет меньше или равен текущему, пропускаем итерацию.
                if (task.Priority <= primaryTask.Priority)
                {
                    continue;
                }
                // В противном случае обновляем искомую задачу на текущую.
                primaryTask = task;
            }
            // Возвращаем найденное значение.
            return primaryTask;
        }
    }
}
