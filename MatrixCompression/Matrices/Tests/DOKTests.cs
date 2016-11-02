using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrices.Tets
{
    public class DOKTests
    {
        public static void Main()
        {
            TestSlicing();
            TestConversion();
        }

        private static void TestSlicing()
        {
            var dok = BuildMatrix();

            Console.WriteLine("Original: ");
            Console.WriteLine(dok);
            Console.WriteLine();

            var sliced1 = dok.Slice(new int[] { 1, 2, 3 });
            Console.WriteLine("Keep 1, 2, 3: ");
            Console.WriteLine(sliced1);
            Console.WriteLine();

            var sliced2 = dok.Slice(new int[] { 0, 3 });
            Console.WriteLine("Keep 0, 3: ");
            Console.WriteLine(sliced2);
            Console.WriteLine();

            var sliced3 = dok.Slice(new int[] { 0, 4 });
            Console.WriteLine("Keep 0, 4: ");
            Console.WriteLine(sliced3);
            Console.WriteLine();

            var sliced4 = dok.Slice(new int[] { 0, 1, 2, 3, 4 });
            Console.WriteLine("Keep all: ");
            Console.WriteLine(sliced4);
            Console.WriteLine();
        }

        private static void TestConversion()
        {
            var dok = BuildMatrix();
            Console.WriteLine("DOK: ");
            Console.WriteLine(dok);
            Console.WriteLine();

            var csr = dok.ToCSR();
            Console.WriteLine("CSR: ");
            Console.WriteLine(csr);
            Console.WriteLine();
        }

        private static SparseMatrixDOK BuildMatrix()
        {
            int order = 5;
            var dok = new SparseMatrixDOK(order);
            dok[0, 0] = 1; dok[0, 3] = 2;
            dok[1, 1] = 3; dok[1, 2] = 4; dok[1, 4] = 5;
            dok[2, 2] = 6; dok[2, 3] = 7;
            dok[3, 1] = 5; dok[3, 3] = 8;
            dok[4, 0] = 2; dok[4, 2] = 7; dok[4, 4] = 9;

            return dok;
        }
    }
}
