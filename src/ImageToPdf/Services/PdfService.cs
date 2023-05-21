namespace ImagesToPdf.Services
{
    using iText.IO.Image;
    using iText.Kernel.Geom;
    using iText.Kernel.Pdf;
    using iText.Layout;
    using iText.Layout.Element;

    public class PdfService
    {
        private string[]? inputFiles;
        private PdfName? layout;
        private string? outputFile;

        public static PdfService Create()
        {
            return new PdfService();
        }

        public PdfService WithInputFiles(params string[] images)
        {
            this.inputFiles = images;

            return this;
        }

        public PdfService WithLayout(PdfName? layout)
        {
            this.layout = layout;

            return this;
        }

        public PdfService OutputTo(string outputFile)
        {
            this.outputFile = outputFile;

            return this;
        }

        public async Task ProcessAsync()
        {
            this.ValidateArguments();

            var processType = this.GetProcessType();
            switch (processType)
            {
                case ProcessType.Merge:
                    await this.MergeAsync();
                    break;

                case ProcessType.Modify:
                    await this.ModifyAsync();
                    break;
            }
        }

        private async Task MergeAsync()
        {
            Output.Instance.WriteLine("Starting PDF merge", Color.TextInfo);
            Output.Instance.Write("Outputting to ", Color.TextInfo).WriteLine(this.outputFile, Color.TextMuted);

            await using var pdfWriter = new PdfWriter(this.outputFile!);
            using var pdfDocument = new PdfDocument(pdfWriter);
            using var document = new Document(pdfDocument);

            document.SetMargins(0, 0, 0, 0);

            foreach (var imagePath in this.inputFiles!)
            {
                Output.Instance.WriteLine($"Adding image {imagePath}", Color.TextMuted);

                var imageData = ImageDataFactory.Create(imagePath);
                var image = new Image(imageData);

                pdfDocument.AddNewPage(new PageSize(image.GetImageWidth(), image.GetImageHeight()));
                document.Add(image);
            }

            if (this.layout != null)
            {
                Output.Instance.Write("Set page layout to ", Color.TextInfo).WriteLine(this.layout.GetValue(), Color.TextMuted);
                pdfDocument.GetCatalog().SetPageLayout(this.layout);
            }

            document.Close();

            Output.Instance.WriteLine("Finished PDF generation", Color.TextInfo);
        }

        private async Task ModifyAsync()
        {
            Output.Instance.WriteLine("Starting PDF modification", Color.TextInfo);

            foreach (var inputFile in this.inputFiles!)
            {
                var temporaryFile = inputFile + ".mod";

                Output.Instance.Write("Processing document ", Color.TextPrimary).WriteLine(inputFile, Color.TextInfo);
                using (var pdfReader = new PdfReader(inputFile))
                await using (var pdfWriter = new PdfWriter(temporaryFile))
                using (var pdfDocument = new PdfDocument(pdfReader, pdfWriter))
                {
                    Output.Instance.Write("- Set page layout to ", Color.TextMuted).WriteLine(this.layout.GetValue(), Color.TextInfo);
                    pdfDocument.GetCatalog().SetPageLayout(this.layout);
                    pdfDocument.Close();
                }

                Output.Instance.WriteLine("- Rename temp file", Color.TextMuted);
                File.Delete(inputFile);
                File.Move(temporaryFile, inputFile);
            }

            Output.Instance.WriteLine("Finished PDF generation", Color.TextInfo);
        }

        private void ValidateArguments()
        {
            if (this.inputFiles == null)
            {
                throw new Exception("No input files specified!");
            }

            switch (this.GetProcessType())
            {
                case ProcessType.Merge:
                    if (this.outputFile == null)
                    {
                        throw new Exception("Output file not specified!");
                    }

                    break;

                case ProcessType.Modify:
                    if (this.layout == null)
                    {
                        throw new Exception("No layout specified");
                    }

                    break;
            }
        }

        private ProcessType GetProcessType()
        {
            if (this.inputFiles?.All(e => e.EndsWith(".pdf")) == true)
            {
                return ProcessType.Modify;
            }

            return ProcessType.Merge;
        }

        private enum ProcessType
        {
            Merge,
            Modify,
        }
    }
}
