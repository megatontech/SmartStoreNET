using System;
using System.Xml;
namespace SmartStore.Web.Infrastructure
{
    public class SafeXmlDocument:XmlDocument
    {
        public SafeXmlDocument()
        {
            this.XmlResolver = null;
        }
    }
}
