/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Collections.Generic;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        const int NumOfThreads = 10;

        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            OptionA();
            

            Console.ReadLine();
        }

        private static void OptionA()
        {
            var threads = new List<Thread>();

            CreateThreadInternal(NumOfThreads, threads);

            foreach (var thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine("After threads join");
        }

        private static void OptionB()
        {
            var semaphore = new SemaphoreSlim(NumOfThreads, NumOfThreads);
            

            QueueThreadPoolWorkItem(NumOfThreads);
        }

        private static void QueueThreadPoolWorkItem(int value)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                var i = (int)state;

                Console.WriteLine($"Thread #{Thread.CurrentThread.ManagedThreadId}: state {i}");

                Thread.Sleep(500);

                if (i-- <= 0)
                {
                    return;
                }

                QueueThreadPoolWorkItem(i);

            }, value);
        }

        private static void CreateThreadInternal(int state, ICollection<Thread> threads)
        {
            var thread = new Thread(value =>
            {
                var i = (int)value;
            
                Console.WriteLine($"Thread #{Thread.CurrentThread.ManagedThreadId}: state {i}");
            
                if (i-- <= 0)
                {
                    return;
                }
            
                CreateThreadInternal(i, threads);
            
                // some work
                Thread.Sleep((1 / i + 2) * 1000);
            });
            
            thread.Start(state);
        }

        // private static Thread CreateThread(int value)
        // {
        //     var thread = new Thread(state =>
        //     {
        //         var i = (int)state;
        //
        //         Console.WriteLine($"Thread #{Thread.CurrentThread.ManagedThreadId}: state {i}");
        //
        //         if (i-- <= 0)
        //         {
        //             return;
        //         }
        //
        //         var inner = CreateThread(i);
        //
        //         // some work
        //         Thread.Sleep(i * 100);
        //
        //         inner.Join();
        //     });
        //
        //     thread.Start(value);
        //
        //     return thread;
        // }
    }
}
