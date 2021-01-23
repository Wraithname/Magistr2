using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Magistr2
{
    public class ImageProcessing
    {

        public int[] rest = new int[256];
        public Bitmap MakeGrayscale3(Bitmap original, float k=0f)
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
            new float[] {k, k, k, 0, 1}
               });
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(colorMatrix);
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            g.Dispose();
            return newBitmap;
        }

        public List<int> GetHistogramm(Bitmap image, int[,] mat = null)
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
                        if (i != -1)
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

        public int[,] GetCountorPoints(Bitmap img)
        {
            int colorBackr = img.GetPixel(0, 0).R;
            Bitmap tecImg = new Bitmap(img);
            List<Point> objectsPoints = new List<Point>();
            TextureRes texture = new TextureRes();
            int i = 0, j = 0;
            while (i < img.Width-1)
            {
                if (img.GetPixel(i, j).R != colorBackr)
                {
                    
                        objectsPoints.Add(new Point(i, j));
                }
                if (j == img.Height-1)
                {
                    j = 0;
                    i++;
                }
                else
                j++;
            }
            int xmin = int.MaxValue, ymin = int.MaxValue, ymax = int.MinValue, xmax = int.MinValue;
            foreach (Point pt in objectsPoints)
            {   
                    if (pt.X > xmax) xmax = pt.X;
                    if (pt.X < xmin) xmin = pt.X;
                    if (pt.Y > ymax) ymax = pt.Y;
                    if (pt.Y < ymin) ymin = pt.Y;
            }
            
            int[,] resmat = new int[xmax - xmin + 1, ymax - ymin + 1];
            for(int k=0;k< xmax - xmin+1;k++)
            {
                for (int l = 0; l < ymax - ymin+1; l++)
                    resmat[k, l] = -1;
            }
            int y1 = 0, x1 = 0;
            for (int y = ymin; y <= ymax; y++)
            {
                for (int x = xmin; x <= xmax; x++)
                {
                    if (tecImg.GetPixel(x, y).R != 0)
                    {
                        resmat[x1, y1] = tecImg.GetPixel(x, y).R;
                    }
                    x1++;
                }
                x1 = 0;
                y1++;
            }
            return resmat;
        }
    }
}
