using System;

public class Program
{
    public static void Main(string[] args)
    {
        TaskManager taskManager = new TaskManager();

        while (true)
        {
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. List Tasks");
            Console.WriteLine("3. Remove Task");
            Console.WriteLine("4. Exit");
            Console.Write("Choose an option: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.Write("Enter task description: ");
                    string description = Console.ReadLine();
                    taskManager.AddTask(description);
                    break;
                case "2":
                    taskManager.ListTasks();
                    break;
                case "3":
                    Console.Write("Enter task ID to remove: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        taskManager.RemoveTask(id);
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID.");
                    }
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
}
