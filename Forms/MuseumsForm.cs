using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using MuseumApp.Data;
using MuseumApp.Models;

namespace MuseumApp;

public class MuseumsForm : Form
{
    private DataGridView dgv;

    public MuseumsForm()
    {
        Text = "Музеи";
        Size = new System.Drawing.Size(700, 550);
        StartPosition = FormStartPosition.CenterScreen;

        dgv = new DataGridView
        {
            Dock = DockStyle.Fill,
            AllowUserToAddRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };

        // Панель с кнопками - маленькая сверху
        var panel = new Panel { Dock = DockStyle.Top, Height = 45 };
        panel.Padding = new Padding(5);

        var btnAdd = new Button { Text = "Добавить", Location = new System.Drawing.Point(10, 5), Size = new System.Drawing.Size(90, 32) };
        var btnEdit = new Button { Text = "Редактировать", Location = new System.Drawing.Point(110, 5), Size = new System.Drawing.Size(100, 32) };
        var btnDelete = new Button { Text = "Удалить", Location = new System.Drawing.Point(220, 5), Size = new System.Drawing.Size(90, 32) };
        var btnRefresh = new Button { Text = "Обновить", Location = new System.Drawing.Point(320, 5), Size = new System.Drawing.Size(90, 32) };

        panel.Controls.Add(btnAdd);
        panel.Controls.Add(btnEdit);
        panel.Controls.Add(btnDelete);
        panel.Controls.Add(btnRefresh);

        Controls.Add(dgv);
        Controls.Add(panel);

        LoadData();

        btnAdd.Click += (s, e) => AddMuseum();
        btnEdit.Click += (s, e) => EditMuseum();
        btnDelete.Click += (s, e) => DeleteMuseum();
        btnRefresh.Click += (s, e) => LoadData();
    }

    private void LoadData()
    {
        using var context = new AppDbContext();
        var data = context.Museums.OrderBy(m => m.Name).Select(m => new { m.Id, m.Name }).ToList();
        dgv.DataSource = data;
    }

    private void AddMuseum()
    {
        string name = Microsoft.VisualBasic.Interaction.InputBox("Введите название музея:", "Добавление музея");
        if (!string.IsNullOrWhiteSpace(name))
        {
            using var context = new AppDbContext();
            context.Museums.Add(new Museum { Name = name });
            context.SaveChanges();
            LoadData();
            MessageBox.Show("Музей добавлен!", "Успех");
        }
    }

    private void EditMuseum()
    {
        if (dgv.CurrentRow == null)
        {
            MessageBox.Show("Выберите музей для редактирования!");
            return;
        }

        int id = (int)dgv.CurrentRow.Cells["Id"].Value;
        string oldName = dgv.CurrentRow.Cells["Name"].Value.ToString();

        string newName = Microsoft.VisualBasic.Interaction.InputBox("Введите новое название:", "Редактирование музея", oldName);

        if (!string.IsNullOrWhiteSpace(newName) && newName != oldName)
        {
            using var context = new AppDbContext();
            var museum = context.Museums.Find(id);
            if (museum != null)
            {
                museum.Name = newName;
                context.SaveChanges();
                LoadData();
                MessageBox.Show("Название изменено!", "Успех");
            }
        }
    }

    private void DeleteMuseum()
    {
        if (dgv.CurrentRow == null)
        {
            MessageBox.Show("Выберите музей для удаления!");
            return;
        }

        int id = (int)dgv.CurrentRow.Cells["Id"].Value;
        string name = dgv.CurrentRow.Cells["Name"].Value.ToString();

        using var context = new AppDbContext();

        bool hasExhibits = context.Exhibits.Any(e => e.MuseumId == id);

        if (hasExhibits)
        {
            MessageBox.Show($"Нельзя удалить музей \"{name}\" - в нём есть экспонаты!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (MessageBox.Show($"Удалить музей \"{name}\"?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            var museum = context.Museums.Find(id);
            if (museum != null)
            {
                context.Museums.Remove(museum);
                context.SaveChanges();
                LoadData();
                MessageBox.Show("Музей удалён!", "Успех");
            }
        }
    }
}