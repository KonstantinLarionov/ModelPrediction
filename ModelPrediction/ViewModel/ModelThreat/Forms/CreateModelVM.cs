using ModelPrediction.Model.Entity;
using ModelPrediction.Model.Objects.ModelThreat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelPrediction.ViewModel.ModelThreat.Forms
{
    class CreateModelVM
    {
        private ModelThreatContext db { get; set; }
        public CreateModelVM()
        {
            db = new ModelThreatContext();
        }

        public void CreateNewModel(string nameModel, string nameOrganization, string typeModel)
        {
            ModelType modelType = ModelType.Manual;
            switch (typeModel)
            {
                case "Предполагаемая":
                    modelType = ModelType.Alleged;
                    break;
                case "Экспертная":
                    modelType = ModelType.Expert;
                    break;
                case "Расчетная":
                    modelType = ModelType.Estimated;
                    break;
            }

            db.Models.Add(new Model.Objects.ModelThreat.ModelThreat() {
                NameModel = nameModel,
                NameOrganization = nameOrganization,
                ModelType = modelType,
                DateCreated = DateTime.Now
            });
            db.SaveChanges();
        }
    }
}
