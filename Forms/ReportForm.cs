using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using MuseumApp.Data;

namespace MuseumApp;

public class ReportForm : Form
{
    public ReportForm()
    {
        Text = "Отчёты";
        Size = new System.Drawing.Size(900, 600);
        StartPosition = FormStartPosition.CenterScreen;

        var tabControl = new TabControl { Dock = DockStyle.Fill };

        var tab1 = new TabPage("1. Список экспонатов");
        var dgv1 = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        tab1.Controls.Add(dgv1);

        var tab2 = new TabPage("2. Количество по музеям");
        var dgv2 = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        tab2.Controls.Add(dgv2);

        var tab3 = new TabPage("3. Средняя стоимость");
        var dgv3 = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        tab3.Controls.Add(dgv3);

        tabControl.TabPages.Add(tab1);
        tabControl.TabPages.Add(tab2);
        tabControl.TabPages.Add(tab3);
        Controls.Add(tabControl);

        using var context = new AppDbContext();

        dgv1.DataSource = context.Exhibits.Include(e => e.Museum).Select(e => new { Название = e.Name, Музей = e.Museum!.Name, Стоимость = e.ValueK }).ToList();
        dgv2.DataSource = context.Exhibits.GroupBy(e => e.Museum!.Name).Select(g => new { Музей = g.Key, Количество = g.Count() }).ToList();
        dgv3.DataSource = context.Exhibits.GroupBy(e => e.Museum!.Name).Select(g => new { Музей = g.Key, Средняя = Math.Round(g.Average(e => e.ValueK), 2) }).OrderByDescending(r => r.Средняя).ToList();
    }
}