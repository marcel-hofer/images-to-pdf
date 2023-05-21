using Shared.Processors;
using Shared.Services;

Output.Instance
    .DivisionLine()
    .Center("PdfLayoutChanger")
    .DivisionLine()
    .Center("Changes the page layout of one or multiple PDF files", Color.TextPrimary)
    .Write("  Version: ", Color.TextMuted).WriteLine(VersionInfoResolver.Version, Color.TextPrimary)
    .Write("  Informational version: ", Color.TextMuted).WriteLine(VersionInfoResolver.InformationalVersion, Color.TextPrimary)
    .DivisionLine()
    .Center("Usage", Color.TextPrimary)
    .Write("  simply drag & drop files over ").Write("PdfLayoutChanger.exe ", Color.TextInfo).WriteLine("or use the command line")
    .WriteLine("  Warning: The input files are being overwritten!", Color.TextDanger)
    .WriteLine()
    .Write("  PdfLayoutChanger.exe ").WriteLine("[...files or directories] [--layout:name]", Color.TextMuted)
    .WriteLine()
    .WriteLine("  Arguments:")
    .Write("  --layout:name ", Color.TextInfo).WriteLine("The page layout for the PDF viewer")
    .WriteLayoutOptions()
    .WriteLine()
    .WriteLine("  Examples:")
    .Write("  PdfLayoutChanger.exe ").WriteLine("document-1.pdf document-2.pdf path/to/dir --layout:TwoPageLeft", Color.TextMuted)
    .DivisionLine();

try
{
    var processor = new PdfLayoutChanger();
    processor.ParseArguments(args);
    await processor.ProcessAsync();
}
catch (Exception e)
{
    Output.Instance.Error(e.Message);
}

Console.ReadLine();
