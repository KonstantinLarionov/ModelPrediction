using ModelPrediction.Model.Objects.ModelThreat;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelPrediction.Model.Entity
{
    class ModelThreatContext : DbContext
    {
        public ModelThreatContext() : base("PredictionDB") { }
        public DbSet<ModelPrediction.Model.Objects.ModelThreat.ModelThreat> Models { get; set; }
        public DbSet<Threat> Threats { get; set; }
        public DbSet<StatisticData> StatisticData { get; set; }
    }
}
