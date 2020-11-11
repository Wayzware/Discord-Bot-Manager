using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Discord_Bot_Manager
{
    interface IStatusSocket
    {
        public bool IsActive { get; set; }
        public void RefreshView();
    }

    public class StatusSocket : IStatusSocket
    {
        private CheckBox _checkBox;
        public bool IsActive { get; set; }

        public StatusSocket(CheckBox checkBox)
        {
            _checkBox = checkBox;
        }

        public void RefreshView()
        {
            _checkBox.Checked = IsActive;
        }
    }
}
