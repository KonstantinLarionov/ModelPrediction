using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelPrediction.Model
{
    public static class PredictionUpdate
    {
        public static List<Darbin> darbins = new List<Darbin>();
        public static List<double> Prediction(List<double> data, int number, bool darbin = false, double dn = 1.75, double dv = 2.25, string count = "")
        {
            List<double> prediction = new List<double>();
            //double[] trend = TrendLine(data, number);
            var trend = TrendLine2(data, number);
            double y_mean = data.Average(); // Среднее значение массива

            #region Подсчет коэффициентов
            List<double> listA = new List<double>(); // Набор коэффициентов А по каждой гармоники
            List<double> listB = new List<double>(); // Набор коэффициентов B по каждой гармоники
            for (int i = 1; i < data.Count / 2 + 1; i++) // Цикл гармоник
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
            for (int i = 1; i < data.Count / 2 + 1; i++)
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
                harmonic[i - 1] += 0/*среднее*/;
                harmonics.Add(harmonic);
            }
            #endregion
           
            #region Суммирование гармоник
            if (darbin)
            {
                for (int k = 0; k < data.Count / 2; k++)
                {
                    prediction.Clear();
                    //Суммирование гармоник
                    for (int i = 0; i < data.Count + number; i++) // Цикл элементов
                    {
                        double bufferPrediction = 0;
                        //Работаем с prediction и data
                        //Условие по дарбину и на выход из цикла
                        //Записать значение дарбина
                        for (int j = 0; j < k; j++) //Цикл гармоник
                        {
                            bufferPrediction += harmonics[j][i];
                        }
                        prediction.Add(bufferPrediction + trend/* /*trend[i] (data[i] - bufferPrediction)*/);
                    }

                    #region Проверка Дарбина
                    double darbinC = 0;
                    double darbinZ = 0;
                    for (int i = 1; i < data.Count; i++)
                    {
                        darbinC += Math.Pow(((data[i] - prediction[i]) - (data[i - 1] - prediction[i - 1])), 2);
                        darbinZ += Math.Pow((data[i] - prediction[i]), 2);
                    }
                    Darbin bufferDarbin = new Darbin();
                    bufferDarbin.N = k.ToString();
                    bufferDarbin.D = darbinC / darbinZ;
                    if (bufferDarbin.D >= dn && bufferDarbin.D <= dv)
                    {
                        darbins.Add(bufferDarbin);
                        break;
                    }
                    else
                    {
                        darbins.Add(bufferDarbin);
                    }
                    #endregion
                }
            }
            else
            {
                if (count != string.Empty )
                {
                    //Суммирование гармоник
                    for (int i = 0; i < data.Count + number; i++) // Цикл элементов
                    {
                        double bufferPrediction = 0;
                        //Работаем с prediction и data
                        //Условие по дарбину и на выход из цикла
                        //Записать значение дарбина
                        for (int j = 0; j < Convert.ToInt32(count); j++) //Цикл гармоник
                        {
                            bufferPrediction += harmonics[j][i];
                        }
                        prediction.Add(bufferPrediction + trend/*+ (data[i] - bufferPrediction)*/  /*+ trend[i]*/);
                    }
                }
                else
                {
                    //Суммирование гармоник
                    for (int i = 0; i < data.Count + number; i++) // Цикл элементов
                    {
                        double bufferPrediction = 0;
                        //Работаем с prediction и data
                        //Условие по дарбину и на выход из цикла
                        //Записать значение дарбина
                        for (int j = 0; j < data.Count / 2; j++) //Цикл гармоник
                        {
                            bufferPrediction += harmonics[j][i];
                        }
                        prediction.Add(bufferPrediction + trend /*(data[i] - bufferPrediction)*/  /*+ trend[i]*/);
                    }
                }
            }
            #endregion

            return prediction;
        }

        public static List<double> PredictionUPD(List<double> data, int number, bool darbin = false, double dn = 1.75, double dv = 2.25, string count="")
        {
            int g = Convert.ToInt32(count);
            List<double> coef = data;
            //for (int i = 0; i < data.Length; i++)
            //{
            //    coef.Add(data[i]);
            //}
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
            return values.ToList();
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
        public static double TrendLine2(List<double> y, int number)
        {
            //double a = 0;
            //double b = 0;
            //double[] x = new double[y.Count];
            //for (int i = 0; i < x.Length; i++)
            //{
            //    x[i] = i;
            //}

            //if (x.Length != y.Count || x.Length <= 1)
            //    throw new ArgumentException("Неверные размеры данных");
            //double a11 = 0.0, a12 = 0.0, a22 = x.Length, b1 = 0.0, b2 = 0.0;
            //for (int i = 0; i < x.Length; i++)
            //{
            //    a11 += x[i] * x[i];
            //    a12 += x[i];
            //    b1 += x[i] * y[i];
            //    b2 += y[i];
            //}
            //double det = a11 * a22 - a12 * a12;
            //if (Math.Abs(det) < 1e-17)
            //    throw new ArgumentException("Данные не верны");
            //a = (b1 * a22 - a12 * b2) / det;
            //b = (a11 * b2 - b1 * a12) / det;


            ////double[] points = new double[y.Count + number];
            ////for (int i = 0; i < points.Length; i++)
            ////{
            ////    points[i] = b + (a * i);
            ////}
            return y.Average();
        }
    }
}
