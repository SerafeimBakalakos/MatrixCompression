using Matrices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace Abaqus
{
    public class SymmetricMatrixReader
    {
        private static readonly char[] separators = { };

        private readonly FileStream stream;
        private readonly StreamReader reader;

        public SymmetricMatrixReader(string path)
        {
            stream = new FileStream(path, FileMode.Open);
            reader = new StreamReader(stream);
        }

        public SymmetricSparseMatrixDOK<double> ReadCooMatrix()
        {
            int order = FindOrder();
            var dok = new SymmetricSparseMatrixDOK<double>(order);

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
    }
}
