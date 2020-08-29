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
    public class ProductCategoryDetailViewModel : DetailViewModelBase, IProductCategoryDetailViewModel
    {
        private IProductCategoryRepository _productCategoryRepository;
        private ProductCategoryWrapper _productCategory;

        public ProductCategoryDetailViewModel(
            IProductCategoryRepository productCategoryRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Detalles de Categoria de Producto", dialogCoordinator)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        public ProductCategoryWrapper ProductCategory
        {
            get => _productCategory;

            set
            {
                _productCategory = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc/>
        public override async Task LoadDetailAsync(int id)
        {
            var model = await _productCategoryRepository.GetByIdAsync(id);

            Application.Current.Dispatcher.Invoke(() =>
            {
                ProductCategory = new ProductCategoryWrapper(model);
                ProductCategory.PropertyChanged += Model_PropertyChanged;
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
                await _productCategoryRepository.AddAsync(ProductCategory.Model);
            }
            else
            {
                await _productCategoryRepository.SaveAsync(ProductCategory.Model);
            }

            HasChanges = false;
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(ProductCategoryViewModel),
                });
        }

        /// <inheritdoc/>
        protected override bool OnSaveCanExecute()
        {
            return OnSaveCanExecute(ProductCategory);
        }

        /// <inheritdoc/>
        protected override async void OnDeleteExecute()
        {
            base.OnDeleteExecute();
            await _productCategoryRepository.DeleteAsync(ProductCategory.Model);
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(ProductCategoryViewModel),
                });
        }

        protected override void OnCancelExecute()
        {
            base.OnCancelExecute();
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = null,
                    ViewModel = nameof(ProductCategoryViewModel),
                });
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _productCategoryRepository.HasChanges();
            }

            if (e.PropertyName == nameof(ProductCategory.HasErrors))
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

                ProductCategory = new ProductCategoryWrapper();
                ProductCategory.PropertyChanged += Model_PropertyChanged;
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

                ProductCategory.Name = string.Empty;
            });

            await base.LoadDetailAsync().ConfigureAwait(false);
        }
    }
}
