using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrices
{
    public class SparseMatrixCOO<T>
    {
        private readonly int nonZeroEntriesCount;
        private readonly int[] rowIndexes;
        private readonly int[] columnIndexes;
        private readonly T[] values;

        public SparseMatrixCOO(int[] rowIndexes, int[] columnIndexes, T[] values)
        {
            this.rowIndexes = rowIndexes;
            this.columnIndexes = columnIndexes;
            this.values = values;
            this.nonZeroEntriesCount = values.Length;
        }

        public T this[int row, int col]
        {
            get
            {
                int index = FindIndexOf(row, col);
                return (index == -1) ? default(T) : values[index];
            }
            set
            {
                int index = FindIndexOf(row, col);
                if (index != -1) values[index] = value;
                else throw new IndexOutOfRangeException("Cannot overwrite a zero entry");
            }
        }

        private int FindIndexOf(int row, int col)
        {
            int firstIndex = Array.IndexOf(rowIndexes, row);
            int lastIndex = Array.LastIndexOf(rowIndexes, row, firstIndex + 1);
            for (int i = firstIndex; i <= lastIndex; ++i)
            {
                if (columnIndexes[i] == col) return i;
            }
            return -1;
        }
    }
}
