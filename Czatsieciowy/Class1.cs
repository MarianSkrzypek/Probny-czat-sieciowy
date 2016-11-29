using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
namespace Czatsieciowy
{
    public class Chat :MarshalByRefObject
    {
        private ArrayList usersList = new ArrayList();
        private String talk = String.Empty;
        public Chat() { }
        #region adduser 
        public bool AddUser(string userName)
        {
            if (usersList.Contains(userName))
                return false;
            if (userName != null && userName.Trim() != null)
            {
                lock (usersList)
                {
                    usersList.Add(userName);
                }
                return true;
            }
            return false;
        }
        #endregion
        #region removeuser
        public void RemoveUser(string userName)
        {
            lock (usersList)
            {
                usersList.Remove(userName);
            }
        }
        #endregion
        public void AddMessage(string newMessage)
        {
            if (newMessage != null && newMessage.Trim() != null)
            {
                newMessage.Replace("\t", " ");
                lock (talk)
                {
                    talk += newMessage +"\r\n";
                }
            }
        }
        public ArrayList UsersList
        {
            get { return usersList; }
        }
        public string Talk
        {
            get { return talk; }
        }
    }
}
