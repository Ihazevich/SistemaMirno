using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;
using Prism.Events;
using SistemaMirno.UI.ViewModel.Detail.Interfaces;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Detail
{
    public class WorkOrderDetailViewModel : DetailViewModelBase, IWorkOrderDetailViewModel
    {
        public WorkOrderDetailViewModel(IEventAggregator eventAggregator, IDialogCoordinator dialogCoordinator) 
            : base(eventAggregator, "Detalles de Orden de Trabajo", dialogCoordinator)
        {
        }

        public override Task LoadAsync(int? id = null)
        {
            return Task.CompletedTask;
        }

        protected override bool OnSaveCanExecute()
        {
            return false;
        }
    }
}
