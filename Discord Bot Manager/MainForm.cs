using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Discord_Bot_Manager
{
    public partial class Form1 : Form
    {
        private Household.Household _household;

        private Task _householdTask;

        public Form1()
        {
            InitializeComponent();
        }

        private void ButtonStartBot_Click(object sender, EventArgs e)
        {
            _household = new Household.Household(new TextBoxConsoleSocket(TextBoxConsoleA));
            _householdTask = _household.Start();
        }

        private void ButtonStopBot_Click(object sender, EventArgs e)
        {
            _household?.Stop();
            _household = null;
            _householdTask = null;
        }

        private void ButtonKillBot_Click(object sender, EventArgs e)
        {
            _household?.Kill();
            _household = null;
            _householdTask = null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
