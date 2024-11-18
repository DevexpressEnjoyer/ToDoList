using System.Windows;


namespace ToDoList
{
    public partial class MainWindow : Window
    {
        public List<Item> DatabaseItems { get; private set; }
        private bool notificationSent = false;
        private bool dateBoundriesApplied = false;
        private bool includeCompletedTasks = true;
        public MainWindow()
        {
            InitializeComponent();
            Read();
        }

        public void Read()
        {
            if (dateBoundriesApplied)
            {
                ReadWithDateBoundries();
                return;
            }

            var context = new DataContext();

            DatabaseItems = context.Items.ToList();
            ItemList.ItemsSource = DatabaseItems.Where(i => includeCompletedTasks || (!includeCompletedTasks && !i.IsCompleted)).OrderBy(i =>
                i.Priority == "High" ? 1 :
                i.Priority == "Medium" ? 2 : 3
            );

            if (!notificationSent)
            {
                CheckAndNotify(DatabaseItems);
            }
        }

        public void ReadWithDateBoundries()
        {
            var context = new DataContext();

            DatabaseItems = context.Items.ToList();

            if (!String.IsNullOrEmpty(DateFrom.Text) && !String.IsNullOrEmpty(DateTo.Text))
            {
                ItemList.ItemsSource = DatabaseItems.Where(i => 
                    DateTime.Parse(i.Date) >= DateTime.Parse(DateFrom.Text) && 
                    DateTime.Parse(i.Date) <= DateTime.Parse(DateTo.Text) && 
                    (includeCompletedTasks || (!includeCompletedTasks && !i.IsCompleted))
                ).OrderBy(i =>
                    i.Priority == "High" ? 1 :
                    i.Priority == "Medium" ? 2 : 3
                );
            }
            else if (!String.IsNullOrEmpty(DateFrom.Text))
            {
                ItemList.ItemsSource = DatabaseItems.Where(i => 
                    DateTime.Parse(i.Date) >= DateTime.Parse(DateFrom.Text) &&
                    (includeCompletedTasks || (!includeCompletedTasks && !i.IsCompleted))
                ).OrderBy(i =>
                    i.Priority == "High" ? 1 :
                    i.Priority == "Medium" ? 2 : 3
                );
            }
            else if (!String.IsNullOrEmpty(DateTo.Text))
            {
                ItemList.ItemsSource = DatabaseItems.Where(i => 
                    DateTime.Parse(i.Date) <= DateTime.Parse(DateTo.Text) &&
                    (includeCompletedTasks || (!includeCompletedTasks && !i.IsCompleted))
                ).OrderBy(i =>
                    i.Priority == "High" ? 1 :
                    i.Priority == "Medium" ? 2 : 3
                );
            }
            else
            {
                ItemList.ItemsSource = DatabaseItems.Where(i =>
                    (includeCompletedTasks || (!includeCompletedTasks && !i.IsCompleted))
                ).OrderBy(i =>
                    i.Priority == "High" ? 1 :
                    i.Priority == "Medium" ? 2 : 3
                );
            }
        }

        public void Delete()
        {
            var selectedItem = ItemList.SelectedItem as Item;
            var context = new DataContext();

            if (selectedItem is not null)
            {
                var item = context.Items.Find(selectedItem.Id);

                if(item is not null)
                {
                    context.Remove(item);
                    context.SaveChanges();
                }

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
            dateBoundriesApplied = true;
            Read();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            dateBoundriesApplied = false;
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
            includeCompletedTasks = true;
            Read();
        }

        private void CompletedTasks_Unchecked(object sender, RoutedEventArgs e)
        {
            includeCompletedTasks = false;
            Read();
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

        private void DeleteAllButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("You are going to remove all tasks from the database. Do you want to continue?", "Confirm tasks deletion", MessageBoxButton.OKCancel);

            if(result == MessageBoxResult.OK)
            {
                var context = new DataContext();

                foreach (var item in context.Items)
                {
                    context.Remove(item);
                }

                context.SaveChanges();
            }

            Read();
        }
    }
}