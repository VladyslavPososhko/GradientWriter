using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradientConsole
{
    public enum GradientPreset
    {
        PinkToPurple,
        BlueToCyan,
        Sunset,
        Fire,
        Matrix,
        Rainbow
    }

    public static class GradientPresets
    {
        public static (int r, int g, int b) StartColor(this GradientPreset preset) =>
            preset switch
            {
                GradientPreset.PinkToPurple => (255, 105, 180),
                GradientPreset.BlueToCyan => (0, 120, 255),
                GradientPreset.Sunset => (255, 94, 0),
                GradientPreset.Fire => (255, 0, 0),
                GradientPreset.Matrix => (0, 255, 70),
                GradientPreset.Rainbow => (255, 0, 0),
                _ => (255, 255, 255)
            };

        public static (int r, int g, int b) EndColor(this GradientPreset preset) =>
            preset switch
            {
                GradientPreset.PinkToPurple => (147, 0, 211),
                GradientPreset.BlueToCyan => (0, 255, 255),
                GradientPreset.Sunset => (255, 165, 0),
                GradientPreset.Fire => (255, 255, 0),
                GradientPreset.Matrix => (0, 120, 0),
                GradientPreset.Rainbow => (0, 0, 255),
                _ => (255, 255, 255)
            };
    }
}
