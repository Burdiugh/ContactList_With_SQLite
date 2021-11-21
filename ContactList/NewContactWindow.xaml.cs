using ContactList.Data.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace ContactList
{
    /// <summary>
    /// Interaction logic for NewContactWindow.xaml
    /// </summary>
    public partial class NewContactWindow : Window
    {
       
        Contact contact { get; set; }
        string namePattern = @"^([a-zA-Z]{2,}\s[a-zA-Z]{1,}'?-?[a-zA-Z]{2,}\s?([a-zA-Z]{1,})?)";
        string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
        public NewContactWindow()
        {
            InitializeComponent();
            btnRemove.IsEnabled = false;
            btnSave.Content = "Save";
        }
        public NewContactWindow(Contact contact)
        {
            InitializeComponent();
            Title = "Info";
            btnSave.Content = "Done";
            nameTextBox.Text = contact.Name;
            phoneTextBox.Text = contact.Phone;
            emailTextBox.Text = contact.Email;
            btnRemove.IsEnabled = true;
            this.contact = contact;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            string phonePattern = @"^\+\d{12}$";
            if (btnSave.Content == "Save")
            {
                if (nameTextBox.Text.Length > 0 && emailTextBox.Text.Length > 0 && phoneTextBox.Text.Length > 0)
                {
                    if (Regex.IsMatch(nameTextBox.Text, namePattern) && Regex.IsMatch(emailTextBox.Text, emailPattern) && Regex.IsMatch(phoneTextBox.Text, phonePattern))
                    {
                        Contact contact = new Contact
                        {
                            Name = nameTextBox.Text,
                            Email = emailTextBox.Text,
                            Phone = phoneTextBox.Text
                        };

                        using (System.Data.SQLite.SQLiteConnection connection = new System.Data.SQLite.SQLiteConnection($"data source={App.dbPath}"))
                        {

                            try
                            {
                                connection.Open();
                                string updateperson = $"insert into Contact  (Name, Email, Phone) values ('{contact.Name}','{contact.Email}','{contact.Phone}');";
                                System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(updateperson, connection);
                                command.ExecuteNonQuery();

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Input info in valid format!");
                        if (!Regex.IsMatch(nameTextBox.Text, namePattern))
                        {
                            nameTextBox.Text = "";
                        }
                        if (!Regex.IsMatch(emailTextBox.Text, emailPattern))
                        {
                            emailTextBox.Text = "";
                        }
                        if (!Regex.IsMatch(phoneTextBox.Text, phonePattern))
                        {
                            phoneTextBox.Text = "";
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Fields can not be is empty!");
                }
            }
            else if (btnSave.Content == "Done")
            {
                if (nameTextBox.Text.Length > 0 && emailTextBox.Text.Length > 0 && phoneTextBox.Text.Length > 0)
                {
                    if (Regex.IsMatch(nameTextBox.Text, namePattern) && Regex.IsMatch(phoneTextBox.Text, phonePattern))
                    {
                        contact.Name = nameTextBox.Text;
                        contact.Email = emailTextBox.Text;
                        contact.Phone = phoneTextBox.Text;

                        using (System.Data.SQLite.SQLiteConnection connection = new System.Data.SQLite.SQLiteConnection($"data source={App.dbPath}"))
                        {

                            try
                            {
                                connection.Open();
                                string updateperson = $"update Contact set Name='{contact.Name}', Phone='{contact.Phone}', Email='{contact.Email}' where Id='{contact.Id}';";
                                System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(updateperson, connection);
                                command.ExecuteNonQuery();
                                connection.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                connection.Close();
                            }

                        }

                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Input info in valid format!");
                        if (!Regex.IsMatch(nameTextBox.Text, namePattern))
                        {
                            nameTextBox.Text = "";
                        }
                        if (!Regex.IsMatch(phoneTextBox.Text, phonePattern))
                        {
                            phoneTextBox.Text = "";
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Fields can not be is empty!");
            }

        }

       

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            using (System.Data.SQLite.SQLiteConnection connection = new System.Data.SQLite.SQLiteConnection($"data source={App.dbPath}"))
            {
                try
                {
                    connection.Open();
                    string deleteperson = $"delete from Contact where Id='{contact.Id}';";
                    System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(deleteperson, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                MessageBox.Show($"{contact.Name} was removed from db!");
                Close();
            }
        }
    }
}

