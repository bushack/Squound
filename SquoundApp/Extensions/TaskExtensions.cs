using System.Diagnostics;


namespace SquoundApp.Extensions
{
    public static class TaskExtensions
    {
        public static void FireAndForget(this Task task) =>
            task.ContinueWith(t => Debug.WriteLine(t.Exception), TaskContinuationOptions.OnlyOnFaulted);
    }
}
