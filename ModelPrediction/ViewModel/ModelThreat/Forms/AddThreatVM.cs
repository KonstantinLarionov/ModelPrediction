using Microsoft.Office.Interop.Excel;
using ModelPrediction.Model.Entity;
using ModelPrediction.Model.Objects.ModelThreat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ModelPrediction.ViewModel.ModelThreat.Forms
{
    class AddThreatVM
    {
        private ModelThreatContext db { get; set; }
        private Model.Objects.ModelThreat.ModelThreat ModelThreat { get; set; }
        private string PathToData { get; set; }
        public AddThreatVM()
        {
            db = new ModelThreatContext();
            ModelThreat = ModelThreatVM.GetModel();
        }

        public void GetPath(string path)
        {
            PathToData = path;
        }

        private List<StatisticData> SetStatisticData()
        {
            Microsoft.Office.Interop.Excel.Application xlsApp = null;
            try
            {
                xlsApp = new Microsoft.Office.Interop.Excel.Application();
            }
            catch
            {
                MessageBox.Show("Excel не может быть запущен. Возможно Excel не установлен на данном компьютере.");
                return null ;
            }
            try
            {
                Workbook wb = xlsApp.Workbooks.Open(PathToData,
                    0, true, 5, "", "", true, XlPlatform.xlWindows, "\t", false, false, 0, true);
                Sheets sheets = wb.Worksheets;
                Worksheet ws = (Worksheet)sheets.get_Item(1);

                Range secondColumn = ws.UsedRange.Columns[2];
                System.Array myvalues = (System.Array)secondColumn.Cells.Value;
                double[] data = myvalues.OfType<object>().Select(o => Double(o.ToString())).ToArray();
                Range firstColumn = ws.UsedRange.Columns[1];
                System.Array myTimes = (System.Array)firstColumn.Cells.Value;
                DateTime[] dateTime = null;
                int[] x = null;
                try
                {
                    dateTime = myTimes.OfType<object>().Select(o => Convert.ToDateTime(o.ToString())).ToArray();
                }
                catch
                {
                    x = myTimes.OfType<object>().Select(o => Convert.ToInt32(o.ToString())).ToArray();
                }

                List<StatisticData> statistics = new List<StatisticData>();
                if (dateTime != null)
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        statistics.Add(new StatisticData()
                        {
                            Damage = data[i],
                            Time = dateTime[i]
                        });
                    }
                }
                else
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        statistics.Add(new StatisticData()
                        {
                            Damage = data[i],
                            X = x[i]
                        });
                    }
                }
                return statistics;
            }
            catch
            {
                MessageBox.Show("Не получилось собрать данные. Проверьте правильность составления документа! см. Руководство");
                return null;
            }
        }

        public void SetThread(System.Windows.Controls.TextBox[] textBoxes, ComboBox[] comboBox)
        {
            Threat threat = new Threat();
            foreach (var item in textBoxes)
            {
                switch (item.Name)
                {
                    case "numberFSTEK":
                        threat.Number = item.Text;
                        break;
                    case "name":
                        threat.Name = item.Text;
                        break;
                    case "verReal":
                        threat.Probability = Double(item.Text);
                        break;
                    case "uwerb":
                        threat.Damage = Double(item.Text);
                        break;
                    default:
                        break;
                }
            }
            foreach (var item in comboBox)
            {
                switch (item.Name)
                {
                    case "actual":
                        if (item.Text == "Актуальная")
                        {
                            threat.Relevance = Relevance.Актуальная;
                        }
                        else
                        {
                            threat.Relevance = Relevance.НеАктуальная;
                        }
                        break;
                    default:
                        break;
                }
            }
            List<StatisticData> statisticDatas = new List<StatisticData>();

            if (PathToData != null)
            {
                statisticDatas = SetStatisticData();
                //foreach (var statisticData in statisticDatas)
                //{
                //    db.StatisticData.Add(statisticData);
                //}
                threat.Data = statisticDatas;
            }
            

            db.Threats.Add(threat);
            db.SaveChanges();
            var modelDB = db.Models.Where(m => m.Id == ModelThreat.Id).FirstOrDefault();
            modelDB.Threats.Add(threat);
            db.SaveChanges();
        }
        public double Double(string str)
        {
            if (str == null)
            {
                return 0;
            }
            return Convert.ToDouble(str.Replace(".",","));
        }
    }
}
