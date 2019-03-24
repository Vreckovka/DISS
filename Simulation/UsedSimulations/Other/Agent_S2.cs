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
