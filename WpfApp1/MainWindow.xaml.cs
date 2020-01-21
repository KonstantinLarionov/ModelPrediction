using ModelPrediction.Model;
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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Drawer pen = new Drawer(predictionChart);
            Drawer pen2 = new Drawer(predictionChart2);
            Drawer pen3 = new Drawer(predictionChart3);

            List<double> vs = new List<double>();
            for (int i = 0; i < 30; i++)
            {
                vs.Add(Math.Sin(i));
            }
            pen.DrawLinerChart(vs.ToArray(), "Pred");
            var pred = PredictionUpdate.Prediction(vs,20,true);
            pen.DrawLinerChart(pred.ToArray(),"Pred");

            MainData mainData = new MainData();
            mainData.Load_TXT("Сетевые-атаки(кол-во).txt");
            var pred2 = PredictionUpdate.Prediction(mainData.GetData().ToList(), 0, true);
            pen3.DrawLinerChart(pred2.ToArray(), "pred");
            pen3.DrawLinerChart(mainData.GetData(), "main");

            MainData mainData2 = new MainData();
            mainData2.Load_TXT("Сетевые-атаки.txt");
            var pred3 = PredictionUpdate.Prediction(mainData2.GetData().ToList(), 0, true);
            pen2.DrawLinerChart(pred3.ToArray(), "pred");
            pen2.DrawLinerChart(mainData2.GetData(), "main");

        }
    }
}
