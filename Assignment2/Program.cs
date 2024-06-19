

using System;
using System.Linq.Expressions;

class Program
{
    static void Main(string[] args)
    {
        using (var dbContext = new AppDbContext())
        {
    
            dbContext.Database.EnsureCreated();

    
            IRepository<User> userRepository = new Repository<User>(dbContext);

            // CRUD operations example with user input
            while (true)
            {
                Console.WriteLine("\nChoose an operation:");
                Console.WriteLine("1. List all users");
                Console.WriteLine("2. Add a new user");
                Console.WriteLine("3. Update a user");
                Console.WriteLine("4. Delete a user");
                Console.WriteLine("5. Exit");

                Console.Write("\nEnter your choice: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListAllUsers(userRepository);
                        break;
                    case "2":
                        Console.Write("Enter username: ");
                        var username = Console.ReadLine();
                        Console.Write("Enter email: ");
                        var email = Console.ReadLine();
                        var newUser = new User { Username = username, Email = email };
                        AddUser(userRepository, newUser);
                        break;
                    case "3":
                        Console.Write("Enter user ID to update: ");
                        if (int.TryParse(Console.ReadLine(), out int updateUserId))
                        {
                            Console.Write("Enter new username: ");
                            var newUsername = Console.ReadLine();
                            UpdateUser(userRepository, updateUserId, newUsername);
                        }
                        else
                        {
                            Console.WriteLine("Invalid user ID.");
                        }
                        break;
                    case "4":
                        Console.Write("Enter user ID to delete: ");
                        if (int.TryParse(Console.ReadLine(), out int deleteUserId))
                        {
                            DeleteUser(userRepository, deleteUserId);
                        }
                        else
                        {
                            Console.WriteLine("Invalid user ID.");
                        }
                        break;
                    case "5":
                        Console.WriteLine("Exiting application...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number from 1 to 5.");
                        break;
                }
            }
        }
    }

    static void ListAllUsers(IRepository<User> userRepository)
    {
        var users = userRepository.GetAll();
        Console.WriteLine("\nCurrent users:");
        foreach (var user in users)
        {
            Console.WriteLine($"User(Id={user.Id}, Username='{user.Username}', Email='{user.Email}')");
        }
    }

    static void AddUser(IRepository<User> userRepository, User user)
    {
        userRepository.Add(user);
        Console.WriteLine($"\nUser added successfully: {user}");
    }

    static void UpdateUser(IRepository<User> userRepository, int userId, string newUsername)
    {
        var userToUpdate = userRepository.GetById(userId);
        if (userToUpdate != null)
        {
            userToUpdate.Username = newUsername;
            userRepository.Update(userToUpdate);
            Console.WriteLine($"\nUser updated successfully: {userToUpdate}");
        }
        else
        {
            Console.WriteLine($"User with id={userId} not found.");
        }
    }

    static void DeleteUser(IRepository<User> userRepository, int userId)
    {
        var userToDelete = userRepository.GetById(userId);
        if (userToDelete != null)
        {
            userRepository.Delete(userToDelete);
            Console.WriteLine($"\nUser with ID {userId} deleted successfully.");
        }
        else
        {
            Console.WriteLine($"User with id={userId} not found.");
        }
    }
}
