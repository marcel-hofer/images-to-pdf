using ImageToPdf.Services;

Console.WriteLine("ImageToPdf - magically merge multiple images to a single PDF file");
Console.WriteLine($"  Version: {VersionInfoResolver.Version}");
Console.WriteLine($"  Informational version: {VersionInfoResolver.InformationalVersion}");
Console.WriteLine("");

Console.WriteLine("Hello, World!");
Console.WriteLine($"Args: {string.Join(", ", args)}");
Console.ReadLine();
