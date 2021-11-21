using ContactList.Data.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ContactList
{
   
    public partial class MainWindow : Window
    {
        List<Contact> contacts;

        public MainWindow()
        {
            InitializeComponent();
            contacts = new List<Contact>();
           // finderTB.SelectedText = "Enter some text to find...";
            GetContacts();
        }

        private void newContactBtn_Click(object sender, RoutedEventArgs e)
        {
            NewContactWindow newContactWindow = new NewContactWindow();
            newContactWindow.ShowDialog();
            GetContacts();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            TextBox text = sender as TextBox;
            string theText="";
            if (text != null) theText = text.Text;
            contactsListView.ItemsSource = contacts.Where(t => t.Name.ToLower().Contains(theText));

        }

        void Update()
        {
            contactsListView.ItemsSource = null;
            contactsListView.ItemsSource = contacts;
            value.Content = contacts.Count;
        }
        void GetContacts()
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.dbPath))
            {
                connection.CreateTable<Contact>();
                contacts = connection.Table<Contact>().ToList();
            }
            Update();
        }

        private void contactsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (contactsListView.SelectedIndex != -1)
            {
                Contact contact = contactsListView.SelectedItem as Contact;
                NewContactWindow newContactWindow = new NewContactWindow(contact);
                newContactWindow.ShowDialog();
                finderTB.Text = "";
                GetContacts();
            }
        }

    }
}
