using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrices
{
    // Only the lower triangle is stored
    public class SymmetricSparseMatrixDOK<T>
    {
        protected readonly SortedDictionary<int, T>[] data;

        public SymmetricSparseMatrixDOK(int order)
        {
            if (order < 1)
            {
                throw new ArgumentException("Order must be >= 1, but was: " + order);
            }
            Order = order;

            data = new SortedDictionary<int, T>[order];
            for (int row = 0; row < order; ++row)
            {
                data[row] = new SortedDictionary<int, T>();
            }
        }

        public int Order { get; }

        // O(order)
        public int NonZeroEntriesCount
        {
            get
            {
                int count = 0;
                foreach (var dictionary in data)
                {
                    count += dictionary.Count;
                }
                return count;
            }
        }

        public T this[int row, int col]
        {
            get
            {
                ProcessIndexes(ref row, ref col);
                if (data[row].ContainsKey(col)) return data[row][col];
                else return default(T);
            }
            set
            {
                ProcessIndexes(ref row, ref col);
                if (data[row].ContainsKey(col)) data[row][col] = value;
                else data[row].Add(col, value);
            }
        }

        public SymmetricSparseMatrixDOK<T> Slice(int[] rowsToKeep)
        {
            var keep = new SortedSet<int>(rowsToKeep); // sort and remove duplicates
            int[] oldToNew = new int[Order];
            int newIndex = 0;
            for (int i = 0; i < Order; ++i)
            {
                if (keep.Contains(i))
                {

                    oldToNew[i] = newIndex;
                    ++newIndex;
                }
                else oldToNew[i] = -1;
            }

            var newDOK = new SymmetricSparseMatrixDOK<T>(keep.Count);
            int newRow = 0;
            for (int row = 0; row < this.Order; ++row)
            {
                if (keep.Contains(row))
                {
                    foreach (var pair in this.data[row])
                    {
                        if (keep.Contains(pair.Key))
                        {
                            newDOK[newRow, oldToNew[pair.Key]] = pair.Value;
                        }
                    }
                    ++newRow;
                }
            }
            return newDOK;
        }

        public SparseMatrixCOO<T> ToCOO()
        {
            int nonZeros = NonZeroEntriesCount;
            int[] rowIndexes = new int[nonZeros];
            int[] columnIndexes = new int[nonZeros];
            T[] values = new T[nonZeros];

            int counter = 0;
            for (int row = 0; row < Order; ++row)
            {
                foreach (var pair in data[row])
                {
                    rowIndexes[counter] = row;
                    columnIndexes[counter] = pair.Key;
                    values[counter] = pair.Value;
                    ++counter;
                }
            }
            return new SparseMatrixCOO<T>(rowIndexes, columnIndexes, values);
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

        private void ProcessIndexes(ref int row, ref int col)
        {
            if (col > row)
            {
                int temp = row;
                row = col;
                col = temp;
            }
        }
    }
}
