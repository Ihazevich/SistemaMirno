using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail.Interfaces;
using SistemaMirno.UI.ViewModel.General;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class MaterialDetailViewModel : DetailViewModelBase, IMaterialDetailViewModel
    {
        private IMaterialRepository _materialRepository;
        private MaterialWrapper _material;

        public MaterialDetailViewModel(
            IMaterialRepository materialRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Detalles de Material", dialogCoordinator)
        {
            _materialRepository = materialRepository;
        }

        public MaterialWrapper Material
        {
            get => _material;

            set
            {
                _material = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc/>
        public override async Task LoadDetailAsync(int id)
        {
            var model = await _materialRepository.GetByIdAsync(id);

            Application.Current.Dispatcher.Invoke(() =>
            {
                Material = new MaterialWrapper(model);
                Material.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            });

            await base.LoadDetailAsync(id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected override async void OnSaveExecute()
        {
            base.OnSaveExecute();

            if (IsNew)
            {
                await _materialRepository.AddAsync(Material.Model);
            }
            else
            {
                await _materialRepository.SaveAsync(Material.Model);
            }

            HasChanges = false;
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(MaterialViewModel),
                });
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return OnSaveCanExecute(Material);
        }

        /// <inheritdoc/>
        protected override async void OnDeleteExecute()
        {
            await _materialRepository.DeleteAsync(Material.Model);
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(MaterialViewModel),
                });
        }

        protected override void OnCancelExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(MaterialViewModel),
                });
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _materialRepository.HasChanges();
            }

            if (e.PropertyName == nameof(Material.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync(int? id = null)
        {
            if (id.HasValue)
            {
                await LoadDetailAsync(id.Value);
                return;
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                IsNew = true;

                Material = new MaterialWrapper();
                Material.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

                Material.Name = string.Empty;
            });

            await base.LoadDetailAsync().ConfigureAwait(false);
        }
    }
}
