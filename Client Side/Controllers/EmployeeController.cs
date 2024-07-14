using System;
public class EmployeeController : IUserController
{
    private string email;
    private string message;
    private int userId;

    public EmployeeController(string email, int userId)
    {
        this.email = email;
        this.userId = userId;
    }
    public void HandleActions()
    {
        while (true)
        {
            Console.WriteLine("Select action:\n1) View Menu \n2) Give Feedback \n3) Give Vote \n4) Update your Profile \n5) Logout");
            string employeeAction = Console.ReadLine();

            switch (employeeAction)
            {
                case "1":
                    message = ViewMenu(userId);
                    break;
                case "2":
                    message = GiveFeedback(userId);
                    break;
                case "3":
                    message = GiveVote(userId);
                    break;
                case "4":
                    message = UpdateProfile(userId);
                    break;
                case "5":
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

    private string ViewMenu(int userId)
    {
        Console.WriteLine("Enter meal type (Breakfast, Lunch, Dinner):");
        string mealType = Console.ReadLine();
        return $"viewmenu:{userId};{mealType}";
    }

    private string GiveFeedback(int userId)
    {
        Console.WriteLine("Enter item ID:");
        string feedbackItemId = Console.ReadLine();
        Console.WriteLine("Enter comment:");
        string comment = Console.ReadLine();
        Console.WriteLine("Enter rating (1-5):");
        string rating = Console.ReadLine();
        if (int.TryParse(feedbackItemId, out int itemId) && int.TryParse(rating, out int ratingValue))
        {
            return $"givefeedback:{userId};{itemId};{comment};{ratingValue}";
        }
        else
        {
            Console.WriteLine("Invalid input format. Item ID and Rating must be integers.");
            return "";
        }
    }

    private string GiveVote(int userId)
    {
        Console.WriteLine("Enter the item id for which you want to vote an item");
        string itemId = Console.ReadLine();
        return $"voteitem:{userId};{itemId}";
    }

    private string UpdateProfile(int userId)
    {
        Console.WriteLine("Please answer these questions to know your preferences");

        Console.WriteLine("1) Please select one- \n1) Vegetarian \n2) Non Vegetarian \n3) Eggetarian");
        string dietChoice = Console.ReadLine();
        string dietPreference;
        switch (dietChoice)
        {
            case "1":
                dietPreference = "Vegetarian";
                break;
            case "2":
                dietPreference = "Non Vegetarian";
                break;
            case "3":
                dietPreference = "Eggetarian";
                break;
            default:
                dietPreference = "Unknown";
                break;
        };

        Console.WriteLine("2) Please select your spice level \n1) High \n2) Medium \n3) Low");
        string spiceLevelChoice = Console.ReadLine();
        string spicePreference;
        switch (spiceLevelChoice)
        {
            case "1":
                spicePreference = "High";
                break;
            case "2":
                spicePreference = "Medium";
                break;
            case "3":
                spicePreference = "Low";
                break;
            default:
                spicePreference = "Unknown";
                break;
        }

        Console.WriteLine("3) What do you prefer most? \n1) North Indian \n2) South Indian \n3) Other");
        string foodChoice = Console.ReadLine();
        string foodPreference;
        switch (foodChoice)
        {
            case "1":
                foodPreference = "North Indian";
                break;
            case "2":
                foodPreference = "South Indian";
                break;
            case "3":
                foodPreference = "Other";
                break;
            default:
                foodPreference = "Unknown";
                break;
        }

        Console.WriteLine("4) Do you have a sweet tooth? \n1) Yes \n2) No");
        string sweetToothChoice = Console.ReadLine();
        string sweetToothPreference;
        switch (sweetToothChoice)
        {
            case "1":
                sweetToothPreference = "Yes";
                break;
            case "2":
                sweetToothPreference = "No";
                break;
            default:
                sweetToothPreference = "Unknown";
                break;
        }
        return $"updateprofile:{userId};{dietPreference};{spicePreference};{foodPreference};{sweetToothPreference}";
} 
}
