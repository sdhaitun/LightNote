using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightNote.DAO;
using LightNote.Models;

namespace LightNote.Service
{
    public class UserService
    {
        public ISupportAllDao<User, int> UserDao { get; set; }

        public UserService()
        {
            UserDao = new SupportAllDao<User, int>();
        }
    }
}
