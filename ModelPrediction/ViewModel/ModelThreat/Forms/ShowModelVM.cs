using ModelPrediction.Model.Entity;
using ModelPrediction.Model.Objects;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ModelPrediction.ViewModel.ModelThreat.Forms
{
    class ShowModelVM
    {
        private ModelThreatContext db { get; set; }
        public ShowModelVM()
        {
            db = new ModelThreatContext();
        }
        public void ShowAllModel(DataGrid dataGrid)
        {
            dataGrid.ItemsSource = db.Models.ToList();
        }

        public void ShowModel(int id, TextBox[] textBoxes)
        {
            var model = db.Models.Include("Threats").Where(m => m.Id == id).FirstOrDefault();
            foreach (var item in textBoxes)
            {
                switch (item.Name)
                {
                    case "nameModel":
                        item.Text = model.NameModel;
                        break;
                    case "nameOrg":
                        item.Text = model.NameOrganization;
                        break;
                    case "dateCr":
                        item.Text = model.DateCreated.ToString();
                        break;
                    case "countThreat":
                        if (model.Threats == null)
                        {
                            item.Text = "0";
                        }
                        else
                        {
                            item.Text = model.Threats.Count.ToString();
                        }
                        break;
                }

            }
        }
    }
}
