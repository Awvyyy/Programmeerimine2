using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.WinFormsApp;

[ExcludeFromCodeCoverage]
internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }
}
