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
    }
}
