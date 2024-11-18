using System.Windows;

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
                ((MainWindow)Application.Current.MainWindow).Read();
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
