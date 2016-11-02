using Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Abaqus.Tests
{
    class ReadMatrixExample
    {
        public static void Main()
        {
            var reader = new SymmetricMatrixReader(@"C:\Abaqus_workspace\Truss2DExample\Deformed_STIF2.mtx");
            int order = reader.FindOrder();
            SymmetricSparseMatrixDOK<double> matrix = reader.ReadCooMatrix();

            Console.WriteLine("order = " + order);
            for (int row = 0; row < order; ++row)
            {
                for (int col = 0; col < order; ++col)
                {
                    Console.WriteLine("K[{0}, {1}] = {2}", row, col, matrix[row, col]);
                }
            }
        }
    }
}
