using EmailContentParserAPI.Business;
using EmailContentParserAPI.Data;
using EmailContentParserAPI.HelperUtilities;
using EmailContentParserAPI.Model;
using EmailContentParserAPI.Model.DTO;
using Microsoft.AspNetCore.Mvc;
//using MimeKit;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace EmailContentParserAPI.Controller
{

    [Route("api/EmailContent")]
    [ApiController]
    public class EmailContentController : ControllerBase
    {
        private readonly DbConnectionContext _dbContext;
        private readonly ILogger<EmailContentController> _logger;
        public EmailContentController(DbConnectionContext dbContext, ILogger<EmailContentController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Get the complete inserted data of Expenses
        /// </summary>
        /// <returns></returns>
        [Route("GetExpenses"), HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ExpenseClaimDto>> GetExpenses()
        {
            _logger.LogInformation(DateTime.Now.ToShortTimeString() + "Method - " + "GetExpense ::" + "Message - " + "Getting List of expenceses");
            return Ok(_dbContext.ExpenseClaims.ToList());
        }

        /// <summary>
        /// Get the complete inserted data of Reservation
        /// </summary>
        /// <returns></returns>
        [Route("GetReservations"), HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ReservationDto>> GetReservations()
        {
            //Log
            return Ok(_dbContext.Reservations.ToList());
        }

        /// <summary>
        /// This is a HTTPPOST method accepts the block of string parameter to parse and record data
        /// </summary>
        /// <param name="inputMessage"></param>
        /// <returns></returns>
        [Route("ProcessMessage"), HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ProcessMessage(string inputMessage)
        {
            try
            {
                EmailContentDistinguisher emailContentDistinguisher = new EmailContentDistinguisher(_dbContext);
                var result = emailContentDistinguisher.ParseEmailContnet(inputMessage);
                _logger.LogInformation(DateTime.Now.ToShortTimeString() + "Method - " + "ProcessMessage ::" + "Message - " + result);
                return Content(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [Route("UpdateExpense"), HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateExpense(int id, [FromBody] ExpenseClaimDto expenseDto)
        {
            if (expenseDto == null || expenseDto.Id != id)
            {
                return BadRequest();
            }
            ProcessExpense pm = new ProcessExpense(_dbContext);
            string response = pm.UpdateExpense(expenseDto);
            _logger.LogInformation(DateTime.Now.ToShortTimeString() + "Method - " + "UpdateExpense ::" + "Message - " + response);
            return Content(response);
        }
        [Route("UpdateReservation"), HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateReservation(int id, [FromBody] ReservationDto reservationDto)
        {
            if (reservationDto == null || reservationDto.Id != id)
            {
                return BadRequest();
            }
            ProcessReservataion reservation = new ProcessReservataion(_dbContext);
            string response = reservation.UpdateReservation(reservationDto);
            _logger.LogInformation(DateTime.Now.ToShortTimeString() + "Method - " + "UpdateReservation ::" + "Message - " + response);
            return Content(response);
        }

        [Route("DeleteExpense"), HttpDelete]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteExpense(int id)
        {
            if (id == 0)
            {
                _logger.LogInformation(DateTime.Now.ToShortTimeString() + "Method - " + "DeleteExpense ::" + "Message - " + "Bad Request");
                return BadRequest();
            }
            ProcessExpense pe = new ProcessExpense(_dbContext);
            string response = pe.DeleteExpense(id);
            _logger.LogInformation(DateTime.Now.ToShortTimeString() + "Method - " + "DeleteExpense ::" + "Message - " + response);
            return Content(response);

        }
        [Route("DeleteReservation"), HttpDelete]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteReservation(int id)
        {
            if (id == 0)
            {
                _logger.LogInformation(DateTime.Now.ToShortTimeString() + "Method - " + "DeleteReservation ::" + "Message - " + "Bad Request");
                return BadRequest();
            }
            ProcessReservataion pr = new ProcessReservataion(_dbContext);
            string response = pr.DeleteReservation(id);
            _logger.LogInformation(DateTime.Now.ToShortTimeString() + "Method - " + "DeleteReservation ::" + "Message - " + response);
            return Content(response);

        }

    }
}
