// <copyright file="IDeliveryRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    public interface IDeliveryRepository : IGenericRepository<Delivery>
    {
        Task<IEnumerable<Delivery>> GetAllInProcessAsync();
    }
}