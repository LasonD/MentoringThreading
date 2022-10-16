/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        private static readonly List<int> sharedCollection = new List<int>();

        static async Task Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            const int collectionLength = 10;

            var writeHandle = new AutoResetEvent(true);
            var readHandle = new AutoResetEvent(false);

            var fillCollectionTask = Task.Run(() =>
            {
                for (int i = 0; i < collectionLength; i++)
                {
                    writeHandle.WaitOne();

                    sharedCollection.Add(i);

                    readHandle.Set();
                }
            });

            var printCollectionTask = Task.Run(() =>
            {
                for (int i = 0; i < collectionLength; i++)
                {
                    readHandle.WaitOne();

                    foreach (var item in sharedCollection)
                    {
                        Console.Write($"{item}, ");
                    }

                    Console.WriteLine();
                    writeHandle.Set();
                }
            });

            await Task.WhenAll(fillCollectionTask, printCollectionTask);

            Console.ReadLine();
        }
    }
}
