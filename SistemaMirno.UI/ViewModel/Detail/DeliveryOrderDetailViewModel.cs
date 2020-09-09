using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;
using Prism.Events;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class DeliveryOrderDetailViewModel : DetailViewModelBase
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
