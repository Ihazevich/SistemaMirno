// <copyright file="IViewModelBase.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace SistemaMirno.UI.ViewModel
{
    public interface IViewModelBase
    {
        Task LoadAsync(int? id = null);
    }
}