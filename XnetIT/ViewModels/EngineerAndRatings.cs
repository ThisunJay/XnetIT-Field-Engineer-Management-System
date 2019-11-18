using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XnetIT.Models;

namespace XnetIT.ViewModels
{
    public class EngineerAndRatings
    {
        public engineer Engineer { get; set; }
        public List<eng_ratings> Ratings { get; set; }
    }
}