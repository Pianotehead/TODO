using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace TODO
{
    public class MyTask
    {
        
        public MyTask (string name)
        {
            Name = name;
        }
        public MyTask(string name, DateTime? dueDate)
        {
            Name = name;
            DueDate = dueDate;
        }

        public MyTask(int id, string name, DateTime? dueDate)
        {
            Id = id;
            Name = name;
            DueDate = dueDate;
        }

        public MyTask(int id, string name, DateTime? dueDate, DateTime? completedAt)
        {
            Id = id;
            Name = name;
            DueDate = dueDate;
        }

        private string name;
        public int Id { get; }
        public string Name
        {
            get { return name; }
            private set
            {
                int maxLength = 50;
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

        private DateTime? dueDate;

        public DateTime? DueDate
        {
            get { return dueDate; }
            private set
            {
                if (value != null && value < DateTime.Now)
                {
                    throw new ArgumentException("  A new task cannot have a date in the past.", "DueDate");
                }
                dueDate = value;
            }
        }


        public static void ListTasks(List<MyTask> myTasks, int[] columnWidths)
        {
            Clear();
            string paddingLeft = new string(' ', 2);
            Write($"\n\n{paddingLeft}"); // Two empty rows, two spaces
            Write($"{"Description".PadRight(columnWidths[0])}");
            Write($"{"Due date".PadRight(columnWidths[1])}");
            int sumWidths = 0;

            for (int i = 0; i < columnWidths.Length; i++)
            {
                sumWidths += columnWidths[i];
            }
            if (sumWidths > LargestWindowWidth)
            {
                throw new ArgumentOutOfRangeException("The table is too big for the screen");
            }

            WriteLine();
            WriteLine("  " + new string((char)175, sumWidths));
            //char175 = border on top

            foreach (var myTask in myTasks)
            {
                WriteLine($"{paddingLeft}{myTask.Name.PadRight(columnWidths[0])}" +
                    $"{myTask.DueDate.ToString().PadRight(columnWidths[1])}");
            }
            WriteLine("\n\n  [D] Delete | [Esc] Back to main menu");
        }




    }
}
