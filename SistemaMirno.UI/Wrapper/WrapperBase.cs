using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SistemaMirno.UI.Wrapper
{
    /// <summary>
    /// Data wrapper base class.
    /// </summary>
    public class WrapperBase : INotifyPropertyChanged
    {
        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

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
