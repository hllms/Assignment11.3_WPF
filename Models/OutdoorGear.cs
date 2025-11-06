using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment11._3_WPF.Models
{
    public enum SeasonType { Dry, Wet, Hot, Cold, All }
    public enum ConditionType { Good, Fair, Poor, Nonfunctional }
    public enum GearType
    {
        Shelter, Sleeping, Clothing, Food, Water, Pack, Tools, Medical,
        Navigation, Lighting, Fire, Hygiene
    }

    public class OutdoorGear
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public GearType Type { get; set; }
        public SeasonType Season { get; set; }
        public ConditionType Condition { get; set; }
        public bool IsDurable { get; set; }
        public decimal Price { get; set; }
        public double WeightInPounds { get; set; }
        public string Notes { get; set; } = "";
        public int Quantity { get; set; } = 1;
        public int PurchaseYear { get; set; }
        public string? SerialNumber { get; set; }
    }
}

