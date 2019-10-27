using ModelPrediction.ViewModel.ModelThreat;
using ModelPrediction.ViewModel.ModelThreat.Forms;

using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace ModelPrediction.View.ModelThreat.Forms
{
    /// <summary>
    /// Логика взаимодействия для ShowModel.xaml
    /// </summary>
    public partial class ShowModel : Window
    {
        private ShowModelVM VM { get; set; }
        private Model.Objects.ModelThreat.ModelThreat Model;
        public ShowModel()
        {
            InitializeComponent();
            VM = new ShowModelVM();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VM.ShowAllModel(models);
        }

        private void Models_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model = (Model.Objects.ModelThreat.ModelThreat)models.SelectedItems[0];
            VM.ShowModel(Model.Id, new TextBox[] { nameModel,nameOrg,dateCr, countThreat});
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Model != null)
            {
                ModelThreatVM.SetModelThreat(Model);
            }
        }
    }
}
