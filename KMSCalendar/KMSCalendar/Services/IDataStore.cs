﻿using System.Collections.Generic;
using System.Threading.Tasks;

using KMSCalendar.Models.Entities;

namespace KMSCalendar.Services
{
    public interface IDataStore<T> where T : TableData
    {
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
        Task<T> GetItemAsync(string id);
        Task<T> AddItemAsync(T item);
        Task<T> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(string id);
    }
}