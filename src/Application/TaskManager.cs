using System.Collections.Generic;
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
        /// Очередь задач.
        /// </summary>
        private readonly List<Task> _taskQueue = new();

        /// <summary>
        /// Информация о тактах.
        /// </summary>
        private readonly List<TactData> _tactDatas = new();

        /// <summary>
        /// Вычислитель приоритета.
        /// </summary>
        private readonly IPriorityComputer _priorityComputer;

        /// <summary>
        /// Процессор.
        /// </summary>
        private readonly Processor _processor;

        /// <summary>
        /// Интервал, через который будет меняться приоритет.
        /// </summary>
        private int _interval = 1;

        /// <summary>
        /// Текущая задача.
        /// </summary>
        public Task? CurrentTask { get; private set; }

        /// <summary>
        /// Пока очередь не закончилась, диспетчер задач работает.
        /// </summary>
        public bool IsRunning => _tasks.Count > 0;

        /// <summary>
        /// Публичное свойство подлежащие выполнению задачи.
        /// </summary>
        public List<Task> Tasks => _tasks;

        /// <summary>
        /// Публичное свойство информация о тактах.
        /// </summary>
        public List<TactData> TactDatas => _tactDatas;

        /// <summary>
        /// Публичное свойство интервал обновления приоритетов.
        /// </summary>
        public int Interval => _interval;

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
            _tasks.Add(task);
        }

        /// <summary>
        /// Установить интервал обновления приоритетов.
        /// </summary>
        /// <param name="interval"></param>
        public void SetInterval(int interval)
        {
            if (interval < 1)
            {
                _interval = 1;

                return;
            }

            _interval = interval;
        }

        /// <summary>
        /// Метод обновления (один такт).
        /// </summary>
        public void Update()
        {
            // Обновить состояния задач.
            UpdateTaskStates();
            // Сформировать очередь задач.
            FormTaskQueue();
            // Если такт процессора кратен интервалу обновления приоритетов...
            if (_processor.Tact % _interval == 0)
            {
                // Пересчитать приоритеты.
                RecomputePriority();
            }
            // Добавить информацию о такте.
            AddTactData();
            // Обновить текущую задачу.
            UpdateCurrentTask();
            // Выполнить текущую задачу.
            ExecuteCurrentTask();
        }

        /// <summary>
        /// Добавить информацию о такте.
        /// </summary>
        private void AddTactData()
        {
            _tactDatas.Add(new TactData(
                _processor.Tact,
                CurrentTask, 
                _taskQueue.ToArray()));
        }

        /// <summary>
        /// Обновить состояния задач.
        /// </summary>
        private void UpdateTaskStates()
        {
            foreach (var task in _tasks)
            {
                if (task.State == TaskState.Sleeping &&
                    _processor.Tact >= task.StartTact)
                {
                    task.SwitchState(TaskState.Ready);
                }
            }
        }

        /// <summary>
        /// Обновление текущей задачи.
        /// </summary>
        private void UpdateCurrentTask()
        {
            // Получаем задачу с наивысшим приоритетом.
            var primaryTask = FindPrimaryTask();
            // Если текущая задача не определена, выбираем найденную и выходим.
            if (CurrentTask == null)
            {
                SetCurrentTask(primaryTask);

                return;
            }
            // Если текущая задача оказалась выполненной...
            if (CurrentTask.IsCompleted)
            {
                // Убираем из списка текущую задачу.
                _tasks.Remove(CurrentTask);
                // Устанавливаем новую задачу.
                SetCurrentTask(primaryTask);

                return;
            }
            // Если задача найдена и ее приоритет больше текущей...
            if (primaryTask != null && primaryTask.Priority > CurrentTask.Priority)
            {
                // Устанавливаем новую задачу на исполнение.
                SetCurrentTask(primaryTask);
            }
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
        }

        /// <summary>
        /// Сформировать очередь задач
        /// </summary>
        private void FormTaskQueue()
        {
            // Очищаем очередь.
            _taskQueue.Clear();
            // Перебрать все задачи.
            foreach (var task in _tasks)
            {
                // Если задача готова к выполнению.
                if (task.State == TaskState.Ready)
                {
                    // Добавить задачу в очередь.
                    _taskQueue.Add(task);
                }
            }
        }

        /// <summary>
        /// Устанавливает текущую задачу на исполнение.
        /// </summary>
        /// <param name="task"></param>
        private void SetCurrentTask(Task? task)
        {
            // Старую задачу переводим в состояние готовности, если она есть.
            CurrentTask?.SwitchState(TaskState.Ready);
            // Устанавливаем новую задачу.
            CurrentTask = task;
            // Устанавливаем новой задаче состояние выполнения, если она есть.
            CurrentTask?.SwitchState(TaskState.Running);
        }

        /// <summary>
        /// Рассчитать новые значения приоритетов.
        /// </summary>
        private void RecomputePriority()
        {
            // Пройтись по всем задачам.
            foreach (var task in _taskQueue)
            {
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
            foreach (var task in _taskQueue)
            {
                // Если задача еще не найдена, задаем ее и пропускаем итерацию.
                if (primaryTask == null)
                {
                    primaryTask = task;

                    continue;
                }
                // Если приоритет меньше или равен найденному, пропускаем итерацию.
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
