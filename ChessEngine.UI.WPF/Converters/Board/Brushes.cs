﻿using System.Linq;
using System.Windows.Media;

namespace ChessEngine.UI.WPF.Converters.Board
{
    public static class Brushes
    {
        private static readonly Color DarkColor = Color.FromRgb(0xB8, 0x8C, 0x78);
        private static readonly Color LightColor = Color.FromRgb(0xF1, 0xD9, 0xC8);
        private static readonly Color SelectedColor = Color.FromArgb(0xFF, 0x29, 0x84, 0xCB);
        private static readonly Color HighlightedColor = Color.FromArgb(0xB0, 0x12, 0xA0, 0xDA);
        private static readonly Color MarkedColor = Color.FromArgb(0xB0, 0xDA, 0xCF, 0x12);


        public static readonly Brush DarkColorBrush = new SolidColorBrush(DarkColor);
        public static readonly Brush LightColorBrush = new SolidColorBrush(LightColor);
        public static readonly Brush SelectedDarkColorBrush = new SolidColorBrush(Merge(DarkColor, SelectedColor));
        public static readonly Brush SelectedLightColorBrush = new SolidColorBrush(Merge(LightColor, SelectedColor));
        public static readonly Brush HighlightedDarkColorBrush = new SolidColorBrush(Merge(DarkColor, HighlightedColor));
        public static readonly Brush HighlightedLightColorBrush = new SolidColorBrush(Merge(LightColor, HighlightedColor));
        public static readonly Brush MarkedDarkColorBrush = new SolidColorBrush(Merge(DarkColor, MarkedColor));
        public static readonly Brush MarkedLightColorBrush = new SolidColorBrush(Merge(LightColor, MarkedColor));

        private static Color Merge(params Color[] colors)
        {
            Color result = colors.Any() ? colors.First() : Colors.Transparent;
            for (int i = 1; i < colors.Length; i++)
            {
                float add = (colors[i].A + 1f) / 256;
                result = result * (1 - add) + Color.FromArgb(0xFF, colors[i].R, colors[i].G, colors[i].B) * add;
            }
            result.Clamp();
            return result;
        }
    }
}
