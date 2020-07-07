using System;
using System.Collections.Generic;
using System.Web;

namespace SmartStore.Web.Infrastructure
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg) 
        {

        }
     }
}