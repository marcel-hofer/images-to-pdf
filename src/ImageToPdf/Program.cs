using ImageToPdf.Services;

Output.Instance
    .DivisionLine()
    .Center("ImageToPdf")
    .DivisionLine()
    .Center("magically merges multiple images into a single PDF file", Color.TextPrimary)
    .Write("  Version: ", Color.TextMuted).WriteLine(VersionInfoResolver.Version, Color.TextPrimary)
    .Write("  Informational version: ", Color.TextMuted).WriteLine(VersionInfoResolver.InformationalVersion, Color.TextPrimary)
    .DivisionLine();

try
{
    //var images = new[]
    //{
    //    @"D:\temp\pdf\input\000 200dpi.jpeg",
    //    @"D:\temp\pdf\input\001 200dpi.jpeg",
    //    @"D:\temp\pdf\input\002 200dpi.jpeg",
    //};

    var images = Directory.GetFiles(@"D:\temp\pdf\input").OrderBy(e => e).ToArray();
    var outputFile = @"D:\temp\pdf\output.pdf";

    await PdfService.Create()
        .WithImages(images)
        .OutputTo(outputFile)
        .ProcessAsync();
}
catch (Exception e)
{
    Output.Instance.Error(e.Message);
}

Console.ReadLine();
