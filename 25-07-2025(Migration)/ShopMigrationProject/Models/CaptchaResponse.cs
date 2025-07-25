using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChienVHShopOnline.Models
{
    public class CaptchaResponse
    {
        public bool Success { get; set; }
        public List<string> ErrorCodes { get; set; }
    }
}