namespace Shared.Processors
{
    using iText.Kernel.Pdf;
    using iText.Kernel.XMP;

    using Shared.Services;

    public class PdfMetadataRemover : PdfProcessorBase
    {
        public override async Task ProcessAsync()
        {
            Output.Instance.WriteLine("Starting metadata removal", Color.TextInfo);

            foreach (var inputFile in this.inputFiles!)
            {
                var temporaryFile = inputFile + ".mod.pdf";

                Output.Instance.Write("Processing document ", Color.TextPrimary).WriteLine(inputFile, Color.TextInfo);
                using (var pdfReader = new PdfReader(inputFile))
                await using (var pdfWriter = new PdfWriter(temporaryFile))
                using (var pdfDocument = new PdfDocument(pdfReader, pdfWriter))
                {
                    // Remove default document info
                    var info = pdfDocument.GetDocumentInfo();
                    info.SetTitle(string.Empty);
                    info.SetSubject(string.Empty);
                    info.SetCreator(string.Empty);
                    info.SetAuthor(string.Empty);
                    info.SetProducer(string.Empty);
                    info.SetKeywords(string.Empty);

                    // Remove additional XMP metadata
                    pdfDocument.SetXmpMetadata(XMPMetaFactory.Create());

                    pdfDocument.Close();
                }

                Output.Instance.WriteLine("Rename temp file", Color.TextMuted);
                File.Delete(inputFile);
                File.Move(temporaryFile, inputFile);
            }

            Output.Instance.WriteLine("Finished metadata removal", Color.TextInfo);
        }
    }
}
