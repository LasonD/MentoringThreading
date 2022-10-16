/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            Console.Write("Select example you want to run: ");

            switch (Console.ReadKey().KeyChar)
            {
                case 'a':
                    await OptionAAsync();
                    break;
                case 'b':
                    await OptionBAsync();
                    break;
                case 'c':
                    await OptionCAsync();
                    break;
                case 'd':
                    await OptionDAsync();
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }

        private static async Task OptionAAsync()
        {
            Console.WriteLine();
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");

            try
            {
                await RunTaskThatThrowsAsync().ContinueWith(t =>
                {
                    Console.WriteLine($"Executing continuation... Parent task status: {t.Status}");
                });

                await RunTaskThatRunsToCompletionAsync().ContinueWith(t =>
                {
                    Console.WriteLine($"Executing continuation... Parent task status: {t.Status}");
                });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception handled: {e.Message}");
            }
        }

        private static async Task OptionBAsync()
        {
            Console.WriteLine();
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");

            try
            {
                await RunTaskThatThrowsAsync().ContinueWith(
                    t =>
                    {
                        Console.WriteLine($"Executing continuation, because the parent didn't run to completion. Parent task status: {t.Status}");
                    }, TaskContinuationOptions.NotOnRanToCompletion);

                await RunTaskThatRunsToCompletionAsync().ContinueWith(
                    t =>
                    {
                        Console.WriteLine(
                            $"This should not be executed, because the parent task should have run to completion");
                    }, TaskContinuationOptions.NotOnRanToCompletion);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception handled: {e.Message}");
            }
        }

        private static async Task OptionCAsync()
        {
            Console.WriteLine();
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");

            try
            {
                await RunTaskThatThrowsAsync().ContinueWith(
                    t =>
                    {
                        Console.WriteLine($"Executing continuation on thread {Thread.CurrentThread.ManagedThreadId}, because the parent didn't run to completion. Parent task status: {t.Status}");
                    }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception handled: {e.Message}");
            }
        }

        private static async Task OptionDAsync()
        {
            Console.WriteLine();
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");

            var cts = new CancellationTokenSource();

            try
            {
                var workTask = RunTaskThatRunsToCompletionAsync(cts.Token).ContinueWith(
                    t =>
                    {
                        Console.WriteLine($"Executing continuation on isThreadPollTread: {Thread.CurrentThread.IsThreadPoolThread}, because the parent was cancelled. Parent task status: {t.Status}");
                    }, TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning);

                Console.Write("Press x to cancel the task: ");
                if (Console.ReadKey().KeyChar == 'x')
                {
                    cts.Cancel();
                }

                await workTask;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception handled: {e.Message}");
            }
        }

        private static Task RunTaskThatRunsToCompletionAsync(CancellationToken cancellationToken = default) => 
            Task.Run(() => DoSomethingAsync(10_000_000, 10_000_000, cancellationToken), cancellationToken);

        private static Task RunTaskThatThrowsAsync(CancellationToken cancellationToken = default) => 
            Task.Run(() => DoSomethingAsync(10_000_000, 9_999_999, cancellationToken), cancellationToken);

        private static async Task DoSomethingAsync(int iterations, int limit, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Starting work execution on thread {Thread.CurrentThread.ManagedThreadId}...");

            for (int i = 0; i < iterations; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (i % 1_000_000 == 0)
                {
                    Console.WriteLine($"Executing work... {i}");

                    await Task.Delay(500, cancellationToken);
                }

                if (i >= limit)
                {
                    throw new ArgumentOutOfRangeException($"Something went wrong on thread {Thread.CurrentThread.ManagedThreadId}...");
                }
            }

            Console.WriteLine($"Finished work execution on thread {Thread.CurrentThread.ManagedThreadId}...");
        }
    }
}
