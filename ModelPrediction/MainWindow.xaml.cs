using ModelPrediction.View;
using ModelPrediction.View.ModelThreat;
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

namespace ModelPrediction
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ModelPrediction.View.ModelThreat.ModelThreat ModelThreat;
        private AnaliticData AnaliticData;
        private PredictionData PredictionData;
        private OutputePanel OutputePanel;
        public MainWindow()
        {
            InitializeComponent();
            ModelThreat = new ModelPrediction.View.ModelThreat.ModelThreat();
            AnaliticData = new AnaliticData();
            PredictionData = new PredictionData();
            OutputePanel = new OutputePanel();

            ModelThreatVM.SetLabels(nameModel, countThreat, dateCreate, nameThreat, number, countData);
        }

        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            if (menu.Width == 250)
            {
                menu.Width = 252 - ( 250 - button.Width);
                menuIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Menu;
            }else
            {
                menu.Width = 250;
                menuIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.MenuLeft;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void PanelSwicher(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Name)
            {
                case "model":
                    panelView.Children.Clear();
                    panelView.Children.Add(ModelThreat);
                    break;
                case "analitic":
                    panelView.Children.Clear();
                    panelView.Children.Add(AnaliticData);
                    break;
                case "predict":
                    panelView.Children.Clear();
                    panelView.Children.Add(PredictionData);
                    break;
                case "result":
                    panelView.Children.Clear();
                    panelView.Children.Add(OutputePanel);
                    break;
                case "about":
                    MessageBox.Show("Программа: \"Метод анализа и прогнозирования параметров модели угроз на основе полигармонического полинома\"\nАвторы: Т.З. Аралбаев, К.О. Ларионов.\nОписание: Программа предназначена для исследования модели полигармонического полинома в рамках статистических данных модели угроз.");
                    break;
            }
            
        }
    }
}
