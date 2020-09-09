using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;
using Prism.Events;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class DeliveryOrderDetailViewModel : DetailViewModelBase, IDeliveryOrderDetailViewModel
    {
        public DeliveryOrderDetailViewModel(IEventAggregator eventAggregator, IDialogCoordinator dialogCoordinator)
            : base(eventAggregator, "Detalle de Orden de Entrega", dialogCoordinator)
        {
        }

        public override Task LoadAsync(int? id = null)
        {
            return Task.CompletedTask;
        }

        protected override bool OnSaveCanExecute()
        {
            return true;
        }
    }
}
