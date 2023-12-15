using System.Xml;
using System.Xml.Linq;

namespace EmailContentParserAPI.Business
{
    public interface IProcessManager
    {
       // void ProcessExtractedData();
        string FilterEmailContent(XmlDocument str);
        
    }
}
