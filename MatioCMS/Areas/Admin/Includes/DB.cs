using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MatioCMS.Database;
using MatioCMS.Includes;

namespace MatioCMS.Areas.Admin.Includes
{
    public class DB : Database.DB
    {
        public DB(string Username, string Passkey, string RootPath, Translation translation) : base(RootPath)
        {
            if (this.Admins.Where(user => user.Username == Username && user.Passkey == Passkey).Count() != 1)
                throw new AdminLoginException(translation.GetMessage("Admin", 0));
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<sessionmodel> Sessions { get; set; }

    }
}
