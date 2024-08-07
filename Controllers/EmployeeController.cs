using Microsoft.AspNetCore.Mvc;
using Software_Developer_CSharp_Test_01_v1_Dec_2023.Models;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Software_Developer_CSharp_Test_01_v1_Dec_2023.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private static readonly String _apiCall = "https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==";       
        public IActionResult EmployeeTable()
        {
            List<TimeEntry> employees = GetTimeEntries().Result;
            

            return View(employees.GroupBy(e => e.EmployeeName).
                Select(emp => new Employee { Name = emp.Key, HoursWorked = emp.Sum(e => e.TotalTimeWorked) }).
                OrderByDescending(e => e.HoursWorked).ToList()  
                );
        }

      

        static async Task<List<TimeEntry>> GetTimeEntries()
        {
            using HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(_apiCall);
            return JsonSerializer.Deserialize<List<TimeEntry>>(response);
        }

    }
}
