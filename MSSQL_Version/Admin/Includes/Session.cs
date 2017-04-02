using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MatioCMS.Includes;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using MatioCMS.Admin.Includes.Models;

namespace MatioCMS.Admin.Includes
{
    public class Session
    {
        public Session(DB Database, HttpContext HTTP)
        {
            this.DB = Database ?? throw new ArgumentNullException("Database");
            foreach (var session in DB.Sessions.Where(entry => (DateTime.Now.ToUniversalTime() - entry.TimeAdded).Days == 1))
                DB.Sessions.Remove(session);

            if (HTTP.Request.Cookies.Keys.Where(key => key == "SESSIONID").Count() == 0)
            {
                this.data = new Dictionary<string, string>();
                var id = Guid.NewGuid();
                DB.Sessions.Add(new sessionmodel { ID = id });
                SessionID = id.ToString("X");
            }
            else 
            {
                SessionID = HTTP.Request.Cookies["SESSIONID"];
                Guid guid;
                if (!Guid.TryParse(SessionID, out guid) || DB.Sessions.Where(item => item.ID == guid).Count() != 1)
                {
                    this.data = new Dictionary<string, string>();
                    var id = Guid.NewGuid();
                    DB.Sessions.Add(new sessionmodel { ID = id });
                    SessionID = id.ToString("X");
                    return;
                }
                string data = DB.Sessions.Where(item => item.ID == guid).First().Data;
                if(!string.IsNullOrWhiteSpace(data))
                {   try { this.data = JsonConvert.DeserializeObject<Dictionary<string, string>>(data); }
                    catch(Exception)
                    {
                        this.data = new Dictionary<string, string>();
                        var obj = DB.Sessions.Where(item => item.ID == guid).First();
                        obj.Data = JsonConvert.SerializeObject(this.data);
                        obj.TimeAdded = DateTime.UtcNow;
                    }
                }
            }
        }

        public void Save()
        {
            var id = Guid.Parse(SessionID);
            var obj = DB.Sessions.Where(item => item.ID == id).First();
            obj.TimeAdded = DateTime.UtcNow;
            obj.Data = JsonConvert.SerializeObject(this.data);
            DB.SaveChanges();
        }

        public void UpdateTime()
        {
            var id = Guid.Parse(SessionID);
            DB.Sessions.Where(item => item.ID == id).First().TimeAdded = DateTime.UtcNow;
            DB.SaveChanges();
        }

        private DB DB;
        public string SessionID { get; private set; }
        private Dictionary<string, string> data;

        public string this[string key]
        {
            get { return this.data[key]; }
            set { this.data[key] = value; }
        }
    }
}
