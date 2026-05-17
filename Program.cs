using System;
using System.Windows.Forms;
using MuseumApp.Data;

namespace MuseumApp;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        using (var context = new AppDbContext())
        {
            context.Database.EnsureCreated();
        }

        Application.Run(new MainForm());
    }
}