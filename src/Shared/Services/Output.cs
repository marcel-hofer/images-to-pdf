namespace Shared.Services
{
    public class Output
    {
        public static Output Instance { get; } = new Output();

        public Output Write(string message, Color color = Color.TextDefault)
        {
            SetColor(color);

            Console.Write(message);

            Console.ResetColor();

            return this;
        }

        public Output WriteLine(string? message = "", Color color = Color.TextDefault)
        {
            SetColor(color);

            var emptySpace = GetEmptySpace(message);
            Console.WriteLine($"{message.PadRight(emptySpace)}");

            Console.ResetColor();

            return this;
        }

        public Output Left(string message, Color color = Color.TextDefault)
        {
            return this.WriteLine($"{message.PadRight(FullWidth())}", color);
        }

        public Output Right(string message, Color color = Color.TextDefault)
        {
            return this.WriteLine($"{message.PadLeft(FullWidth())}", color);
        }

        public Output Center(string message, Color color = Color.TextDefault)
        {
            decimal size = FullWidth() - message.Length;
            int rightSize = (int)Math.Round(size / 2);
            int leftSize = (int)(size - rightSize);
            string leftMargin = new String(' ', leftSize);
            string rightMargin = new String(' ', rightSize);

            return this.WriteLine(leftMargin + message + rightMargin, color);
        }

        public Output Extreme(string left, string right, Color color = Color.TextDefault)
        {
            decimal size = FullWidth();
            int rightMargin = (int)Math.Round(size / 2);
            int leftMargin = (int)(size - rightMargin);

            return this.WriteLine($"{left}".PadRight(rightMargin) + $"{right}".PadLeft(leftMargin), color);
        }

        public Output DivisionLine(char character = '-', Color color = Color.TextMuted)
        {
            var text = new string(character, FullWidth());
            return this.WriteLine(text, color);
        }

        public Output BlankLines(int? lines = 1, Color color = Color.TextDefault)
        {
            for (int i = 0; i < lines; i++)
            {
                this.DivisionLine(' ', color);
            }

            return this;
        }

        public void Error(string message)
        {
            this.DivisionLine(color: Color.BgDanger)
                .Center("Error", Color.BgDanger)
                .DivisionLine(color: Color.BgDanger)
                .WriteLine(message, Color.BgDanger)
                .DivisionLine(color: Color.BgDanger);
        }

        public void Sample()
        {
            this.DivisionLine(color: Color.TextMuted);
            this.Center("Output sample", Color.TextMuted);
            this.DivisionLine(color: Color.TextMuted);
            this.Write("Text: ")
                .Write("Muted", Color.TextMuted).Write(" | ")
                .Write("Primary", Color.TextPrimary).Write(" | ")
                .Write("Warning", Color.TextWarning).Write(" | ")
                .Write("Danger", Color.TextDanger).Write(" | ")
                .Write("Success", Color.TextSuccess).Write(" | ")
                .Write("Info", Color.TextInfo).WriteLine();

            this.Write("Bg  : ")
                .Write("Muted", Color.BgMuted).Write(" | ")
                .Write("Primary", Color.BgPrimary).Write(" | ")
                .Write("Warning", Color.BgWarning).Write(" | ")
                .Write("Danger", Color.BgDanger).Write(" | ")
                .Write("Success", Color.BgSuccess).Write(" | ")
                .Write("Info", Color.BgInfo).WriteLine();
            this.DivisionLine(color: Color.TextMuted);
        }

        private static int FullWidth() => Console.WindowWidth - 1;

        private static int GetEmptySpace(string? message)
        {
            message ??= string.Empty;

            int size = FullWidth();
            if (size != Console.CursorLeft + 1)
            {
                size -= Console.CursorLeft;
            }

            if (size < message.Length)
            {
                size = Console.WindowWidth + size;
            }

            if (size < 0)
            {
                size = message.Length;
            }

            return size;
        }

        private static void SetColor(Color color)
        {
            switch (color)
            {
                case Color.TextMuted:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case Color.TextPrimary:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case Color.TextWarning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case Color.TextDanger:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case Color.TextSuccess:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case Color.TextInfo:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
                    
                case Color.BgMuted:
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case Color.BgPrimary:
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case Color.BgWarning:
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case Color.BgDanger:
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case Color.BgSuccess:
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case Color.BgInfo:
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }
    }

    public enum Color
    {
        TextDefault,
        TextMuted,
        TextPrimary,
        TextWarning,
        TextDanger,
        TextSuccess,
        TextInfo,

        BgDefault,
        BgMuted,
        BgPrimary,
        BgWarning,
        BgDanger,
        BgSuccess,
        BgInfo,
    }
}
