using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;
using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories;
using SistemaMirno.UI.Event;
using SistemaMirno.UI.ViewModel.General;
using SistemaMirno.UI.Wrapper;

namespace SistemaMirno.UI.ViewModel.Main
{
    /// <summary>
    /// Production areas navigation view model class.
    /// </summary>
    public class WorkAreaNavigationViewModel : ViewModelBase, IWorkAreaNavigationViewModel
    {
        private bool _navigationEnabled = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkAreaNavigationViewModel"/> class.
        /// </summary>
        /// <param name="productionAreaRepository">A <see cref="IWorkAreaRepository"/> representing the model repository.</param>
        /// <param name="eventAggregator">A <see cref="IEventAggregator"/> representing the event aggregator.</param>
        public WorkAreaNavigationViewModel(
            IEventAggregator eventAggregator,
            IDialogCoordinator dialogCoordinator)
            : base (eventAggregator, "Navegacion", dialogCoordinator)
        {
            EventAggregator.GetEvent<AfterDataModelSavedEvent<WorkArea>>()
                .Subscribe(AfterWorkAreaSaved);
            EventAggregator.GetEvent<AfterDataModelDeletedEvent<WorkArea>>()
                .Subscribe(AfterWorkAreaDeleted);
            EventAggregator.GetEvent<ChangeNavigationStatusEvent>()
                .Subscribe(ChangeNavigation);
        }

        public bool NavigationEnabled
        {
            get => _navigationEnabled;

            set
            {
                _navigationEnabled = value;
                OnPropertyChanged();
            }
        }

        private void ChangeNavigation(bool arg)
        {
            NavigationEnabled = arg;
            if (arg)
            {
                //SelectedWorkArea = null;
            }
        }

        /// <summary>
        /// Gets or sets the selected work area.
        /// </summary>
        /*
        public WorkAreaWrapper SelectedWorkArea
        {
            get
            {
                return _selectedWorkArea;
            }

            set
            {
                _selectedWorkArea = value;
                OnPropertyChanged();
                if (_selectedWorkArea != null)
                {
                    // If the work area name is Pedidos or Stock, then redirect to the specialized views of those areas.
                    if (_selectedWorkArea.Name == "Pedidos")
                    {
                        _eventAggregator.GetEvent<ChangeViewEvent>()
                            .Publish(new ChangeViewEventArgs { ViewModel = nameof(RequisitionViewModel) });
                    }
                    else if (_selectedWorkArea.Name == "Stock")
                    {
                        _eventAggregator.GetEvent<ChangeViewEvent>()
                            .Publish(new ChangeViewEventArgs { ViewModel = nameof(StockViewModel) });
                    }
                    else
                    {
                        _eventAggregator.GetEvent<ChangeViewEvent>()
                            .Publish(new ChangeViewEventArgs { ViewModel = nameof(WorkUnitViewModel), Id = SelectedWorkArea.Id });
                    }
                    _eventAggregator.GetEvent<ChangeNavigationStatusEvent>()
                        .Publish(false);
                }
            }
        }

        */


        /// <inheritdoc/>
        public override async Task LoadAsync(int? id = null)
        {
        }

        /// <summary>
        /// Reloads the view model based on the parameter string.
        /// </summary>
        /// <param name="viewModel">Name of the view model to be reloaded.</param>
        private void AfterWorkAreaSaved(AfterDataModelSavedEventArgs<WorkArea> args)
        {
            /*
            var item = WorkAreas.SingleOrDefault(p => p.Id == args.Model.Id);

            if (item == null)
            {
                //WorkAreas.Add(new WorkAreaWrapper(args.Model));
            }
            else
            {
                item.Name = args.Model.Name;
            }*/
        }

        private void AfterWorkAreaDeleted(AfterDataModelDeletedEventArgs<WorkArea> args)
        {
            /*
            var item = WorkAreas.SingleOrDefault(p => p.Id == args.Model.Id);

            if (item != null)
            {
                WorkAreas.Remove(item);
            }*/
        }
    }
}