using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.ViewModel.General.Interfaces;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    public class ColorViewModel : ViewModelBase , IColorViewModel
    {
        private IColorRepository _colorRepository;
        private Func<IColorRepository> _colorRepositoryCreator;
        private ColorWrapper _selectedColor;

        public ColorViewModel(
            Func<IColorRepository> colorRepositoryCreator,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Colores", dialogCoordinator)
        {
            _colorRepositoryCreator = colorRepositoryCreator;

            Colors = new ObservableCollection<ColorWrapper>();
            CreateNewCommand = new DelegateCommand(OnCreateNewExecute);
            OpenDetailCommand = new DelegateCommand(OnOpenDetailExecute, OnOpenDetailCanExecute);
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
            _colorRepository = _colorRepositoryCreator();

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
    }
}
