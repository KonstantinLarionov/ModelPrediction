using AForge.Math;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ModelPrediction.Model
{
    class MainData
    {
        private double[] main_data { get; set; }

        public MainData()
        { }

        public MainData(double[] d)
        {
            SetData(d);
        }

        public void SetData(double[] d)
        {
            main_data = d; 
        }

        public double[] GetData()
        {
            return main_data;
        }


        /// <summary>
        /// Загрузка с файла *.txt
        /// </summary>
        /// <param name="p">Path к файлу txt</param>
        /// <returns></returns>
        public void Load_TXT(string p)
        {

            try
            {
                List<string> fill = new List<string>();
                using (StreamReader sc = new StreamReader(p))
                {
                    sc.ReadLine();
                    while (!sc.EndOfStream)
                    {
                        fill.Add(sc.ReadLine());
                    }
                }

                List<string> info2 = new List<string>();
                for (int j = 0; j < fill.Count; j++)
                {
                    info2.Add(fill[j].Split('/')[1].Replace(".", ","));
                }

                double[] data = new double[info2.Count];

                for (int i = 0; i < info2.Count; i++)
                {
                    try
                    {
                        data[i] += double.Parse(info2[i]);
                    }
                    catch
                    {
                        data[i] += 0;
                    }
                }
                SetData(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не получилось собрать данные. Проверьте правильность составления документа! см. Руководство");
            }

        }

        public void Load_XLS(string p)
        {
            Microsoft.Office.Interop.Excel.Application xlsApp = null;
            try
            {
                xlsApp = new Microsoft.Office.Interop.Excel.Application();
            }
            catch
            {
                MessageBox.Show("Excel не может быть запущен. Возможно Excel не установлен на данном компьютере.");
                return;
            }

            //Displays Excel so you can see what is happening
            //xlsApp.Visible = true;
            try
            {
                Workbook wb = xlsApp.Workbooks.Open(p,
                    0, true, 5, "", "", true, XlPlatform.xlWindows, "\t", false, false, 0, true);
                Sheets sheets = wb.Worksheets;
                Worksheet ws = (Worksheet)sheets.get_Item(1);

                Range firstColumn = ws.UsedRange.Columns[1];
                System.Array myvalues = (System.Array)firstColumn.Cells.Value;
                double[] data = myvalues.OfType<object>().Select(o => Convert.ToDouble(o.ToString().Replace(".", ","))).ToArray();
                SetData(data);
            }
            catch
            {
                MessageBox.Show("Не получилось собрать данные. Проверьте правильность составления документа! см. Руководство");
                return;
            }
        }

        public static AForge.Math.Complex[] FFTClear(AForge.Math.Complex[] inpute)
        {

            AForge.Math.Complex[] outpute = inpute;
            AForge.Math.FourierTransform.FFT(outpute, FourierTransform.Direction.Forward);
            return outpute;
        }
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

        public static double[] FFTBackward(AForge.Math.Complex[] inpute)
        {
            AForge.Math.Complex[] outpute = inpute;
            AForge.Math.FourierTransform.FFT(outpute, FourierTransform.Direction.Backward);
            double[] arr_out = new double[outpute.Length];
            for (int i = 0; i < outpute.Length; i++)
            {
                arr_out[i] = outpute[i].Re * outpute[i].Im;
            }
            return arr_out;
        }


    }
}
