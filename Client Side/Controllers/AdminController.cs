using System;
public class AdminController : IUserController
{

    private string email;
    private string message;
    private int userId;

    public AdminController(string email, int userId)
    {
        this.email = email;
        this.userId = userId;
    }
    
    public void HandleActions()
    {
        while (true)
        {
            Console.WriteLine("Select action: \n1) View Menu Item \n2) Add Menu Item \n3) Update Menu Item \n4) Delete Menu Item \n5) View Discard Menu Item List \n6) Logout");
            string adminAction = Console.ReadLine();

            switch (adminAction)
            {
                case "1":
                    message = "viewitems:";
                    break;
                case "2":
                    message = AddMenuItem();
                    break;
                case "3":
                    message = UpdateMenuItem();
                    break;
                case "4":
                    message = DeleteMenuItem();
                    break;
                case "5":
                    message = "discardmenuitems:";
                    break;
                case "6":
                    message = "logout:";
                    return;
                default:
                    Console.WriteLine("Please enter a valid option.");
                    continue;
            }
            break;
        }
    }

    public string GetActionMessage()
    {
        return message;
    }

    private string AddMenuItem()
    {
        Console.WriteLine("Enter item name:");
        string itemName = Console.ReadLine();
        Console.WriteLine("Enter price:");
        string price = Console.ReadLine();
        Console.WriteLine("Enter availability status (true/false):");
        string availabilityStatus = Console.ReadLine();
        Console.WriteLine("Enter meal type:");
        string mealType = Console.ReadLine();
        Console.WriteLine("Enter diet preference (Vegetarian/Non Vegetarian/Eggetarian):");
        string dietPreference = Console.ReadLine();
        Console.WriteLine("Enter spice level (High/Medium/Low):");
        string spiceLevel = Console.ReadLine();
        Console.WriteLine("Enter Food preference (North Indian/South Indian/Other):");
        string FoodPreference = Console.ReadLine();
        Console.WriteLine("Is the food sweet? (Yes/No):");
        string sweetTooth = Console.ReadLine();

        return $"additem:{itemName};{price};{availabilityStatus};{mealType};{dietPreference};{spiceLevel};{FoodPreference};{sweetTooth}";
    }

    private string UpdateMenuItem()
    {
        Console.WriteLine("Enter item ID:");
        string updateItemId = Console.ReadLine();
        Console.WriteLine("Enter updated price:");
        string updatedPrice = Console.ReadLine();
        Console.WriteLine("Enter updated availability status (true/false):");
        string updatedAvailabilityStatus = Console.ReadLine();

        return $"updateitem:{updateItemId};{updatedPrice};{updatedAvailabilityStatus}";
    }

    private string DeleteMenuItem()
    {
        Console.WriteLine("Enter item ID:");
        string deleteItemId = Console.ReadLine();

        return $"deleteitem:{deleteItemId}";
    }
}
