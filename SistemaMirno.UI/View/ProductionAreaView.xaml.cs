using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace SistemaMirno.UI.View
{
    /// <summary>
    /// Interaction logic for ProductionAreaView.xaml
    /// </summary>
    public partial class ProductionAreaView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductionAreaView"/> class.
        /// </summary>
        public ProductionAreaView()
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
