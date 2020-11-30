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
        public double[] Calculation(int[,] matrix)
        {
            int[,] graycl = GrayClasses(matrix);
            double[,] resMat = MatrixCalculation(graycl);
            double[] res = new double[11];
            res[0] = MatrixPower(resMat);
            return res;
        }
        int[,] GrayClasses(int[,] matrix)
        {
            int[,] matrixGrayCl = new int[matrix.GetUpperBound(0) + 1, matrix.Length / (matrix.GetUpperBound(0) + 1)];
            for (int i = 0; i < (matrix.GetUpperBound(0) + 1); i++)
            {
                for (int j = 0; j < matrix.Length / (matrix.GetUpperBound(0) + 1); j++)
                {
                    if (matrix[i, j] <= 93)
                    {
                        matrixGrayCl[i, j] = 0;
                    }
                    if (matrix[i, j] > 93 && matrix[i, j] <= 115)
                    { matrixGrayCl[i, j] = 1; }
                    if (matrix[i, j] > 115 && matrix[i, j] <= 137)
                    { matrixGrayCl[i, j] = 2; }
                    if (matrix[i, j] > 137 && matrix[i, j] <= 159)
                    { matrixGrayCl[i, j] = 3; }
                    if (matrix[i, j] > 159)
                    { matrixGrayCl[i, j] = 4; }
                }
            }
            return matrixGrayCl;
        }
        double[,] MatrixCalculation(int[,] matrix)
        {
            double[,] result = new double[5, 5];

            for (int m = 0; m < (result.GetUpperBound(0) + 1); m++)
            {
                for (int p = 0; p < result.Length / (result.GetUpperBound(0) + 1); p++)
                {
                    //Расчёт коэффициентов
                    for (int i = 0; i < (matrix.GetUpperBound(0) + 1) - 1; i++)
                    {
                        for (int j = 0; j < matrix.Length / (matrix.GetUpperBound(0) + 1); j++)
                        {
                            if (matrix[j, i] == m && matrix[j, i+1] == p)
                                result[m, p] += 1;
                            else
                                result[m, p] += 0;
                        }
                    }
                }
            }
            int sum = 0;
            for (int m = 0; m < result.GetUpperBound(0) + 1; m++)
            {
                for (int p = 0; p < result.Length / (result.GetUpperBound(0) + 1); p++)
                {
                    sum +=(int) result[m, p];
                }
            }
            for (int m = 0; m < result.GetUpperBound(0) + 1; m++)
            {
                for (int p = 0; p < result.Length / (result.GetUpperBound(0) + 1); p++)
                {
                    result[m, p] = result[m, p] / sum;
                }
            }
            return result;
        }
        double MatrixPower(double[,] matrix)
        {
            double resSum = 0;
            for (int i = 0; i < (matrix.GetUpperBound(0) + 1); i++)
                for (int j = 0; j < matrix.Length / (matrix.GetUpperBound(0) + 1); j++)
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
                    resSum += matrix[i, j] * ((i + 1) - (j + 1)) * ((i + 1) - (j + 1));
                }
            return resSum;
        }
    }
}
