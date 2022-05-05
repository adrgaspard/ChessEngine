using System;

namespace ChessEngine.UI.WPF
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            App application = new();
            application.InitializeComponent();
            application.Run();
        }
    }
}
