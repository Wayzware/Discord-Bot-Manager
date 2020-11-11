using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Discord;

namespace Discord_Bot_Manager
{
    //this interface exists in case I find a better way to implement a console
    interface IConsoleSocket
    {
        public void ClearAll();
        public void WriteLine(string line);
        public void WriteLine(string line, int severity);

    }

    public class TextBoxConsoleSocket : IConsoleSocket
    {
        private readonly TextBox _console;

        public TextBoxConsoleSocket(TextBox console)
        {
            _console = console;
            
        }

        public void ClearAll()
        {
            _console.Text = "";
        }

        public void WriteLine(string line)
        {
            line = "[" + DateTime.Now.TimeOfDay.ToString() + "] " + line;

            _console.AppendText(_console.Text == "" ? line : Environment.NewLine + line);
        }

        public void WriteLine(string line, int severity)
        {
            line = severity switch
            {
                0 => "[INFO] " + line,
                1 => "[WARNING] " + line,
                2 => "[ERROR] " + line,
                3 => "### [FATAL ERROR] ### " + line,
                _ => "[" + severity + "] " + line
            };
            WriteLine(line);
        }

    }
}
