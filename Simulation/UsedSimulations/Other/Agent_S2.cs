using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;

namespace Simulations.UsedSimulations.Other
{
    public class Agent_S2 : Agent
    {
        public Table Table { get; set; }
        public int AgentCount { get; set; }
        public double ArrivalTime { get; set; }
        public double StartOrder { get; set; }
        public double EndOrder { get; set; }
        public double DeliveredFood { get; set; }
        public double EndEatingFood { get; set; }
        public double StartPaying { get; set; }

        public int FoodLeft { get; set; }

        public Agent_S2(int agentCount)
        {
            AgentCount = agentCount;
        }

        public override string ToString()
        {
            return $"{ID} ({AgentCount})";
        }
    }
}
