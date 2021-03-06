using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace TODO
{
    public class MyTask
    {
        public MyTask(string name, DateTime dueDate)
        {
            Name = name;
            DueDate = dueDate;
        }

        private string name;

        public string Name
        {
            get { return name; }
            private set
            {
                int maxLength = 25;
                if (!string.IsNullOrWhiteSpace(value) || value.Length <= maxLength)
                {
                    name = value;
                }
                else
                {
                    throw new ArgumentException("  Text for task was either empty or too long.", "Name");
                }
            }
        }

        private DateTime dueDate;

        public DateTime DueDate
        {
            get { return dueDate; }
            private set
            {
                if (value.Date < DateTime.Now.Date)
                {
                    throw new ArgumentException("  A new task cannot happen in the past.", "DueDate");
                }
                dueDate = value;
            }
        }

        public static void WriteAllTasks(List<MyTask> myTasks)
        {
            WriteLine();
            WriteLine($"{ "  Task",-27} { "Due Date"}");
            WriteLine("  " + new string((char)175, 36)); //Overscore D)
            foreach (var myTask in myTasks)
            {
                WriteLine($"  {myTask.name, -25} {myTask.dueDate.ToShortDateString()}");
            }
        }




    }
}
