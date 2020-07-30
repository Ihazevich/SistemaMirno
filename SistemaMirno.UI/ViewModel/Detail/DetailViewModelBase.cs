using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;

namespace SistemaMirno.UI.ViewModel.Detail
{
    /// <summary>
    /// A class representing the base detail view model.
    /// </summary>
    /// <typeparam name="T">The type of data model the view model will use.</typeparam>
    public class DetailViewModelBase<T> : INotifyPropertyChanged, IDetailViewModelBase
    {
        private T _model;

        /// <summary>
        /// Initializes a new instance of the <see cref="DetailViewModelBase{T}"/> class.
        /// </summary>
        /// <param name="model">A model wrapper instance of type <see cref="T"/>.</param>
        public DetailViewModelBase(T model)
        {
            Model = model;
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the data model wrapper.
        /// </summary>
        public T Model
        {
            get
            {
                return _model;
            }

            set
            {
                _model = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the save command for the view model.
        /// </summary>
        public ICommand SaveCommand { get; set; }

        /// <summary>
        /// Invokes the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sends the selected area to the data service for saving, and publishes the Reload View event to reload the navigation view.
        /// </summary>
        protected virtual void OnSaveExecute()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if the Save Command can be executed.
        /// </summary>
        /// <returns>True or false.</returns>
        protected virtual bool OnSaveCanExecute()
        {
            throw new NotImplementedException();
        }
    }
}
