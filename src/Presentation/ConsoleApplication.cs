using TaskManagerModel.Infrastructure;

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

            _tasksTable = new ConsoleTable(new int[] { 10, 16, 30, 9 });

            _resultTable = new ConsoleTable(new int[] { 11, 19, 18 });

            _system.Processor.OnUpdate += OnProcessorUpdate;
        }

        ~ConsoleApplication()
        {
            _system.Processor.OnUpdate -= OnProcessorUpdate;
        }

        public void Run()
        {
            Console.Write("Введите интервал через который будет меняться приоритет -> ");
            var interval = int.Parse(Console.ReadLine());

            _system.TaskManager.SetInterval(interval);

            Console.Write("Введите количество задач -> ");
            var count = int.Parse(Console.ReadLine());

            Console.WriteLine();

            for (int i = 0; i < count; i++)
            {
                InputTask();

                Console.WriteLine();
            }

            _system.Start();
        }

        private void OnProcessorUpdate()
        {
            Console.WriteLine(_system.Processor.Tact);

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
                    queue += task.ToString() + " ";
                }

                _resultTable.PrintRow(new string[] {
                    item.Tact.ToString(),
                    item.Task == null ? "-" : item.Task.Name,
                    queue 
                });
            }

            _resultTable.PrintLine();
        }

        private void PrintTable()
        {
            _tasksTable.PrintLine();
            _tasksTable.PrintRow(new string[]
            {
                "Имя задачи",
                "Момент активации",
                "Время выполнения на процессоре",
                "Приоритет"
            });
            _tasksTable.PrintLine();
            foreach (var task in _system.TaskManager.TaskQueue)
            {
                PrintTask(task);
            }
            _tasksTable.PrintLine();
        }
        private void PrintTask(Task task)
        {
            _tasksTable.PrintRow(new string[]
            {
                task.Name,
                task.StartTact.ToString(),
                task.TactCount.ToString(),
                task.Priority.ToString()
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
