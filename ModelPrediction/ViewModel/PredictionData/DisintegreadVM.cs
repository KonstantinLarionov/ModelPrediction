using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Math;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using ModelPrediction.Model;
using Prediction.Model;

namespace Prediction.ViewModel
{
    class DisintegreadVM
    {

        static double MySinus(double angle) // Синус угла 
        {
            return Math.Sin((angle / 180D) * Math.PI);
        }
        static double MyCosinus(double angle) // Косинус угла 
        {
            return Math.Cos((angle / 180D) * Math.PI);
        }
        /// <summary>
        /// Метод наименьших квадратов
        /// </summary>
        /// <param name="cc"></param>
        /// <param name="y"></param>
        public static void ShowLiner(LiveCharts.Wpf.CartesianChart cc, double[] y)
        {
            double a = 0;
            double b = 0;
            double[] x = new double[y.Length];
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = i;
            }

            if (x.Length != y.Length || x.Length <= 1)
                throw new ArgumentException("Неверные размеры данных");
            double a11 = 0.0, a12 = 0.0, a22 = x.Length, b1 = 0.0, b2 = 0.0;
            for (int i = 0; i < x.Length; i++)
            {
                a11 += x[i] * x[i];
                a12 += x[i];
                b1 += x[i] * y[i];
                b2 += y[i];
            }
            double det = a11 * a22 - a12 * a12;
            if (Math.Abs(det) < 1e-17)
                throw new ArgumentException("Данные не верны");
            a = (b1 * a22 - a12 * b2) / det;
            b = (a11 * b2 - b1 * a12) / det;

            Drawer pen = new Drawer(cc);
            double[] points = new double[y.Length];
            for (int i = 0; i < points.Length; i++)
			{
			    points[i] = b + (a*i); 
			}

            pen.DrawLinerChart(points,"Линейная компонета");
        }

        public static void ShowGarmonic(LiveCharts.Wpf.CartesianChart cc, LiveCharts.Wpf.CartesianChart cc2, double[] data, string count = "15")
        {
            
            int g = Convert.ToInt32(count);
            List<double> coef = new List<double>();
            for (int i = 0; i < data.Length; i++)
            {
                coef.Add(data[i]);
            }
            double[] U = new double[coef.Count];
            double a01 = 0;
            for (int i = 0; i < coef.Count; i++)
            {
                a01 += Convert.ToDouble(coef[i]);
            }
            a01 /= coef.Count;
            double[] ak1 = new double[coef.Count / 2];
            double[] bk1 = new double[coef.Count / 2];
            int costilI = 0;
            int costilJ = 0;
            for (int i = 0; i < ak1.Length; i++)
            {
                costilI++;
                for (int j = 0; j < coef.Count; j++)
                {
                    costilJ++;
                    ak1[i] += coef[j] * Math.Cos(2 * Math.PI * (costilI) * costilJ / coef.Count);
                    bk1[i] += coef[j] * Math.Sin(2 * Math.PI * (costilI) * costilJ / coef.Count);
                }
                ak1[i] *= 2;
                ak1[i] /= coef.Count;
                bk1[i] *= 2;
                bk1[i] /= coef.Count;
            }
            double[,] arr_x = new double[(coef.Count / 2), coef.Count];
            int cost = 0;
            int costi = 0;
            for (int i = 0; i < (coef.Count / 2); i++)
            {
                cost++;
                for (int j = 0; j < coef.Count; j++)
                {
                    costi++;
                    if (cost < 2)
                    {
                        arr_x[i, j] = a01 + (ak1[1] * Math.Cos(2 * Math.PI * 1 * 1 / coef.Count) + bk1[1] * Math.Sin(2 * Math.PI * 1 * 1 / coef.Count))/* + + */ + 0;
                    }
                    else
                    {
                        arr_x[i, j] = (ak1[i] * Math.Cos(2 * Math.PI * cost * costi / coef.Count) + bk1[i] * Math.Sin(2 * Math.PI * cost * costi / coef.Count))/* + + */ ;
                    }
                }
            }
            double[] values = new double[coef.Count];
            for (int i = 0; i < values.Length; i++)
            {
                for (int c = 0; c < g; c++)
                {
                    try
                    {
                        values[i] += arr_x[c, i];
                    }
                    catch
                    {
                        continue;
                    }
                }
               
            }
            Drawer pen = new Drawer(cc);
            pen.DrawLinerChart(values, "Сезонная компонента");
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = Math.Abs(values[i] - data[i]);
            }
            Drawer pen2 = new Drawer(cc2);
            pen2.DrawLinerChart(values, "Случайная компонента");
        }
    }
}
