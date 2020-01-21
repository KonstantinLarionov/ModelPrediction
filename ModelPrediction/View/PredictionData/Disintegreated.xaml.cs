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
using Prediction.ViewModel;

namespace ModelThreat.View
{
    /// <summary>
    /// Логика взаимодействия для Disintegreated.xaml
    /// </summary>
    public partial class Disintegreated : Window
    {
        private double[] data = null;
        public Disintegreated(double[] d)
        {
            InitializeComponent();
            data = d;
            for (int i = 0; i < data.Length/2; i++)
            {
                count.Items.Add(i+1);
            }
            count.Text = count.Items[data.Length / 2 - 1].ToString();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DisintegreadVM.ShowLiner(linar, data);
            DisintegreadVM.ShowGarmonic(garmonik, randomic, data);
        }

        private void Count_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisintegreadVM.ShowGarmonic(garmonik, randomic, data, count.SelectedItem.ToString());
        }
    }
}
