using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MenuItem
{
    public int ItemId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public bool AvailabilityStatus { get; set; }
    public string MealType { get; set; }
    public string DietPreference { get; set; }
    public string SpiceLevel { get; set; }
    public string FoodPreference { get; set; }
    public bool SweetTooth { get; set; }
}
