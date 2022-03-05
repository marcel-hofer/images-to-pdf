namespace ImageToPdf.Services
{
    public static class FileFinder
    {
        public static string[] GetInputFiles(params string[] args)
        {
            return args
                .Where(e => !e.StartsWith("-"))
                .SelectMany(GetFiles)
                .OrderBy(e => e)
                .ToArray();
        }

        public static string GetOutputFile(string[] args, string[] files)
        {
            // Find specific output
            var outputFile = args.FirstOrDefault(e => e.StartsWith("--out:"));
            if (outputFile != null)
            {
                outputFile = outputFile[6..];
                if (Path.HasExtension(outputFile))
                {
                    return outputFile;
                }
                

                Output.Instance.Error($"The defined output '{outputFile}' is not valid.");
            }

            // Ask user for output file
            Output.Instance
                .Write("Where should the PDF be saved?", Color.BgInfo)
                .Write(" (default: output.pdf): ", Color.TextMuted);

            outputFile = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(outputFile))
            {
                outputFile = "output.pdf";
            }

            if (!Path.HasExtension(outputFile))
            {
                outputFile += ".pdf";
            }

            if (!Path.IsPathRooted(outputFile))
            {
                outputFile = Path.Combine(GetMostRelevantPath(), outputFile);
            }

            return outputFile;
        }

        public static string GetMostRelevantPath()
        {
            return Path.GetDirectoryName(Environment.ProcessPath)
                ?? Environment.CurrentDirectory;
        }

        private static IEnumerable<string> GetFiles(string fileOrDirectory)
        {
            if (File.Exists(fileOrDirectory))
            {
                yield return fileOrDirectory;
            }

            if (Directory.Exists(fileOrDirectory))
            {
                var directoryFiles = Enumerable.Concat(
                    Directory.GetFiles(fileOrDirectory),
                    Directory.GetDirectories(fileOrDirectory)
                ).SelectMany(GetFiles);

                foreach (var file in directoryFiles)
                {
                    yield return file;
                }
            }
        }
    }
}
