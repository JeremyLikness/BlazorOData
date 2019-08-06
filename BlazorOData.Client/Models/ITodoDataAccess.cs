using BlazorOData.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorOData.Client.Models
{
    public interface ITodoDataAccess
    {
        Task<IEnumerable<Todo>> GetAsync(bool showAll, bool byCreated, bool byCompleted);
        Task<Todo> GetAsync(int id);
        Task<Todo> AddAsync(Todo itemToUpdate);
        Task<Todo> UpdateAsync(Todo item);
        Task<bool> DeleteAsync(Todo item);
    }
}
