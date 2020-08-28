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
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail;
using SistemaMirno.UI.ViewModel.General.Interfaces;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.General
{
    public class MaterialViewModel : ViewModelBase, IMaterialViewModel
    {
        private IMaterialRepository _materialRepository;
        private Func<IMaterialRepository> _materialRepositoryCreator;
        private MaterialWrapper _selectecMaterial;

        public MaterialViewModel(
            Func<IMaterialRepository> materialRepositoryCreator,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Materiales", dialogCoordinator)
        {
            _materialRepositoryCreator = materialRepositoryCreator;

            Materials = new ObservableCollection<MaterialWrapper>();
            CreateNewCommand = new DelegateCommand(OnCreateNewExecute);
            OpenDetailCommand = new DelegateCommand(OnOpenDetailExecute, OnOpenDetailCanExecute);
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

        private bool OnOpenDetailCanExecute()
        {
            return SelectedMaterial != null;
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

        public ObservableCollection<MaterialWrapper> Materials { get; set; }

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

        public ICommand CreateNewCommand { get; }

        public ICommand OpenDetailCommand { get; }

        public override async Task LoadAsync(int? id = null)
        {
            Materials.Clear();
            _materialRepository = _materialRepositoryCreator();

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
    }
}
