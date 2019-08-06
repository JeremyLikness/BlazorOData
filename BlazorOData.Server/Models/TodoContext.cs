using BlazorOData.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorOData.Server.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Todo> TodoList { get; set; }
    }
}
