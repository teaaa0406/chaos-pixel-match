using System.Numerics;

namespace ChaosPixelMatch.View
{
    internal readonly struct Color
    {
        public byte R { get; }
        public byte G { get; }
        public byte B { get; }

        public Color(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public static Color Black => new(0, 0, 0);

        public static Color White => new(255, 255, 255);

        public static Color Compress(Color color, int levels)
        {
            return Compress(color.R, color.G, color.B, levels);
        }

        public static Color Compress(byte r, byte g, byte b, int colorLevels)
        {
            var factor = 255f / colorLevels;
            var newR = (byte)(Math.Round(r / factor) * factor);
            var newG = (byte)(Math.Round(g / factor) * factor);
            var newB = (byte)(Math.Round(b / factor) * factor);
            return new Color(newR, newG, newB);
        }

        public decimal GetSimilarity(Color other)
        {
            return (decimal)(CountMatchingBits(R, other.R) + CountMatchingBits(G, other.G) + CountMatchingBits(B, other.B)) / 24;
        }

        private static int CountMatchingBits(byte a, byte b)
        {
            int diff = a ^ b;
            int mismatches = BitOperations.PopCount((uint)diff);
            return 8 - mismatches;
        }
    }
}
