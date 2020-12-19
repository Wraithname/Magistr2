using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Magistr2
{
    public class ImageProcessing
    {

        public int[] rest = new int[256];
        public Bitmap MakeGrayscale3(Bitmap original, float k)
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
        private Bitmap bitmapToBlack(Bitmap bit)
        {
            for (int i = 0; i < bit.Width; i++)
            {
                for (int j = 0; j < bit.Height; j++)
                {
                    if (bit.GetPixel(i, j).R <= 155 && bit.GetPixel(i, j).G <= 155 && bit.GetPixel(i, j).B <= 155)
                        bit.SetPixel(i, j, Color.Black);
                    else
                        bit.SetPixel(i, j, Color.White);
                }
            }
            return bit;
        }
        public Color[][] GetCountur(Image img)
        {
            int height = img.Height, width = img.Width;
            Bitmap bp = bitmapToBlack(new Bitmap(img));
            Color[][] colorMatrix = new Color[width][];
            for (int i = 1; i < width - 1; i++)
            {
                colorMatrix[i] = new Color[height];
                for (int j = 1; j < height - 1; j++)
                {
                    if (bp.GetPixel(i, j).R == 0 && bp.GetPixel(i, j).G == 0 && bp.GetPixel(i, j).B == 0)
                        if (bp.GetPixel(i + 1, j + 1).R == 255 && bp.GetPixel(i + 1, j + 1).G == 255 && bp.GetPixel(i + 1, j + 1).B == 255 ||
                            bp.GetPixel(i + 1, j).R == 255 && bp.GetPixel(i + 1, j).G == 255 && bp.GetPixel(i + 1, j).B == 255 ||
                            bp.GetPixel(i + 1, j - 1).R == 255 && bp.GetPixel(i + 1, j - 1).G == 255 && bp.GetPixel(i + 1, j - 1).B == 255 ||
                            bp.GetPixel(i, j + 1).R == 255 && bp.GetPixel(i, j + 1).G == 255 && bp.GetPixel(i, j + 1).B == 255 ||
                            bp.GetPixel(i, j - 1).R == 255 && bp.GetPixel(i, j - 1).G == 255 && bp.GetPixel(i, j - 1).B == 255 ||
                            bp.GetPixel(i - 1, j + 1).R == 255 && bp.GetPixel(i - 1, j + 1).G == 255 && bp.GetPixel(i - 1, j + 1).B == 255 ||
                            bp.GetPixel(i - 1, j).R == 255 && bp.GetPixel(i - 1, j).G == 255 && bp.GetPixel(i - 1, j).B == 255 ||
                            bp.GetPixel(i - 1, j - 1).R == 255 && bp.GetPixel(i - 1, j - 1).G == 255 && bp.GetPixel(i - 1, j - 1).B == 255)
                        {
                            bp.SetPixel(i, j, Color.Red);
                        }
                }
            }
            return colorMatrix;
        }
        private Bitmap BitmapToBlack(Bitmap bit)
        {
            for (int i = 0; i < bit.Width; i++)
            {
                for (int j = 0; j < bit.Height; j++)
                {
                    if (bit.GetPixel(i, j).R <= 155 && bit.GetPixel(i, j).G <= 155 && bit.GetPixel(i, j).B <= 155)
                        bit.SetPixel(i, j, Color.Black);
                    else
                        bit.SetPixel(i, j, Color.White);
                }
            }
            return bit;
        }
        public List<Point> GetCountorPoints(Bitmap img)
        {
            Bitmap bp = BitmapToBlack((Bitmap)img);
            List<Point> objectsPoints = new List<Point>();
            for (int i = 1; i < img.Width - 1; i++)
            {
                for (int j = 1; j < img.Height - 1; j++)
                {
                    if (bp.GetPixel(i, j).R == 0 && bp.GetPixel(i, j).G == 0 && bp.GetPixel(i, j).B == 0)
                        if (bp.GetPixel(i + 1, j + 1).R == 255 && bp.GetPixel(i + 1, j + 1).G == 255 && bp.GetPixel(i + 1, j + 1).B == 255 ||
                            bp.GetPixel(i + 1, j).R == 255 && bp.GetPixel(i + 1, j).G == 255 && bp.GetPixel(i + 1, j).B == 255 ||
                            bp.GetPixel(i + 1, j - 1).R == 255 && bp.GetPixel(i + 1, j - 1).G == 255 && bp.GetPixel(i + 1, j - 1).B == 255 ||
                            bp.GetPixel(i, j + 1).R == 255 && bp.GetPixel(i, j + 1).G == 255 && bp.GetPixel(i, j + 1).B == 255 ||
                            bp.GetPixel(i, j - 1).R == 255 && bp.GetPixel(i, j - 1).G == 255 && bp.GetPixel(i, j - 1).B == 255 ||
                            bp.GetPixel(i - 1, j + 1).R == 255 && bp.GetPixel(i - 1, j + 1).G == 255 && bp.GetPixel(i - 1, j + 1).B == 255 ||
                            bp.GetPixel(i - 1, j).R == 255 && bp.GetPixel(i - 1, j).G == 255 && bp.GetPixel(i - 1, j).B == 255 ||
                            bp.GetPixel(i - 1, j - 1).R == 255 && bp.GetPixel(i - 1, j - 1).G == 255 && bp.GetPixel(i - 1, j - 1).B == 255)
                        {
                            objectsPoints.Add(new Point(i, j));
                        }
                }
            }
            return objectsPoints;
        }
    }
}
