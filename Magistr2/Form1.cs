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
                    resimg.SetPixel(i, j, newColor); // Now greyscale
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
