using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelPrediction.Model;
using AForge.Math;
using System.Windows;
using System.Windows.Controls;
using ModelPrediction.ViewModel.AnaliticData;

namespace ModelPrediction.ViewModel.PredictionData
{
    class PredictionDataVM
    {
        public static double[] arr_data { get; set; }
        public static double[] a_spector { get; set; }
        public static double[] f_spector { get; set; }
        public static double[] arr_analitics { get; set; }
        public static int _x { get; set; }
        public static PredictionM analitics;
        public static void DrawMainData(CartesianChart cc1, CartesianChart cc2, double[] data)
        {
            try
            {
                Drawer pen = new Drawer(cc1);
                Drawer pen2 = new Drawer(cc2);
                Drawer pen3 = new Drawer(cc1);
                MainData maindata = new MainData();

                double logLength = Math.Ceiling(Math.Log((double)data.Length, 2.0));
                int paddedLength = (int)Math.Pow(2.0, Math.Min(Math.Max(1.0, logLength), 14.0));

                Complex[] arr_main = new Complex[paddedLength / 2];

                for (int i = 0; i < paddedLength / 2; i++)
                {
                    try
                    {
                        Complex arr = new Complex(data[i], 0);
                        arr_main[i] = arr;
                    }
                    catch
                    {
                        Complex arr = new Complex(0, 0);
                        arr_main[i] = arr;
                    }
                }

                a_spector = maindata.FFTForward(arr_main);
                f_spector = maindata.FFTForwardQ(arr_main);
                pen2.DrawColumnChart(a_spector, "Амплитудный спектр");
                pen.DrawLinerChart(data, "Основные данные");
                pen3.DrawAreas(AnaliticDataVM.ValueLevels,data.Length-1);
            }
            catch
            {
                MessageBox.Show("Данные не загружены. Пожалуйста загрузите данные в нужном разделе.");
            }

        }

        public static void ReDrawColumn(CartesianChart cc, bool a_to_f = true)
        {
            cc.Series.Clear();
            if (a_to_f)
            {
                Drawer pen3 = new Drawer(cc);
                pen3.DrawColumnChart(a_spector, "Амплитудный спектор");
            }
            else
            {
                Drawer pen3 = new Drawer(cc);
                pen3.DrawColumnChart(f_spector, "Фазовый спектор");
            }
        }

        public static void GetAnaliticsPoly(CartesianChart cc, DataGrid dgv, double[] data, int x, int y, bool darbin, string dn, string dv, string countG)
        {
            _x = x;
            analitics = new PredictionM();
            Drawer pen = new Drawer(cc);

            var buffer_data = data.Skip(x).Take(y - x + 1);
            int count = 0;
            double[] buffer = new double[y - x + 1];
            foreach (var item in buffer_data)
            {
                buffer[count] = Convert.ToDouble(item);
                count++;
            }

            
            double[] arr_analitics = PredictionUpdate.Prediction(buffer.ToList(), 0, darbin, Convert.ToDouble(dn.Replace(".",",")), Convert.ToDouble(dv.Replace(".", ",")), countG).ToArray();

            pen.DrawLinerChart(arr_analitics, x, "Аналитика");
            //List<Darbin> table = new List<Darbin>();
            //for (int i = 0; i < analitics.darbi.Length; i++)
            //{
            //    table.Add(new Darbin { N = (i + 1).ToString(), D = analitics.darbi[i] });
            //}
            double error = 0;
            double sum = 0;
            for (int i = 0; i < buffer.Length; i++)
            {
                sum += (Math.Abs(buffer[i] - arr_analitics[i]) / buffer[i]) * 100;
            }
            error = sum / buffer.Length;

            //table.Add(new Darbin { N = "Средняя ошибка", D = Math.Round(error, 2) });
            //dgv.ItemsSource = table;
            Darbins = PredictionUpdate.darbins;
        }
        public static List<Darbin> Darbins { get; set; }
        public static void GetPredictionPoly(CartesianChart cc, double[] data, int x, int y, int number, bool darbin, string dn, string dv)
        {
            if (analitics == null)
            {
                MessageBox.Show("Вы не проанализировали временной ряд.");
                return;
            }
            Drawer pen = new Drawer(cc);

            var buffer_data = data.Skip(x).Take(y - x + 1);
            int count = 0;
            double[] buffer = new double[y - x + 1];
            foreach (var item in buffer_data)
            {
                buffer[count] = Convert.ToDouble(item);
                count++;
            }
            
            double[] arr_analitics = PredictionUpdate.Prediction(buffer.ToList(), number, darbin, Convert.ToDouble(dn.Replace(".", ",")), Convert.ToDouble(dv.Replace(".", ","))).ToArray();
            arr_data = arr_analitics;
            pen.DrawLinerChart(arr_analitics.Skip(buffer.Length - 1).ToArray(), y, "Прогноз");
            Darbins = PredictionUpdate.darbins;
        }

        public static void GetAnaliticsExp(CartesianChart cc, DataGrid dgv, double[] data, int x, int y, double error_need)
        {
            analitics = new PredictionM();
            Drawer pen = new Drawer(cc);

            var buffer_data = data.Skip(x).Take(y - x + 1);
            int count = 0;
            double[] buffer = new double[y - x + 1];
            foreach (var item in buffer_data)
            {
                buffer[count] = Convert.ToDouble(item);
                count++;
            }

            double alfa = 0.3;
            double error = 0;
            arr_analitics = analitics.GetExpPrediction(buffer, 0, alfa);
            //do
            //{
                
            //    double sum = 0;

            //    for (int i = 0; i < buffer.Length; i++)
            //    {
            //        sum += ((buffer[i] - arr_analitics[i])/ buffer[i]) * 100;
            //    }
            //    error = sum / buffer.Length;

            //    List<Darbin> table = new List<Darbin>();
            //    table.Add(new Darbin { N = alfa.ToString(), D = Math.Round(error, 2) });
            //    dgv.Items.Add(table);
            //    alfa += 0.1;

            //} while (error > error_need);
            pen.DrawLinerChart(arr_analitics, x, "Аналитика");
            Darbins = PredictionUpdate.darbins;

        }

        public static void GetPredictionExp(CartesianChart cc, double[] data, int x, double[] anal, int number)
        {
            if (analitics == null)
            {
                MessageBox.Show("Вы не анализировали временной ряд.");
            }
            Drawer pen = new Drawer(cc);
            double[] arr_predict = analitics.GetExpPrediction(data, number, 0, anal);
            pen.DrawLinerChart(arr_predict, x, "Аналитика");
            arr_data = arr_predict;
        }

        public static void GetAnaliticsMidle(CartesianChart cc, DataGrid dgv, double[] data, int x, int y, int number)
        {
            analitics = new PredictionM();
            Drawer pen = new Drawer(cc);

            var buffer_data = data.Skip(x).Take(y - x + 1);
            int count = 0;
            double[] buffer = new double[y - x + 1];
            foreach (var item in buffer_data)
            {
                buffer[count] = Convert.ToDouble(item);
                count++;
            }

            double[] arr_analitics = analitics.GetMAPrediction(buffer, number);
            arr_data = arr_analitics;
            List<Darbin> table = new List<Darbin>();

            double error = 0;
            double sum = 0;
            for (int i = 0; i < buffer.Length; i++)
            {
                sum += (Math.Abs(buffer[i] - arr_analitics[i]) / buffer[i]) * 100;
            }
            error = sum / buffer.Length;
            pen.DrawLinerChart(arr_analitics, x, "Аналитика");
            table.Add(new Darbin { N = "Средняя ошибка", D = Math.Round(error, 2) });
            dgv.ItemsSource = table;
        }
    }
}
