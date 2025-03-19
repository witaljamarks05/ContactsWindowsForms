using ContactsWindowsForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

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



            // Настройка цветов
            this.BackColor = Color.FromArgb(240, 240, 240); // Светло-серый фон
            listContacts.BackColor = Color.White; // Белый фон списка
            listContacts.ForeColor = Color.FromArgb(64, 64, 64); // Темно-серый текст

            // Стиль кнопок
            foreach (Button btn in new[] { btnAdd, btnUpdate, btnDelete })
            {
                btn.BackColor = Color.FromArgb(0, 122, 204); // Синий цвет
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            }


        }

        // Загрузка контактов из файла
        private void LoadContacts()
        {
            contacts = DataManager.LoadContacts();
            UpdateContactList();
        }

        // Обновление списка контактов в ListBox
        private void UpdateContactList()
        {
            listContacts.Items.Clear();
            foreach (var contact in contacts)
            {
                listContacts.Items.Add(contact.Name);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Поле 'Имя' обязательно для заполнения!");
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
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedContact != null)
            {
                contacts.Remove(selectedContact);

                DataManager.SaveContacts(contacts);

                UpdateContactList();

                ClearFields();
            }
        }

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

        private void listContacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listContacts.SelectedIndex != -1)
            {
                selectedContact = contacts[listContacts.SelectedIndex];

                // Заполнение полей ввода данными выбранного контакта
                txtName.Text = selectedContact.Name;
                txtPhone.Text = selectedContact.Phone;
                txtAddress.Text = selectedContact.Address;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var searchText = txtSearch.Text.ToLower();
            var filtered = contacts.FindAll(c =>
                c.Name.ToLower().Contains(searchText));

            listContacts.Items.Clear();

            foreach (var contact in filtered)
            {
                listContacts.Items.Add(contact.Name);
            }
        }

        private void ClearFields()
        {
            txtName.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
            selectedContact = null;
        }


    }
}