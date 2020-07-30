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
    public class DetailViewModelBase : INotifyPropertyChanged, IDetailViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DetailViewModelBase"/> class.
        /// </summary>
        /// <param name="model">A model wrapper instance of type <see cref="T"/>.</param>
        public DetailViewModelBase()
        {
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the save command for the view model.
        /// </summary>
        public ICommand SaveCommand { get; set; }

        /// <summary>
        /// Gets or sets the save command for the view model.
        /// </summary>
        public ICommand DeleteCommand { get; set; }

        /// <summary>
        /// Invokes the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Executes the save command.
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

        protected virtual void OnDeleteExecute()
        {
            throw new NotImplementedException();
        }
    }
}
