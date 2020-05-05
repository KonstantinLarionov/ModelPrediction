using Microsoft.Office.Interop.Excel;
using ModelPrediction.Model.Entity;
using ModelPrediction.Model.Objects.ModelThreat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Excel = Microsoft.Office.Interop.Excel;

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
            var ext = Path.GetExtension(PathToData);
            if (ext == ".txt")
            {
                try
                {
                    List<string> fill = new List<string>();
                    using (StreamReader sc = new StreamReader(PathToData))
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

                    List<StatisticData> statistics = new List<StatisticData>();

                    for (int i = 0; i < data.Length; i++)
                    {
                        statistics.Add(new StatisticData()
                        {
                            Damage = data[i],
                            X = i
                        });
                    }
                    return statistics;
                }
                catch
                {
                    MessageBox.Show("Не получилось собрать данные. Проверьте правильность составления документа! см. Руководство");
                    return null;
                }
            }
            else if (ext == ".xls" || ext == ".xlsx")
            {
                Microsoft.Office.Interop.Excel.Application xlsApp = null;
                try
                {
                    xlsApp = new Microsoft.Office.Interop.Excel.Application();
                }
                catch
                {
                    MessageBox.Show("Возникла проблема при добавлении статистических данных через { API Excel }. Возможно на компьютере не установлен Excel.");
                    return null;
                }

                try
                {
                    Workbook wb = xlsApp.Workbooks.Open(PathToData,
                        0, true, 5, "", "", true, XlPlatform.xlWindows, "\t", false, false, 0, true);
                    Sheets sheets = wb.Worksheets;
                    Worksheet ws = (Worksheet)sheets.get_Item(1);

                    Range firstColumn = ws.UsedRange.Columns[2];
                    System.Array myvalues = (System.Array)firstColumn.Cells.Value;
                    List<StatisticData> statistics = new List<StatisticData>();
                    int count = 0;
                    foreach (var item in myvalues)
                    {
                        try
                        {
                            statistics.Add(new StatisticData()
                            {
                                Damage = Convert.ToInt32(item),
                                X = count
                            });
                            count++;
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    
                    return statistics;
                }
                catch(Exception e)
                {
                    MessageBox.Show("Не получилось собрать данные. Проверьте правильность составления документа! см. Руководство");
                    return null;
                }
            }
            else
            {
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
