using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using Magistr2.CSVWork;

namespace Magistr2
{
    public partial class Form1 : Form
    {
        #region Переменные
        TextureRes texture;
        double[] result;
        ImageProcessing imgproc;
        CSVProcess process;
        string[] name = new string[]{"Мощность", "Корреляция", "Дисперсия",  "Сумма средних",
            "Сумма дисперсии", "Сумма энтропии", "Энтропия"};
        #endregion
        public Form1()
        {
            InitializeComponent();
            process = new CSVProcess();
            imgproc = new ImageProcessing();
            texture = new TextureRes();
        }

        private void OpenImg_Click(object sender, EventArgs e)
        {
            #region Работа с массивом изображений
            /*
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    Thread th = new Thread(() => process.CalculatingTextureByTasks(fbd.SelectedPath, 0f));
                    th.IsBackground = true;
                    Thread[] listthread = new Thread[3];
                    Thread[] listthread2 = new Thread[3];
                    listthread[0] = new Thread(() => process.CalculatingTextureByTasks(fbd.SelectedPath, -.3f));
                    listthread[1] = new Thread(() => process.CalculatingTextureByTasks(fbd.SelectedPath, -.2f));
                    listthread[2] = new Thread(() => process.CalculatingTextureByTasks(fbd.SelectedPath, -.1f));
                    th.Start();
                    foreach (var thread in listthread)
                    {
                        thread.Start();
                    }
                    foreach (var thread in listthread)
                    {
                        thread.Join();
                    }
                    listthread2[0] = new Thread(() => process.CalculatingTextureByTasks(fbd.SelectedPath, .1f));
                    listthread2[1] = new Thread(() => process.CalculatingTextureByTasks(fbd.SelectedPath, .2f));
                    listthread2[2] = new Thread(() => process.CalculatingTextureByTasks(fbd.SelectedPath, .3f));
                    foreach (var thread in listthread2)
                    {
                        thread.Start();
                    }
                    foreach (var thread in listthread2)
                    {
                        thread.Join();
                    }
                    th.Join();
                }
            }
            MessageBox.Show("Расчёт окончен. Результаты находятся в папке 'C:\r'", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            */
            #endregion
            #region Работа с одним изображением
            
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Image img = Image.FromFile(ofd.FileName);
                    
                    Bitmap gray = imgproc.MakeGrayscale3(new Bitmap(ofd.FileName), 0f);
                    var objectsPoints = imgproc.GetCountorPoints(gray);
                    ImagePlace.Image = gray;

                    int[,] colorGray1 = new int[,] {
                      {132,110,83,155,133,133,138,165,128,85},
                      {113,111,98,160,137,138,131,141,149,100},
                      {125,127,135,142,113,161,130,142,140,91},
                      {128,132,139,144,115,174,135,150,128,92},
                      {153,144,98,150,126,142,131,145,128,113},
                      {155,137,94,155,142,131,112,119,115,120},
                      {140,119,111,131,129,129,116,137,101,116},
                      {117,112,124,133,131,105,111,136,96,114},
                      {111,113,115,130,135,149,103,136,109,111},
                      {115,101,126,147,119,138,115,139,93,100}
                  };
                    
                    //int[,] colorGray = texture.ConvertImgToMatrix(gray);
                    var qvant = imgproc.GetHistogramm(gray, objectsPoints);
                    for (int j = 0; j < 255; j++)
                    {
                        chart1.Series[0].Name = "Повтор яркостей";
                        chart1.ChartAreas[0].AxisX.Interval = 30;
                        chart1.Series[0].Points.AddXY(j + 1, imgproc.rest[j]);
                    }
                    result = texture.Calculation(objectsPoints, qvant, imgproc.rest);
                    for (int l = 0; l < result.Length; l++)
                        ResultRes.Text += name[l] + ": " + result[l] + Environment.NewLine;

                }
            }
            
            #endregion

        }

    }
}
