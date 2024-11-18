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

namespace ToDoList
{
    public partial class UpdateWindow : Window
    {
        public int Id {  get; set; }

        public UpdateWindow()
        {
            InitializeComponent();
        }

        public void Update()
        {
            var context = new DataContext();

            var name = NameTextBox.Text;
            var description = DescriptionTextBox.Text;
            var dateTime = DateTextBox.Text;
            var priority = PriorityTextBox.Text;

            var item = context.Items.Find(this.Id);
            if (item is not null)
            {
                item.Name = name;
                item.Description = description;
                item.Date = dateTime;
                item.Priority = priority;

                context.SaveChanges();
            }

        }

        private void UpdateItemButton_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(NameTextBox.Text) && !String.IsNullOrEmpty(DateTextBox.Text))
            {
                Update();
                ((MainWindow)System.Windows.Application.Current.MainWindow).Read();
                this.Close();
            }
            else
            {
                MessageBox.Show("Name and Date cannot be empty", "Required fields missing", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelUpdateButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
