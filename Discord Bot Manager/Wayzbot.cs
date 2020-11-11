using System;
using System.Collections.Generic;
using System.Text;

namespace Discord_Bot_Manager
{
    /// <summary>
    /// This class will hold static utility functions that are not unique to any bot
    /// </summary>
    static class Wayzbot
    {
        public static string[] CommandToArray(string command, char discriminator)
        {
            if (!command.Contains('"')) return command.Split(discriminator, StringSplitOptions.RemoveEmptyEntries); //check for this first
            return StringSplitByDiscriminator(command, discriminator);
        }
        private static string[] StringSplitByDiscriminator(string command, char discriminator)
        {
            string workingLine = command;
            List<string> output = new List<string>();
            while (workingLine.Length > 0)
            {
                int start = 0;
                int end = 0;
                int index = -1;
                if (workingLine.StartsWith('"'.ToString())) //this item is in quotes
                {
                    if (workingLine.Length > 0)
                    {
                        start = 1;
                        end = workingLine.IndexOf('"', 1);
                        index = end + 2;
                        end--;
                    }
                }
                else
                {
                    index = workingLine.IndexOf(discriminator, 0);
                    end = index;
                    index++;
                }

                if (index < 0 || end < 0)
                {
                    output.Add(workingLine);
                    workingLine = "";
                }
                else
                {
                    string element = workingLine.Substring(start, (end + start > workingLine.Length ? workingLine.Length - start : end));
                    if (index > workingLine.Length)
                    {
                        workingLine = "";
                    }
                    else
                    {
                        workingLine = workingLine.Remove(0, (index > workingLine.Length ? workingLine.Length : index));
                    }

                    output.Add(element);
                }
            }

            return output.ToArray();
        }
    }
}
