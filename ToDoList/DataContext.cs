using Microsoft.EntityFrameworkCore;

namespace ToDoList
{
    public class DataContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source =ToDoList.db");
        }

        public DbSet<Item> Items { get; set; }
    }
}
