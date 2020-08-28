using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;
using Prism.Events;
using SistemaMirno.UI.ViewModel.General.Interfaces;

namespace SistemaMirno.UI.ViewModel.General
{
    public class StockViewModel : ViewModelBase, IStockViewModel
    {
        public StockViewModel(IEventAggregator eventAggregator, IDialogCoordinator dialogCoordinator) 
            : base(eventAggregator, "Stock", dialogCoordinator)
        {
        }

        public override Task LoadAsync(int? id = null)
        {
            return Task.CompletedTask;
        }
    }
}
