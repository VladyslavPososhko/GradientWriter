using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradientConsole
{
    public static class GradientExtensions
    {
        public static void UseGradient(this TextWriter writer, GradientPreset preset)
        {
            Console.SetOut(
                new GradientWriter(
                    writer,
                    preset.StartColor(),
                    preset.EndColor()
                )
            );
        }
    }
}
