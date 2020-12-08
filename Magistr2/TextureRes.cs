using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Magistr2
{
    class TextureRes
    {
        /// <summary>
        /// Расчёт текстурных показателей
        /// </summary>
        /// <param name="matrix">Матрица яркостей</param>
        /// <param name="qvant">Цветовой спектр</param>
        /// <param name="rst">Спектр с количеством пикселей каждой яркости</param>
        /// <returns>Текстурные признаки</returns>
        public double[] Calculation(int[,] matrix, List<int> qvant, int[] rst)
        {
            if (qvant[0] == 0)
                qvant.Remove(0);
            masslength = qvant.Count();
            int[,] graycl = GrayClasses(matrix, qvant);
            double[,] resMat = MatrixCalculation(graycl);
            double[] res = new double[11];
            res[0] = MatrixPower(resMat);
            res[1] = Contrast(resMat);
            res[2] = Correl(resMat);
            res[3] = Disper(resMat);
            res[4] = Odnorod(resMat);
            res[5] = SumSr(resMat);
            res[6] = SumDisp(resMat);
            res[7] = SumEntr(resMat);
            res[8] = Entrop(resMat);
            res[9] = diffDisp(resMat);
            res[10] = diffEntr(resMat);
            return res;
        }
        #region Работа с матрицей
        public int[,] ConvertImgToMatrix(Bitmap img)
        {
            int[,] matrix = new int[img.Width, img.Height];
            for (int i = 0; i < img.Height; i++)
            {
                for (int j = 0; j < img.Width; j++)
                {
                    matrix[i, j] = img.GetPixel(j, i).R;
                }
            }
            return matrix;
        }
        int[,] GrayClasses(int[,] matrix, List<int> qvant)
        {
            int[,] matrixGrayCl = new int[matrix.GetUpperBound(0) + 1, matrix.Length / (matrix.GetUpperBound(0) + 1)];
            for (int i = 0; i < (matrix.GetUpperBound(0) + 1); i++)
            {
                for (int j = 0; j < matrix.Length / (matrix.GetUpperBound(0) + 1); j++)
                {
                    if (matrix[i, j] <= qvant[0])
                        matrixGrayCl[i, j] = 0;
                    if (matrix[i, j] > qvant[0] && matrix[i, j] <= qvant[1])
                        matrixGrayCl[i, j] = 1;
                    if (matrix[i, j] > qvant[1] && matrix[i, j] <= qvant[2])
                        matrixGrayCl[i, j] = 2;
                    if (matrix[i, j] > qvant[2] && matrix[i, j] <= qvant[3])
                        matrixGrayCl[i, j] = 3;
                    if (matrix[i, j] > qvant[3])
                        matrixGrayCl[i, j] = 4;
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
                            if (matrix[j, i] == m && matrix[j, i + 1] == p)
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
                    sum += (int)result[m, p];
                }
            }
            for (int m = 0; m < result.GetUpperBound(0) + 1; m++)
            {
                for (int p = 0; p < result.Length / (result.GetUpperBound(0) + 1); p++)
                {
                    result[m, p] = Math.Round(result[m, p] / sum, 2, MidpointRounding.AwayFromZero);
                }
            }
            return result;
        }
        #endregion
        #region Текстурные признаки
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
            for (int i = 0; i < (matrix.GetUpperBound(0) + 1); i++)
                for (int j = 0; j < matrix.Length / (matrix.GetUpperBound(0) + 1); j++)
                {
                    resSum += matrix[i, j] * ((i + 1) * (i + 1) - 2 * (i + 1) * (j + 1) + (j + 1) * (j + 1));
                }
            return resSum;
        }
        double Correl(double[,] matrix)
        {
            double entr = 0, left = 0, right = 0;
            var hx = midleqvadrx(matrix);
            var hy = midleqvadry(matrix);
            var dx = srqvadrx(matrix, hx);
            var dy = srqvadry(matrix, hy);
            for (int i = 0; i < (matrix.GetUpperBound(0) + 1); i++)
            {
                for (int j = 0; j < matrix.Length / (matrix.GetUpperBound(0) + 1); j++)
                {
                    left = ((i + 1) * (j + 1)) * matrix[i, j] - (hx[i] * hy[j]);
                    right = (dx[i] * dy[j]);
                    if (left != 0 && right != 0)
                        entr += left / right;
                }
            }
            return entr;
        }
        double Disper(double[,] matrix)
        {
            double res = 0;
            var sr = matrixsr(matrix);
            for (int i = 0; i < (matrix.GetUpperBound(0) + 1); i++)
            {
                for (int j = 0; j < matrix.Length / (matrix.GetUpperBound(0) + 1); j++)
                {
                    res += ((i + 1) - sr) * ((i + 1) - sr) * matrix[i, j];
                }
            }
            return res;
        }
        double Odnorod(double[,] matrix)
        {
            double res = 0;
            for (int i = 0; i < (matrix.GetUpperBound(0) + 1); i++)
            {
                for (int j = 0; j < matrix.Length / (matrix.GetUpperBound(0) + 1); j++)
                {
                    res += (1 / (1 + ((i + 1) * (i + 1) - 2 * (i + 1) * (j + 1) + (j + 1) * (j + 1)))) * matrix[i, j];
                }
            }
            return res;
        }
        double SumSr(double[,] matrix)
        {
            double res = 0;
            var sr = VectSum(matrix);
            for (int i = 0; i < (matrix.GetUpperBound(0) + 1); i++)
            {
                for (int j = 0; j < matrix.Length / (matrix.GetUpperBound(0) + 1); j++)
                {
                    res += ((i + 1) + (j + 1)) * sr[i + j];
                }
            }
            return res;
        }
        double SumEntr(double[,] matrix)
        {
            double res = 0, left = 0, right = 0;
            var sr = VectSum(matrix);
            for (int i = 0; i < (matrix.GetUpperBound(0) + 1); i++)
            {
                for (int j = 0; j < matrix.Length / (matrix.GetUpperBound(0) + 1); j++)
                {
                    left = sr[i + j];
                    right = Math.Log(sr[i + j]);
                    if (left != 0 && right != 0)
                        res += left * right;
                }
            }
            return -res;
        }
        double SumDisp(double[,] matrix)
        {
            double res = 0, left = 0, right = 0; ;
            var sr = VectSum(matrix);
            var f = SumEntr(matrix);
            for (int k = 0; k < sr.Length; k++)
            {
                left = ((k + 1) - f) * ((k + 1) - f);
                if (left != 0 && sr[k] != 0)
                    res += left * sr[k];
            }
            return res;
        }
        double Entrop(double[,] matrix)
        {
            double res = 0, right = 0;
            for (int i = 0; i < (matrix.GetUpperBound(0) + 1); i++)
            {
                for (int j = 0; j < matrix.Length / (matrix.GetUpperBound(0) + 1); j++)
                {
                    right = Math.Log(matrix[i, j]);
                    if (matrix[i, j] != 0 && right != 0)
                        res += matrix[i, j] * right;
                }
            }
            return -res;
        }
        double diffDisp(double[,] matrix)
        {
            double res = 0;
            var sr = UnVectSum(matrix);
            var h = matrixsr(matrix);
            for (int k = 0; k < sr.Length; k++)
            {
                res += (sr[k] - h) * (sr[k] - h);
            }
            return res / (sr.Length - 2);
        }
        double diffEntr(double[,] matrix)
        {
            double res = 0, right = 0;
            var sr = UnVectSum(matrix);
            for (int k = 0; k < sr.Length; k++)
            {
                right = Math.Log(sr[k]);
                if (sr[k] != 0 && right != 0)
                    res += sr[k] * right;
            }
            return -res;
        }
        #endregion
        #region Вспомогательные функции
        double[] midleqvadrx(double[,] matrix)
        {
            double[] res = new double[(matrix.GetUpperBound(0) + 1)];
            for (int i = 0; i < (matrix.GetUpperBound(0) + 1); i++)
            {
                for (int j = 0; j < matrix.Length / (matrix.GetUpperBound(0) + 1); j++)
                {
                    res[i] += matrix[i, j];
                }
                res[i] = res[i] / (matrix.GetUpperBound(0) + 1);
            }
            return res;
        }
        double[] midleqvadry(double[,] matrix)
        {
            double[] res = new double[(matrix.Length / (matrix.GetUpperBound(0) + 1))];

            for (int j = 0; j < matrix.Length / (matrix.GetUpperBound(0) + 1); j++)
            {
                for (int i = 0; i < (matrix.GetUpperBound(0) + 1); i++)
                {
                    res[j] += matrix[i, j];
                }
                res[j] = res[j] / (matrix.Length / (matrix.GetUpperBound(0) + 1));
            }
            return res;
        }
        double[] srqvadrx(double[,] matrix, double[] srarx)
        {
            double[] qvadx = new double[(matrix.GetUpperBound(0) + 1)];
            for (int i = 0; i < (matrix.GetUpperBound(0) + 1); i++)
            {
                for (int j = 0; j < matrix.Length / (matrix.GetUpperBound(0) + 1); j++)
                {
                    qvadx[i] += (matrix[i, j] - srarx[i]) * (matrix[i, j] - srarx[i]);
                }
                qvadx[i] = Math.Sqrt(qvadx[i] / (matrix.GetUpperBound(0) + 1));
            }
            return qvadx;
        }
        double[] srqvadry(double[,] matrix, double[] srary)
        {
            double[] qvady = new double[(matrix.Length / (matrix.GetUpperBound(0) + 1))];
            for (int j = 0; j < matrix.Length / (matrix.GetUpperBound(0) + 1); j++)
            {
                for (int i = 0; i < (matrix.GetUpperBound(0) + 1); i++)
                {
                    qvady[j] += (matrix[i, j] - srary[j]) * (matrix[i, j] - srary[j]);
                }
                qvady[j] = Math.Sqrt(qvady[j] / (matrix.Length / (matrix.GetUpperBound(0) + 1)));
            }
            return qvady;
        }
        double matrixsr(double[,] matrix)
        {
            double sr = 0;
            double allelement = (matrix.GetUpperBound(0) + 1) * matrix.Length / (matrix.GetUpperBound(0) + 1);
            for (int i = 0; i < (matrix.GetUpperBound(0) + 1); i++)
            {
                for (int j = 0; j < matrix.Length / (matrix.GetUpperBound(0) + 1); j++)
                {
                    sr += matrix[i, j];
                }
            }
            return sr / allelement;
        }
        double[] VectSum(double[,] matrix)
        {
            double[] res = new double[(matrix.GetUpperBound(0) + 1) + matrix.Length / (matrix.GetUpperBound(0) + 1)];
            for (int k = 0; k < res.Length; k++)
            {
                for (int i = 0; i < (matrix.GetUpperBound(0) + 1); i++)
                {
                    for (int j = 0; j < matrix.Length / (matrix.GetUpperBound(0) + 1); j++)
                    {
                        if (k == (i + j))
                            res[k] += matrix[i, j];
                    }
                }
            }
            return res;
        }
        double[] UnVectSum(double[,] matrix)
        {
            double[] res = new double[(matrix.GetUpperBound(0) + 1) + matrix.Length / (matrix.GetUpperBound(0) + 1)];
            for (int k = 0; k < res.Length; k++)
            {
                for (int i = (matrix.GetUpperBound(0) + 1) - 1; i >= 0; i--)
                {
                    for (int j = matrix.Length / (matrix.GetUpperBound(0) + 1) - 1; j >= 0; j--)
                    {
                        if (k == (i - j))
                            res[k] += matrix[i, j];
                    }
                }
            }
            return res;
        }
        #endregion
    }
}
