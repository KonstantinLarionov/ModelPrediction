using ModelPrediction.View.ModelThreat.Forms;
using ModelPrediction.ViewModel.ModelThreat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModelPrediction.View.ModelThreat
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class ModelThreat : UserControl
    {
        private ModelThreatVM VM { get; set; }
        public ModelThreat()
        {
            InitializeComponent();
            VM = new ModelThreatVM(threats);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreateModel createModel = new CreateModel();
            createModel.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ShowModel showModel = new ShowModel();
            showModel.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            AddThreat addThreat = new AddThreat();
            addThreat.Show();
        }

        private void Threats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Threat = (Model.Objects.ModelThreat.Threat)threats.SelectedItems[0];
            ModelThreatVM.SetInfoThreat(Threat);
        }
    }
}
