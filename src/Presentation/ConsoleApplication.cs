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
            _system.AddTask("Task1", 0, 2, 0);
            _system.AddTask("Task2", 1, 4, 0);
            _system.AddTask("Task3", 2, 3, 9);

            _system.Start();
        }

        private void OnProcessorUpdate()
        {
            Console.WriteLine(_system.Processor.Tact);
        }
    }
}
