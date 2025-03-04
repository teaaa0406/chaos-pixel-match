using ChaosPixelMatch.View;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Color = ChaosPixelMatch.View.Color;
using Image = ChaosPixelMatch.View.Image;

namespace ChaosPixelMatch.IO
{
    internal static class ImageImporter
    {
        private const int MAX_SIZE = 64;

        public static bool TryImport(string path, byte colorLevels, [MaybeNullWhen(false)] out Image result)
        {
            Bitmap? bmp = null;
            try
            {
                bmp = new Bitmap(path);

                // 画像サイズが指定されたサイズを超える場合、アスペクト比を維持してリサイズ
                if (bmp.Width > MAX_SIZE || bmp.Height > MAX_SIZE)
                {
                    float ratio = Math.Min((float)MAX_SIZE / bmp.Width, (float)MAX_SIZE / bmp.Height);
                    int newWidth = (int)(bmp.Width * ratio);
                    int newHeight = (int)(bmp.Height * ratio);

                    var resizedBmp = new Bitmap(newWidth, newHeight);
                    using (Graphics g = Graphics.FromImage(resizedBmp))
                    {
                        g.DrawImage(bmp, 0, 0, newWidth, newHeight);
                    }

                    bmp.Dispose();
                    bmp = resizedBmp;
                }

                var image = new Image(bmp.Width, bmp.Height);
                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        var rawColor = bmp.GetPixel(x, y);
                        var compressed = Color.Compress(rawColor.R, rawColor.G, rawColor.B, colorLevels);
                        var pixel = new Pixel(compressed);
                        image.SetPixel(x, y, pixel);
                    }
                }

                result = image;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"画像ファイルの取得に失敗しました: {ex.Message}");
                result = null;
                return false;
            }
            finally
            {
                bmp?.Dispose();
            }
        }
    }
}