using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magistr2
{
    class TextureRes
    {
        double[,] matrixFo = new double[5, 5];
        public double[] Calculation(Image img)
        {
            double[] res = new double[11];

            return res;
        }
        double MatrixCalculation(Image img)
        {
            double result = 0;
            for(int i=0;i<img.Height;i++)
            {
                for(int j=0;j<img.Width;j++)
                {

                }
            }
            return result;
        }
        double MatrixPower(double[,] matrix)
        {
            double resSum = 0;
            for(int i=0;i<matrix.Rank;i++)
                for(int j=0;j<matrix.Length;j++)
                {
                    resSum += matrix[i, j] * matrix[i, j];
                }
            return resSum;
        }
        double Contrast(double[,] matrix)
        {
            double resSum = 0;
            for (int i = 0; i < matrix.Rank; i++)
                for (int j = 0; j < matrix.Length; j++)
                {
                    resSum += matrix[i, j] * ((i+1)-(j+1))* ((i + 1) - (j + 1));
                }
            return resSum;
        }
    }
}
