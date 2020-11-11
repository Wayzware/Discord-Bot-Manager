using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Discord;

namespace Discord_Bot_Manager.Household
{
    class Chore
    {
        public int Days = 0;
        public IGuildUser[] Users;
        public IGuildUser CurrentUser;
        public string name;
        public DateTime DateNextDue;
        public DateTime ReminderTime;

        public Chore(string name)
        {
            this.name = name;
            DateNextDue = new DateTime(2000, 11, 24);
        }

        public Chore Copy()
        {
            Chore newChore = new Chore(name)
            {
                Users = Users,
                CurrentUser = CurrentUser,
                DateNextDue = DateNextDue,
                ReminderTime = ReminderTime
            };
            return newChore;
        }

        public string GetFormattedUsers()
        {
            if (Users == null) return "";
            string returnVal = " | ";
            foreach (IGuildUser user in Users)
            {
                if (user.Equals(CurrentUser))
                {
                    returnVal += "**" + user.Nickname + "** -> ";
                }
                else
                {
                    returnVal += user.Nickname + " -> ";
                }
            }

            return returnVal.Length > 3 ? returnVal[..^3] : "";
        }

        public bool IsDue()
        {
            return DateNextDue != new DateTime(2000, 11, 24) && (DateNextDue - DateTime.Now).Duration().TotalDays <= 0;
        }

        public void ToggleUser(IGuildUser user)
        {
            if (Users == null)
            {
                Users = new[] {user};
            }
            else if (Users.Contains(user))
            {
                Users = Users.Length > 1 ? Users.Where(u => u != user).ToArray() : new IGuildUser[0];
            }
            else
            {
                Users = Users.Append(user).ToArray();
            }
        }

        public void ToggleUsers(IGuildUser[] users)
        {
            foreach (var user in users) ToggleUser(user);
        }

        public void NextUser()
        {
            if (Users == null || Users.Length == 0) return;
            if (Users.Last() == CurrentUser || Users.Length == 1) CurrentUser = Users.First();
            else CurrentUser = Users[Array.IndexOf(Users, CurrentUser) + 1];
        }

        public void Done()
        {
            DateNextDue = DateTime.Today + TimeSpan.FromDays(Days);
            NextUser();
        }

        public void SetCurrentUser(IGuildUser user)
        {
            CurrentUser = user;
        }

    }
}
