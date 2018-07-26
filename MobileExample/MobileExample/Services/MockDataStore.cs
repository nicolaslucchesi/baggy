using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MobileExample.Models;

[assembly: Xamarin.Forms.Dependency(typeof(MobileExample.Services.MockDataStore))]
namespace MobileExample.Services
{
    public class MockDataStore : IDataStore<Mochila>
    {
        List<Mochila> items;

        public MockDataStore()
        {
            items = new List<Mochila>();
            var mockItems = new List<Mochila>
            {
                new Mochila { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
                new Mochila { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
                new Mochila { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
                new Mochila { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
                new Mochila { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
                new Mochila { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." },
            };

            foreach (var item in mockItems)
            {
                items.Add(item);
            }
        }

        public async Task<bool> AddItemAsync(Mochila item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Mochila item)
        {
            var _item = items.Where((Mochila arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var _item = items.Where((Mochila arg) => arg.Id == id).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<Mochila> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Mochila>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}