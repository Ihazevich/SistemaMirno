using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.Detail.Interfaces;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    /// <summary>
    /// A class representing the base detail view model.
    /// </summary>
    /// <typeparam name="T">The type of data model the view model will use.</typeparam>
    public abstract class DetailViewModelBase : ViewModelBase, IDetailViewModelBase
    {
        private bool _hasChanges;
        private bool _isNew;
        private bool _isEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="DetailViewModelBase"/> class.
        /// </summary>
        /// <param name="model">A model wrapper instance of type <see cref="T"/>.</param>
        public DetailViewModelBase(IEventAggregator eventAggregator, string name, IDialogCoordinator dialogCoordinator)
            : base (eventAggregator, name, dialogCoordinator)
        {
            _isNew = false;
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute, OnDeleteCanExecute);
            CancelCommand = new DelegateCommand(OnCancelExecute);
            IsEnabled = false;
        }

        private bool OnDeleteCanExecute()
        {
            return !IsNew;
        }

        /// <summary>
        /// Gets or sets the save command for the view model.
        /// </summary>
        public ICommand SaveCommand { get; set; }

        /// <summary>
        /// Gets or sets the save command for the view model.
        /// </summary>
        public ICommand DeleteCommand { get; set; }

        public ICommand CancelCommand { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the database context has changes.
        /// </summary>
        public bool HasChanges
        {
            get => _hasChanges;

            set
            {
                if (_hasChanges == value)
                {
                    return;
                }

                _hasChanges = value;
                OnPropertyChanged();
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public bool IsNew
        {
            get => _isNew;

            set
            {
                _isNew = value;
                ((DelegateCommand)CancelCommand).RaiseCanExecuteChanged();
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;

            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        public virtual Task LoadDetailAsync(int id = 0)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                IsEnabled = true;
                ProgressVisibility = Visibility.Collapsed;
            });
            return Task.CompletedTask;
        }
        
        protected virtual void OnSaveExecute()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressVisibility = Visibility.Visible;
                IsEnabled = false;
            });
        }

        /// <summary>
        /// Checks if the Save Command can be executed.
        /// </summary>
        /// <returns>True or false.</returns>
        protected bool OnSaveCanExecute(IModelWrapper wrapper)
        {
            return wrapper != null && !wrapper.HasErrors && (HasChanges || IsNew);
        }

        protected abstract bool OnSaveCanExecute();

        protected virtual void OnDeleteExecute()
        {
            throw new NotImplementedException();
        }

        protected virtual void OnCancelExecute()
        {
            throw new NotImplementedException();
        }
    }
}