namespace TaskManagerModel.Models
{
    /// <summary>
    /// Класс процессора.
    /// </summary>
    public class Processor
    {
        /// <summary>
        /// Текущий такт.
        /// </summary>
        public int Tact { get; private set; }

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
            task.Execute();

            Thread.Sleep(millisecondsTimeout: 1000);

            Tact++;
        }
    }
}
