using System;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

class Client
{
    public static TcpClient client;
    public static NetworkStream stream;
    public static void Main()
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

                byte[] buffer = new byte[1024];
                int byteCount = stream.Read(buffer, 0, buffer.Length);
                response = Encoding.ASCII.GetString(buffer, 0, byteCount);
                Console.WriteLine("Received from server: " + response);

                if (response.Contains("Login successful"))
                {
                    string[] responseParts = response.Split(' ');
                    userId = int.Parse(responseParts[6].TrimEnd('.'));
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
                        Console.WriteLine("Select action: \n1) View Menu Item \n2) Add Menu Item \n3) Update Menu Item \n4) Delete Menu Item");
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
                            default:
                                Console.WriteLine("Please enter a valid option.");
                                continue;
                        }
                        break;

                    case "Chef":
                        Console.WriteLine("Select action: \n1) Roll Out Me \n2) View Feedback \n3) View Monthly Report \n4) View Menu Items");
                        string chefAction = Console.ReadLine();
                        switch (chefAction)
                        {
                            case "1":
                                Console.WriteLine("Enter meal type (Breakfast, Lunch, Dinner):");
                                string mealType = Console.ReadLine();
                                Console.WriteLine("Enter items to recommend (comma-separated):");
                                string items = Console.ReadLine();
                                message += "recommenditem:" + mealType + ";" + items;
                                break;
                            case "2":
                                message += "viewfeedback:";
                                break;
                            case "3":
                                message += "generatereport:";
                                break;
                            case "4":
                                message += "viewmenuitem:";
                                break;
                            default:
                                Console.WriteLine("Please enter a valid option.");
                                continue;
                        }
                        break;

                    case "Employee":
                        Console.WriteLine("Select action:\n1) View Menu\n2) Give Feedback");
                        string employeeAction = Console.ReadLine();
                        switch (employeeAction)
                        {
                            case "1":
                                Console.WriteLine("Enter meal type (Breakfast, Lunch, Dinner):");
                                string viewMealType = Console.ReadLine();
                                message += "viewmenu:" + viewMealType;
                                break;
                            case "2":
                                message +=GiveFeedback(userId);
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

                byte[] responseBuffer = new byte[1024];
                int responseByteCount = stream.Read(responseBuffer, 0, responseBuffer.Length);
                response = Encoding.ASCII.GetString(responseBuffer, 0, responseByteCount);
                Console.WriteLine("Received from server: " + response);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
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
        return $"additem:{itemName};{price};{availabilityStatus};{mealType}";
    }

    public static string UpdateMenuItem()
    {
        Console.WriteLine("Enter item ID:");
        string updateItemId = Console.ReadLine();
        Console.WriteLine("Enter updated item name:");
        string updatedItemName = Console.ReadLine();
        Console.WriteLine("Enter updated price:");
        string updatedPrice = Console.ReadLine();
        Console.WriteLine("Enter updated availability status (true/false):");
        string updatedAvailabilityStatus = Console.ReadLine();
        Console.WriteLine("Enter updated meal type:");
        string updatedMealType = Console.ReadLine();
        return $"updateitem:{updateItemId};{updatedItemName};{updatedPrice};{updatedAvailabilityStatus};{updatedMealType}";
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
}