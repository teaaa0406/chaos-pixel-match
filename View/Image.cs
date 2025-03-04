namespace ChaosPixelMatch.View
{
    internal class Image
    {
        private readonly int width;
        private readonly int height;
        private readonly Pixel?[,] pixels;

        public int Width => width;
        public int Height => height;

        public Image(int width, int height)
        {
            this.width = width;
            this.height = height;
            pixels = new Pixel?[width, height];
        }

        public static Image CreateRandom(int width, int height, byte colorLevels)
        {
            var image = new Image(width, height);
            var random = new Random();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var r = (byte)random.Next(0, 256);
                    var g = (byte)random.Next(0, 256);
                    var b = (byte)random.Next(0, 256);
                    var color = Color.Compress(r, g, b, colorLevels);
                    var pixel = new Pixel(color);
                    image.SetPixel(x, y, pixel);
                }
            }
            return image;
        }

        public Pixel GetPixel(int x, int y)
        {
            return pixels[x, y] ?? Pixel.Black;
        }

        public void SetPixel(int x, int y, Pixel pixel)
        {
            pixels[x, y] = pixel;
        }

        public void InsertText(int x, int y, string text, Color color)
        {
            if (x < 0 || x >= width) throw new ArgumentOutOfRangeException(nameof(x));
            if (y < 0 || y >= height) throw new ArgumentOutOfRangeException(nameof(y));

            var ptrX = x;
            var ptrY = y;
            var i = 0;
            while (i < text.Length)
            {
                if (ptrX >= width)
                {
                    ptrX = 0;
                    ptrY++;
                    if (ptrY >= height) return;
                }
                var pixel = new Pixel(text[i], color);
                SetPixel(ptrX, ptrY, pixel);
                ptrX++;
                i++;
            }
        }

        public decimal GetSimilarity(Image other)
        {
            if (other.Width != width || other.Height != height) throw new ArgumentException(nameof(other));
            decimal similaritySum = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var ownPixelColor = GetPixel(x, y).Color;
                    var otherPixelColor = other.GetPixel(x, y).Color;
                    similaritySum += ownPixelColor.GetSimilarity(otherPixelColor);
                }
            }
            return similaritySum / (width * height);
        }

    }
}
