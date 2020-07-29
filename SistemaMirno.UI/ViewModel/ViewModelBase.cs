using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SistemaMirno.UI.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged, IViewModelBase
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SaveCommand { get; set; }

        public ViewModelBase()
        {
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnSaveExecute()
        {
            throw new NotImplementedException();
        }

        protected virtual bool OnSaveCanExecute()
        {
            throw new NotImplementedException();
        }
    }
}
