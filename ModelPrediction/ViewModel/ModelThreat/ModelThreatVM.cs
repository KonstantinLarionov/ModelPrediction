
using ModelPrediction.Model.Entity;
using ModelPrediction.Model.Objects.ModelThreat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ModelPrediction.ViewModel.ModelThreat
{
    class ModelThreatVM
    {
        private static ModelThreatContext db { get; set; }
        private static Model.Objects.ModelThreat.ModelThreat ModelThreat { get; set; }
        private static DataGrid DataTableThreat { get; set; }
        private static Label ModelNameLabel { get; set; }
        private static Label ModelCountThreatLabel { get; set; }
        private static Label ThreatNameLabel { get; set; }
        private static Label ThreatNumberLabel { get; set; }
        private static Label ThreatCountDataLabel { get; set; }
        private static Label ModelDateLabel { get; set; }
        public static Threat MainThreat { get; set; }
        public ModelThreatVM(DataGrid dataGrid)
        {
            db = new ModelThreatContext();
            DataTableThreat = dataGrid;

        }
        public static void SetLabels( Label name, Label count, Label date, Label nameThreat, Label number, Label countData)
        {
            ModelNameLabel = name;
            ModelCountThreatLabel = count;
            ModelDateLabel = date;
            ThreatNameLabel = nameThreat;
            ThreatNumberLabel = number;
            ThreatCountDataLabel = countData;
        }
        public static void RefreshTableThreat()
        {
            var modelDB = db.Models.Include("Threats").Where(m => m.Id == ModelThreat.Id).FirstOrDefault();
            if (modelDB != null)
            {
                if (modelDB.Threats != null)
                {
                    DataTableThreat.ItemsSource = null;
                    DataTableThreat.ItemsSource = modelDB.Threats;
                }
                else
                {
                    MessageBox.Show("Отсутствуют угрозы в модели! Необходимо добавить угрозы в модель.");
                }
            }
        }

        public static void SetInfoModel(Model.Objects.ModelThreat.ModelThreat modelThreat)
        {
            ModelNameLabel.Content = modelThreat.NameModel;
            ModelCountThreatLabel.Content = modelThreat.Threats.Count.ToString();
            ModelDateLabel.Content = modelThreat.DateCreated.Date.ToString();
        }
        public static void SetModelThreat(Model.Objects.ModelThreat.ModelThreat modelThreat)
        {
            ModelThreat = modelThreat;
            SetInfoModel(modelThreat);
            RefreshTableThreat();
        }

        public static void SetInfoThreat(Model.Objects.ModelThreat.Threat threat)
        {
            ThreatNameLabel.Content = threat.Name;
            ThreatNumberLabel.Content = threat.Number.ToString();
            var threatDB = db.Threats.Include("Data").Where(t => t.Id == threat.Id).FirstOrDefault();
            MainThreat = threatDB;
            ThreatCountDataLabel.Content = threatDB.Data.Count.ToString();
        }

        public static Model.Objects.ModelThreat.ModelThreat GetModel()
        {
            return ModelThreat;
        }
    }
}
