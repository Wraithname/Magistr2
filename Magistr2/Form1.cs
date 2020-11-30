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
                    ImagePlace.Image = new Bitmap(ofd.FileName);
                    result=texture.Calculation(new Bitmap(ofd.FileName));
                    foreach (double s in result)
                        ResultRes.Text += s.ToString() + Environment.NewLine;
                }
            }
        }
    }
}
