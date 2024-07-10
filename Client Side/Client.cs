/*using System;
using System.Net.Sockets;
using System.Text;

class Client
{
    public static TcpClient client;
    public static NetworkStream stream;
    public static void Main()
    {
        while (true)
        {
            try
            {
                client = new TcpClient("127.0.0.1", 5001);
                stream = client.GetStream();
                Console.WriteLine("Connected to server.");

                string email;
                string response;
                int userId = 0;

                while (true)
                {
                    Console.WriteLine("Enter your email:");
                    email = Console.ReadLine();

                    string loginMessage = email + ":";
                    byte[] loginData = Encoding.ASCII.GetBytes(loginMessage);
                    stream.Write(loginData, 0, loginData.Length);

                    byte[] buffer = new byte[8192];
                    int byteCount = stream.Read(buffer, 0, buffer.Length);
                    response = Encoding.ASCII.GetString(buffer, 0, byteCount);
                    Console.WriteLine($"{response.Split('.')[0].TrimEnd()}");

                    if (response.Contains("Login successful"))
                    {
                        int userIdIndex = response.IndexOf("UserId") + "UserId".Length;
                        string userIdString = response.Substring(userIdIndex).Split('.')[0].Trim();
                        if (int.TryParse(userIdString, out userId))
                        {
                            if (response.Contains("Notifications:"))
                            {
                                Console.WriteLine("Notifications:");
                                string[] notificationParts = response.Split(new string[] { "Notifications:" }, StringSplitOptions.None);
                                if (notificationParts.Length > 1)
                                {
                                    string notificationsText = notificationParts[1].Trim();
                                    Console.WriteLine(notificationsText);
                                }
                            }
                            if (response.Contains("Please provide your feedback"))
                            {
                                Console.WriteLine($"Please provide feedback for: ");
                                Console.WriteLine("Q1. What didn’t you like about the food item?");
                                string q1Response = Console.ReadLine();
                                Console.WriteLine("Q2. How would you like the food item to taste?");
                                string q2Response = Console.ReadLine();
                                Console.WriteLine("Q3. Share your mom’s recipe (optional):");
                                string q3Response = Console.ReadLine();
                            }
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Login failed. Please check your email and try again.");
                    }
                }

                string role = response.Split(' ')[3].Trim('.');

                while (true)
                {
                    string message = email + ":";
                    switch (role)
                    {
                        case "Admin":
                            Console.WriteLine("Select action: \n1) View Menu Item \n2) Add Menu Item \n3) Update Menu Item \n4) Delete Menu Item \n5) View Discard Menu Item List \n6) Logout");
                            string adminAction = Console.ReadLine();
                            switch (adminAction)
                            {
                                case "1":
                                    message += "viewitems:";
                                    break;
                                case "2":
                                    message += AddMenuItem();
                                    break;
                                case "3":
                                    message += UpdateMenuItem();
                                    break;
                                case "4":
                                    message += DeleteMenuItem();
                                    break;
                                case "5":
                                    message += "discardmenuitems:";
                                    break;
                                case "6":
                                    message += "logout:";
                                    break;
                                default:
                                    Console.WriteLine("Please enter a valid option.");
                                    continue;
                            }
                            break;

                        case "Chef":
                            Console.WriteLine("Select action: \n1) View Recommended Item \n2) View Feedback \n3) View Employees Votes for Items \n4) View Menu Items \n5) Roll Out Items For the Next Day \n6) View Discard Menu Item List \n7) Logout ");
                            string chefAction = Console.ReadLine();
                            switch (chefAction)
                            {
                                case "1":
                                    Console.WriteLine("Enter meal type (Breakfast, Lunch, Dinner):");
                                    string mealType = Console.ReadLine();
                                    Console.WriteLine("Enter number of items to recommend:");
                                    string size = Console.ReadLine();
                                    message += $"recommenditem:{mealType};{size}";
                                    break;
                                case "2":
                                    message += "viewfeedback:";
                                    break;
                                case "3":
                                    message += "viewemployeevote:";
                                    break;
                                case "4":
                                    message += "viewmenuitem:";
                                    break;
                                case "5":
                                    Console.WriteLine("Enter Items Id For Recommend Items to Employee:");
                                    string itemID = Console.ReadLine();
                                    message += $"rolloutmenu:{itemID}";
                                    break;
                                case "6":
                                    message += "discardmenuitems:";
                                    break;
                                case "7":
                                    message += "logout:";
                                    break;
                                default:
                                    Console.WriteLine("Please enter a valid option.");
                                    continue;
                            }
                            break;

                        case "Employee":
                            Console.WriteLine("Select action:\n1) View Menu \n2) Give Feedback \n3) Give Vote \n4) Update your Profile \n5) Logout");
                            string employeeAction = Console.ReadLine();
                            switch (employeeAction)
                            {
                                case "1":
                                    Console.WriteLine("Enter meal type (Breakfast, Lunch, Dinner):");
                                    string viewMealType = Console.ReadLine();
                                    message += "viewmenu:" + viewMealType;
                                    break;
                                case "2":
                                    message += GiveFeedback(userId);
                                    break;
                                case "3":
                                    Console.WriteLine("Enter the item id for which you want to vote an item");
                                    string itemId = Console.ReadLine();
                                    message += "voteitem:" + itemId;
                                    break;
                                case "4":
                                    message += UpdateProfile(userId);
                                    break;
                                case "5":
                                    message += "logout:";
                                    break;
                                default:
                                    Console.WriteLine("Please enter a valid option.");
                                    continue;
                            }
                            break;

                        default:
                            Console.WriteLine("Unknown role");
                            continue;
                    }

                    byte[] data = Encoding.ASCII.GetBytes(message);
                    stream.Write(data, 0, data.Length);

                    byte[] responseBuffer = new byte[8192];
                    int responseByteCount = stream.Read(responseBuffer, 0, responseBuffer.Length);
                    response = Encoding.ASCII.GetString(responseBuffer, 0, responseByteCount);
                    Console.WriteLine(response);

                    if (response.Contains("Discard Menu Item List"))
                    {
                        while (true)
                        {
                            Console.WriteLine("Select one action from the following action: \n1) Remove the Food Item from Menu List \n2) Get Detailed Feedback ");
                            string discardAction = Console.ReadLine();
                            switch (discardAction)
                            {
                                case "1":
                                    Console.WriteLine("Enter the food item name to remove:");
                                    string removeItemName = Console.ReadLine();
                                    message = email + ":removefooditem:" + removeItemName;
                                    break;
                                case "2":
                                    Console.WriteLine("Enter the food item name to get detailed feedback:");
                                    string feedbackItemName = Console.ReadLine();
                                    message = email + ":insertfeedbacknotification:" + feedbackItemName;
                                    break;
                                default:
                                    Console.WriteLine("Please enter a valid option.");
                                    continue;
                            }

                            data = Encoding.ASCII.GetBytes(message);
                            stream.Write(data, 0, data.Length);

                            responseBuffer = new byte[8192];
                            responseByteCount = stream.Read(responseBuffer, 0, responseBuffer.Length);
                            response = Encoding.ASCII.GetString(responseBuffer, 0, responseByteCount);
                            Console.WriteLine(response);
                            break;
                        }
                    }

                    if (response.Contains("Logout successful"))
                    {
                        Console.WriteLine("You have been logged out.");
                        break;
                    }

                }
                stream.Close();
                client.Close();
            }

            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
    }
    public static string AddMenuItem()
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

        Console.WriteLine("Do you have a sweet tooth? (Yes/No):");
        string sweetTooth = Console.ReadLine();

        return $"additem:{itemName};{price};{availabilityStatus};{mealType};{dietPreference};{spiceLevel};{FoodPreference};{sweetTooth}";
    }


    public static string UpdateMenuItem()
    {
        Console.WriteLine("Enter item ID:");
        string updateItemId = Console.ReadLine();
        Console.WriteLine("Enter updated price:");
        string updatedPrice = Console.ReadLine();
        Console.WriteLine("Enter updated availability status (true/false):");
        string updatedAvailabilityStatus = Console.ReadLine();

        return $"updateitem:{updateItemId};{updatedPrice};{updatedAvailabilityStatus}";
    }

    public static string DeleteMenuItem()
    {
        Console.WriteLine("Enter item ID:");
        string deleteItemId = Console.ReadLine();
        return $"deleteitem:{deleteItemId}";
    }

    public static string GiveFeedback(int userId)
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
    public static string UpdateProfile(int userId)
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
*/