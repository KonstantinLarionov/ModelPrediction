using AForge.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelPrediction.Model
{
    class PredictionM
    {
        public int value_garmoniks = 0;
        public List<double[]> darbiV { get; set; }
        public double[] darbi { get; set; }
        double[,] arr_x = null;
        double[] ak1 = null;
        double[] ak2 = null;
        double[] bk1 = null;
        double a01 = 0;
        public double alfa = 0;

        public double[] GetExpPrediction(double[] main_data, int number, double alfa, double[] analitics = null)
        {
            if (analitics == null)
            {
                int length = main_data.Length + number;
                double[] predict = new double[length + number];
                predict[0] = main_data[0];
                for (int i = 1; i < length; i++)
                {
                    if (i < main_data.Length)
                    {
                        predict[i] = alfa * main_data[i] + (1 - alfa) * predict[i - 1];
                    }
                    else
                    {
                        predict[i] = alfa * predict[i - 1] + (1 - alfa) * predict[i - 1];
                    }

                }

                return predict;
            }
            else
            {
                int length = analitics.Length + number;
                double[] predict = new double[length];
                predict[0] = main_data[0];
                for (int i = 1; i < length; i++)
                {
                    if (i < analitics.Length)
                    {
                        predict[i] = analitics[i];
                    }
                    else
                    {
                        predict[i] = this.alfa * predict[i - 1] + (1 - this.alfa) * predict[i - 1];
                    }

                }

                return predict;
            }
        }

        public double[] GetMAPrediction(double[] main_data, int number)
        {
            int length = main_data.Length;
            double[] predict = new double[length + number];

            for (int i = 0; i < length + number; i++)
            {
                if (i < length)
                {
                    double sum = 0;
                    for (int j = 0; j <= i; j++)
                    {
                        sum += main_data[j];
                    }
                    predict[i] = Math.Round(sum / (i + 1), 5);
                }
                else
                {
                    predict[i] = predict[i - 1];
                }

            }
            return predict;
        }

        //public double[] GetPolyPrediction2(double[] coef1, int number)
        //{
        //    List<double> mainArray = coef1.ToList();
        //    a01 = 0;
        //    for (int i = 0; i < mainArray.Count; i++)
        //    {
        //        a01 += Convert.ToDouble(mainArray[i]);
        //    }
        //    a01 = a01/mainArray.Count;

        //}
        public double[] GetPolyPrediction(double[] coef1, int number)
        {
            if (value_garmoniks == 0)
            {
                List<double> coef = new List<double>();
                for (int i = 0; i < coef1.Length; i++)
                {
                    coef.Add(coef1[i]);
                }
                double[] U = new double[coef.Count];
                a01 = 0;
                for (int i = 0; i < coef.Count; i++)
                {
                    a01 += Convert.ToDouble(coef[i]);
                }
                a01 /= coef.Count;

                ak1 = new double[coef.Count / 2];
                bk1 = new double[coef.Count / 2];
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

                arr_x = new double[(coef.Count / 2), coef.Count + number];
                int cost = 0;
                int costi = 0;
                for (int i = 0; i < (coef.Count / 2) - 4; i++)
                {
                    cost++;
                    for (int j = 0; j < coef.Count + number; j++)
                    {
                        costi++;
                        if (cost < 2)
                        {
                            arr_x[i, j] = a01 + (ak1[1] * Math.Cos(2 * Math.PI * 1 * 1 / coef.Count) + bk1[1] * Math.Sin(2 * Math.PI * 1 * 1 / coef.Count))/* + + */ + 0;
                        }
                        else
                        {
                            arr_x[i, j] = (ak1[i] * Math.Cos(2 * Math.PI * cost * costi / coef.Count) + (bk1[i] * Math.Sin(2 * Math.PI * cost * costi / coef.Count)))/* + + */ ;
                        }
                    }
                }
                double dVerh = 0;
                double dNiz = 0;
                double d = 0;
                darbi = new double[(coef.Count / 2) - 4];

                for (int j = 0; j < (coef.Count / 2) - 4; j++)
                {
                    //if (d == 0 || d < 1.76 || d > 2.25)
                    //{
                    if (j == 0)
                    {

                        for (int i = 1; i < coef.Count; i++)
                        {
                            dVerh += Math.Pow((Convert.ToDouble(coef[i]) - arr_x[0, i]) - (Convert.ToDouble(coef[i - 1]) - arr_x[0, i - 1]), 2);
                        }
                        for (int i = 0; i < coef.Count; i++)
                        {
                            dNiz += Math.Pow((Convert.ToDouble(coef[i]) - arr_x[0, i]), 2);
                        }
                        d = dVerh / dNiz;
                        d = Math.Round(d, 2);
                        darbi[j] = d;
                        dVerh = 0; dNiz = 0;
                    }
                    else
                    {

                        for (int i = 1; i < coef.Count; i++)
                        {
                            dVerh += Math.Pow((Convert.ToDouble(coef[i]) - arr_x[0, i]) - (Convert.ToDouble(coef[i - 1]) - arr_x[0, i - 1]), 2);
                        }
                        for (int i = 0; i < coef.Count; i++)
                        {
                            arr_x[0, i] += arr_x[j, i];
                            dNiz += Math.Pow((Convert.ToDouble(coef[i]) - arr_x[0, i]), 2);
                        }
                        d = dVerh / dNiz;
                        d = Math.Round(d, 2);
                        darbi[j] = d;
                        dVerh = 0; dNiz = 0;
                    }

                    //}
                    //else
                    //{
                    value_garmoniks = j;
                    //    break;
                    //}
                }
                for (int i = 0; i < coef.Count; i++)
                {
                    U[i] = arr_x[0, i];
                }
                return U;
            }
            else
            {
                double[] arr_out = new double[coef1.Length + number];
                for (int i = 0; i < coef1.Length; i++)
                {
                    arr_out[i] = arr_x[0, i];
                }
                int count = 0;
                for (int i = arr_out.Length - number; i < arr_out.Length; i++)
                {
                    arr_out[i] = arr_x[0, count];
                    count++;
                }
                return arr_out;
            }
        }

        public static double GetStudent(double[] main, double[] pred)
        {
            double student = 0;
            double buffer = 0;
            double standart1 = 0;
            try
            {
                for (int i = 0; i < main.Length; i++)
                {
                    buffer += main[i];
                    standart1 += Math.Pow(pred[i] - (main[i] - pred[i]), 2);
                }
            }
            catch
            {
                standart1 = 0;
                buffer = 0;
                for (int i = 0; i < pred.Length; i++)
                {
                    buffer += main[i];
                    standart1 += Math.Pow(pred[i] - (main[i] - pred[i]), 2);
                }
            }
            double sredMain = buffer / main.Length;
            standart1 = Math.Sqrt(standart1 / main.Length);

            double buffer2 = 0;
            double standart2 = 0;
            try
            {
                for (int i = 0; i < main.Length; i++)
                {
                    buffer2 += pred[i];
                    standart2 += Math.Pow(main[i] - (pred[i] - main[i]), 2);
                }
            }
            catch
            {
                buffer2 = 0;
                standart2 = 0;
                for (int i = 0; i < pred.Length; i++)
                {
                    buffer2 += pred[i];
                    standart2 += Math.Pow(main[i] - (pred[i] - main[i]), 2);
                }
            }
            standart2 = Math.Sqrt(standart2 / pred.Length);
            double sredMain2 = buffer2 / pred.Length;

            student = (Math.Abs(sredMain - sredMain2)) / (Math.Sqrt((Math.Pow(standart1, 2) / main.Length) + (Math.Pow(standart2, 2) / pred.Length)));


            return student;
        }
        /*-----*/
        public double[] FFTForward(AForge.Math.Complex[] inpute)
        {
            AForge.Math.Complex[] outpute = inpute;
            AForge.Math.FourierTransform.FFT(outpute, FourierTransform.Direction.Forward);
            double[] arr_out = new double[outpute.Length];
            for (int i = 0; i < outpute.Length; i++)
            {
                arr_out[i] = Math.Sqrt((Math.Pow(outpute[i].Re, 2) + Math.Pow(outpute[i].Im, 2)));
            }
            return arr_out;
        }

        public double[] FFTForwardQ(AForge.Math.Complex[] inpute)
        {
            AForge.Math.Complex[] outpute = inpute;
            AForge.Math.FourierTransform.FFT(outpute, FourierTransform.Direction.Forward);
            double[] arr_out = new double[outpute.Length];
            for (int i = 0; i < outpute.Length; i++)
            {
                arr_out[i] = Math.Atan(outpute[i].Im / outpute[i].Re);
            }
            return arr_out;
        }

        public double[] FFTBackward(AForge.Math.Complex[] inpute)
        {
            AForge.Math.Complex[] outpute = inpute;
            AForge.Math.FourierTransform.FFT(outpute, FourierTransform.Direction.Backward);
            double[] arr_out = new double[outpute.Length];
            for (int i = 0; i < outpute.Length; i++)
            {
                if (i == 0) { continue; }
                arr_out[i] = outpute[i].Re;
            }
            return arr_out;
        }


    }
}
