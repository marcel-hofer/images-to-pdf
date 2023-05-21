namespace Shared.Processors
{
    using iText.Kernel.Pdf;

    using Shared.Services;

    public class PdfLayoutChanger : PdfProcessorBase
    {
        private PdfName? layout;

        public override void ParseArguments(string[] args)
        {
            base.ParseArguments(args);

            this.layout = Layout.GetLayoutOrDefault(args);
            if (this.layout == null)
            {
                throw new Exception("No layout specified");
            }
        }

        public override async Task ProcessAsync()
        {
            Output.Instance.WriteLine("Starting PDF layout change", Color.TextInfo);

            foreach (var inputFile in this.inputFiles!)
            {
                var temporaryFile = inputFile + ".mod.pdf";

                Output.Instance.Write("Processing document ", Color.TextPrimary).WriteLine(inputFile, Color.TextInfo);
                using (var pdfReader = new PdfReader(inputFile))
                await using (var pdfWriter = new PdfWriter(temporaryFile))
                using (var pdfDocument = new PdfDocument(pdfReader, pdfWriter))
                {
                    ChangeLayout(pdfDocument, this.layout);
                    pdfDocument.Close();
                }

                Output.Instance.WriteLine("Rename temp file", Color.TextMuted);
                File.Delete(inputFile);
                File.Move(temporaryFile, inputFile);
            }

            Output.Instance.WriteLine("Finished PDF modification", Color.TextInfo);
        }

        public static void ChangeLayout(PdfDocument pdfDocument, PdfName? layout)
        {
            if (layout == null)
            {
                return;
            }

            Output.Instance.Write("Set page layout to ", Color.TextMuted).WriteLine(layout.GetValue(), Color.TextInfo);
            pdfDocument.GetCatalog().SetPageLayout(layout);
        }
    }
}
