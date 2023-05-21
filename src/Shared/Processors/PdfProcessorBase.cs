namespace Shared.Processors
{
    using Shared.Services;

    public abstract class PdfProcessorBase
    {
        protected string[]? inputFiles;

        public virtual void ParseArguments(string[] args)
        {
            this.inputFiles = FileFinder.GetInputFiles(args);
            if (!this.inputFiles.Any())
            {
                Output.Instance
                    .Error("No input files specified");
                Console.ReadLine();

                Environment.Exit(-1);
            }
        }

        public abstract Task ProcessAsync();
    }
}
