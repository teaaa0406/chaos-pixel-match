using System.Text;

namespace ChaosPixelMatch.View
{
    internal static class ConsoleDisplay
    {
        public static void Clear()
        {
            Console.Clear();
        }

        public static void Render(Image image)
        {
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;
            Console.OutputEncoding = Encoding.UTF8;

            var sb = new StringBuilder();
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Pixel pixel = image.GetPixel(x, y);
                    if (pixel.Char == Pixel.DOT_CHAR)
                    {
                        sb.Append($"\x1b[38;2;{pixel.Color.R};{pixel.Color.G};{pixel.Color.B}m{pixel.Char}");
                        sb.Append($"\x1b[38;2;{pixel.Color.R};{pixel.Color.G};{pixel.Color.B}m{pixel.Char}");
                    }
                    else
                    {
                        sb.Append($"\x1b[38;2;{pixel.Color.R};{pixel.Color.G};{pixel.Color.B}m{pixel.Char}");
                    }
                }
                sb.AppendLine("\x1b[0m");
            }
            Console.Write(sb.ToString());
        }
    }
}
