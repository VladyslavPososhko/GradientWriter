using System.Text;
using System;
using System.IO;

namespace GradientConsole
{
    /// <summary>
    /// Provides gradient text rendering for console apps.
    /// </summary>
    public class GradientWriter : TextWriter
    {
        private readonly TextWriter original;
        private readonly (int r, int g, int b) startColor;
        private readonly (int r, int g, int b) endColor;

        public GradientWriter(
            TextWriter original,
            (int r, int g, int b) startColor,
            (int r, int g, int b) endColor)
        {
            this.original = original;
            this.startColor = startColor;
            this.endColor = endColor;
        }

        public override Encoding Encoding => original.Encoding;

        // Helper: write a segment (без переводов строки) с градиентом
        private void WriteColoredSegment(string segment)
        {
            if (string.IsNullOrEmpty(segment))
                return;

            for (int i = 0; i < segment.Length; i++)
            {
                double t = segment.Length == 1 ? 0.0 : (double)i / (segment.Length - 1);
                int r = (int)(startColor.r + (endColor.r - startColor.r) * t);
                int g = (int)(startColor.g + (endColor.g - startColor.g) * t);
                int b = (int)(startColor.b + (endColor.b - startColor.b) * t);

                original.Write($"\u001b[38;2;{r};{g};{b}m{segment[i]}");
            }
        }

        // Основной Write для строк: разбиваем по \r и \n, пишем сегменты с градиентом,
        // а переводы строк выводим "сырыми" (и перед ними делаем сброс цвета).
        public override void Write(string? value)
        {
            if (value is null)
                return;

            int len = value.Length;
            if (len == 0)
                return;

            int idx = 0;
            while (idx < len)
            {
                int next = value.IndexOfAny(new[] { '\r', '\n' }, idx);
                if (next == -1)
                {
                    // остаток без переводов
                    WriteColoredSegment(value.Substring(idx));
                    idx = len;
                }
                else
                {
                    // сегмент перед переводом
                    if (next > idx)
                    {
                        WriteColoredSegment(value.Substring(idx, next - idx));
                    }

                    // перед фактическим переводом сбрасываем цвет
                    original.Write("\u001b[0m");

                    // обработка возможной последовательности \r\n
                    if (value[next] == '\r' && next + 1 < len && value[next + 1] == '\n')
                    {
                        original.Write("\r\n");
                        idx = next + 2;
                    }
                    else
                    {
                        original.Write(value[next]);
                        idx = next + 1;
                    }
                }
            }

            // финальный сброс цвета (на всякий случай, если строка не заканчивалась переводом)
            original.Write("\u001b[0m");
        }

        // Переопределяем Write(char), чтобы корректно обрабатывать одиночные \r,\n
        public override void Write(char value)
        {
            if (value == '\r' || value == '\n')
            {
                // просто пишем символ новой строки "сырым"
                original.Write(value);
            }
            else
            {
                // для одного символа используем стартовый цвет и сразу сбрасываем
                int r = startColor.r;
                int g = startColor.g;
                int b = startColor.b;
                original.Write($"\u001b[38;2;{r};{g};{b}m{value}\u001b[0m");
            }
        }

        // Обеспечиваем корректную работу пустого WriteLine() и других перегрузок WriteLine
        public override void WriteLine()
        {
            // перед переводом сбрасываем цвет
            original.Write("\u001b[0m");
            original.WriteLine();
        }

        public override void WriteLine(char value)
        {
            Write(value);
            original.WriteLine();
        }

        public override void WriteLine(string? value)
        {
            Write(value);
            original.WriteLine();
        }
    }
}