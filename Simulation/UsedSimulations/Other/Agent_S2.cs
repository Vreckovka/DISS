using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;

namespace Simulations.UsedSimulations.Other
{
    class Agent_S2 : Agent
    {
        public Table Table { get; set; }
        public int AgentCount { get; set; }
        public TimeSpan WaitingTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public TimeSpan StartOrder { get; set; }
        public TimeSpan EndOrder { get; set; }
        public TimeSpan DeliveredFood { get; set; }
        public TimeSpan EndEatingFood { get; set; }
        public TimeSpan StartPaying { get; set; }

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
