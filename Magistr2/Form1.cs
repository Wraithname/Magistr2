using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Magistr2
{
    public partial class Form1 : Form
    {
        TextureRes texture;
        double[] result;
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
                    ImagePlace.Image = new Bitmap(ofd.FileName);
                    Bitmap gray = ImageProc(new Bitmap(ofd.FileName));
                    int[,] colorGray = ConvertImgToMatrix(gray);
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
                    result=texture.Calculation(colorGray);
                    foreach (double s in result)
                        ResultRes.Text += s.ToString() + Environment.NewLine;
                }
            }
        }
        Bitmap ImageProc(Bitmap img)
        {
            Bitmap resimg =new Bitmap(img.Width,img.Height);
            for(int i=0;i<resimg.Height;i++)
            {
                for(int j=0;j<resimg.Width;j++)
                {
                    Color pixelColor = img.GetPixel(i, j);
                    Color newColor = Color.FromArgb(pixelColor.R, 0, 0);
                    resimg.SetPixel(i, j, newColor);
                }
            }    
            return resimg;
        }
        int[,] ConvertImgToMatrix(Bitmap img)
        {
            int[,] matrix = new int[img.Width, img.Height];
            for(int i=0;i<img.Height;i++)
            {
                for(int j=0;j<img.Width;j++)
                {
                    matrix[i, j] = img.GetPixel(j, i).R;
                }
            }
            return matrix;
        }
    }
}
