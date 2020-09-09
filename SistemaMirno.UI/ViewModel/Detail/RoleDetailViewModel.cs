using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.General;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class RoleDetailViewModel : DetailViewModelBase
    {
        private readonly IRoleRepository _roleRepository;
        private RoleWrapper _role;

        public RoleDetailViewModel(
            IRoleRepository roleRepository,
            IEventAggregator eventAggregator, 
            IDialogCoordinator dialogCoordinator) 
            : base(eventAggregator, "Detalles de Rol", dialogCoordinator)
        {
            _roleRepository = roleRepository;
            Branches = new ObservableCollection<BranchWrapper>();

            SelectFileCommand = new DelegateCommand(OnSelectFileExecute);
        }

        private void OnSelectFileExecute()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = Directory.GetCurrentDirectory();
            dlg.DefaultExt = ".pdf"; // Default file extension
            dlg.Filter = "Archivos PDF(.pdf)|*.pdf"; // Filter files by extension

            bool? result = dlg.ShowDialog();
            // Process open file dialog box results
            if (result == true)
            {
                // Save filename
                Application.Current.Dispatcher.Invoke(() => Role.ProceduresManualPdfFile = dlg.FileName);
            }
        }

        /// <summary>
        /// Gets or sets the data model wrapper.
        /// </summary>
        public RoleWrapper Role
        {
            get => _role;

            set
            {
                _role = value;
                OnPropertyChanged();
            }

        }

        public ObservableCollection<BranchWrapper> Branches { get; }

        public ICommand SelectFileCommand { get; }

        /// <inheritdoc/>
        public override async Task LoadDetailAsync(int id)
        {
            var model = await _roleRepository.GetByIdAsync(id);
            
            Application.Current.Dispatcher.Invoke(() =>
            {
                Role = new RoleWrapper(model);
                Role.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
            });

            await base.LoadDetailAsync(id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected override async void OnSaveExecute()
        {
            base.OnSaveExecute();
            if (IsNew)
            {
                await _roleRepository.AddAsync(Role.Model);
            }
            else
            {
                await _roleRepository.SaveAsync(Role.Model);
            }

            HasChanges = false;
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(RoleViewModel),
                });
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return OnSaveCanExecute(Role);
        }

        /// <inheritdoc/>
        protected override async void OnDeleteExecute()
        {
            base.OnDeleteExecute();
            await _roleRepository.DeleteAsync(Role.Model);
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(RoleViewModel),
                });
        }

        protected override void OnCancelExecute()
        {
            base.OnCancelExecute();
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(RoleViewModel),
                });
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _roleRepository.HasChanges();
            }

            if (e.PropertyName == nameof(Role.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync(int? id = null)
        {
            await LoadBranches();

            if (id.HasValue)
            {
                await LoadDetailAsync(id.Value);
                return;
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                IsNew = true;

                Role = new RoleWrapper();
                Role.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();

                Role.Description = string.Empty;
                Role.BranchId = -1;
                Role.HasAccessToAccounting = false;
                Role.HasAccessToHumanResources = false;
                Role.HasAccessToLogistics = false;
                Role.HasAccessToProduction = false;
                Role.HasAccessToSales = false;
                Role.IsSystemAdmin = false;
                Role.HasProceduresManual = false;
                Role.ProceduresManualPdfFile = string.Empty;

                ProgressVisibility = Visibility.Collapsed;
            });

            await base.LoadDetailAsync().ConfigureAwait(false);
        }

        private async Task LoadBranches()
        {
            var branches = await _roleRepository.GetAllBranchesAsync();

            foreach (var branch in branches)
            {
                Application.Current.Dispatcher.Invoke(() => Branches.Add(new BranchWrapper(branch)));
            }
        }
    }
}
