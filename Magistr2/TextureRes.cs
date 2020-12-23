using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Magistr2
{
    class TextureRes
    {
        int masslength = 0;
        /// <summary>
        /// Расчёт текстурных показателей
        /// </summary>
        /// <param name="matrix">Матрица яркостей</param>
        /// <param name="qvant">Цветовой спектр</param>
        /// <param name="rst">Спектр с количеством пикселей каждой яркости</param>
        /// <returns>Текстурные признаки</returns>
        public double[] Calculation(int[,] matrix, List<int> qvant, int[] rst)
        {
            masslength = qvant.Count();
            int[,] graycl = GrayClasses(matrix, qvant);
            double[,] resMat = MatrixCalculation(graycl);
            double[] res = new double[7];
            res[0] = MatrixPower(resMat);
            res[1] = Correl(resMat);
            res[2] = Disper(resMat);
            res[3] = SumSr(resMat);
            res[4] = SumDisp(resMat);
            res[5] = SumEntr(resMat);
            res[6] = Entrop(resMat);
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
        public int[,] ConvertImgToMatrix(Bitmap img,int fx)
        {
            int[,] matrix = new int[img.Width, img.Height];
            for (int i = 0; i < img.Height; i++)
            {
                for (int j = 0; j < img.Width; j++)
                {
                    if (img.GetPixel(j, i).R + fx > 255)
                        matrix[i, j] = 255;
                    else if(img.GetPixel(j, i).R + fx < 0)
                        matrix[i, j] = 0;
                   else
                        matrix[i, j] = img.GetPixel(j, i).R+fx;
                }
            }
            return matrix;
        }
        public int[,] GrayClasses(int[,] matrix, List<int> qvant)
        {
            int step = qvant.Count() / 4;
            int[,] matrixGrayCl = new int[matrix.GetUpperBound(0) + 1, matrix.Length / (matrix.GetUpperBound(0) + 1)];
            for (int i = 0; i < (matrix.GetUpperBound(0) + 1); i++)
            {
                for (int j = 0; j < matrix.Length / (matrix.GetUpperBound(0) + 1); j++)
                {
                    if (matrix[i, j] != -1)
                    {
                        if (matrix[i, j] <= qvant[step])
                            matrixGrayCl[i, j] = 0;
                        if (matrix[i, j] > qvant[step] && matrix[i, j] <= qvant[step * 2])
                            matrixGrayCl[i, j] = 1;
                        if (matrix[i, j] > qvant[step * 2] * 2 && matrix[i, j] <= qvant[step * 3])
                            matrixGrayCl[i, j] = 2;
                        if (matrix[i, j] > qvant[step * 3] && matrix[i, j] <= qvant[step * 4 - 1])
                            matrixGrayCl[i, j] = 3;
                        if (matrix[i, j] > qvant[step * 4 - 1])
                            matrixGrayCl[i, j] = 4;
                    }
                }
            }
            return matrixGrayCl;
        }
        public double[,] MatrixCalculation(int[,] matrix)
        {
            double[,] result = new double[5, 5];

            for (int m = 0; m < (result.GetUpperBound(0) + 1); m++)
            {
                for (int p = 0; p < result.Length / (result.GetUpperBound(0) + 1); p++)
                {
                    int test = matrix.GetUpperBound(0)+1;
                    int test2 = (matrix.Length / (matrix.GetUpperBound(0)+1));
                    //Расчёт коэффициентов
                    for (int i = 0; i < (matrix.Length / (matrix.GetUpperBound(0)+1))-1; i++)
                    {
                        for (int j = 0; j <  matrix.GetUpperBound(0)+1; j++)
                        {
                            if (matrix[j, i] != -1)
                            {
                                if (matrix[j, i] == m && matrix[j, i + 1] == p)
                                    result[m, p] += 1;
                                else
                                    result[m, p] += 0;
                            }
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
        public double MatrixPower(double[,] matrix)
        {
            double resSum = 0;
            for (int i = 0; i < (matrix.GetUpperBound(0) + 1); i++)
                for (int j = 0; j < matrix.Length / (matrix.GetUpperBound(0) + 1); j++)
                {
                    resSum += matrix[i, j] * matrix[i, j];
                }
            return resSum;
        }
        public double Correl(double[,] matrix)
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
        public double Disper(double[,] matrix)
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
        public double SumSr(double[,] matrix)
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
        public double SumEntr(double[,] matrix)
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
        public double SumDisp(double[,] matrix)
        {
            double res = 0, left = 0;
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
        public double Entrop(double[,] matrix)
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
        #endregion
    }
}
