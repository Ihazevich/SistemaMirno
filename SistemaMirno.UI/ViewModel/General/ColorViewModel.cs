// <copyright file="ColorViewModel.cs" company="HazeLabs">
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
    public class ColorViewModel : ViewModelBase
    {
        private readonly IColorRepository _colorRepository;
        private ColorWrapper _selectedColor;

        public ColorViewModel(
            IColorRepository colorRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Colores", dialogCoordinator)
        {
            _colorRepository = colorRepository;

            Colors = new ObservableCollection<ColorWrapper>();
            CreateNewCommand = new DelegateCommand(OnCreateNewExecute);
            OpenDetailCommand = new DelegateCommand(OnOpenDetailExecute, OnOpenDetailCanExecute);
        }

        public ObservableCollection<ColorWrapper> Colors { get; }

        public ColorWrapper SelectedColor
        {
            get
            {
                return _selectedColor;
            }

            set
            {
                OnPropertyChanged();
                _selectedColor = value;
                ((DelegateCommand)OpenDetailCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand CreateNewCommand { get; }

        public ICommand OpenDetailCommand { get; }

        public override async Task LoadAsync(int? id = null)
        {
            Colors.Clear();

            var colors = await _colorRepository.GetAllAsync();

            foreach (var color in colors)
            {
                Application.Current.Dispatcher.Invoke(() => Colors.Add(new ColorWrapper(color)));
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressVisibility = Visibility.Collapsed;
                ViewVisibility = Visibility.Visible;
            });
        }

        private void OnOpenDetailExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = SelectedColor.Id,
                    ViewModel = nameof(ColorDetailViewModel),
                });
        }

        private bool OnOpenDetailCanExecute()
        {
            return SelectedColor != null;
        }

        private void OnCreateNewExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(ColorDetailViewModel),
                });
        }
    }
}
