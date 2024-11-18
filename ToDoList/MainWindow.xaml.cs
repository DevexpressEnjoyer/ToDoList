using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ToDoList
{
    public partial class MainWindow : Window
    {
        public List<Item> DatabaseItems { get; private set; }
        private bool notificationSent = false;
        public MainWindow()
        {
            InitializeComponent();
            Read();
        }

        public void Read()
        {
            var context = new DataContext();

            DatabaseItems = context.Items.ToList();
            ItemList.ItemsSource = DatabaseItems.OrderBy(i =>
                i.Priority == "High" ? 1 :
                i.Priority == "Medium" ? 2 : 3
            );

            if (!notificationSent)
            {
                CheckAndNotify(DatabaseItems);
            }
        }

        public void Delete()
        {
            var selectedItem = ItemList.SelectedItem as Item;
            var context = new DataContext();

            if (selectedItem is not null)
            {
                var item = context.Items.Find(selectedItem.Id);
                context.Remove(item);
                context.SaveChanges();
            }
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            CreateWindow createWindow = new CreateWindow();
            createWindow.Show();   
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            UpdateWindow updateWindow = new UpdateWindow();
            updateWindow.Show();

            var selectedItem = ItemList.SelectedItem as Item;

            if(selectedItem is not null)
            {
                updateWindow.Id = selectedItem.Id;
                updateWindow.NameTextBox.Text = selectedItem.Name;
                updateWindow.DescriptionTextBox.Text = selectedItem.Description;
                updateWindow.DateTextBox.Text = selectedItem.Date.ToString();
                updateWindow.PriorityTextBox.Text = selectedItem.Priority;
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            Delete();
            Read();
        }

        private void RefreshButton_Click(Object sender, EventArgs e)
        {
            Read();
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            ItemList.Items.Clear();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            var context = new DataContext();

            DatabaseItems = context.Items.ToList();

            if(!String.IsNullOrEmpty(DateFrom.Text) && !String.IsNullOrEmpty(DateTo.Text))
            {
                ItemList.ItemsSource = DatabaseItems.Where(i => DateTime.Parse(i.Date) >= DateTime.Parse(DateFrom.Text) && DateTime.Parse(i.Date) <= DateTime.Parse(DateTo.Text));
            }else if(!String.IsNullOrEmpty(DateFrom.Text))
            {
                ItemList.ItemsSource = DatabaseItems.Where(i => DateTime.Parse(i.Date) >= DateTime.Parse(DateFrom.Text));
            }
            else if(!String.IsNullOrEmpty(DateTo.Text))
            {
                ItemList.ItemsSource = DatabaseItems.Where(i => DateTime.Parse(i.Date) <= DateTime.Parse(DateTo.Text));
            }
            else
            {
                ItemList.ItemsSource = DatabaseItems;
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Read();
        }

        private void CompletedButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = ItemList.SelectedItem as Item;
            var context = new DataContext();

            if(selectedItem is not null)
            {
                var item = context.Items.Find(selectedItem.Id);
                if (item is not null)
                {
                    item.IsCompleted = true;
                    context.SaveChanges();
                }
            }
            Read();
        }

        private void CompletedTasks_Checked(object sender, RoutedEventArgs e)
        {
            Read();
        }

        private void CompletedTasks_Unchecked(object sender, RoutedEventArgs e)
        {
            var context = new DataContext();

            DatabaseItems = context.Items.ToList();

            ItemList.ItemsSource = DatabaseItems.Where(i => !i.IsCompleted).OrderBy(i =>
                i.Priority == "High" ? 1 :
                i.Priority == "Medium" ? 2 : 3
            );
        }

        private void CheckAndNotify(List<Item> items)
        {
            var uncompletedItems = items.Where(i => DateTime.Parse(i.Date) <= DateTime.Today && !i.IsCompleted).Count();

            if(uncompletedItems > 0)
            {
                MessageBox.Show("You have " + uncompletedItems.ToString() + " uncompleted task(s).", "Uncompleted tasks", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            this.notificationSent = true;
        }
    }
}