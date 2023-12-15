using EmailContentParserAPI.Data;
using EmailContentParserAPI.HelperUtilities;
using EmailContentParserAPI.Model;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Xml;
using System.Xml.Linq;

namespace EmailContentParserAPI.Business.Factory
{
    public class EmailProcessFactory
    {
        private readonly DbConnectionContext _db;
        public EmailProcessFactory(DbConnectionContext db)
        {
            _db = db;
        }
        /// <summary>
        /// This method determine which object instance to be created for the current request
        /// </summary>
        /// <param name="processData"></param>
        /// <returns>IProcessManager</returns>
        public IProcessManager GetProcessManager(XmlDocument processData)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                IProcessManager returnValue = null;

                foreach (XmlNode node in processData.DocumentElement.ChildNodes)
                {
                    if (node.Name.Contains("expense") || node.Name.Contains("cost_centre") || node.Name.Contains("total") || node.Name.Contains("payment_method"))
                    {
                        returnValue = new ProcessExpense(_db);
                        break;
                    }
                    else if(node.Name.Contains("reservation") || node.Name.Contains("date") || node.Name.Contains("vendor") || node.Name.Contains("description"))
                    {
                        returnValue = new ProcessReservataion(_db);
                        break;
                    }
                }
                return returnValue;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
