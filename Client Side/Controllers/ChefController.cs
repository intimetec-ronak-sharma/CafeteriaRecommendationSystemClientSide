using System;
public class ChefController : IUserController
{

    private string email;
    private int userId;
    private string message;

    public ChefController(string email, int userId)
    {
        this.email = email;
        this.userId = userId;

    }
    public void HandleActions()
    {
        while (true)
        {
            Console.WriteLine("Select action: \n1) View Recommended Item \n2) View Feedback \n3) View Employees Votes for Items \n4) View Menu Items \n5) Roll Out Items For the Next Day \n6) View Discard Menu Item List \n7) Logout ");
            string chefAction = Console.ReadLine();

            switch (chefAction)
            {
                case "1":
                    message = ViewRecommendedItem();
                    break;
                case "2":
                    message = "viewfeedback:";
                    break;
                case "3":
                    message = "viewemployeevote:";
                    break;
                case "4":
                    message = "viewmenuitem:";
                    break;
                case "5":
                    message = RollOutItems();
                    break;
                case "6":
                    message = "discardmenuitems:";
                    break;
                case "7":
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

    private string ViewRecommendedItem()
    {
        Console.WriteLine("Enter meal type (Breakfast, Lunch, Dinner):");
        string mealType = Console.ReadLine();
        Console.WriteLine("Enter number of items to recommend:");
        string size = Console.ReadLine();
        return $"recommenditem:{mealType};{size}";
    }

    private string RollOutItems()
    {
        Console.WriteLine("Enter Items Id For Recommend Items to Employee:");
        string itemID = Console.ReadLine();
        return $"rolloutmenu:{itemID}";
    }
}

