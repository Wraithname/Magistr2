using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Magistr2.CSVWork
{
    class CSVProcess
    {
        private void WriteToFile(List<double[]> result, string name, float fx)
        {
            if (!Directory.Exists(@"C:\Users\UserHome\Desktop\Магистр\Result"))
                Directory.CreateDirectory(@"C:\Users\UserHome\Desktop\Магистр\Result");
            if (!Directory.Exists(@"C:\Users\UserHome\Desktop\Магистр\Result\" + name))
                Directory.CreateDirectory(@"C:\Users\UserHome\Desktop\Магистр\Result\" + name);
            string pathCsvFile = @"C:\Users\UserHome\Desktop\Магистр\Result\"+name+@"\Brightnes" + fx.ToString() + ".csv";
            string delimiter = ";";
            StringBuilder sb = new StringBuilder();
            int j = 0;
            foreach (double[] t in result)
            {
                sb.AppendLine(string.Join(delimiter, result[j]));
                j++;
            }
            File.WriteAllText(pathCsvFile, sb.ToString());
        }
        public void CalculatingTextureByTasks(string folderPath, float fx, int colvo = 2)
        {
            string namefolder = folderPath + "\\Brightnes" + fx.ToString();
            string[] imgall = Directory.GetFiles(namefolder);
            string[] result = new string[imgall.Length];
            for (int k = 0; k < imgall.Length; k++)
            {
                string tpr = imgall[k].Split('\\').Last();
                int num = Convert.ToInt32(tpr.Split('.').First());
                result[num] = imgall[k];
            }
            string folder = folderPath.Split('\\').Last();
            TextureRes texture = new TextureRes();
            ImageProcessing imgproc = new ImageProcessing();
            int i = 0;
           List<double[]> resultCalculation = new List<double[]>();
            foreach (string img in result)
            {
                double[] rec = new double[11];
                Bitmap gray = imgproc.MakeGrayscale3(new Bitmap(img));
                int[,] colorGray = imgproc.GetCountorPoints(gray);
                var qvant = imgproc.GetHistogramm(gray, colorGray);
                int[,] graycl = texture.GrayClasses(colorGray, qvant);
                double[,] resMat = texture.MatrixCalculation(graycl,32);
                texture.CalculateValues(resMat);
                rec[0] = texture.MatrixPower(resMat);
                rec[1] = texture.Correl(resMat);
                rec[2] = texture.Autocorrel(resMat);
                rec[3] = texture.SumSr(resMat);
                rec[4] = texture.SumDisp(resMat);
                rec[5] = texture.ClusterProm(resMat);
                rec[6] = texture.Dissimilarity(resMat);
                rec[7] = texture.Contrast(resMat);
                rec[8] = texture.Odnorod(resMat);
                rec[9] = texture.diffVar(resMat);
                rec[10] = texture.diffEntr(resMat);
                resultCalculation.Add(rec);
                i++;
            }
            WriteToFile(resultCalculation, folder, fx);

        }
    }
}
