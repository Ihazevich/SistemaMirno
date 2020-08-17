using Prism.Commands;
using Prism.Events;
using SistemaMirno.UI.Event;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SistemaMirno.UI.ViewModel
{
    /// <summary>
    /// Base view model class.
    /// </summary>
    public abstract class ViewModelBase : IViewModelBase, INotifyPropertyChanged
    {
        protected IEventAggregator _eventAggregator;

        public ViewModelBase(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            ExitView = new DelegateCommand(OnExitViewExecute);
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ExitView { get; }

        public abstract Task LoadAsync(int? id);

        private void OnExitViewExecute()
        {
            _eventAggregator.GetEvent<ChangeNavigationStatusEvent>()
                .Publish(true);
        }

        /// <summary>
        /// Invokes the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}