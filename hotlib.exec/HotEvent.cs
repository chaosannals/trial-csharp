namespace HotLib.Exec
{
    public class HotEvent
    {
        public string Message { get; set; }
    }

    public delegate void HotEventHandler();

    public static class HotEventQueue
    {
        public static event HotEventHandler Hoted;

        public static void Run() {
            Hoted.Invoke();
        }
    }
}