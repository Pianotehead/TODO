using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace TODO.Domain
{
    class Menu
    {
        public string[] Items { get; }

        public Menu(string[] items)
        {
            if (items == null || items.Length == 0)
            {
                throw new NullReferenceException("\n  Input must contain at least one string\n");
            }
            Items = items;
        }

        public string GetByIndex(int index)
        {
            if (index >= Items.Length)
            {
                WriteLine($"\n  You asked for element nr. {index + 1}");
                throw new IndexOutOfRangeException($"\n  This array only has {Items.Length} elements");
            }
            return Items[index];
        }

        public void CreateNumberedMenu()
        {
            Clear();
            WriteLine();
            for (int i = 0; i < Items.Length; i++)
            {
                WriteLine($"  {(i + 1)}. {Items[i]}");
            }
        }

        public void CreatePromptsWithDots()
        {
            Clear();
            WriteLine();
            // Want to insert dots, so all items will be equally long
            // ending on a colon.
            string numberOfDots;
            for (int i = 0; i < Items.Length; i++)
            {
                numberOfDots = new string('.', FindLongest() - Items[i].Length);
                WriteLine($"  {Items[i]}{numberOfDots}: \n");
            }
        }

        public void CreateTable(string[] column2)
        {
            Clear();
            WriteLine();
            if (column2.Length == 0)
            {
                throw new NullReferenceException("\n  Input must contain at least one string\n");
            }
            else if (column2.Length < Items.Length)
            {
                throw new IndexOutOfRangeException("\n  The second must be at least as big as the first\n");
            }
            int marginRight = FindLongest();
            for (int i = 0; i < Items.Length; i++)
            {
                WriteLine($"  {Items[i].PadRight(marginRight)} {column2[i]}");
            }
        }

        public int FindLongest()
        {
            int longest = Items[0].Length;
            if (Items.Length == 1)
            {
                return longest;
            }
            for (int i = 1; i < Items.Length; i++)
            {
                if (Items[i].Length > longest)
                {
                    longest = Items[i].Length;
                }
            }
            return longest;

        }

        public int GetMenuLength()
        {
            return Items.Length;
        }

        public string[] AskForInputs()
        {
            string[] inputs = new string[GetMenuLength()];
            int marginLeft = FindLongest() + "  : ".Length;
            // Prompts start with spaces, end with colon and space 
            bool inputWrong = true;
            ConsoleKey yesOrNo;
            do
            {
                CursorVisible = true;
                Clear();
                CreatePromptsWithDots();
                int marginTop = 1;
                for (int i = 0; i < inputs.Length; i++)
                {
                    SetCursorPosition(marginLeft, marginTop);
                    inputs[i] = ReadLine();
                    if (string.IsNullOrEmpty(inputs[i]))
                    {
                        throw new ArgumentNullException("\n\n  Empty input not allowed!\n\n");
                    }
                    marginTop += 2;
                }
                CursorVisible = false;
                WriteLine("\n\n  Is this correct? (Y)es (N)o ");
                yesOrNo = ActOnOnlyTheseKeys(ConsoleKey.Y, ConsoleKey.N);
                if (yesOrNo == ConsoleKey.Y)
                {
                    inputWrong = false;
                }
            } while (inputWrong || yesOrNo == ConsoleKey.N);
            return inputs;
        }

        public static ConsoleKey ActOnOnlyTheseKeys(params ConsoleKey[] keys)
        {
            ConsoleKeyInfo choice;
            bool allowed = false;

            do
            {
                choice = ReadKey(true);
                for (int i = 0; i < keys.Length; i++)
                {
                    allowed = allowed || (choice.Key == keys[i]);
                }
            } while (!allowed);
            return choice.Key;
        }
    }
}
