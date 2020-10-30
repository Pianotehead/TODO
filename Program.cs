using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using TODO.Domain;
using static System.Console;

namespace TODO
{
    class Program
    {
        static string connectionString = "Server=localhost;Database=TODO;Integrated Security=True";
        public static List<MyTask> myTaskList = new List<MyTask>();

        static void Main(string[] args)
        {
            Menu mainMenu = new Menu(new string[] { "Add task", "List tasks", "Exit" });
            ConsoleKey input;
            bool applicationRunning = true;
            do
            {
                mainMenu.CreateNumberedMenu();
                CursorVisible = false;
                int[] columnWidths = { 4, 50, 20, 20 };
                input = Menu.ActOnOnlyTheseKeys
                (
                    ConsoleKey.D1, ConsoleKey.NumPad1,
                    ConsoleKey.D2, ConsoleKey.NumPad2,
                    ConsoleKey.D3, ConsoleKey.NumPad3
                );
                Clear();
                switch (input)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        AddTask();
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        var myTaskList = FetchMyTasks();
                        MyTask.ListTasks(myTaskList, columnWidths, false);
                        ConsoleKey deleteOrNot = Menu.ActOnOnlyTheseKeys(ConsoleKey.D, ConsoleKey.Escape);
                        switch (deleteOrNot)
                        {
                            case ConsoleKey.D:
                                MyTask.ListTasks(myTaskList, columnWidths);
                                int idToRemove = ConvertSuccessfullyToInt(ReadLine());
                                CursorVisible = false;
                                DeleteTask(idToRemove);
                                break;

                        }
                        break;
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        applicationRunning = false;
                        break;
                }
            } while (applicationRunning);
            Clear();
            WriteLine("\n  Thank you for using this To Do application");
            Thread.Sleep(2000);
            CursorVisible = true;
        }

        private static void AddTask()
        {
            Clear();
            Menu askForData = new Menu(new string[] { "Task", "Due Date (YYYY-MM-DD)" });
            string[] inputsForTask = askForData.AskForInputs();
            MyTask newTask;
            DateTime dateInput;
            if (!string.IsNullOrWhiteSpace(inputsForTask[1]))
            {
                dateInput = ConvertSuccessfullyToDate(inputsForTask[1]);
                newTask = new MyTask(inputsForTask[0], dateInput);
            }
            else
            {
                newTask = new MyTask(inputsForTask[0]);
            }
            InsertMyTask(newTask);
            WriteLine("\n  Task registered");
            Thread.Sleep(2000);
        }

        private static void InsertMyTask(MyTask myTask)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command;
            if (!(myTask.DueDate == null))
            {
                var sql = $@"
                INSERT INTO MyTask (Name, DueDate)
                VALUES (@Name, @DueDate)";

                command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@Name", myTask.Name);

                command.Parameters.AddWithValue("@DueDate", myTask.DueDate);

            }
            else
            {
                var sql = $@"
                INSERT INTO MyTask (Name)
                VALUES (@Name)";

                command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@Name", myTask.Name);

            }


            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        private static IList<MyTask> FetchMyTasks()
        {
            string sql = "SELECT Id, Name, DueDate, CompletedAt FROM MyTask";

            List<MyTask> myTaskList = new List<MyTask>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var id = (int)reader["Id"];
                    var name = (string)reader["Name"];
                    string dueDateAsString = reader["DueDate"].ToString();
                    string completionDate = reader["CompletedAt"].ToString();
                    if (string.IsNullOrWhiteSpace(dueDateAsString) && string.IsNullOrWhiteSpace(completionDate))
                    {
                        myTaskList.Add(new MyTask(id, name, null));
                    }
                    else if (string.IsNullOrWhiteSpace(dueDateAsString) && !string.IsNullOrWhiteSpace(completionDate))
                    {
                        var completedAt = (DateTime)reader["CompletedAt"];
                        myTaskList.Add(new MyTask(id, name, null, completedAt));
                    }
                    else if (!string.IsNullOrWhiteSpace(dueDateAsString) && string.IsNullOrWhiteSpace(completionDate))
                    {
                        var dueDate = (DateTime)reader["DueDate"];
                        myTaskList.Add(new MyTask(id, name, dueDate));
                    }
                    else
                    {
                        var dueDate = (DateTime)reader["CompletedAt"];
                        var completedAt = (DateTime)reader["CompletedAt"];
                        myTaskList.Add(new MyTask(id, name, dueDate, completedAt));
                    }
                }

                connection.Close();
            }
            return myTaskList;
        }

            private static void DeleteTask(int idToRemove)
            {
                var sql = $@"DELETE FROM MyTask WHERE Id = @Id";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sql, connection);

                    command.Parameters.AddWithValue("@Id", idToRemove);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                Clear();
                WriteLine($"\n\n  Task nr. {idToRemove} was deleted from the task list...");
                Thread.Sleep(2000);
            }

        public static DateTime ConvertSuccessfullyToDate(string dateAsText)
        {
            bool conversionSuccessful;
            DateTime dateInCorrectFormat;
            do
            {
                conversionSuccessful = DateTime.TryParse(dateAsText, out dateInCorrectFormat);
                if (!conversionSuccessful)
                {
                    Clear();
                    WriteLine("\n Input must be in the format YYYY-MM-DD");
                    Write(" Please try again. ");
                    dateAsText = ReadLine();
                }


            } while (!conversionSuccessful);
            return dateInCorrectFormat;
        }

        public static int ConvertSuccessfullyToInt(string numberAsText)
        {
            bool conversionSuccessful;
            int result;
            do
            {
                conversionSuccessful = Int32.TryParse(numberAsText, out result);
                if (!conversionSuccessful ||(conversionSuccessful && result == 0))
                {
                    Clear();
                    WriteLine("\n Input must be a positive integer, greater than zero.");
                    Write(" Please try again. ");
                    numberAsText = ReadLine();
                }
                

            } while (!conversionSuccessful);
            return result;
        }
    }
}
