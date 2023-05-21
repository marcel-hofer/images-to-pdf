namespace Shared.Processors
{
    using iText.IO.Image;
    using iText.Kernel.Geom;
    using iText.Kernel.Pdf;
    using iText.Layout;
    using iText.Layout.Element;

    using Shared.Services;

    public class PdfMerger : PdfProcessorBase
    {
        private PdfName? layout;
        private string? outputFile;

        public override void ParseArguments(string[] args)
        {
            base.ParseArguments(args);

            this.outputFile = FileFinder.GetOutputFile(args, this.inputFiles!);
            if (this.outputFile == null)
            {
                throw new Exception("Output file not specified!");
            }

            this.layout = Layout.GetLayoutOrDefault(args);
        }

        public override async Task ProcessAsync()
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

            PdfLayoutChanger.ChangeLayout(pdfDocument, this.layout);

            document.Close();

            Output.Instance.WriteLine("Finished PDF generation", Color.TextInfo);
        }
    }
}
