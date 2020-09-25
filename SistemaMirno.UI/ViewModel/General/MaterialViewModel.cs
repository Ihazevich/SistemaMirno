// <copyright file="MaterialViewModel.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    public class MaterialViewModel : ViewModelBase
    {
        private readonly IMaterialRepository _materialRepository;
        private MaterialWrapper _selectecMaterial;

        public MaterialViewModel(
            IMaterialRepository materialRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Materiales", dialogCoordinator)
        {
            _materialRepository = materialRepository;

            Materials = new ObservableCollection<MaterialWrapper>();
            CreateNewCommand = new DelegateCommand(OnCreateNewExecute);
            OpenDetailCommand = new DelegateCommand(OnOpenDetailExecute, OnOpenDetailCanExecute);
        }

        public ICommand CreateNewCommand { get; }

        public ObservableCollection<MaterialWrapper> Materials { get; }

        public ICommand OpenDetailCommand { get; }

        public MaterialWrapper SelectedMaterial
        {
            get
            {
                return _selectecMaterial;
            }

            set
            {
                OnPropertyChanged();
                _selectecMaterial = value;
                ((DelegateCommand)OpenDetailCommand).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync(int? id = null)
        {
            Materials.Clear();

            var materials = await _materialRepository.GetAllAsync();

            foreach (var material in materials)
            {
                Application.Current.Dispatcher.Invoke(() => Materials.Add(new MaterialWrapper(material)));
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressVisibility = Visibility.Collapsed;
                ViewVisibility = Visibility.Visible;
            });
        }

        private void OnCreateNewExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(MaterialDetailViewModel),
                });
        }

        private bool OnOpenDetailCanExecute()
        {
            return SelectedMaterial != null;
        }

        private void OnOpenDetailExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = SelectedMaterial.Id,
                    ViewModel = nameof(MaterialDetailViewModel),
                });
        }
    }
}
