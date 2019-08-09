using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorOData.Models;
using Simple.OData.Client;

namespace BlazorOData.Client.Models
{
    public class TodoSimpleOData : ITodoDataAccess
    {
        private readonly IODataClient _client;

        public TodoSimpleOData(HttpClient client)
        {
            client.BaseAddress = new Uri("http://localhost:5000/odata/");
            var settings = new ODataClientSettings(client);
            _client = new Simple.OData.Client.ODataClient(settings);
        }

        public async Task<Todo> AddAsync(Todo itemToAdd)
        {
            var results = new List<ValidationResult>();
            var validation = new ValidationContext(itemToAdd);
            if (Validator.TryValidateObject(itemToAdd, validation, results))
            {
                return await _client.For<Todo>().Set(itemToAdd).InsertEntryAsync();
            }
            else
            {
                throw new ValidationException();
            }
        }

        public async Task<bool> DeleteAsync(Todo item)
        {
            await _client.For<Todo>().Key(item.Id).DeleteEntryAsync();
            return true;
        }

        public async Task<IEnumerable<Todo>> GetAsync(bool showAll, bool byCreated, bool byCompleted)
        {
            var helper = _client.For<Todo>();
            if (!showAll)
            {
                helper.Filter(todo => !todo.Complete);
            }
            if (showAll && byCompleted)
            {
                helper.OrderByDescending(todo => todo.MarkedComplete)
                    .ThenByDescending(todo => todo.Created);                
            }
            else if (byCreated)
            {
                helper.OrderByDescending(todo => todo.Created);
            }
            else
            {
                helper.OrderBy(todo => todo.Description);
            }
            return await helper.FindEntriesAsync();
        }

        public Task<Todo> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Todo> UpdateAsync(Todo item)
        {
            var results = new List<ValidationResult>();
            var validation = new ValidationContext(item);
            if (Validator.TryValidateObject(item, validation, results))
            {
                await _client.For<Todo>().Key(item.Id).Set(item).UpdateEntryAsync();
                return item;
            }
            else
            {
                throw new ValidationException();
            }
        }
    }
}
