using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        TaskManager taskManager = new TaskManager();

        while (true)
        {
            ShowMenu();
            string input = Console.ReadLine();

            Console.Clear(); // Tyhjennetään konsoli ennen kuin toiminto suoritetaan

            switch (input)
            {
                case "1":
                    Console.Write("Enter task description: ");
                    string description = Console.ReadLine();
                    taskManager.AddTask(description);
                    break;
                case "2":
                    taskManager.ListTasks();
                    Console.WriteLine("Press any key to return to the menu.");
                    Console.ReadKey();
                    break;
                case "3":
                    taskManager.ListCompletedTasks();
                    Console.WriteLine("Press any key to return to the menu.");
                    Console.ReadKey();
                    break;
                case "4":
                    Console.Write("Enter task ID to mark as completed: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        taskManager.MarkAsCompleted(id);
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID.");
                    }
                    break;
                case "5":
                    Console.Write("Enter task ID to remove: ");
                    if (int.TryParse(Console.ReadLine(), out int removeId))
                    {
                        taskManager.RemoveTask(removeId);
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID.");
                    }
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private static void ShowMenu()
    {
        Console.Clear(); // Tyhjennetään konsoli ennen valikon näyttämistä
        Console.WriteLine("1. Add Task");
        Console.WriteLine("2. List Tasks");
        Console.WriteLine("3. List Completed Tasks");
        Console.WriteLine("4. Mark Task as Completed");
        Console.WriteLine("5. Remove Task");
        Console.WriteLine("6. Exit");
        Console.Write("Choose an option: ");
    }
}

public class TaskManager
{
    private List<Task> tasks;
    private List<Task> completedTasks;
    private const string tasksFilePath = "tasks.txt";
    private const string completedTasksFilePath = "completed_tasks.txt";

    public TaskManager()
    {
        tasks = new List<Task>();
        completedTasks = new List<Task>();
        LoadTasksFromFile();
    }

    public void AddTask(string description)
    {
        int id = tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1;
        Task newTask = new Task(id, description);
        tasks.Add(newTask);
        SaveTasksToFile(tasksFilePath, tasks);
    }

    public void ListTasks()
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks found.");
            return;
        }

        Console.WriteLine("Tasks:");
        foreach (Task task in tasks)
        {
            Console.WriteLine(task);
        }
    }

    public void ListCompletedTasks()
    {
        if (completedTasks.Count == 0)
        {
            Console.WriteLine("No completed tasks found.");
            return;
        }

        Console.WriteLine("Completed Tasks:");
        foreach (Task task in completedTasks)
        {
            Console.WriteLine(task);
        }
    }

    public void MarkAsCompleted(int id)
    {
        Task taskToComplete = tasks.FirstOrDefault(t => t.Id == id);
        if (taskToComplete != null)
        {
            completedTasks.Add(taskToComplete);
            tasks.Remove(taskToComplete);
            SaveTasksToFile(tasksFilePath, tasks);
            SaveTasksToFile(completedTasksFilePath, completedTasks);
        }
        else
        {
            Console.WriteLine("Task not found.");
        }
    }

    public void RemoveTask(int id)
    {
        Task taskToRemove = tasks.FirstOrDefault(t => t.Id == id);
        if (taskToRemove != null)
        {
            tasks.Remove(taskToRemove);
            SaveTasksToFile(tasksFilePath, tasks);
        }
        else
        {
            Console.WriteLine("Task not found.");
        }
    }

    private void LoadTasksFromFile()
    {
        LoadTasksFromFile(tasksFilePath, tasks);
        LoadTasksFromFile(completedTasksFilePath, completedTasks);
    }

    private void LoadTasksFromFile(string filePath, List<Task> taskList)
    {
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                if (parts.Length == 3 && int.TryParse(parts[0], out int id) && bool.TryParse(parts[2], out bool isCompleted))
                {
                    Task task = new Task(id, parts[1]) { IsCompleted = isCompleted };
                    taskList.Add(task);
                }
            }
        }
    }

    private void SaveTasksToFile(string filePath, List<Task> taskList)
    {
        List<string> lines = taskList.Select(t => $"{t.Id}|{t.Description}|{t.IsCompleted}").ToList();
        File.WriteAllLines(filePath, lines);
    }
}

public class Task
{
    public int Id { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }

    public Task(int id, string description)
    {
        Id = id;
        Description = description;
        IsCompleted = false;
    }

    public string Status => IsCompleted ? "Completed" : "Pending";

    public override string ToString()
    {
        if (IsCompleted)
        {
            return $"{Description} - Completed";
        }
        else
        {
            return $"[ID: {Id}] {Description} - Pending";
        }
    }

}
