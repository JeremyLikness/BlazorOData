using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BlazorOData.Models;

namespace BlazorOData.Client.Models
{
    public class MockTodoData : ITodoDataAccess
    {
        private readonly List<Todo> _database = new List<Todo>(new Todo[]
        {
            new Todo
            {
                Id = 1,
                Complete = true,
                Description = "Alpha and omega",
                MarkedComplete = DateTime.UtcNow.AddDays(-1),
                Created = DateTime.UtcNow.AddDays(-2)
            },
            new Todo
            {
                Id = 2,
                Complete = false,
                Description = "Zed says Fred"
            }
            });

        public Task<bool> DeleteAsync(Todo item)
        {
            var delete = _database.Where(todo => todo.Id == item.Id).FirstOrDefault();
            if (delete != null)
            {
                _database.Remove(delete);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<IEnumerable<Todo>> GetAsync(bool showAll, bool byCreated, bool byCompleted)
        {
            IQueryable<Todo> query = _database.AsQueryable();
            if (byCreated == true)
            {
                byCompleted = false;
            }
            if (!showAll)
            {
                query = query.Where(todo => !todo.Complete);
            }
            if (showAll && byCompleted)
            {
                query = query.OrderBy(todo => -(todo.MarkedComplete.HasValue ? 
                    todo.MarkedComplete.Value.Ticks : -(long.MaxValue - todo.Created.Ticks)));
            }
            else if (byCreated)
            {
                query = query.OrderBy(todo => -todo.Created.Ticks);
            }
            else
            {
                query = query.OrderBy(todo => todo.Description);
            }
            return Task.FromResult(query.AsEnumerable());
        }

        public Task<Todo> GetAsync(int id)
        {
            return Task.FromResult(_database.Where(item => item.Id == id).FirstOrDefault());
        }

        public Task<Todo> AddAsync(Todo itemToAdd)
        {
            var results = new List<ValidationResult>();
            var validation = new ValidationContext(itemToAdd);
            if (Validator.TryValidateObject(itemToAdd, validation, results))
            {
                itemToAdd.Id = _database.Max(todo => todo.Id) + 1;
                _database.Add(itemToAdd);
                return Task.FromResult(itemToAdd);
            }
            else
            {
                throw new ValidationException();
            }
        }

        public Task<Todo> UpdateAsync(Todo item)
        {
            var results = new List<ValidationResult>();
            var validation = new ValidationContext(item);
            if (Validator.TryValidateObject(item, validation, results))
            {
                var dbItem = _database.Where(todo => todo.Id == item.Id).First();
                if (!dbItem.Complete && item.Complete)
                {
                    dbItem.MarkComplete();
                }
                dbItem.Description = item.Description;
                return Task.FromResult(dbItem);
            }
            else
            {
                throw new ValidationException();
            }
        }
    }
}
