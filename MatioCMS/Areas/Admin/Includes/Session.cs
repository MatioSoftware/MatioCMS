using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatioCMS.Includes;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MatioCMS.Areas.Admin.Includes
{
    public class Session
    {
        public Session(DB Database, HttpContext HTTP)
        {
            if (Database == null)
                throw new ArgumentNullException("Database");
            this.DB = Database;
            foreach (var session in DB.Sessions.Where(entry => (DateTime.Now.ToUniversalTime() - entry.TimeAdded).Days == 1))
                DB.Sessions.Remove(session);

            if (HTTP.Request.Cookies.Keys.Where(key => key == "SESSIONID").Count() > 0)
            {
                SessionID = HTTP.Request.Cookies["SESSIONID"];
                if (DB.Sessions.Where(item => item.ID == SessionID).Count() != 1)
                    HTTP.Response.Cookies.Delete("SESSIONID");
                return;
            }
        }

        private DB DB;
        public string SessionID { get; private set; }

    }
}
