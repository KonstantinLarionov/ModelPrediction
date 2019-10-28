using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;
using ModelPrediction.Model;
using ModelPrediction.Model.Objects.ModelThreat;
using AForge.Math;

namespace ModelPrediction.ViewModel.AnaliticData
{
    class AnaliticDataVM
    {
        private Threat Threat { get; set; }
        private double[] A_spector { get; set; }
        private double[] F_spector { get; set; }
        public AnaliticDataVM(Threat threat)
        {
            Threat = threat;
        }

        public void PaintChartLine(CartesianChart cc)
        {
            Drawer pen = new Drawer(cc);
            pen.DrawLinerChart(Threat.Data.Select(x=>x.Damage).ToArray(), "Основные данные");
        }
        public void PaintChartSpecters(CartesianChart cc, CartesianChart cc2)
        {
            Drawer pen = new Drawer(cc);
            Drawer pen2 = new Drawer(cc2);
            MainData maindata = new MainData();
            double[] data = Threat.Data.Select(x => x.Damage).ToArray();
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

            A_spector = maindata.FFTForward(arr_main);
            F_spector = maindata.FFTForwardQ(arr_main);
            pen.DrawColumnChart(A_spector, "Амплитудный спектр");
            pen2.DrawColumnChart(F_spector, "Фазовый спектр");
        }
        public void CalcStatisticsParams(TextBox[] textBoxes)
        {
            double[] data = Threat.Data.Select(x => x.Damage).ToArray();

            foreach (var item in textBoxes)
            {
                switch (item.Name)
                {
                    case "МАКС":
                        item.Text = data.Max().ToString();
                        break;
                    case "МИН":
                        item.Text = data.Min().ToString();
                        break;
                    case "САЗ":
                        item.Text = data.Average().ToString();
                        break;
                    case "МО":

                        double moda = 0;
                        List<(double,int)> counts = new List<(double, int)>();
                        for (int i = 0; i < data.Length; i++)
                        {
                            int countBuffer = 0;
                            for (int j = 0; j < data.Length; j++)
                            {
                                if (data[i] == data[j])
                                {
                                    countBuffer++;
                                }
                            }
                            counts.Add((data[i], countBuffer));
                        }
                        moda = counts.Where(w => w.Item2 == counts.Max(x => x.Item2)).Select(o => o.Item1).FirstOrDefault();

                        item.Text = moda.ToString();
                        break;
                    case "МЕ":
                        item.Text = data[Convert.ToInt32((data.Length - 1) / 2)].ToString();
                        break;
                    case "РВ":
                        item.Text = (data.Max() - data.Min()).ToString();
                        break;
                    case "ДИСП":
                        double disp = 0;
                        double buff = 0;
                        double aver = data.Average();
                        for (int i = 0; i < data.Length; i++)
                        {
                            buff += Math.Pow((data[i] - aver), 2);
                        }
                        disp = buff / data.Length;
                        item.Text = disp.ToString();
                        break;
                    case "СКО":
                        double disp2 = 0;
                        double buff2 = 0;
                        double aver2 = data.Average();
                        for (int i = 0; i < data.Length; i++)
                        {
                            buff2 += Math.Pow((data[i] - aver2), 2);
                        }
                        disp2 = buff2 / data.Length;
                        disp2 = Math.Sqrt(disp2);
                        item.Text = disp2.ToString();
                        break;
                    case "КВ":
                        double disp3 = 0;
                        double buff3 = 0;
                        double aver3 = data.Average();
                        for (int i = 0; i < data.Length; i++)
                        {
                            buff3 += Math.Pow((data[i] - aver3), 2);
                        }
                        disp3 = buff3 / data.Length;
                        disp3 = Math.Sqrt(disp3);
                        disp3 = disp3 / aver3;
                        item.Text = disp3.ToString();
                        break;
                    default:
                        break;
                }
            }
        }
        public static double[] AreaValue { get; set; }
        public void PaintArea(CartesianChart cc, List<Level> values)
        {
            Drawer drawer = new Drawer(cc);
            AreaValue = values.Select(x => x.Value).ToArray();
            drawer.DrawAreas(AreaValue,Threat.Data.Count);
        }
    }
}
