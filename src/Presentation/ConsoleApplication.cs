using TaskManagerModel.Infrastructure;
using TaskManagerModel.Models;

using Task = TaskManagerModel.Models.Task;

namespace TaskManagerModel.Presentation
{
    public class ConsoleApplication
    {
        private readonly VirtualSystem _system;

        private readonly ConsoleTable _tasksTable;

        private readonly ConsoleTable _resultTable;

        public ConsoleApplication(VirtualSystem system)
        {
            _system = system;

            _tasksTable = new ConsoleTable(new int[] { 12, 18, 32, 11, 11 });

            _resultTable = new ConsoleTable(new int[] { 13, 21, 20 });

            _system.Processor.OnUpdate += OnProcessorUpdate;

            _system.OnWorkEnd += PrintResult;
        }

        ~ConsoleApplication()
        {
            _system.Processor.OnUpdate -= OnProcessorUpdate;

            _system.OnWorkEnd -= PrintResult;
        }

        public void Run()
        {
            Console.Write("Введите интервал через который будет меняться приоритет -> ");
            var interval = int.Parse(Console.ReadLine());

            _system.TaskManager.SetInterval(interval);

            /*Console.Write("Введите количество задач -> ");
            var count = int.Parse(Console.ReadLine());

            Console.WriteLine();

            for (int i = 0; i < count; i++)
            {
                InputTask();

                Console.WriteLine();
            }*/

            _system.AddTask("A", 1, 1, 6, 0);
            _system.AddTask("B", 1, 2, 4, 0);
            _system.AddTask("C", 2, 1, 5, 3);

            _system.Start();
        }

        private void OnProcessorUpdate()
        {
            Console.Write($"Такт №{_system.Processor.Tact}");

            if(_system.Processor.Tact % _system.TaskManager.Interval == 0)
            {
                Console.Write(" - обновление приоритетов!");
            }

            Console.WriteLine();

            PrintTable();
        }

        private void PrintResult()
        {
            _resultTable.PrintLine();
            _resultTable.PrintRow(new string[]
            {
                "Номер такта",
                "Загрузка процессора",
                "Очередь готовности"
            });
            _resultTable.PrintLine();

            foreach (var item in _system.TaskManager.TactDatas)
            {
                string queue = item.Queue.Length == 0 ? "-" : string.Empty;

                foreach (var task in item.Queue)
                {
                    queue += task.Name + " ";
                }

                _resultTable.PrintRow(new string[] {
                    item.Tact.ToString(),
                    item.Task == null ? "-" : item.Task.Name,
                    queue 
                });
            }

            _resultTable.PrintLine();

            Console.WriteLine();
        }

        private void PrintTable()
        {
            if(_system.TaskManager.Tasks.Count == 0)
            {
                Console.WriteLine("Все задачи выполнены...");
                Console.WriteLine();
                return;
            }

            _tasksTable.PrintLine();
            _tasksTable.PrintRow(new string[]
            {
                "Имя задачи",
                "Момент активации",
                "Время выполнения на процессоре",
                "Приоритет",
                "Состояние"
            });

            _tasksTable.PrintLine();

            foreach (var task in _system.TaskManager.Tasks)
            {
                PrintTask(task);
            }

            _tasksTable.PrintLine();

            Console.WriteLine();
        }

        private void PrintTask(Task task)
        {
            _tasksTable.PrintRow(new string[]
            {
                task.Name,
                task.StartTact.ToString(),
                task.TactCount.ToString(),
                task.Priority.ToString(),
                task.State.ToString()
            });
        }

        private void InputTask()
        {
            Console.Write("Введите название задачи -> ");
            var name = Console.ReadLine();
            Console.Write("Введите приоритет задачи -> ");
            var priority = double.Parse(Console.ReadLine());
            Console.Write("Введите коэффициент приоритета -> ");
            var coefficient = double.Parse(Console.ReadLine());
            Console.Write("Введите время исполнения в тактах -> ");
            var tackCount = int.Parse(Console.ReadLine());
            Console.Write("Введите время активации -> ");
            var startTact = int.Parse(Console.ReadLine());

            _system.AddTask(name, priority, coefficient, tackCount, startTact);
        }
    }
}
