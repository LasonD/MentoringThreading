/*
 * 1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.
 * Each Task should iterate from 1 to 1000 and print into the console the following string:
 * “Task #0 – {iteration number}”.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task1._100Tasks
{
    public class Program
    {
        public const int TaskAmount = 100;
        private const int MaxIterationsCount = 1000;

        private static async Task Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. Multi threading V1.");
            Console.WriteLine("1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.");
            Console.WriteLine("Each Task should iterate from 1 to 1000 and print into the console the following string:");
            Console.WriteLine("“Task #0 – {iteration number}”.");
            Console.WriteLine();
            
            await RunHundredTasksAsync();

            Console.ReadLine();
        }

        private static async Task RunHundredTasksAsync()
        {
            await Task.WhenAll(Enumerable.Range(0, TaskAmount).Select(x => Task.Run(() =>
            {
                for (var i = 1; i <= MaxIterationsCount; i++)
                {
                    Output(x, i);
                }
            })).ToArray());
        }

        private static void Output(int taskNumber, int iterationNumber)
        {
            Console.WriteLine($"Task #{taskNumber} – {iterationNumber}");
        }
    }
}
