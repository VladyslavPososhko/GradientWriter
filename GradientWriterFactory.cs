using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradientConsole
{
    public static class GradientWriterFactory
    {
        public static GradientWriter FromPreset(
            TextWriter original,
            GradientPreset preset)
        {
            return new GradientWriter(
                original,
                preset.StartColor(),
                preset.EndColor()
            );
        }
    }
}
