using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XnetIT.Models;

namespace XnetIT.ViewModels
{
    public class JobAndRatingsViewModel
    {
        public List<job> Jobs { get; set; }
        public List<job_ratings> Ratings { get; set; }
    }
}