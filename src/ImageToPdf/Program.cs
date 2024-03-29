using Shared.Processors;
using Shared.Services;

Output.Instance
    .DivisionLine()
    .Center("ImagesToPdf")
    .DivisionLine()
    .Center("magically merges multiple images into a single PDF file", Color.TextPrimary)
    .Write("  Version: ", Color.TextMuted).WriteLine(VersionInfoResolver.Version, Color.TextPrimary)
    .Write("  Informational version: ", Color.TextMuted).WriteLine(VersionInfoResolver.InformationalVersion, Color.TextPrimary)
    .DivisionLine()
    .Center("Usage", Color.TextPrimary)
    .Write("  simply drag & drop files over ").Write("ImagesToPdf.exe ", Color.TextInfo).WriteLine("or use the command line")
    .WriteLine()
    .Write("  ImagesToPdf.exe ").WriteLine("[...files or directories] [--out:output] [--layout:name]", Color.TextMuted)
    .WriteLine()
    .WriteLine("  Arguments:")
    .Write("  --out:path ", Color.TextInfo).WriteLine("Path to the output file or directory")
    .Write("  --layout:name ", Color.TextInfo).WriteLine("The page layout for the PDF viewer")
    .WriteLayoutOptions()
    .WriteLine()
    .WriteLine("  Examples:")
    .Write("  ImagesToPdf.exe ").WriteLine("image-1.jpeg image-2.jpeg path/to/dir --out:output.pdf --layout:TwoPageLeft", Color.TextMuted)
    .DivisionLine();

try
{
    var processor = new PdfMerger();
    processor.ParseArguments(args);
    await processor.ProcessAsync();
}
catch (Exception e)
{
    Output.Instance.Error(e.Message);
}

Console.ReadLine();
