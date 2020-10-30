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
            CompletedAt = completedAt;
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
                dueDate = value;
            }
        }
        public DateTime? CompletedAt { get; }

        public static void ListTasks(IList<MyTask> myTasks, int[] columnWidths, bool includeId = true)
        {
            Clear();
            string paddingLeft = new string(' ', 2);
            Write($"\n\n{paddingLeft}"); // Two empty rows, two spaces
            if (includeId)
            {
                Write($"{"ID".PadRight(columnWidths[0])}");                
            }
            Write($"{"Description".PadRight(columnWidths[1])}");
            Write($"{"Due date".PadRight(columnWidths[2])}");
            Write($"{"Completion date".PadRight(columnWidths[3])}");
            int sumWidths = 0;


            for (int i = 0; i < columnWidths.Length; i++)
            {
                sumWidths += columnWidths[i];
            }

            string footer = "";

            if (!includeId)
            {
                footer = "\n\n  [C] Completed | [D] Delete | [Esc] Back to main menu";
                sumWidths -= columnWidths[0];                
            }
            else
            {
                footer = "\n\n  ID: ";
            }
            if (sumWidths > LargestWindowWidth)
            {
                throw new ArgumentOutOfRangeException("The table is too big for the screen");
            }

            WriteLine();
            WriteLine("  " + new string((char)175, sumWidths));
            //char175 = border under headers
            foreach (var myTask in myTasks)
            {
                Write($"{paddingLeft}");
                if (includeId)
                {
                    Write($"{myTask.Id.ToString().PadRight(columnWidths[0])}");
                }
                Write($"{myTask.Name.PadRight(columnWidths[1])}");
                Write($"{myTask.DueDate.ToString().PadRight(columnWidths[2])}");
                Write($"{myTask.CompletedAt.ToString().PadRight(columnWidths[2])}\n");
            }
            Write(footer);
        }




    }
}
