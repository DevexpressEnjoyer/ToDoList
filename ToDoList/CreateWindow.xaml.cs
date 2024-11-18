using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace ToDoList
{
    public partial class CreateWindow : Window
    {
        public CreateWindow()
        {
            InitializeComponent();
        }

        public void Create()
        {
            var context = new DataContext();


            var name = NameTextBox.Text;
            var description = DescriptionTextBox.Text;
            var dateTime = DateTextBox.Text;
            var priority = PriorityTextBox.Text;

            context.Items.Add(new Item { Name = name, Description = description, Date = dateTime, Priority = priority });
            context.SaveChanges();
            
        }

        private void CreateItemButton_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(NameTextBox.Text) && !String.IsNullOrEmpty(DateTextBox.Text))
            {
                Create();
                ((MainWindow)System.Windows.Application.Current.MainWindow).Read();
                this.Close();
            }
            else
            {
                MessageBox.Show("Name and Date cannot be empty", "Required fields missing", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelCreateButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
