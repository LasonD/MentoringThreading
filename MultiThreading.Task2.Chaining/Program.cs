/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    public static class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            // feel free to add your code

            await AwaitStyle();
            await ContinueWithStyle();

            Console.ReadLine();
        }

        private static async Task<double> AwaitStyle()
        {
            const int arrayLength = 10;

            var array = await Task.Run(() => GenerateRandomArray(arrayLength));
            var multipliedArray = await Task.Run(() => MultiplyArrayByRandomNumber(array));
            var orderedArray = await Task.Run(() => SortArrayAscending(multipliedArray));
            var average = await Task.Run(() => CalcAverage(orderedArray));

            return average;
        }

        private static async Task<double> ContinueWithStyle()
        {
            const int arrayLength = 10;

            return await Task.Run(() => GenerateRandomArray(arrayLength))
                .ContinueWith(t => MultiplyArrayByRandomNumber(t.Result))
                .ContinueWith(t => SortArrayAscending(t.Result))
                .ContinueWith(t => CalcAverage(t.Result));
        }

        private static int[] GenerateRandomArray(int length)
        {
            var rnd = new Random();

            return Enumerable.Range(0, 10).Select(i => rnd.Next()).ToArray().ReportToConsole($"Generating a random array of {length} integers:");
        }

        private static int[] MultiplyArrayByRandomNumber(int[] originalArr)
        {
            var rnd = new Random();
            var multiplier = rnd.Next();

            return originalArr.Select(x => x * multiplier).ToArray().ReportToConsole($"Multiplying array by integer {multiplier}:");
        }

        private static int[] SortArrayAscending(int[] originalArr)
        {
            return originalArr.OrderBy(x => x).ToArray().ReportToConsole("Sorting array ascending:");
        }

        private static double CalcAverage(int[] arr)
        {
            var average = arr.Average();

            Console.WriteLine($"Average: {average}");

            return average;
        }

        private static int[] ReportToConsole(this int[] arr, string caption)
        {
            Console.WriteLine(caption);

            foreach (var item in arr.Take(arr.Length - 1))
            {
                Console.Write($"{item}, ");
            }

            Console.WriteLine(arr.Last());

            return arr;
        }
    }
}
