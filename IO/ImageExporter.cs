using ChaosPixelMatch.View;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Image = ChaosPixelMatch.View.Image;

namespace ChaosPixelMatch.IO
{
    internal class ImageExporter
    {
        private const int CELL_SIZE = 32;
        private const int FONT_SIZE = 26;
        public static void Export(Image image, string path)
        {
            int totalWidth = image.Width * CELL_SIZE;
            int totalHeight = image.Height * CELL_SIZE;

            using var bitmap = new Bitmap(totalWidth, totalHeight);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                g.Clear(System.Drawing.Color.Black);

                using var font = new Font("Consolas", FONT_SIZE, GraphicsUnit.Pixel);
                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        Pixel pixel = image.GetPixel(x, y);
                        Rectangle cellRect = new(x * CELL_SIZE, y * CELL_SIZE, CELL_SIZE, CELL_SIZE);

                        System.Drawing.Color drawingColor = System.Drawing.Color.FromArgb(
                            pixel.Color.R,
                            pixel.Color.G,
                            pixel.Color.B);

                        if (pixel.Char == Pixel.DOT_CHAR)
                        {
                            using var brush = new SolidBrush(drawingColor);
                            g.FillRectangle(brush, cellRect);
                        }
                        else if (pixel.Char != ' ')
                        {
                            using var brush = new SolidBrush(drawingColor);
                            StringFormat sf = new()
                            {
                                Alignment = StringAlignment.Center,
                                LineAlignment = StringAlignment.Center
                            };
                            g.DrawString(pixel.Char.ToString(), font, brush, cellRect, sf);
                        }
                    }
                }
            }
            bitmap.Save(path);
        }
    }
}
