using LiveCharts;
using LiveCharts.Wpf;
using ModelPrediction.ViewModel.ModelThreat;
using ModelPrediction.ViewModel.PredictionData;
using ModelPrediction.View;
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
using ModelThreat.View;

namespace ModelPrediction.View.ModelThreat
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class PredictionData : UserControl
    {
        private double[] data { get; set; } = null;
        public PredictionData()
        {
            InitializeComponent();
            predictionChart.Zoom = ZoomingOptions.X;
            predictionChart.DisableAnimations = false;
            predictionChart.Hoverable = false;
            predictionColumn.Zoom = ZoomingOptions.X;
            predictionColumn.DisableAnimations = false;
            predictionColumn.Hoverable = false;
           
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            

        }

        private void TriggerColumn_Click(object sender, RoutedEventArgs e)
        {
            if (triggerColumn.IsChecked == true)
            {
                PredictionDataVM.ReDrawColumn(predictionColumn, false);
            }
            else if (triggerColumn.IsChecked == false)
            {
                PredictionDataVM.ReDrawColumn(predictionColumn);
            }
        }

        int tickclick = 0;
        int x = 0;
        int y = 0;
        private void PredictionChart_DataClick(object sender, ChartPoint chartPoint)
        {
            tickclick++;
            if (tickclick == 3)
            {
                tickclick = 0;
                predictionChart.AxisX.Clear();
                x = 0;
                y = 0;

                from.Text = x.ToString();
                to.Text = y.ToString();

                return;
            }

            if (tickclick == 1)
            {
                x = Convert.ToInt32(chartPoint.X);
            }

            if (x != 0 && x != y && tickclick == 2)
            {
                y = Convert.ToInt32(chartPoint.X);
            }

            from.Text = x.ToString();
            to.Text = y.ToString();
            if (x != 0 && y != 0)
            {

                SectionsCollection Sections = new SectionsCollection
                    {
                    new AxisSection
                    {

                        Value = Convert.ToInt32(from.Text),
                        SectionWidth = Convert.ToInt32(to.Text) - Convert.ToInt32(from.Text),
                        Fill = new System.Windows.Media.SolidColorBrush
                        {
                            Color = System.Windows.Media.Color.FromRgb(254,132,132),
                            Opacity = .4
                        }
                    }
                    };

                Axis a = new Axis();
                a.MaxValue = data.Length;
                a.MinValue = 0;
                a.Sections = Sections;
                predictionChart.AxisX.Clear();
                predictionChart.AxisX.Add(a);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (typePrediction.Text == "Полигармонический")
            {
                if (from.Text != "" && to.Text != "" || from.Text == "0" && to.Text == "0")
                {
                    PredictionDataVM.GetAnaliticsPoly(predictionChart, outDG, data, Convert.ToInt32(from.Text), Convert.ToInt32(to.Text));
                }
                else
                {
                    PredictionDataVM.GetAnaliticsPoly(predictionChart, outDG, data, 0, data.Length - 1);
                }
            }
            else if (typePrediction.Text == "Экспоненциальный")
            {
                if (from.Text != "" && to.Text != "")
                {
                    PredictionDataVM.GetAnaliticsExp(predictionChart, outDG, data, Convert.ToInt32(from.Text), Convert.ToInt32(to.Text), Convert.ToDouble(need_error.Text));
                }
                else
                {
                    PredictionDataVM.GetAnaliticsExp(predictionChart, outDG, data, 0, data.Length - 1, Convert.ToDouble(need_error.Text));
                }
            }
            else if (typePrediction.Text == "Скользящее среднее")
            {
                if (from.Text != "" && to.Text != "")
                {
                    PredictionDataVM.GetAnaliticsMidle(predictionChart, outDG, data, Convert.ToInt32(from.Text), Convert.ToInt32(to.Text), 0);
                }
                else
                {
                    PredictionDataVM.GetAnaliticsMidle(predictionChart, outDG, data, 0, data.Length - 1, 0);
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (number.Text == "" || number.Text == "0")
            {
                MessageBox.Show("Не введена длина прогноза.");
                return;
            }
            if (typePrediction.Text == "Полигармонический")
            {
                if (from.Text != "" && to.Text != "" || from.Text == "0" && to.Text == "0")
                {
                    PredictionDataVM.GetPredictionPoly(predictionChart, data, Convert.ToInt32(from.Text), Convert.ToInt32(to.Text), Convert.ToInt32(number.Text));
                }
                else
                {
                    PredictionDataVM.GetPredictionPoly(predictionChart, data, 0, data.Length - 1, Convert.ToInt32(number.Text));
                }
            }
            else if (typePrediction.Text == "Экспоненциальный")
            {
                if (from.Text != "" && to.Text != "")
                {
                    PredictionDataVM.GetPredictionExp(predictionChart, data, Convert.ToInt32(from.Text), PredictionDataVM.arr_analitics, Convert.ToInt32(number.Text));
                }
                else
                {
                    PredictionDataVM.GetPredictionExp(predictionChart, data, 0, PredictionDataVM.arr_analitics, Convert.ToInt32(number.Text));
                }
            }
            else if (typePrediction.Text == "Скользящее среднее")
            {
                if (from.Text != "" && to.Text != "")
                {
                    PredictionDataVM.GetAnaliticsMidle(predictionChart, outDG, data, Convert.ToInt32(from.Text), Convert.ToInt32(to.Text), Convert.ToInt32(number.Text));
                }
                else
                {
                    PredictionDataVM.GetAnaliticsMidle(predictionChart, outDG, data, 0, data.Length - 1, Convert.ToInt32(number.Text));
                }
            }
        }

        private void TypePrediction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (typePrediction.SelectedIndex == 1)
            {   
                need_error.IsEnabled = true;
            }
            else
            {
                need_error.IsEnabled = false;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Disintegreated dis = new Disintegreated(data);
            dis.Show();
            Disintegreated2 dis2 = new Disintegreated2(data);
            dis2.Show();
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            data = ModelThreatVM.MainThreat.Data.Select(x => x.Damage).ToArray();
            if (predictionChart.Series.Count == 0)
            {
                PredictionDataVM.DrawMainData(predictionChart, predictionColumn, data);
            }
            
        }
    }
}
