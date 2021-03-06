﻿using Microsoft.Data.SqlClient;
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
        public static List<MyTask> myTaskList = new List<MyTask>();

        static void Main(string[] args)
        {
            Menu mainMenu = new Menu(new string[] { "Add task", "Exit" });
            ConsoleKey input;
            bool applicationRunning = true;
            do
            {
                mainMenu.CreateNumberedMenu();
                CursorVisible = false;
                input = Menu.ActOnOnlyTheseKeys
                (
                    ConsoleKey.D1, ConsoleKey.NumPad1,
                    ConsoleKey.D2, ConsoleKey.NumPad2
                );
                Console.Clear();
                switch (input)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        AddTask();
                        WriteLine("\n  Task registered");
                        Thread.Sleep(2000);
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        applicationRunning = false;
                        break;
                }
            } while (applicationRunning);
            Clear();
            WriteLine("\n  Thank you for using this To Do application");
            Thread.Sleep(2000);

        }

        private static void AddTask()
        {
            Clear();
            Menu askForData = new Menu(new string[] { "Task", "Due Date (YYYY-MM-DD)" });
            string[] inputsForTask = askForData.AskForInputs();
            DateTime dateInput = ConvertSuccessfullyToDate(inputsForTask[1]);
            MyTask newTask = new MyTask(inputsForTask[0], dateInput);
            myTaskList.Add(newTask);
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
                    Write(" Also note dates in the past are not allowed. Please try again. ");
                    dateAsText = ReadLine();
                }


            } while (!conversionSuccessful);
            return dateInCorrectFormat;
        }
    }
}
