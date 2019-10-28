using ModelPrediction.Model;
using Prediction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ModelPrediction.ViewModel.Result
{
    class OutDataVM
    {
        public static int numberExpiriment = 0;

        public static void ShowOUT(DataGrid dg, double[] mainData, double[] predictionData, int x)
        {
            numberExpiriment++;
            int length = predictionData.Length;
            List<DataOUT> output = new List<DataOUT>();
            for (int i = 0; i < length + x; i++)
            {
                try
                {
                    if (i < x)
                    {
                        output.Add(new DataOUT
                        {
                            nExp = numberExpiriment,
                            t = i,
                            xmt = mainData[i],
                            xpt = 0,
                            err = 0
                        });
                    }
                    else
                    {
                        output.Add(new DataOUT
                        {
                            nExp = numberExpiriment,
                            t = i,
                            xmt = mainData[i],
                            xpt = predictionData[i-x],
                            err = Math.Abs(predictionData[i - x] - mainData[i])
                        });
                    }
                }
                catch
                {
                   
                        output.Add(new DataOUT
                        {
                            nExp = numberExpiriment,
                            t = i,
                            xmt = 0,
                            xpt = predictionData[i-x],
                            err = predictionData[i-x]
                        });
                   
                }
            }
            dg.ItemsSource = output;
        }

        public static void ShowStatisticalAnaliz(List<TextBox> arr_textbox, double[] mainData, double[] predictionData)
        {
            int length = predictionData.Length;
            int n = mainData.Length;
            double saz = 0;
            double dispersion = 0;
            for (int i = 0; i < length; i++)
            {
                saz += predictionData[i];
            }
            saz = saz / length;

            for (int i = 0; i < length; i++)
            {
                dispersion += Math.Abs(Math.Pow((predictionData[i] - saz), 2));
            }
            dispersion = dispersion / (length - 1);

            double saop = 0; double sao = 0; double sko = 0; double so = 0; double sto = 0;
            for (int i = 0; i < n; i++)
            {
                try
                {
                    saop += (Math.Abs(mainData[i] - predictionData[i]) / mainData[i]) * 100;
                    sao += Math.Abs(mainData[i] - predictionData[i]);
                    sko += Math.Pow((mainData[i] - predictionData[i]), 2);
                    so += mainData[i] - predictionData[i];
                }
                catch
                {

                }
            }
            saop = saop / n;
            sao = sao / n;
            sko = sko / n;
            so = so / n;
            for (int i = 0; i < n; i++)
            {
                try
                {
                    sto += Math.Pow(predictionData[i] - so, 2);
                }
                catch { }
            }
            sto = sto / n;
            arr_textbox[0].Text = Math.Round(predictionData.Max(), 3).ToString();
            arr_textbox[1].Text = Math.Round(predictionData.Min(), 3).ToString();
            arr_textbox[2].Text = Math.Round(saz, 3).ToString();

            arr_textbox[4].Text = Math.Round(predictionData[length/2], 3).ToString();
            arr_textbox[5].Text = Math.Round(predictionData.Max() - predictionData.Min(), 3).ToString();
            arr_textbox[6].Text = Math.Round(dispersion, 3).ToString();
            arr_textbox[7].Text = Math.Round(Math.Sqrt(dispersion), 3).ToString();
            arr_textbox[8].Text = Math.Round(Math.Sqrt(dispersion) / Math.Sqrt(length), 3).ToString();
            arr_textbox[9].Text = Math.Round((Math.Sqrt(dispersion) / saz)*100, 3).ToString();
            arr_textbox[10].Text = Math.Round(saop, 3).ToString();
            arr_textbox[11].Text = Math.Round(sao, 3).ToString();
            arr_textbox[12].Text = Math.Round(Math.Sqrt(sko), 3).ToString();
            arr_textbox[13].Text = Math.Round(Math.Sqrt(so), 3).ToString();
            arr_textbox[14].Text = Math.Round(Math.Sqrt(sto), 3).ToString();
        }
    }
}
