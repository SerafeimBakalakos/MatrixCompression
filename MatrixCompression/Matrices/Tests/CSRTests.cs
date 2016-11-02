using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrices.Tests
{
    public class CSRTests
    {
        public static void Main()
        {
            var matrix = BuildMatrix();
            //Console.WriteLine(matrix);

            double[] x = { 2, -3, 7, 9, 4, -12 };
            double[] y = new double[matrix.Order];
            matrix.SpMV(x, y);
            CheckResult(y);
        }

        private static SparseMatrixCSR BuildMatrix()
        {
            int order = 6;
            double[] values = { 10, -2, 3, 9, 3, 7, 8, 7, 3, 8, 7, 5, 8, 9, 9, 13, 4, 2, -1 };
            int[] colIndexs = { 0,   4, 0, 1, 5, 1, 2, 3, 0, 2, 3, 4, 1, 3, 4,  5, 1, 4,  5 };
            int[] rowPointers = { 0, 2, 5, 8, 12, 16, 19 };
            return new SparseMatrixCSR(order, values, colIndexs, rowPointers);
        }

        private static string Array2String(double[] array)
        {
            var builder = new StringBuilder("{ ");
            foreach (var entry in array)
            {
                builder.Append(entry);
                builder.Append(' ');
            }
            builder.Append("}");
            return builder.ToString();
        }

        private static void CheckResult(double[] result)
        {
            double[] expected = { 12, -57, 98, 145, -63, 8 };
            double tolerance = 1e-6;

            bool isCorrect = true;
            for (int i = 0; i < result.Length; ++i)
            {
                if (Math.Abs((result[i] - expected[i]) / expected[i]) > tolerance)
                {
                    isCorrect = false;
                    break;
                }
            }
            if (isCorrect) Console.WriteLine("SpMV was correct.");
            else Console.WriteLine("Error in SpMV");
        }
    }
}
