// <copyright file="IViewModelBase.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace SistemaMirno.UI.ViewModel
{
    /// <summary>
    /// Represents the base interface for all view models.
    /// </summary>
    public interface IViewModelBase
    {
        /// <summary>
        /// Loads all the relevant view model data.
        /// </summary>
        /// <param name="id">Indicates whether a specific entity should be loaded,
        /// used as a general parameter, its use varies between different viewmodels.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task LoadAsync(int? id = null);
    }
}