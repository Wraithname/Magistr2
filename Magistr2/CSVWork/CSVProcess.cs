using System.Drawing;
using System.IO;
using System.Text;

namespace Magistr2.CSVWork
{
    class CSVProcess
    {
        private void WriteToFile(double[] resultie, string name, float fx)
        {
            if (!Directory.Exists(@"C:\r"))
                Directory.CreateDirectory(@"C:\r");
            if (!Directory.Exists(@"C:\r\" + name))
                Directory.CreateDirectory(@"C:\r\" + name);

            string pathCsvFile = @"C:\r\" + name + @"\Britnes" + fx.ToString() + ".csv";
            string delimiter = ",";
            StringBuilder sb = new StringBuilder();
            for (int index = 0; index < resultie.Length; index++)
                sb.AppendLine(string.Join(delimiter, resultie[index]));
            File.WriteAllText(pathCsvFile, sb.ToString());
        }
        public void CalculatingTextureByTasks(string folderPath, float fx)
        {
            string[] imgall = Directory.GetFiles(folderPath+ "//Brightness "+fx.ToString());
            TextureRes texture = new TextureRes();
            ImageProcessing imgproc = new ImageProcessing();
            int i = 0;
            double[] resultCalculation = new double[imgall.Length];
            double[] resultCalculation1 = new double[imgall.Length];
            double[] resultCalculation2 = new double[imgall.Length];
            double[] resultCalculation3 = new double[imgall.Length];
            double[] resultCalculation4 = new double[imgall.Length];
            double[] resultCalculation5 = new double[imgall.Length];
            double[] resultCalculation6 = new double[imgall.Length];
            foreach (string img in imgall)
            {
                Bitmap gray = imgproc.MakeGrayscale3(new Bitmap(img));
                int[,] colorGray = imgproc.GetCountorPoints(gray);
                var qvant = imgproc.GetHistogramm(gray, colorGray);
                int[,] graycl = texture.GrayClasses(colorGray, qvant);
                double[,] resMat = texture.MatrixCalculation(graycl);
                resultCalculation[i] = texture.MatrixPower(resMat);
                resultCalculation1[i] = texture.Correl(resMat);
                resultCalculation2[i] = texture.Disper(resMat);
                resultCalculation3[i] = texture.SumSr(resMat);
                resultCalculation4[i] = texture.SumDisp(resMat);
                resultCalculation5[i] = texture.SumEntr(resMat);
                resultCalculation6[i] = texture.Entrop(resMat);
                i++;
            }
            WriteToFile(resultCalculation, "Power", fx);
            WriteToFile(resultCalculation1, "Correl", fx);
            WriteToFile(resultCalculation2, "Disper", fx);
            WriteToFile(resultCalculation3, "SumSr", fx);
            WriteToFile(resultCalculation4, "SumDisp", fx);
            WriteToFile(resultCalculation5, "SumEntr", fx);
            WriteToFile(resultCalculation6, "Entrop", fx);

        }
    }
}
