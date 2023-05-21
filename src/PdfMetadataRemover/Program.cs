using Shared.Processors;
using Shared.Services;

Output.Instance
    .DivisionLine()
    .Center("PdfMetadataRemover")
    .DivisionLine()
    .Center("Removes most metadata from one or more PDF files", Color.TextPrimary)
    .Write("  Version: ", Color.TextMuted).WriteLine(VersionInfoResolver.Version, Color.TextPrimary)
    .Write("  Informational version: ", Color.TextMuted).WriteLine(VersionInfoResolver.InformationalVersion, Color.TextPrimary)
    .DivisionLine()
    .Center("Usage", Color.TextPrimary)
    .Write("  simply drag & drop files over ").Write("PdfMetadataRemover.exe ", Color.TextInfo).WriteLine("or use the command line")
    .WriteLine("  Warning: The input files are being overwritten!", Color.TextDanger)
    .WriteLine()
    .Write("  PdfMetadataRemover.exe ").WriteLine("[...files or directories]", Color.TextMuted)
    .WriteLine()
    .WriteLine("  Examples:")
    .Write("  PdfMetadataRemover.exe ").WriteLine("document-1.pdf document-2.pdf path/to/dir", Color.TextMuted)
    .DivisionLine();

try
{
    var processor = new PdfMetadataRemover();
    processor.ParseArguments(args);
    await processor.ProcessAsync();
}
catch (Exception e)
{
    Output.Instance.Error(e.Message);
}

Console.ReadLine();
