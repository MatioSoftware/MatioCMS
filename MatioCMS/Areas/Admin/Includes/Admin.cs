using System;
using System.Collections.Generic;
using MatioCMS.Includes.Models;

namespace MatioCMS.Areas.Admin.Includes
{
    public class Admin
    {
        public string Username { get; set; }
        public string Passkey { get; set; }
        public string Fullname { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public AdminRole Role { get; set; }

        public IEnumerable<Log> Logs { get; set; }
        public IEnumerable<adminstartpage> AdminStartPage_Widgets { get; set; }
        public IEnumerable<Gallery> MyGalleryItems { get; set; }

    }

    public enum AdminRole
    {
        SuperAdmin = 0, // Root
        Administrator = 1, // Full Admin
        Editor = 2, // Editing and publishing all pages, posts, categories and tags
        Author = 3, // Editing and publishing their own pages and posts
        Contributor = 4 // Editing their own posts and save as draft
    }
}
