using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace TODO.Domain
{
    class TableMaker
    {
        // Seems too awkward to use object, because so many
        // classes cannot be cast to object, or vice versa
        // Better to have different constructors for various data types
        public IList<MyTask> MyTasks { get; }

        public TableMaker(IList<MyTask> myTasks)
        {
            MyTasks = myTasks;
        }

        public void CreateTable(bool includeId = true)
        {
            Clear();

            int[] rightMargins = FindLongest();
            int sumWidths = 0;
            for (int i = 0; i < rightMargins.Length; i++)
            {
                sumWidths += rightMargins[i];
            }

            string footer = "";
            if (!includeId)
            {
                footer = "\n\n  [C] Completed | [D] Delete | [Any other key] Back to main menu";
                sumWidths -= rightMargins[0];
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
            string paddingLeft = new string(' ', 2);
            Write($"\n\n{paddingLeft}"); // Two empty rows, two spaces

            if (includeId)
            {
                Write($"{"ID".PadRight(rightMargins[0])}");
            }

            Write($"{"Description".PadRight(rightMargins[1])}");
            Write($"{"Due date".PadRight(rightMargins[2])}");
            Write($"{"Completion date".PadRight(rightMargins[3])}");

            string separator = new string('-', sumWidths);
            WriteLine("\n" + paddingLeft + separator);

            foreach (var myTask in MyTasks)
            {
                Write($"{paddingLeft}");
                if (includeId)
                {
                    Write($"{myTask.Id.ToString().PadRight(rightMargins[0])}");
                }
                Write($"{myTask.Name.PadRight(rightMargins[1])}");
                Write($"{myTask.DueDate.ToString().PadRight(rightMargins[2])}");
                Write($"{myTask.CompletedAt.ToString().PadRight(rightMargins[2])}\n");
            }

            Write(footer);

        }

        private int[] FindLongest()
        {
            Type type = typeof(MyTask);
            int numberOfColumns = type.GetProperties().Length;
            int[] longest = new int[numberOfColumns];

            foreach (var myTask in MyTasks)
            {
                if (myTask.Id.ToString().Length > longest[0])
                {
                    longest[0] = myTask.Id.ToString().Length;
                }
                if (myTask.Name.Length > longest[1])
                {
                    longest[1] = myTask.Name.Length;
                }
                if (myTask.DueDate.ToString().Length > longest[2])
                {
                    longest[2] = myTask.DueDate.ToString().Length;
                }
                if (myTask.CompletedAt.ToString().Length > longest[3])
                {
                    longest[3] = myTask.CompletedAt.ToString().Length;
                }
            }

            for (int i = 0; i < longest.Length; i++)
            {
                longest[i] += 3; //Must have space between columns
            }
            if (longest[3] <= 3) //means all CompletedAt were NULLs
            {
                longest[3] = longest[2];
            }

            return longest;
        }
    }
}

