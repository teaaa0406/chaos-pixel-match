using ChaosPixelMatch;
using ChaosPixelMatch.IO;
using ChaosPixelMatch.View;

long createCount = 0;
long matchedCount = 0;

// 構成を入力
var config = Configuration.Create();

ConsoleDisplay.Clear();
while (true)
{
    var targetImage = config.TargetImage;

    // 画像生成(ランダム)
    var randomImage = Image.CreateRandom(targetImage.Width, targetImage.Height, config.ColorLevels);
    createCount++;

    // 一致度を判定
    var similarity = targetImage.GetSimilarity(randomImage);
    
    // 描画
    var displayImage = CreateDisplayImage(targetImage, randomImage, similarity);
    ConsoleDisplay.Render(displayImage);

    // 一致していた際の処理
    if (similarity >= config.SimilarityThreshold)
    {
        var fileName = $"{config.ExportName}_{string.Format("{0:F4}", similarity * 100)}%.png";
        ImageExporter.Export(displayImage, Path.Combine(config.ExportDirectory, fileName));
        matchedCount++;
    }

    // 終了処理
    if (ShouldQuit()) break;
}


Image CreateDisplayImage(Image targetImage, Image randomImage, decimal similarity)
{
    var displayImage = new Image(targetImage.Width * 2 + 1, targetImage.Height + 7);

    // ターゲット画像を描画
    for (int y = 0; y < targetImage.Height; y++)
    {
        for (int x = 0; x < targetImage.Width; x++)
        {
            displayImage.SetPixel(x, y + 1, targetImage.GetPixel(x, y));
        }
    }

    // ランダム画像を描画
    for (int y = 0; y < targetImage.Height; y++)
    {
        for (int x = 0; x < targetImage.Width; x++)
        {
            displayImage.SetPixel(x + targetImage.Width + 1, y + 1, randomImage.GetPixel(x, y));
        }
    }

    // 情報文字列を描画
    displayImage.InsertText(0, targetImage.Height + 2, $"色階調数    : {config.ColorLevels + 1}", Color.White);
    displayImage.InsertText(0, targetImage.Height + 3, $"一致判定しきい値: {config.SimilarityThreshold * 100}%", Color.White);
    displayImage.InsertText(0, targetImage.Height + 4, $"一致度     : {string.Format("{0:F4}", similarity * 100)}%", Color.White);
    displayImage.InsertText(0, targetImage.Height + 5, $"生成回数    : {createCount}", Color.White);
    displayImage.InsertText(0, targetImage.Height + 6, $"一致判定回数  : {matchedCount}", Color.White);

    return displayImage;
}

bool ShouldQuit()
{
    if (Console.KeyAvailable)
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        if (keyInfo.Key == ConsoleKey.Q)
        {
            return true;
        }
    }
    return false;
}