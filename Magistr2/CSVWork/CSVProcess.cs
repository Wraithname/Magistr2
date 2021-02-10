using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Magistr2.CSVWork
{
    class CSVProcess
    {
        private void WriteToFile(List<double[]> result, string name, float fx)
        {
            if (!Directory.Exists(@"C:\r"))
                Directory.CreateDirectory(@"C:\r");

            string pathCsvFile = @"C:\r\Britnes" + fx.ToString() + ".csv";
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
        public void CalculatingTextureByTasks(string folderPath, float fx)
        {
            string[] imgall = Directory.GetFiles(folderPath+ "//Brightness "+fx.ToString());
            TextureRes texture = new TextureRes();
            ImageProcessing imgproc = new ImageProcessing();
            int i = 0;
           List<double[]> resultCalculation = new List<double[]>();
            foreach (string img in imgall)
            {
                double[] rec = new double[11];
                Bitmap gray = imgproc.MakeGrayscale3(new Bitmap(img));
                int[,] colorGray = imgproc.GetCountorPoints(gray);
                var qvant = imgproc.GetHistogramm(gray, colorGray);
                int[,] graycl = texture.GrayClasses(colorGray, qvant);
                double[,] resMat = texture.MatrixCalculation(graycl,32);
                rec[0] = texture.MatrixPower(resMat);
                rec[1] = texture.Correl(resMat);
                rec[2] = texture.Disper(resMat);
                rec[3] = texture.SumSr(resMat);
                rec[4] = texture.SumDisp(resMat);
                rec[5] = texture.SumEntr(resMat);
                rec[6] = texture.Entrop(resMat);
                rec[7] = texture.Contrast(resMat);
                rec[8] = texture.Odnorod(resMat);
                rec[9] = texture.diffDisp(resMat);
                rec[10] = texture.diffEntr(resMat);
                resultCalculation.Add(rec);
                i++;
            }
            WriteToFile(resultCalculation, "Power", fx);

        }
    }
}
