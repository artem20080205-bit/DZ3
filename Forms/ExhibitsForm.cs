using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using MuseumApp.Data;
using MuseumApp.Models;

namespace MuseumApp;

public class ExhibitsForm : Form
{
    private DataGridView dgv;

    public ExhibitsForm()
    {
        Text = "Экспонаты";
        Size = new System.Drawing.Size(950, 650);
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

        btnAdd.Click += (s, e) => AddExhibit();
        btnEdit.Click += (s, e) => EditExhibit();
        btnDelete.Click += (s, e) => DeleteExhibit();
        btnRefresh.Click += (s, e) => LoadData();
    }

    private void LoadData()
    {
        using var context = new AppDbContext();
        var data = context.Exhibits
            .Include(e => e.Museum)
            .Select(e => new { e.Id, Музей = e.Museum!.Name, Название = e.Name, Стоимость_тыс = e.ValueK })
            .ToList();
        dgv.DataSource = data;
    }

    private void AddExhibit()
    {
        using var context = new AppDbContext();
        var museums = context.Museums.ToList();

        var form = new Form { Text = "Добавление экспоната", Size = new System.Drawing.Size(420, 280), StartPosition = FormStartPosition.CenterParent };

        var cmbMuseum = new ComboBox { Location = new System.Drawing.Point(130, 20), Width = 240, DropDownStyle = ComboBoxStyle.DropDownList };
        cmbMuseum.DataSource = museums;
        cmbMuseum.DisplayMember = "Name";

        var txtName = new TextBox { Location = new System.Drawing.Point(130, 60), Width = 240 };
        var txtValue = new TextBox { Location = new System.Drawing.Point(150, 100), Width = 220 };

        var btnOk = new Button { Text = "OK", Location = new System.Drawing.Point(100, 160), Width = 80, DialogResult = DialogResult.OK };
        var btnCancel = new Button { Text = "Отмена", Location = new System.Drawing.Point(200, 160), Width = 80, DialogResult = DialogResult.Cancel };

        form.Controls.Add(new Label { Text = "Музей:", Location = new System.Drawing.Point(20, 23), Width = 100 });
        form.Controls.Add(cmbMuseum);
        form.Controls.Add(new Label { Text = "Название:", Location = new System.Drawing.Point(20, 63), Width = 100 });
        form.Controls.Add(txtName);
        form.Controls.Add(new Label { Text = "Стоимость (тыс.руб.):", Location = new System.Drawing.Point(20, 103), Width = 120 });
        form.Controls.Add(txtValue);
        form.Controls.Add(btnOk);
        form.Controls.Add(btnCancel);

        if (form.ShowDialog() == DialogResult.OK)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Введите название экспоната!");
                    return;
                }

                double value = double.Parse(txtValue.Text);
                if (value < 0) throw new Exception("Стоимость не может быть отрицательной");

                context.Exhibits.Add(new Exhibit
                {
                    MuseumId = ((Museum)cmbMuseum.SelectedItem).Id,
                    Name = txtName.Text,
                    ValueK = value
                });
                context.SaveChanges();
                LoadData();
                MessageBox.Show("Экспонат добавлен!", "Успех");
            }
            catch (FormatException)
            {
                MessageBox.Show("Введите корректное число для стоимости!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }

    private void EditExhibit()
    {
        if (dgv.CurrentRow == null)
        {
            MessageBox.Show("Выберите экспонат для редактирования!");
            return;
        }

        int id = (int)dgv.CurrentRow.Cells["Id"].Value;

        using var context = new AppDbContext();
        var exhibit = context.Exhibits.Find(id);
        if (exhibit == null) return;

        var museums = context.Museums.ToList();

        var form = new Form { Text = "Редактирование экспоната", Size = new System.Drawing.Size(420, 280), StartPosition = FormStartPosition.CenterParent };

        var cmbMuseum = new ComboBox { Location = new System.Drawing.Point(130, 20), Width = 240, DropDownStyle = ComboBoxStyle.DropDownList };
        cmbMuseum.DataSource = museums;
        cmbMuseum.DisplayMember = "Name";
        cmbMuseum.SelectedItem = museums.First(m => m.Id == exhibit.MuseumId);

        var txtName = new TextBox { Location = new System.Drawing.Point(130, 60), Width = 240, Text = exhibit.Name };
        var txtValue = new TextBox { Location = new System.Drawing.Point(150, 100), Width = 220, Text = exhibit.ValueK.ToString() };

        var btnOk = new Button { Text = "OK", Location = new System.Drawing.Point(100, 160), Width = 80, DialogResult = DialogResult.OK };
        var btnCancel = new Button { Text = "Отмена", Location = new System.Drawing.Point(200, 160), Width = 80, DialogResult = DialogResult.Cancel };

        form.Controls.Add(new Label { Text = "Музей:", Location = new System.Drawing.Point(20, 23), Width = 100 });
        form.Controls.Add(cmbMuseum);
        form.Controls.Add(new Label { Text = "Название:", Location = new System.Drawing.Point(20, 63), Width = 100 });
        form.Controls.Add(txtName);
        form.Controls.Add(new Label { Text = "Стоимость (тыс.руб.):", Location = new System.Drawing.Point(20, 103), Width = 120 });
        form.Controls.Add(txtValue);
        form.Controls.Add(btnOk);
        form.Controls.Add(btnCancel);

        if (form.ShowDialog() == DialogResult.OK)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Введите название экспоната!");
                    return;
                }

                double value = double.Parse(txtValue.Text);
                if (value < 0) throw new Exception("Стоимость не может быть отрицательной");

                exhibit.MuseumId = ((Museum)cmbMuseum.SelectedItem).Id;
                exhibit.Name = txtName.Text;
                exhibit.ValueK = value;

                context.SaveChanges();
                LoadData();
                MessageBox.Show("Экспонат изменён!", "Успех");
            }
            catch (FormatException)
            {
                MessageBox.Show("Введите корректное число для стоимости!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }

    private void DeleteExhibit()
    {
        if (dgv.CurrentRow == null)
        {
            MessageBox.Show("Выберите экспонат для удаления!");
            return;
        }

        int id = (int)dgv.CurrentRow.Cells["Id"].Value;
        string name = dgv.CurrentRow.Cells["Название"].Value.ToString();

        if (MessageBox.Show($"Удалить экспонат \"{name}\"?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            using var context = new AppDbContext();
            var exhibit = context.Exhibits.Find(id);
            if (exhibit != null)
            {
                context.Exhibits.Remove(exhibit);
                context.SaveChanges();
                LoadData();
                MessageBox.Show("Экспонат удалён!", "Успех");
            }
        }
 
    }
}