using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiThreading.Task3.MatrixMultiplier.Matrices;
using MultiThreading.Task3.MatrixMultiplier.Multipliers;

namespace MultiThreading.Task3.MatrixMultiplier.Tests
{
    [TestClass]
    public class MultiplierTest
    {
        [TestMethod]
        public void MultiplyMatrix3On3Test()
        {
            TestMatrix3On3(new MatricesMultiplier());
            TestMatrix3On3(new MatricesMultiplierParallel());
        }

        [TestMethod]
        public void ParallelEfficiencyTest()
        {
            // var sw = new StreamWriter(File.OpenWrite(@"C:\Users\Legion\Desktop\MatrixTest.txt"));
            // Console.SetOut(sw);

            var multipliers = new IMatricesMultiplier[]
            {
                new MatricesMultiplier(),
                new MatricesMultiplierParallel(),
                //new MatricesMultiplierParallel2(),
            };

            var results = new long[2];

            for (int i = 1; i < 300; i++)
            {
                results = MeasureMultiplicationTimeForMatrixSize(i, multipliers).ToArray();

                if (results[0] > results[1]) break;

                // Console.WriteLine($"{multipliers[0].GetType().Name} took {results[0]} to multiply {i}x{i} matrix");
                // Console.WriteLine($"{multipliers[1].GetType().Name} took {results[1]} to multiply {i}x{i} matrix");
                // Console.WriteLine($"{multipliers[2].GetType().Name} took {results[2]} to multiply {i}x{i} matrix");
                // Console.WriteLine();
            }

            Assert.IsTrue(results[0] > results[1]);

            //sw.Dispose();
        }

        private static IEnumerable<long> MeasureMultiplicationTimeForMatrixSize(int sizeRowCol, IEnumerable<IMatricesMultiplier> multipliers)
        {
            var matrix1 = new Matrix(sizeRowCol, sizeRowCol, true);
            var matrix2 = new Matrix(sizeRowCol, sizeRowCol, true);

            foreach (var multiplier in multipliers)
            {
                yield return MeasureMultiplicationTime(matrix1, matrix2, multiplier);
            }
        }

        private static long MeasureMultiplicationTime(IMatrix first, IMatrix second, IMatricesMultiplier multiplier) 
        {
            var sw = new Stopwatch();

            sw.Start();
            multiplier.Multiply(first, second);
            sw.Stop();

            return sw.ElapsedMilliseconds;
        }

        #region private methods

        void TestMatrix3On3(IMatricesMultiplier matrixMultiplier)
        {
            if (matrixMultiplier == null)
            {
                throw new ArgumentNullException(nameof(matrixMultiplier));
            }

            var m1 = new Matrix(3, 3);
            m1.SetElement(0, 0, 34);
            m1.SetElement(0, 1, 2);
            m1.SetElement(0, 2, 6);

            m1.SetElement(1, 0, 5);
            m1.SetElement(1, 1, 4);
            m1.SetElement(1, 2, 54);

            m1.SetElement(2, 0, 2);
            m1.SetElement(2, 1, 9);
            m1.SetElement(2, 2, 8);

            var m2 = new Matrix(3, 3);
            m2.SetElement(0, 0, 12);
            m2.SetElement(0, 1, 52);
            m2.SetElement(0, 2, 85);

            m2.SetElement(1, 0, 5);
            m2.SetElement(1, 1, 5);
            m2.SetElement(1, 2, 54);

            m2.SetElement(2, 0, 5);
            m2.SetElement(2, 1, 8);
            m2.SetElement(2, 2, 9);

            var multiplied = matrixMultiplier.Multiply(m1, m2);
            Assert.AreEqual(448, multiplied.GetElement(0, 0));
            Assert.AreEqual(1826, multiplied.GetElement(0, 1));
            Assert.AreEqual(3052, multiplied.GetElement(0, 2));

            Assert.AreEqual(350, multiplied.GetElement(1, 0));
            Assert.AreEqual(712, multiplied.GetElement(1, 1));
            Assert.AreEqual(1127, multiplied.GetElement(1, 2));

            Assert.AreEqual(109, multiplied.GetElement(2, 0));
            Assert.AreEqual(213, multiplied.GetElement(2, 1));
            Assert.AreEqual(728, multiplied.GetElement(2, 2));
        }

        #endregion
    }
}
