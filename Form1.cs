using ContactsWindowsForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ContactManager
{
    public partial class Form1 : Form
    {
        private List<Contact> contacts;
        private Contact selectedContact;

        public Form1()
        {
            InitializeComponent();
            LoadContacts();

            // Настройка цветов и стилей
            this.BackColor = Color.FromArgb(245, 245, 245); // Светло-серый фон
            listContacts.BackColor = Color.White; // Белый фон списка
            listContacts.ForeColor = Color.FromArgb(64, 64, 64); // Темно-серый текст

            // Отключаем стандартную панель заголовка
            this.FormBorderStyle = FormBorderStyle.None;

            // Создаем собственную панель заголовка
            CreateCustomTitleBar();

            // Шрифт для кнопок
            foreach (Button btn in new[] { btnAdd, btnUpdate, btnDelete })
            {
                btn.BackColor = Color.FromArgb(0, 150, 136); // Зеленовато-бирюзовый
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular); // Обычный шрифт
                btn.Cursor = Cursors.Hand; // Курсор в виде руки
            }

            // Шрифт для текстовых полей
            foreach (TextBox txt in new[] { txtName, txtPhone, txtAddress, txtSearch })
            {
                txt.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular); // Обычный шрифт
            }

            // Шрифт для меток (Label)
            foreach (Label lbl in new[] { label1, label2, label3, label4 })
            {
                lbl.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular); // Обычный шрифт
            }
        }

        /// <summary>
        /// Создает кастомную панель заголовка с надписью "Контакты" и кнопками управления.
        /// </summary>
        private void CreateCustomTitleBar()
        {
            // Панель заголовка
            Panel titleBar = new Panel
            {
                BackColor = Color.FromArgb(0, 150, 136), 
                Height = 30,
                Dock = DockStyle.Top
            };

            // Надпись "Контакты"
            Label lblTitle = new Label
            {
                Text = "Контакты",
                Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular), 
                ForeColor = Color.White,
                Dock = DockStyle.Left,
                Padding = new Padding(10, 0, 0, 0),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Кнопка сворачивания
            Button btnMinimize = new Button
            {
                Text = "-",
                Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular), 
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Dock = DockStyle.Right,
                Width = 40
            };
            btnMinimize.Click += (s, e) => this.WindowState = FormWindowState.Minimized;

            // Кнопка закрытия
            Button btnClose = new Button
            {
                Text = "X",
                Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular), 
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Dock = DockStyle.Right,
                Width = 40
            };
            btnClose.Click += (s, e) => this.Close();

            titleBar.Controls.Add(lblTitle);
            titleBar.Controls.Add(btnMinimize);
            titleBar.Controls.Add(btnClose);

            this.Controls.Add(titleBar);
        }

        /// <summary>
        /// Загружает контакты из файла и обновляет список контактов.
        /// </summary>
        private void LoadContacts()
        {
            contacts = DataManager.LoadContacts();
            UpdateContactList();
        }

        /// <summary>
        /// Обновляет список контактов в ListBox.
        /// </summary>
        private void UpdateContactList()
        {
            listContacts.Items.Clear();
            foreach (var contact in contacts)
            {
                listContacts.Items.Add(contact.Name);
            }
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Добавить".
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Поле 'Имя' обязательно для заполнения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Создание нового контакта
                var contact = new Contact
                {
                    Name = txtName.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Address = txtAddress.Text.Trim()
                };

                contacts.Add(contact);
                DataManager.SaveContacts(contacts);
                UpdateContactList();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Удалить".
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedContact != null)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить контакт?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    contacts.Remove(selectedContact);
                    DataManager.SaveContacts(contacts);
                    UpdateContactList();
                    ClearFields();
                }
            }
        }

        /// <summary>
        /// Обрабатывает нажатие кнопки "Обновить".
        /// </summary>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedContact != null)
            {
                selectedContact.Name = txtName.Text.Trim();
                selectedContact.Phone = txtPhone.Text.Trim();
                selectedContact.Address = txtAddress.Text.Trim();

                DataManager.SaveContacts(contacts);
                UpdateContactList();
            }
        }

        /// <summary>
        /// Обрабатывает изменение выбранного элемента в списке контактов.
        /// </summary>
        private void listContacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listContacts.SelectedIndex != -1)
            {
                selectedContact = contacts[listContacts.SelectedIndex];
                txtName.Text = selectedContact.Name;
                txtPhone.Text = selectedContact.Phone;
                txtAddress.Text = selectedContact.Address;
            }
        }

        /// <summary>
        /// Обрабатывает изменение текста в поле поиска.
        /// </summary>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var searchText = txtSearch.Text.ToLower();
            var filtered = contacts.FindAll(c => c.Name.ToLower().Contains(searchText));
            listContacts.Items.Clear();
            foreach (var contact in filtered)
            {
                listContacts.Items.Add(contact.Name);
            }
        }

        /// <summary>
        /// Очищает поля ввода.
        /// </summary>
        private void ClearFields()
        {
            txtName.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
            selectedContact = null;
        }
    }
}