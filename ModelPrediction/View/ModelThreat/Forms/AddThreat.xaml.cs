using Microsoft.Win32;
using ModelPrediction.ViewModel.ModelThreat;
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
    /// Логика взаимодействия для AddThreat.xaml
    /// </summary>
    public partial class AddThreat : Window
    {
        private AddThreatVM VM { get; set; }
        public AddThreat()
        {
            InitializeComponent();
            VM = new AddThreatVM();
        }

        private void Button_Click_1 (object sender, RoutedEventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog();
           // myDialog.Filter = "Статистические данные(*.XLT;*.XLCX;*.XLC;*.TXT)|*.XLT;*.XLCX;*.XLC;*.TXT" + "|Все файлы (*.*)|*.*"; //силен, умен, красив, полон сил
            myDialog.Filter = "Статистические данные(*.XLT;*.XLSX;*.XLS;*.TXT)|*.XLT;*.XLSX;*.XLS;*.TXT" + "|Все файлы (*.*)|*.*";
            myDialog.CheckFileExists = true;
            myDialog.Multiselect = true;

            if (myDialog.ShowDialog() == true)
            {
                VM.GetPath(myDialog.FileName);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VM.SetThread(new TextBox[] { numberFSTEK, name, verReal, uwerb} , new ComboBox[] { actual});
            ModelThreatVM.RefreshTableThreat();
        }
    }
}
