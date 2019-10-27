using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelPrediction.Model.Objects.ModelThreat
{
    /// <summary>
    ///  Статистические данные угрозы
    /// </summary>
    public class StatisticData
    {
        public int Id { get; set; }
        public string IdThreat { get; set; }
        public int X { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
        public double Y { get; set; }
        public double Probability { get; set; }
        public double Damage { get; set; }
        public Relevance Relevance { get; set; }

    }
}
