using Matrices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Abaqus
{
    public class MatrixReader
    {
        private static readonly char[] separators = {};
        private static readonly double constrainSentinel = 1.0e36;
        private static readonly double tolerance = 1e-8;

        private readonly FileStream stream;
        private readonly StreamReader reader;

        public MatrixReader(string path)
        {
            stream = new FileStream(path, FileMode.Open);
            reader = new StreamReader(stream);
        }

        public SparseMatrixDOK ReadCOOMatrix()
        {
            int order = FindOrder();
            SparseMatrixDOK dok = new SparseMatrixDOK(order);

            string line;
            int row, col;
            double value;
            while ((line = reader.ReadLine()) != null)
            {
                ReadEntry(line, out row, out col, out value);
                dok[row - 1, col - 1] = value;
            }

            return dok;
        }

        public SparseMatrixDOK ReadFreeFreeCOOMatrix()
        {
            var stiffnessMatrix = ReadCOOMatrix();
            var freeDofs = FindFreeDofs(stiffnessMatrix);
            return stiffnessMatrix.Slice(freeDofs);
        }

        public int FindOrder()
        {
            int maxRow = 0;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                int row = ReadRow(line);
                if (row > maxRow) maxRow = row;
            }

            // Reset stream and reader
            stream.Position = 0;
            reader.DiscardBufferedData();

            return maxRow;
        }

        private IEnumerable<int> FindFreeDofs(SparseMatrixDOK stiffnessMatrix)
        {

            var freeDofs = new LinkedList<int>();
            for (int dof = 0; dof < stiffnessMatrix.Order; ++dof)
            {
                if (IsFreeDof(stiffnessMatrix[dof, dof])) freeDofs.AddLast(dof);
            }
            return freeDofs;
        }

        private int ReadRow(string line)
        {
            string row = line.Split(separators)[0];
            return Int32.Parse(row);
        }

        private void ReadEntry(string line, out int row, out int col, out double value)
        {
            string[] subStrings = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            row = Int32.Parse(subStrings[0]);
            col = Int32.Parse(subStrings[1]);
            value = Double.Parse(subStrings[2]);
        }

        private static bool IsFreeDof(double dofStiffness)
        {
            if (Math.Abs(1.0 - constrainSentinel / dofStiffness) < tolerance) return false;
            else return true; 
        }
    }
}
