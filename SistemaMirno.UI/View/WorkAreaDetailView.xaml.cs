using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace SistemaMirno.UI.View
{
    /// <summary>
    /// Interaction logic for ProductionAreaDetailView.xaml
    /// </summary>
    public partial class WorkAreaDetailView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkAreaDetailView"/> class.
        /// </summary>
        public WorkAreaDetailView()
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