using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Magistr2
{
    class TextureRes
    {
        int colvo = 0;
        double[] px;
        double[] py;
        double hx, hy, dx, dy,hxy,hx_y;
        double[] pxy;
        double[] px_y;
        /// <summary>
        /// Расчёт текстурных показателей
        /// </summary>
        /// <param name="matrix">Матрица яркостей</param>
        /// <param name="qvant">Цветовой спектр</param>
        /// <param name="rst">Спектр с количеством пикселей каждой яркости</param>
        /// <returns>Текстурные признаки</returns>
        public double[] Calculation(int[,] matrix, List<int> qvant, int[] rst, int colvo = 2)
        {
            this.colvo = colvo;
            int[,] graycl = GrayClasses(matrix, qvant, colvo);
            double[,] resMat = MatrixCalculation(graycl, colvo);
            #region Вспомогательные переменный для расчёта
            px = Px(resMat);
            py = Py(resMat);
            hx = midleqvadrx();
            hy = midleqvadry();
            dx = srqvadrx(px, hx);
            dy = srqvadry(py, hy);
            pxy = VectSum(resMat);
            px_y = UnVectSum(resMat);
            hxy = Hxy(pxy);
            hx_y = Hx_y(px_y);
            #endregion
            double[] res = new double[11];
            res[0] = MatrixPower(resMat);
            res[1] = Correl(resMat);
            res[2] = Disper(resMat);
            res[3] = SumSr(resMat);
            res[4] = SumDisp(resMat);
            res[5] = SumEntr(resMat);
            res[6] = Entrop(resMat);
            res[7] = Contrast(resMat);
            res[8] = Odnorod(resMat);
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
                    matrix[j, i] = (img.GetPixel(j, i).R + img.GetPixel(j, i).G + img.GetPixel(j, i).B) / 3;
                }
            }
            return matrix;
        }
        private double[,] ConvertMatIntToDouble(int[,] mat)
        {
            double[,] res = new double[(mat.GetUpperBound(0) + 1), mat.Length / (mat.GetUpperBound(0) + 1)];
            for (int i = 0; i < (mat.GetUpperBound(0) + 1); i++)
                for (int j = 0; j < mat.Length / (mat.GetUpperBound(0) + 1); j++)
                {
                    res[i, j] = Convert.ToDouble(mat[i, j]);
                }
            return res;
        }
        public int[,] ConvertImgToMatrix(Bitmap img, int fx)
        {
            int[,] matrix = new int[img.Width, img.Height];
            for (int i = 0; i < img.Height; i++)
            {
                for (int j = 0; j < img.Width; j++)
                {
                    if (img.GetPixel(j, i).R + fx > 255)
                        matrix[j, i] = 255;
                    else if (img.GetPixel(j, i).R + fx < 0)
                        matrix[j, i] = 0;
                    else
                        matrix[j, i] = img.GetPixel(j, i).R + fx;
                }
            }
            return matrix;
        }
        public int[,] GrayClasses(int[,] matrix, List<int> qvant, int colvo = 5)
        {
            int[,] matrixGrayCl = new int[matrix.GetUpperBound(0) + 1, matrix.Length / (matrix.GetUpperBound(0) + 1)];
            switch (colvo)
            {
                case 2:
                    {
                        int step = 255 / 2;
                        for (int i = 0; i < (matrix.GetUpperBound(0) + 1); i++)
                        {
                            for (int j = 0; j < matrix.Length / (matrix.GetUpperBound(0) + 1); j++)
                            {
                                if (matrix[i, j] != -1)
                                {
                                    if (matrix[i, j] <= step)
                                        matrixGrayCl[i, j] = 0;
                                    if (matrix[i, j] > step)
                                        matrixGrayCl[i, j] = 1;
                                }
                            }
                        }
                    }
                    break;
                case 3:
                    {
                        int step = qvant.Count() / 3;
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
                                }
                            }
                        }
                    }
                    break;
                case 5:
                    {
                        int step = qvant.Count() / 4;

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
                    }
                    break;
                case 32:
                    {
                        int step = qvant.Count() / 32;

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
                                    if (matrix[i, j] > qvant[step * 3] && matrix[i, j] <= qvant[step * 4])
                                        matrixGrayCl[i, j] = 3;
                                    if (matrix[i, j] > qvant[step * 3] && matrix[i, j] <= qvant[step * 4])
                                        matrixGrayCl[i, j] = 4;
                                    if (matrix[i, j] > qvant[step * 4] && matrix[i, j] <= qvant[step * 5])
                                        matrixGrayCl[i, j] = 5;
                                    if (matrix[i, j] > qvant[step * 5] && matrix[i, j] <= qvant[step * 6])
                                        matrixGrayCl[i, j] = 6;
                                    if (matrix[i, j] > qvant[step * 6] && matrix[i, j] <= qvant[step * 7])
                                        matrixGrayCl[i, j] = 7;
                                    if (matrix[i, j] > qvant[step * 7] && matrix[i, j] <= qvant[step * 8])
                                        matrixGrayCl[i, j] = 8;
                                    if (matrix[i, j] > qvant[step * 8] && matrix[i, j] <= qvant[step * 9])
                                        matrixGrayCl[i, j] = 9;
                                    if (matrix[i, j] > qvant[step * 9] && matrix[i, j] <= qvant[step * 10])
                                        matrixGrayCl[i, j] = 10;
                                    if (matrix[i, j] > qvant[step * 10] && matrix[i, j] <= qvant[step * 11])
                                        matrixGrayCl[i, j] = 11;
                                    if (matrix[i, j] > qvant[step * 11] && matrix[i, j] <= qvant[step * 12])
                                        matrixGrayCl[i, j] = 12;
                                    if (matrix[i, j] > qvant[step * 12] && matrix[i, j] <= qvant[step * 13])
                                        matrixGrayCl[i, j] = 13;
                                    if (matrix[i, j] > qvant[step * 13] && matrix[i, j] <= qvant[step * 14])
                                        matrixGrayCl[i, j] = 14;
                                    if (matrix[i, j] > qvant[step * 14] && matrix[i, j] <= qvant[step * 15])
                                        matrixGrayCl[i, j] = 15;
                                    if (matrix[i, j] > qvant[step * 15] && matrix[i, j] <= qvant[step * 16])
                                        matrixGrayCl[i, j] = 16;
                                    if (matrix[i, j] > qvant[step * 16] && matrix[i, j] <= qvant[step * 17])
                                        matrixGrayCl[i, j] = 17;
                                    if (matrix[i, j] > qvant[step * 17] && matrix[i, j] <= qvant[step * 18])
                                        matrixGrayCl[i, j] = 18;
                                    if (matrix[i, j] > qvant[step * 18] && matrix[i, j] <= qvant[step * 19])
                                        matrixGrayCl[i, j] = 19;
                                    if (matrix[i, j] > qvant[step * 19] && matrix[i, j] <= qvant[step * 20])
                                        matrixGrayCl[i, j] = 20;
                                    if (matrix[i, j] > qvant[step * 20] && matrix[i, j] <= qvant[step * 21])
                                        matrixGrayCl[i, j] = 21;
                                    if (matrix[i, j] > qvant[step * 21] && matrix[i, j] <= qvant[step * 22])
                                        matrixGrayCl[i, j] = 22;
                                    if (matrix[i, j] > qvant[step * 22] && matrix[i, j] <= qvant[step * 23])
                                        matrixGrayCl[i, j] = 23;
                                    if (matrix[i, j] > qvant[step * 23] && matrix[i, j] <= qvant[step * 24])
                                        matrixGrayCl[i, j] = 24;
                                    if (matrix[i, j] > qvant[step * 24] && matrix[i, j] <= qvant[step * 25])
                                        matrixGrayCl[i, j] = 25;
                                    if (matrix[i, j] > qvant[step * 25] && matrix[i, j] <= qvant[step * 26])
                                        matrixGrayCl[i, j] = 26;
                                    if (matrix[i, j] > qvant[step * 26] && matrix[i, j] <= qvant[step * 27])
                                        matrixGrayCl[i, j] = 27;
                                    if (matrix[i, j] > qvant[step * 27] && matrix[i, j] <= qvant[step * 28])
                                        matrixGrayCl[i, j] = 28;
                                    if (matrix[i, j] > qvant[step * 28] && matrix[i, j] <= qvant[step * 29])
                                        matrixGrayCl[i, j] = 29;
                                    if (matrix[i, j] > qvant[step * 29] && matrix[i, j] <= qvant[step * 30])
                                        matrixGrayCl[i, j] = 30;
                                    if (matrix[i, j] > qvant[step * 30] && matrix[i, j] <= qvant[step * 31 - 1])
                                        matrixGrayCl[i, j] = 31;
                                    if (matrix[i, j] > qvant[step * 31 - 1])
                                        matrixGrayCl[i, j] = 32;
                                }
                            }
                        }
                    }
                    break;

            }
            return matrixGrayCl;
        }
        public double[,] MatrixCalculation(int[,] matrix, int porog = 5)
        {
            double[,] result = new double[porog, porog];

            for (int m = 0; m < (result.GetUpperBound(0) + 1); m++)
            {
                for (int p = 0; p < result.Length / (result.GetUpperBound(0) + 1); p++)
                {
                    int test = matrix.GetUpperBound(0) + 1;
                    int test2 = (matrix.Length / (matrix.GetUpperBound(0) + 1));
                    //Расчёт коэффициентов
                    for (int i = 0; i < (matrix.Length / (matrix.GetUpperBound(0) + 1)) - 1; i++)
                    {
                        for (int j = 0; j < matrix.GetUpperBound(0) + 1; j++)
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
            double entr = 0, up = 0;
            for (int i = 0; i < (matrix.GetUpperBound(0) + 1); i++)
                for (int j = 0; j < matrix.Length / (matrix.GetUpperBound(0) + 1); j++)
                    up += (i- hx) * (j- hy) * matrix[i, j];
            entr = up / Math.Sqrt(dx * dy);
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
            for (int i = 0; i < (matrix.GetUpperBound(0) + 1) + matrix.Length / (matrix.GetUpperBound(0) + 1); i++)
            {
                    res += ((i + 1)) * sr[i];
            }
            return res;
        }
        public double SumEntr(double[,] matrix)
        {
            double res = 0, left = 0, right = 0;
            var sr = VectSum(matrix);
            for (int i = 0; i < sr.Length; i++)
            {
                    left = sr[i];
                    right = Math.Log(sr[i]);
                    if (left != 0 && right != 0)
                        res += left * right;
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
        public double Odnorod(double[,] matrix)
        {
            double res = 0;
            for (int i = 0; i < (matrix.GetUpperBound(0) + 1); i++)
            {
                for (int j = 0; j < matrix.Length / (matrix.GetUpperBound(0) + 1); j++)
                    res += matrix[i, j] / (1 + Math.Pow((i + 1) - (j + 1), 2));
            }
            return res;
        }
        public double Contrast(double[,] matrix)
        {
            double resSum = 0;
                    for (int i = 0; i < (matrix.GetUpperBound(0) + 1); i++)
                for (int j = 0; j < matrix.Length / (matrix.GetUpperBound(0) + 1); j++)
                    resSum += matrix[i, j] * Math.Pow((i-j),2);
            return resSum;
        }
        public double diffDisp(double[,] matrix)
        {
            double res = 0;
            var sr = UnVectSum(matrix);
            var h = matrixsr(matrix);
            for (int k = 0; k < sr.Length; k++)
                res += Math.Pow((sr[k] - h),2);
            return res / (sr.Length - 2);
        }
        public double diffEntr(double[,] matrix)
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
        double[] UnVectSum(double[,] matrix)
        {
            double[] res = new double[(matrix.GetUpperBound(0) + 1)];
                for (int i = (matrix.GetUpperBound(0) + 1) - 1; i >= 0; i--)
                    for (int j = matrix.Length / (matrix.GetUpperBound(0) + 1) - 1; j >= 0; j--)
                            res[Math.Abs(i - j)] += matrix[i, j]/colvo;
            return res;
        }
        double Hxy(double[] pxy)
        {
            double res = 0;
            for (int k = 1; k < pxy.Length; k++)
                res += (k + 1) * pxy[k];
            return res;
        }
        double Hx_y(double[] px_y)
        {
            double res = 0;
            for (int k = 0; k < px_y.Length-1; k++)
                res += (k + 1) * px_y[k];
            return res;
        }
        double[] Px(double[,] matrix)
        {
            double[] res = new double[(matrix.GetUpperBound(0) + 1)];
            for (int i = 0; i < (matrix.GetUpperBound(0) + 1); i++)
                for (int j = 0; j < matrix.Length / (matrix.GetUpperBound(0) + 1); j++)
                    res[i] += matrix[i, j]/ colvo;
                    return res;
        }
        double[] Py(double[,] matrix)
        {
            double[] res = new double[(matrix.GetUpperBound(0) + 1)];
            for (int j = 0; j < matrix.Length / (matrix.GetUpperBound(0) + 1); j++)
                for (int i = 0; i < (matrix.GetUpperBound(0) + 1); i++)
                    res[j] += matrix[i, j]/ colvo;
            return res;
        }
        double midleqvadrx()
        {
            double res =0;
           for(int i=0;i<px.Length;i++)
                    res += i* px[i];
            return res;
        }
        double midleqvadry()
        {
            double res = 0;
            for (int j = 0; j < py.Length; j++)
                res += j * py[j];
            return res ;
        }
        double srqvadrx(double[] px,double srarx)
        {
            double sum = 0;
            for (int i = 0; i < px.Length; i++)
                    sum += ((i  - srarx) * (i - srarx)) * px[i];
            return Math.Sqrt(sum);
        }
        double srqvadry(double[] py, double srary)
        {
            double sum = 0;
            for (int i = 0; i < py.Length; i++)
                    sum += ((i - srary) * (i - srary)) * py[i];
            return Math.Sqrt(sum);
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
            double[] res = new double[(matrix.GetUpperBound(0) + 1) +( matrix.Length / (matrix.GetUpperBound(0) + 1))];
                for (int i = 0; i < (matrix.GetUpperBound(0) + 1); i++)
                {
                    for (int j = 0; j < matrix.Length / (matrix.GetUpperBound(0) + 1); j++)
                    {
                            res[(i + j)] += matrix[i, j]/colvo;
                    }
                }
            return res;
        }
        #endregion
    }
}
