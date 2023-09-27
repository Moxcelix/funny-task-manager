using TaskManagerModel.Infrastructure;
using TaskManagerModel.Presentation;

namespace TaskManagerModel
{
    public class Programm
    {
        public static void Main(string[] args)
        {
            var system = new VirtualSystem();
            var app = new ConsoleApplication(system);
            
            app.Run();
        }
    }
}