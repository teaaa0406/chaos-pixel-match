using ChaosPixelMatch.IO;
using ChaosPixelMatch.View;

namespace ChaosPixelMatch
{
    internal class Configuration
    {
        private const int MAX_COLOR_LEVELS = 256;
        private const int MIN_COLOR_LEVELS = 1;
        private const decimal MIN_SIMILARITY_THRESHOLD = 0.5m;

        private readonly byte colorLevels;
        private readonly Image targetImage;
        private readonly string exportName;
        private readonly string exportDirectory;
        private readonly decimal similarityThreshold;

        private Configuration(byte colorLevels, Image targetImage, string exportName, string exportDirectory, decimal similarityThreshold)
        {
            this.colorLevels = colorLevels;
            this.targetImage = targetImage;
            this.exportName = exportName;
            this.exportDirectory = exportDirectory;
            this.similarityThreshold = similarityThreshold;
        }

        public byte ColorLevels => colorLevels;

        public Image TargetImage => targetImage;

        public string ExportName => exportName;

        public string ExportDirectory => exportDirectory;

        public decimal SimilarityThreshold => similarityThreshold;

        public static Configuration Create()
        {
            byte colorLevels;
            Image targetImage;
            string exportName;
            string exportDirectory;
            decimal similarityThreshold;

            // 色階調数の入力
            while (true)
            {
                Console.Write($"プログラムで扱う色階調数を入力してください({MIN_COLOR_LEVELS}~{MAX_COLOR_LEVELS}): ");
                var inputted = Console.ReadLine() ?? string.Empty;
                if (!int.TryParse(inputted, out int result))
                {
                    Console.WriteLine("数値以外が入力されました。");
                    continue;
                }
                if (result < MIN_COLOR_LEVELS || MAX_COLOR_LEVELS < result)
                {
                    Console.WriteLine("入力値が範囲外です。");
                    continue;
                }
                colorLevels = (byte)(result - 1);
                break;
            }

            // 一致対象画像の入力
            while (true)
            {
                Console.Write($"一致対象となる画像のパスを入力してください: ");
                var inputted = Console.ReadLine() ?? string.Empty;
                if (!ImageImporter.TryImport(inputted, colorLevels, out Image? result))
                {
                    Console.WriteLine("画像を読み込めませんでした。");
                    continue;
                }
                targetImage = result;
                break;
            }

            // 出力名の入力
            while (true)
            {
                Console.Write($"出力画像のファイル名を入力してください: ");
                var inputted = Console.ReadLine() ?? string.Empty;
                if (inputted == string.Empty)
                {
                    Console.WriteLine("空の文字列が入力されました。");
                    continue;
                }
                exportName = inputted;
                break;
            }

            // 出力先の入力
            while (true)
            {
                Console.Write($"画像の出力先ディレクトリを入力してください: ");
                var inputted = Console.ReadLine() ?? string.Empty;
                if (!Directory.Exists(inputted))
                {
                    Console.WriteLine("指定されたディレクトリは存在しませんでした。");
                    continue;
                }
                exportDirectory = inputted;
                break;
            }

            // 一致度しきい値を入力
            while (true)
            {
                Console.Write($"一致するとみなすしきい値を入力してください({(int)(MIN_SIMILARITY_THRESHOLD * 100)}~{100}): ");
                var inputted = Console.ReadLine() ?? string.Empty;
                if (!decimal.TryParse(inputted, out decimal result))
                {
                    Console.WriteLine("数値以外が入力されました。");
                    continue;
                }
                if (result < MIN_SIMILARITY_THRESHOLD * 100 || 100 < result)
                {
                    Console.WriteLine("入力値が範囲外です。");
                    continue;
                }
                similarityThreshold = (decimal)result / 100;
                break;
            }

            return new(colorLevels, targetImage, exportName, exportDirectory, similarityThreshold);
        }
    }
}