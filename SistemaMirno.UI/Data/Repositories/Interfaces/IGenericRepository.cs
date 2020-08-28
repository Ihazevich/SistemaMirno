// <copyright file="IGenericRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    /// <summary>
    /// An interface for the generic data repository.
    /// </summary>
    public interface IGenericRepository<T>
        where T : ModelBase
    {
        Task<int> AddAsync(T entity);

        Task<int> AddRangeAsync(IList<T> entitites);

        Task<int> SaveAsync(T entity);

        Task<int> DeleteAsync(int id, byte[] timeStamp);

        Task<int> DeleteAsync(T entity);

        Task<T> GetByIdAsync(int? id);

        Task<List<T>> GetAllAsync();

        bool HasChanges();
    }
}