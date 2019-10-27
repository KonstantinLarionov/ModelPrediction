using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelPrediction.Model.Objects.ModelThreat
{
    /// <summary>
    /// Тип модели угроз
    /// </summary>
    public enum ModelType
    {
        Estimated, //Расчетная
        Alleged, // Предпологаемая
        Expert, //Экспертная
        Manual //Ручная
    }
    /// <summary>
    /// Модель угроз
    /// </summary>
    public class ModelThreat
    {
        public int Id { get; set; }
        public ModelType ModelType { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string NameModel { get; set; }
        public string NameOrganization { get; set; }
        public List<Threat> Threats { get; set; } = new List<Threat>(); // Список угроз
    }
}
