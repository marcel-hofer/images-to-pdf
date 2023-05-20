namespace ImagesToPdf.Services
{
    using iText.Kernel.Pdf;

    public static class Layout
    {
        private static readonly LayoutOption[] layouts = new[]
        {
            new LayoutOption("Default", "No specific layout defined (default)"),
            new LayoutOption("SinglePage", "Single Page"),
            new LayoutOption("OneColumn", "Single Page Continuous"),
            new LayoutOption("TwoPageLeft", "Two-Up (Facing)"),
            new LayoutOption("TwoColumnLeft", "Two-Up Continuous (Facing)"),
            new LayoutOption("TwoPageRight", "Two-Up (Cover Page)"),
            new LayoutOption("TwoColumnRight", "Two-Up Continuous (Cover Page)"),
        };

        public static PdfName? GetLayout(string[] args)
        {
            // Find specific layout
            var layout = args.FirstOrDefault(e => e.StartsWith("--layout:"));
            LayoutOption? option;
            if (layout != null)
            {
                layout = layout[9..];
                option = FindSelectedLayoutOrDefault(layout);
                if (option != null)
                {
                    return option.ToPdfName();
                }

                Output.Instance.Error($"The defined layout '{layout}' is not valid.");
            }

            // Ask user for layout
            Output.Instance
                .WriteLine("No layout specified. You can select one of these options:", Color.BgMuted)
                .WriteLayoutOptions()
                .Write("Which PDF layout should be used?", Color.BgInfo)
                .Write(" (default: Default): ", Color.TextMuted);

            layout = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(layout))
            {
                return layouts[0].ToPdfName();
            }

            option = FindSelectedLayoutOrDefault(layout);
            if (option != null)
            {
                return option.ToPdfName();
            }

            Output.Instance.Error($"The defined layout '{layout}' is not valid.");

            return null;
        }

        public static Output WriteLayoutOptions(this Output output)
        {
            for (var index = 0; index < layouts.Length; index++)
            {
                var option = layouts[index];
                output.Write($"    {index + 1} | {option.Key}: ").WriteLine(option.Description, Color.TextMuted);
            }

            return output;
        }

        private static LayoutOption? FindSelectedLayoutOrDefault(string layout)
        {
            if (int.TryParse(layout, out var index) && layouts.Length >= index)
            {
                return layouts[index - 1];
            }

            return layouts.FirstOrDefault(e => e.Key == layout);
        }

        private class LayoutOption
        {
            public string Key { get; init; }

            public string Description { get; init; }

            public LayoutOption(string key, string description)
            {
                this.Key = key;
                this.Description = description;
            }

            public PdfName? ToPdfName()
            {
                if (this.Key == "Default")
                {
                    return null;
                }

                return PdfName.staticNames[Key];
            }
        }
    }
}
