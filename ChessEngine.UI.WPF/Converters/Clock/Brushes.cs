using System.Windows.Media;

namespace ChessEngine.UI.WPF.Converters.Clock
{
    public static class Brushes
    {
        private static readonly Color White = Colors.White;
        private static readonly Color LightRed = Color.FromRgb(0xFF, 0x98, 0x98);
        private static readonly Color MediumRed = Color.FromRgb(0xFC, 0x68, 0x68);
        private static readonly Color DarkRed = Color.FromRgb(0xC4, 0x08, 0x08);

        public static readonly Brush WhiteBrush = new SolidColorBrush(White);
        public static readonly Brush LightRedBrush = new SolidColorBrush(LightRed);
        public static readonly Brush MediumRedBrush = new SolidColorBrush(MediumRed);
        public static readonly Brush DarkRedBrush = new SolidColorBrush(DarkRed);
    }
}
