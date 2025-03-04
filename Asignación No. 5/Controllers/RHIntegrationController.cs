using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Asignación_No._5.Controllers
{
    [ApiController]
    [Route("[controller]")]
   public class RHIntegrationController : ControllerBase
    {
        private readonly string connectionString = "conectionString";

        [HttpPost("send-to-tss")]
        public IActionResult SendToTSS([FromBody] EmployeePayrollData payrollData)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var query = @"INSERT INTO TSSIntegration (FirstName, LastName, IDNumber, Salary, Department, JobTitle, HireDate) 
                                   VALUES (@FirstName, @LastName, @IDNumber, @Salary, @Department, @JobTitle, @HireDate)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", payrollData.FirstName);
                        command.Parameters.AddWithValue("@LastName", payrollData.LastName);
                        command.Parameters.AddWithValue("@IDNumber", payrollData.IDNumber);
                        command.Parameters.AddWithValue("@Salary", payrollData.Salary);
                        command.Parameters.AddWithValue("@Department", payrollData.Department);
                        command.Parameters.AddWithValue("@JobTitle", payrollData.JobTitle);
                        command.Parameters.AddWithValue("@HireDate", payrollData.HireDate);

                        command.ExecuteNonQuery();
                    }
                }

                var tssResponse = new { Status = "Success", Message = "Data sent to TSS successfully" };

                return Ok(tssResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
            }
        }


        public class EmployeePayrollData
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string IDNumber { get; set; }
            public decimal Salary { get; set; }
            public string Department { get; set; }
            public string JobTitle { get; set; }
            public DateTime HireDate { get; set; }
        }
    }
}