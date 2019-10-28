using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelPrediction.Model
{
    public static class PredictionUpdate
    {

        public static List<double> Prediction(List<double> data, int number)
        {
            List<double> prediction = new List<double>();
            double[] trend = TrendLine(data, number);
            double y_mean = data.Average(x => x); // Среднее значение массива

            #region Подсчет коэффициентов
            List<double> listA = new List<double>(); // Набор коэффициентов А по каждой гармоники
            List<double> listB = new List<double>(); // Набор коэффициентов B по каждой гармоники
            for (int i = 1; i < data.Count / 2 - 1; i++) // Цикл гармоник
            {
                double a = 0; double b = 0;
                double sumdataA = 0; double sumdataB = 0;
                //---> Расчитать коэффициенты гармоники 

                for (int j = 0; j < data.Count; j++) // Цикл перебора данных
                {
                    double angle = (2.0 * Math.PI * i * Convert.ToDouble(j + 1) / Convert.ToDouble(data.Count));
                    sumdataA += data[j] * Math.Cos(angle);
                    sumdataB += data[j] * Math.Sin(angle);
                }
                a = (2.0 * sumdataA) / Convert.ToDouble(data.Count);
                b = (2.0 * sumdataB) / Convert.ToDouble(data.Count);

                listA.Add(a);
                listB.Add(b);
            }
            #endregion

            #region Подсчет гармоник
            List<List<double>> harmonics = new List<List<double>>(); //Набор гармоник
            for (int i = 1; i < data.Count / 2 - 1; i++)
            {
                List<double> harmonic = new List<double>();
                //---> Расчитать гармонику с коэффициентами 
                for (int j = 0; j < data.Count + number; j++) // Цикл перебора данных
                {
                    double angle = (2.0 * Math.PI * i * Convert.ToDouble(j + 1) / Convert.ToDouble(data.Count));
                    double angleCos = listA[i - 1] * Math.Cos(angle);
                    double angleSin = listB[i - 1] * Math.Sin(angle);
                    harmonic.Add(angleCos + angleSin);
                }
                harmonic[i - 1] += y_mean;
                harmonics.Add(harmonic);
            }
            #endregion

            #region Суммирование гармоник
            //Суммирование гармоник
            for (int i = 0; i < data.Count + number; i++) // Цикл элементов
            {
                double bufferPrediction = 0;
                for (int j = 0; j < data.Count /2 -2; j++) //Цикл гармоник
                {
                    bufferPrediction += harmonics[j][i];
                }
                prediction.Add(bufferPrediction + trend[i]);
            }
            #endregion

            return prediction;
        }

        public static double[] TrendLine(List<double> y, int number)
        {
            double a = 0;
            double b = 0;
            double[] x = new double[y.Count];
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = i;
            }

            if (x.Length != y.Count || x.Length <= 1)
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


            double[] points = new double[y.Count + number];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = b + (a * i);
            }
            return points;
        }
    }
}
