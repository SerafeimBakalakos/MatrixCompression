using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrices
{
    public class SparseMatrixCSR
    {
        private readonly double[] values;
        private readonly int[] columnIndexes;
        private readonly int[] rowPointers;

        public SparseMatrixCSR(int order, double[] values, int[] columnIndexes, int[] rowPointers)
        {
            this.Order = order;
            this.NonZerosCount = values.Length;
            this.values = values;
            this.columnIndexes = columnIndexes;
            this.rowPointers = rowPointers;
        }

        public int Order { get; }
        public int NonZerosCount { get; }

        public double this[int row, int col]
        {
            get
            {
                int index = FindIndexOf(row, col);
                return (index == -1) ? 0.0 : values[index];
            }
        }

        public void SpMV(double[] vector, double[] result)
        {
            for (int row = 0; row < Order; ++row)
            {
                int rowStart = rowPointers[row];
                int nextRowStart = rowPointers[row + 1];
                double dotProduct = 0.0;
                for (int i = rowStart; i < nextRowStart; ++i)
                {
                    dotProduct += values[i] * vector[columnIndexes[i]];
                }
                result[row] = dotProduct;
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder("[\n");
            for (int row = 0; row < Order; ++row)
            {
                builder.Append("[ ");
                for (int col = 0; col < Order; ++col)
                {
                    builder.Append(this[row, col]);
                    builder.Append(' ');
                }
                builder.Append("]\n");
            }
            builder.Append("]");
            return builder.ToString();
        }

        private int FindIndexOf(int row, int col)
        {
            int rowStart = rowPointers[row];
            int nextRowStart = rowPointers[row+1];
            for (int i = rowStart; i < nextRowStart; ++i)
            {
                if (columnIndexes[i] == col) return i;
            }
            return -1;
        }
    }
}
