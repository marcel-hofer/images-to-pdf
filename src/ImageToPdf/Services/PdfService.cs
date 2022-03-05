namespace ImagesToPdf.Services
{
    using iText.IO.Image;
    using iText.Kernel.Geom;
    using iText.Kernel.Pdf;
    using iText.Layout;
    using iText.Layout.Element;

    public class PdfService
    {
        private string[]? images;
        private string? outputFile;

        public static PdfService Create()
        {
            return new PdfService();
        }

        public PdfService WithImages(params string[] images)
        {
            this.images = images;

            return this;
        }

        public PdfService OutputTo(string outputFile)
        {
            this.outputFile = outputFile;

            return this;
        }

        public async Task ProcessAsync()
        {
            Output.Instance.WriteLine("Starting PDF generation", Color.TextInfo);

            this.ValidateArguments();

            Output.Instance.WriteLine($"Outputting to {this.outputFile}", Color.TextMuted);

            await using var pdfWriter = new PdfWriter(this.outputFile!);
            using var pdfDocument = new PdfDocument(pdfWriter);
            using var document = new Document(pdfDocument);

            document.SetMargins(0, 0, 0, 0);

            foreach (var imagePath in this.images!)
            {
                Output.Instance.WriteLine($"Adding image {imagePath}", Color.TextMuted);

                var imageData = ImageDataFactory.Create(imagePath);
                var image = new Image(imageData);

                pdfDocument.AddNewPage(new PageSize(image.GetImageWidth(), image.GetImageHeight()));
                document.Add(image);
            }

            document.Close();

            Output.Instance.WriteLine("Finished PDF generation", Color.TextInfo);
        }

        private void ValidateArguments()
        {
            if (this.images == null)
            {
                throw new Exception("No images specified!");
            }

            if (this.outputFile == null)
            {
                throw new Exception("Output file not specified!");
            }
        }
    }
}
