using System;
using System.Net.Sockets;
using System.Text;
public class Program
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
                IUserController controller = null;
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
                            HandleNotificationsAndFeedback(response);
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
                    switch (role)
                    {
                        case "Admin":
                            controller = new AdminController(email, userId);
                            break;
                        case "Chef":
                            controller = new ChefController(email, userId);
                            break;
                        case "Employee":
                            controller = new EmployeeController(email, userId);
                            break;
                        default:
                            Console.WriteLine("Invalid  role");
                            continue;
                    }
                    controller.HandleActions();
                    string message = email + ":" + controller.GetActionMessage();
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
    private static void HandleNotificationsAndFeedback(string response)
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
            Console.WriteLine("Do you want to provide feedback? (yes/no):");
            string feedbackOption = Console.ReadLine().Trim().ToLower();
            if (feedbackOption == "yes")
            {
                Console.WriteLine($"Please provide feedback for: ");
                Console.WriteLine("Q1. What didn’t you like about the food item?");
                string q1Response = Console.ReadLine();
                Console.WriteLine("Q2. How would you like the food item to taste?");
                string q2Response = Console.ReadLine();
                Console.WriteLine("Q3. Share your mom’s recipe (optional):");
                string q3Response = Console.ReadLine();
            }
            else if (feedbackOption == "no")
            {
                Console.WriteLine("You choose not to provide feedback.");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
            }
        }
    }
}