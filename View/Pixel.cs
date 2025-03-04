namespace ChaosPixelMatch.View
{
    internal readonly struct Pixel
    {
        public const char DOT_CHAR = '█';

        public char Char { get; }
        public Color Color { get; }

        public Pixel(char @char, Color color)
        {
            Char = @char;
            Color = color;
        }

        public Pixel(Color color)
        {
            Char = DOT_CHAR;
            Color = color;
        }

        public static Pixel Black => new(' ', new());
    }
}
