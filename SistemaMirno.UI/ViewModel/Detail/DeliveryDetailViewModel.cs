// <copyright file="DeliveryDetailViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;
using Prism.Events;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class DeliveryDetailViewModel : DetailViewModelBase
    {
        public DeliveryDetailViewModel(IEventAggregator eventAggregator, string name, IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, name, dialogCoordinator)
        {
        }

        public override Task LoadAsync(int? id = null)
        {
            throw new NotImplementedException();
        }

        protected override bool OnSaveCanExecute()
        {
            throw new NotImplementedException();
        }
    }
}
