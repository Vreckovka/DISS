using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulations.Simulations.EventSimulation;

namespace Simulations.UsedSimulations.Other
{
    public enum FoodType
    {
        CezarSalad,
        PenneSalad,
        WholeWheatSpaghetti,
        RichSalad
    }
    public class Food
    {
        public FoodType FoodType { get; set; }
        public TimeSpan Time { get; set; }
        public Agent Agent { get; set; }
        public Waiter Waiter { get; set; }
        public Cook Cook { get; set; }
        public bool LastFood { get; set; }
    }
}
