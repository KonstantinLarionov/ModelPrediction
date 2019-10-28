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
using ModelPrediction.Model;
using ModelPrediction.ViewModel.ModelThreat;
using ModelPrediction.ViewModel.PredictionData;
using ModelPrediction.ViewModel.Result;

namespace ModelPrediction.View
{
    /// <summary>
    /// Логика взаимодействия для OutputePanel.xaml
    /// </summary>
    public partial class OutputePanel : UserControl
    {
        public OutputePanel()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            OutDataVM.ShowOUT(dgOUT, ModelThreatVM.MainThreat.Data.Select(x=>x.Damage).ToArray(), PredictionDataVM.arr_data, PredictionDataVM._x);

            OutDataVM.ShowStatisticalAnaliz(new List<TextBox> {МАКС, МИН, САЗ, МО, МЕ, РВ, ДИСП, СКО, ОС, КВ, САОП, САО, ККСКО, СО, СТО}, ModelThreatVM.MainThreat.Data.Select(x => x.Damage).ToArray(), PredictionDataVM.arr_data);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Максимальное значение ряда ẋ(t)\nВычислено как:\n Max(ẋ(t))");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Минимальное значение ряда ẋ(t)\nВычислено как:\n Min(ẋ(t))");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Среднее арифметическое значение ряда ẋ(t)\nВычислено как:\n Sum(ẋ(t))/ẋ(t).length ");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Мода ряда ẋ(t)\nВычислено как:\n ?");
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Медиана ряда ẋ(t)\nВычислено как:\n ẋ(ẋ(t).length/2 + 1)");
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Размах вариации ẋ(t)\nВычислено как:\n Max(ẋ(t) - Min(ẋ(t) ");
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Дисперсия ряда ẋ(t)\nВычислено как:\n SQR(Sum(ẋ(t) - Sum(ẋ(t))/ẋ(t).length))/(ẋ(t).length-1) ");
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Среднее квадратическое (стандартное) отклонение ряда (ẋ(t)\nВычислено как:\n SQRT( SQR(Sum(ẋ(t) - Sum(ẋ(t))/ẋ(t).length))/(ẋ(t).length-1)  )");
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ошибка средней ряда ẋ(t)\nВычислено как:\n SQRT( SQR(Sum(ẋ(t) - Sum(ẋ(t))/ẋ(t).length))/(ẋ(t).length-1) / SQRT(ẋ(t).length)  ");
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Коэффициент вариации ряда ẋ(t)\nВычислено как:\n (SQRT( SQR(Sum(ẋ(t) - Sum(ẋ(t))/ẋ(t).length))/(ẋ(t).length-1) /  Sum(ẋ(t))/ẋ(t).length))/(ẋ(t).length)) * 100%");
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Средняя абсолютная ошибка в процентах ряда ẋ(t)\nВычислено как:\n Sum(ABS(x(t) - ẋ(t))) * 100 /  x(t).length");
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Средняя абсолютная ошибка ряда ẋ(t)\nВычислено как:\n Sum(ABS(x(t) - ẋ(t))) /  x(t).length");
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Квадратный корень из среднеквадратичной ошибки ряда ẋ(t)\nВычислено как:\n SQRT(Sum(SQR(ABS(x(t) - ẋ(t))))) /  x(t).length");
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Cредняя ошибка ряда ẋ(t)\nВычислено как:\n Sum(x(t) - ẋ(t)) /  x(t).length");
        }

        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Стандартное отклонение ряда ẋ(t)\nВычислено как:\n SQRT(Sum(ẋ(t) - (Sum(x(t) - ẋ(t)) /  x(t).length))/ x(t).length) ");
        }
    }
}
