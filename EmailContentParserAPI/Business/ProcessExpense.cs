using EmailContentParserAPI.Data;
using EmailContentParserAPI.HelperUtilities;
using EmailContentParserAPI.Model;
using EmailContentParserAPI.Model.DTO;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Security;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace EmailContentParserAPI.Business
{
    public class ProcessExpense : IProcessManager
    {
        private readonly DbConnectionContext _db;
        /// <summary>
        /// Injecting the database context
        /// </summary>
        /// <param name="db"></param>
        public ProcessExpense(DbConnectionContext db)
        {
            _db = db;
        }
        public string FilterEmailContent(XmlDocument xmlElement)
        {
            string response = ProcessExtractedData(xmlElement);
            return response;
        }
        /// <summary>
        /// This method is to process and update the details to database
        /// </summary>
        /// <param name="xmlElement"></param>
        private string ProcessExtractedData(XmlDocument xmlDoc)
        {
            try
            {
                bool isTotalVailable = false;
                bool isCostCentreAvailable = false;
                string costCenter = string.Empty, paymentMethod = string.Empty;
                double total = 0, productPrice = 0;
                foreach (XmlNode child in xmlDoc.DocumentElement.ChildNodes)
                {
                    if (child.NodeType == XmlNodeType.Element)
                    {
                        if (child.Name.ToLower() == "total")
                        {
                            total = child.InnerText != null ? Convert.ToDouble(child.InnerText) : 0;
                            isTotalVailable = true;
                        }
                        else if (child.Name.ToLower() == "cost_centre")
                        {
                            costCenter = child.InnerText != null ? child.InnerText : "UNKNOWN";
                            isCostCentreAvailable = true;
                        }
                        else if (child.Name.ToLower() == "payment_method")
                        {
                            paymentMethod = child.InnerText != null ? child.InnerText : "Default Payment Method";
                        }
                    }
                }
                if(!isTotalVailable)
                {
                    return "Rejected! There is No Total Found!";
                }

                ExpenseClaim expenseClaim = new ExpenseClaim()
                {
                    ExpenseName = "Exp_"+DateTime.Now.ToFileTime(),
                    CostCenter = isCostCentreAvailable ? costCenter : "UNKNOWN",
                    PaymentMethod = paymentMethod,
                    Total=total,
                    SalesTax = CalculateSalesTax(total),
                    ProductPrice = total- CalculateSalesTax(total),
                    CreatedDate = DateTime.Now
                };
                _db.ExpenseClaims.Add(expenseClaim);
                _db.SaveChanges();
                return "Expense Details Have Been Processed Successfully";
            }
            catch (Exception ex)
            {
                return "Operation Failed! Expense Details Failed to Process";
            }
        }
        private double CalculateSalesTax(double totalAmount)
        {
            return totalAmount * (ECPUtilities.SalesTaxPercentage / 100);
        }
        /// <summary>
        /// This method is to validate the XML syntax and contents
        /// </summary>
        /// <param name="extractedXml"></param>
        /// <returns></returns>
        private XElement ValidateExtractedXmlContent(string extractedXml)
        {
            if (!string.IsNullOrEmpty(extractedXml))
            {
                try
                {
                    XElement xmlElement = XElement.Parse(extractedXml);

                    return xmlElement;
                }
                catch (Exception ex)
                {
                    //process xml closing tag issue
                    return null;
                }
            }
            else
            {
                //  return Content("There is no valid XML data found!");
                return null;
            }
        }
        /// <summary>
        /// This method is to update the existing selected expense
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public string UpdateExpense(ExpenseClaimDto dto)
        {
            try
            {
                ExpenseClaim model = new()
                {
                    CostCenter = dto.CostCenter,
                    Total = dto.Total,
                    Id = dto.Id,
                    PaymentMethod = dto.PaymentMethod,
                    SalesTax = CalculateSalesTax(dto.Total),
                    ProductPrice = dto.Total - CalculateSalesTax(dto.Total),
                    ExpenseName = dto.ExpenseName,
                    ModifiedDate=DateTime.Now
                };
                _db.ExpenseClaims.Update(model);
                _db.SaveChanges();
                return "Expense details updated successfully!";
            }
            catch(Exception ex)
            {
                return "Expense update operation failed!";

            }
        }
        /// <summary>
        /// This method is to delete the existing reservation selected
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DeleteExpense(int id)
        {
            var expense = _db.ExpenseClaims.FirstOrDefault(u => u.Id == id);
            if (expense == null)
            {
                return "Record Not Found!";
            }
            _db.ExpenseClaims.Remove(expense);
            _db.SaveChanges();
            return "Expense has been deleted successfully!";
        }
    }
}
