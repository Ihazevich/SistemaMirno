using Prism.Events;
using SistemaMirno.Model;
using SistemaMirno.UI.Data;
using SistemaMirno.UI.Event;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SistemaMirno.UI.ViewModel
{
    public class ColorViewModel : ViewModelBase, IColorViewModel
    {
        private IColorDataService _colorDataService;
        private IEventAggregator _eventAggregator;

        public ObservableCollection<Color> Colors { get; set; }

        public ColorViewModel(IColorDataService colorDataService, IEventAggregator eventAggregator)
        {
            Colors = new ObservableCollection<Color>();
            _colorDataService = colorDataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<ShowColorViewEvent>()
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
            Colors.Clear();
            var colors = await _colorDataService.GetAllAsync();
            foreach (var color in colors)
            {
                Colors.Add(color);
            }
        }
    }
}
