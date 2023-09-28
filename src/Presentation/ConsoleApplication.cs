using TaskManagerModel.Infrastructure;

namespace TaskManagerModel.Presentation
{
    public class ConsoleApplication
    {
        private readonly VirtualSystem _system;

        public ConsoleApplication(VirtualSystem system)
        {
            _system = system;

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
