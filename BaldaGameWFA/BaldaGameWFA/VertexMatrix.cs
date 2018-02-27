using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaldaGameWFA
{
    public class VertexMatrix
    {
        bool[,] vertexMatrix;
        int size;

        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        public bool this[int row, int col]
        {
            get { return vertexMatrix[row, col]; }
            set
            {
                if ((row >= 0 && row < size) || (col >= 0 && col < size))
                {
                    vertexMatrix[row, col] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public VertexMatrix(int size)
        {
            Size = size;
            vertexMatrix = new bool[Size, Size];

            SetVertexMatrix();
        }
        
        private void SetVertexMatrix()
            // Setting connections between all the vertexes at vertexMatrix (both ways).
        {
            int koef = (int)Math.Sqrt(Size);
            for (int row = 0; row <= vertexMatrix.GetUpperBound(0); row++)
            {
                for (int col = 0; col <= vertexMatrix.GetUpperBound(1) - 1; col++)
                {
                    if ((col + 1) % koef == 0)
                    {
                        col++;
                    }
                    vertexMatrix[col, col + 1] = true;
                    vertexMatrix[col + 1, col] = true;
                }
            }

            for (int col = 0; col <= vertexMatrix.GetUpperBound(1); col++)
            {
                for (int row = 0; row <= vertexMatrix.GetUpperBound(0) - koef; row++)
                {
                    vertexMatrix[row, row + koef] = true;
                    vertexMatrix[row + koef, row] = true;
                }
            }
        }
    }
}
