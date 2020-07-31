using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace SistemaMirno.UI.View
{
    /// <summary>
    /// Interaction logic for ProductionAreaDetailView.xaml
    /// </summary>
    public partial class ProductionAreaDetailView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductionAreaDetailView"/> class.
        /// </summary>
        public ProductionAreaDetailView()
        {
            InitializeComponent();
        }

        private void TextboxNumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}