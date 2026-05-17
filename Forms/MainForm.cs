using System;
using System.Windows.Forms;

namespace MuseumApp;

public class MainForm : Form
{
    public MainForm()
    {
        Text = "Музейный учёт";
        Size = new System.Drawing.Size(400, 300);
        StartPosition = FormStartPosition.CenterScreen;

        var btnMuseums = new Button { Text = "Музеи", Location = new System.Drawing.Point(80, 40), Size = new System.Drawing.Size(200, 40) };
        var btnExhibits = new Button { Text = "Экспонаты", Location = new System.Drawing.Point(80, 100), Size = new System.Drawing.Size(200, 40) };
        var btnReport = new Button { Text = "Отчёты", Location = new System.Drawing.Point(80, 160), Size = new System.Drawing.Size(200, 40) };

        btnMuseums.Click += (s, e) => new MuseumsForm().ShowDialog();
        btnExhibits.Click += (s, e) => new ExhibitsForm().ShowDialog();
        btnReport.Click += (s, e) => new ReportForm().ShowDialog();

        Controls.Add(btnMuseums);
        Controls.Add(btnExhibits);
        Controls.Add(btnReport);
    }
}