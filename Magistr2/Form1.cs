using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Magistr2
{
    public partial class Form1 : Form
    {
        #region Переменные
        TextureRes texture;
        double[] result;
        int[] rest = new int[256];
        string[] name=new string[]{"Мощность","Контрастность", "Корреляция", "Дисперсия", "Однородность", "Сумма средних", 
            "Сумма дисперсии", "Сумма энтропии", "Энтропия", "Разница дисперсии", "Разница энтропии" };
        #endregion
        public Form1()
        {
            InitializeComponent();
            texture = new TextureRes();
        }

        private void OpenImg_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog ofd=new OpenFileDialog())
            {
                ofd.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp";
                if (ofd.ShowDialog()==DialogResult.OK)
                {
                    Image img = Image.FromFile(ofd.FileName);
                    Bitmap gray = MakeGrayscale3(new Bitmap(ofd.FileName));
                    ImagePlace.Image = gray;
                    
                    /*int[,] colorGray1 = new int[,] {
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
                    };*/
                    int[,] colorGray = texture.ConvertImgToMatrix(gray);
                    var qvant = GetHistogramm(gray,colorGray);
                    for(int j=0;j<255;j++)
                    {
                        chart1.Series[0].Name = "Повтор яркостей";
                        chart1.ChartAreas[0].AxisX.Interval = 30;
                        chart1.Series[0].Points.AddXY(j+1, rest[j]);
                    }
                    result=texture.Calculation(colorGray,qvant,rest);
                    for(int l=0;l<result.Length;l++)
                    ResultRes.Text += name[l]+": "+result[l] + Environment.NewLine;
                }
            }
        }
        private static Bitmap MakeGrayscale3(Bitmap original)
        {
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);
            Graphics g = Graphics.FromImage(newBitmap);
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
            new float[] {.3f, .3f, .3f, 0, 0},
            new float[] {.59f, .59f, .59f, 0, 0},
            new float[] {.11f, .11f, .11f, 0, 0},
            new float[] {0, 0, 0, 1, 0},
            new float[] {0, 0, 0, 0, 1}
               });
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(colorMatrix);
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            g.Dispose();
            return newBitmap;
        }
        
        private List<int> GetHistogramm(Bitmap image, int[,] mat=null)
        {
            List<int> qvant = new List<int>();
            if (mat == null)
            {
                
                for (int x = 0; x < image.Width; x++)
                    for (int y = 0; y < image.Height; y++)
                    {
                        int i = image.GetPixel(x, y).R;
                        rest[i]++;
                    }
                for (int i = 0; i < 256; i++)
                {
                    if (rest[i] != 0)
                        qvant.Add(i);
                }
            }
            else
            {
                for (int x = 0; x < (mat.GetUpperBound(0) + 1); x++)
                    for (int y = 0; y < mat.Length / (mat.GetUpperBound(0) + 1); y++)
                    {
                        int i = mat[x, y];
                        rest[i]++;
                    }
                for (int i = 0; i < 256; i++)
                {
                    if (rest[i] != 0)
                        qvant.Add(i);
                }
            }
            return qvant;
        }
    }
}
