using ImagesToPdf.Services;

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
    .DivisionLine()
    .Center("Change layout of existing PDF files")
    .WriteLine("You can also pass one or more PDF files to adjust their layout. ")
    .WriteLine("Warning: The input files are being overwritten!", Color.TextDanger)
    .WriteLine()
    .WriteLine("  Examples:")
    .Write("  ImagesToPdf.exe ").WriteLine("document-1.pdf document-2.pdf path/to/dir --layout:TwoPageLeft", Color.TextMuted)
    .DivisionLine();

try
{
    var images = FileFinder.GetInputFiles(args);
    if (!images.Any())
    {
        Output.Instance
            .Error("No input files specified");
        Console.ReadLine();

        Environment.Exit(-1);
    }

    var outputFile = FileFinder.GetOutputFile(args, images);
    var layout = Layout.GetLayout(args);

    await PdfService.Create()
        .WithInputFiles(images)
        .WithLayout(layout)
        .OutputTo(outputFile)
        .ProcessAsync();
}
catch (Exception e)
{
    Output.Instance.Error(e.Message);
}

Console.ReadLine();
