// <copyright file="ProductCategoryViewModel.cs" company="HazeLabs">
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
    public class ProductCategoryViewModel : ViewModelBase
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private ProductCategoryWrapper _selectecProductCategory;

        public ProductCategoryViewModel(
            IProductCategoryRepository productCategoryRepository,
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Categorias de Productos", dialogCoordinator)
        {
            _productCategoryRepository = productCategoryRepository;

            ProductCategories = new ObservableCollection<ProductCategoryWrapper>();
            CreateNewCommand = new DelegateCommand(OnCreateNewExecute);
            OpenDetailCommand = new DelegateCommand(OnOpenDetailExecute, OnOpenDetailCanExecute);
        }

        public ICommand CreateNewCommand { get; }

        public ICommand OpenDetailCommand { get; }

        public ObservableCollection<ProductCategoryWrapper> ProductCategories { get; }

        public ProductCategoryWrapper SelectedProductCategory
        {
            get
            {
                return _selectecProductCategory;
            }

            set
            {
                OnPropertyChanged();
                _selectecProductCategory = value;
                ((DelegateCommand)OpenDetailCommand).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync(int? id = null)
        {
            ProductCategories.Clear();

            var categories = await _productCategoryRepository.GetAllAsync();

            foreach (var category in categories)
            {
                Application.Current.Dispatcher.Invoke(() => ProductCategories.Add(new ProductCategoryWrapper(category)));
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
                    ViewModel = nameof(ProductCategoryDetailViewModel),
                });
        }

        private bool OnOpenDetailCanExecute()
        {
            return SelectedProductCategory != null;
        }

        private void OnOpenDetailExecute()
        {
            EventAggregator.GetEvent<ChangeViewEvent>()
                .Publish(new ChangeViewEventArgs
                {
                    Id = SelectedProductCategory.Id,
                    ViewModel = nameof(ProductCategoryDetailViewModel),
                });
        }
    }
}
