using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;

namespace LightNote.Models
{
    [ActiveRecord(Table = "`User`")]
    public class User
    {
        [PrimaryKey]
        public int UserId { get; set; }

        [Property]
        public string Name { get; set; }

        [Property]
        public string Password { get; set; }
    }
}
