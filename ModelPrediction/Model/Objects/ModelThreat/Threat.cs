using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelPrediction.Model.Objects.ModelThreat
{
    /// <summary>
    /// Тип актуальности
    /// </summary>
    public enum Relevance
    {
        Relevant,
        NotRelevant
    }
    /// <summary>
    /// Угроза
    /// </summary>
    public class Threat
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public double Probability { get; set; }
        public double Damage { get; set; }
        public Relevance Relevance { get; set; }
        public List<StatisticData> Data { get; set; }
    }
}
