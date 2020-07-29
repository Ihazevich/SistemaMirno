using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data;
using SistemaMirno.UI.Event;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMirno.UI.ViewModel
{
    public class MaterialViewModel : ViewModelBase, IMaterialViewModel
    {
        private IMaterialDataService _materialDataService;
        private IEventAggregator _eventAggregator;

        public ObservableCollection<Material> Materials { get; set; }

        public MaterialViewModel(IMaterialDataService materialDataService, IEventAggregator eventAggregator)
        {
            Materials = new ObservableCollection<Material>();
            _materialDataService = materialDataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ShowMaterialViewEvent>()
                .Subscribe(ViewModelSelected);
        }

        public async void ViewModelSelected()
        {
            await LoadAsync();
            _eventAggregator.GetEvent<ChangeViewEvent>().
                Publish(this);
        }

        public async Task LoadAsync()
        {
            Materials.Clear();
            var materials = await _materialDataService.GetAllAsync();
            foreach (var material in materials)
            {
                Materials.Add(material);
            }
        }
    }
}
