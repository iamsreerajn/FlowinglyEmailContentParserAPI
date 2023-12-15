using EmailContentParserAPI.Data;
using EmailContentParserAPI.Model;
using EmailContentParserAPI.Model.DTO;
using Microsoft.Data.SqlClient.Server;
using System.Xml;
using System.Xml.Linq;
using static EmailContentParserAPI.HelperUtilities.ECPUtilities;

namespace EmailContentParserAPI.Business
{
    public class ProcessReservataion : IProcessManager
    {
        private readonly DbConnectionContext _db;
        /// <summary>
        /// Injecting the database context
        /// </summary>
        /// <param name="db"></param>
        public ProcessReservataion(DbConnectionContext db)
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
                string vendor = String.Empty,description =string.Empty,date =string.Empty;
                foreach (XmlNode child in xmlDoc.DocumentElement.ChildNodes)
                {
                    if (child.NodeType == XmlNodeType.Element)
                    {
                        if (child.Name.ToLower() == "vendor")
                        {
                            vendor = child.InnerText != null ? child.InnerText : " ";
                        }
                        else if (child.Name.ToLower() == "description")
                        {
                            description = child.InnerText != null ? child.InnerText : " ";
                        }
                        else if (child.Name.ToLower() == "date")
                        {
                            date = child.InnerText != null ? child.InnerText : " ";
                        }
                    }
                }
                Reservation model = new Reservation()
                {
                    ReservationName = "Res_" + DateTime.Now.ToFileTime(),
                    Vendor = vendor,
                    Description=description,
                    Date=date,
                    CreatedDate=DateTime.Now
                };
                _db.Reservations.Add(model);
                _db.SaveChanges();
                return "Reservation Details Have Been Processed Successfully";
            }
            catch (Exception ex)
            {
                return "Operation Failed! Resercation Details Failed to Process";

            }
        }
        public string UpdateReservation(ReservationDto dto)
        {
            try
            {
                Reservation model = new()
                {
                    Id=dto.Id,
                    ReservationName = dto.ReservationName,
                    Vendor=dto.Vendor,
                    Description=dto.Description,
                    Date=dto.Date,
                    ModifiedDate = DateTime.Now
                };
                _db.Reservations.Update(model);
                _db.SaveChanges();
                return "Reservation details updated successfully!";
            }
            catch (Exception ex)
            {
                return "Reservation update operation failed!";

            }
        }
        /// <summary>
        /// This method is to delete the selected Reservation
        /// </summary>
        /// <param name="id"></param>
        /// <returns>status string</returns>
        public string DeleteReservation(int id)
        {
            var reservation = _db.Reservations.FirstOrDefault(u => u.Id == id);
            if (reservation == null)
            {
                return "Record Not Found!";
            }
            _db.Reservations.Remove(reservation);
            _db.SaveChanges();
            return "Reservation has been deleted successfully!";
        }
    }
}
