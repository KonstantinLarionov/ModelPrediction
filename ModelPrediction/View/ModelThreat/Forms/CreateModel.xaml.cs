using ModelPrediction.ViewModel.ModelThreat.Forms;
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
using System.Windows.Shapes;

namespace ModelPrediction.View.ModelThreat.Forms
{
    /// <summary>
    /// Логика взаимодействия для CreateModel.xaml
    /// </summary>
    public partial class CreateModel : Window
    {
        private CreateModelVM VM;
        public CreateModel()
        {
            InitializeComponent();
            VM = new CreateModelVM();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VM.CreateNewModel(NameModel.Text, NameOrganization.Text, TypeModel.Text);
            MessageBox.Show("Модель создана и готова к изменению!");
        }
    }
}
