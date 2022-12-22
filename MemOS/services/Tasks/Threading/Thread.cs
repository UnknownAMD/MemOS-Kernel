namespace MemOS.Emulation.Threading
{
    public class Thread
    {
        internal readonly Executable Task;
        public Thread(Executable exec)
        {
            this.Task = exec;
        }
        public Thread(byte[] exec)
        {
            this.Task = new Executable(exec);
        }

        public void Start()
        {
            TaskManager.Start(this);
        }

    }
}
