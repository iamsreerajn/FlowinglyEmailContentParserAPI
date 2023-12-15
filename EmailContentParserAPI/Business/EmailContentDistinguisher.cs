using EmailContentParserAPI.Business.Factory;
using EmailContentParserAPI.Data;
using EmailContentParserAPI.HelperUtilities;
using Org.BouncyCastle.Asn1.Esf;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace EmailContentParserAPI.Business
{
    public class EmailContentDistinguisher
    {
        private readonly DbConnectionContext _dbContext;
        /// <summary>
        /// Injecting the DB context
        /// </summary>
        /// <param name="dbContext"></param>
        public EmailContentDistinguisher(DbConnectionContext dbContext)
        {
            _dbContext = dbContext;
        }
        public string ParseEmailContnet(string inputMessage)
        {
            string responseResult = string.Empty;
            EmailProcessFactory factory = new EmailProcessFactory(_dbContext);
            XmlDocument xElement =  GetFirstElement(inputMessage,out responseResult);
            if (!string.IsNullOrEmpty(responseResult))
            {
                return responseResult;
            }
            else
            {
                IProcessManager processManager = factory.GetProcessManager(xElement);
                string str = processManager.FilterEmailContent(xElement);
                return str;
            }
        }
        /// <summary>
        /// Finding first element from the extracted xml data to determine the type of email.
        /// This includes primary levels of validations
        /// </summary>
        /// <param name="inputMessage"></param>
        /// <returns></returns>
        private XmlDocument GetFirstElement(string inputMessage, out string response)
        {
            try
            {
                response = string.Empty;
                // Retrieve multiple XML tags from the string
                var xmlTags = RetrieveXmlTags(inputMessage);
                XmlDocument doc = new XmlDocument();
                string content = string.Empty;
                try
                {
                    doc.LoadXml(xmlTags);
                    return doc;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("There are multiple root elements")||ex.Message.Contains("Data at the root level is invalid"))
                    {
                        try
                        {
                            doc.LoadXml("<r>" + xmlTags + "</r>");
                            return doc;
                        }
                        catch (Exception innerException)
                        {
                            if (innerException.Message.Contains("does not match the end tag") || ex.Message.Contains("The following elements are not closed"))
                            {
                                response = "Invalid XML Content! No proper close tag found!";
                                return null;
                            }
                            return null;
                        }
                        return doc;
                    }
                    else if (ex.Message.Contains("does not match the end tag") || ex.Message.Contains("The following elements are not closed"))
                    {
                        response = "Invalid XML Content! No proper close tag found!";
                        return null;
                    }
                }
                return doc;
            }
            catch(Exception ex)
            {
                response = "Invalid XML content!";
                return null;
            }
        }
        /// <summary>
        /// Retriving multiple XML tags from the given string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static string RetrieveXmlTags(string input)
        {
            MatchCollection matches = Regex.Matches(input, ECPUtilities.Pattern);
            // Extract and return the matched XML tags
            string[] xmlTags = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                xmlTags[i] = matches[i].Value;
            }
            string xmlString = String.Concat(xmlTags);
            return xmlString;
        }

    }
}
