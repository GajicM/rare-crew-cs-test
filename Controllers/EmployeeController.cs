using Microsoft.AspNetCore.Mvc;
using Software_Developer_CSharp_Test_01_v1_Dec_2023.Models;
using System.Drawing.Imaging;
using System.Drawing;
using System.Text.Json;
using System.Runtime.Versioning;

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
        [HttpGet("/api/employee/pie-chart")]
        [SupportedOSPlatform("windows")]
        public IActionResult EmployeePieChart() {
                return View();
        }

        [HttpGet("/api/employee/pie-chart-png")]
        [SupportedOSPlatform("windows")]
        public IActionResult GetEmployeePieChart() {
            List<TimeEntry> employees = GetTimeEntries().Result;
            using var stream = new MemoryStream();
            GeneratePieChart(
                employees.GroupBy(e => e.EmployeeName).
                          Select(emp => new Employee { Name = emp.Key, HoursWorked = emp.Sum(e => e.TotalTimeWorked) }).
                          OrderByDescending(e => e.HoursWorked).ToList()
             , stream);
            return File(stream.ToArray(), "image/png");
        }


        static async Task<List<TimeEntry>> GetTimeEntries()
        {
            using HttpClient client = new();
            var response = await client.GetStringAsync(_apiCall);
            return JsonSerializer.Deserialize<List<TimeEntry>>(response);
        }


       [SupportedOSPlatform("windows")]
       private static void GeneratePieChart(List<Employee> employees,MemoryStream stream)
        {
            var totalHours = employees.Sum(e => e.HoursWorked);
            var percentages = employees.Select(e => (float)e.HoursWorked / totalHours * 100).ToArray();
            var names = employees.Select(e => e.Name).ToArray();

            int width = 1000;
            int height = 1000;
            Bitmap bitmap = new(width, height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);

            Font font = new Font("Times New Roman", 14);
            Brush brush = new SolidBrush(Color.Black);

            Rectangle rect = new(200, 200, 600, 600);
            float startAngle = 0;
            int legendX = 10;
            int legendY = 10;
            Random rand = new();
            int i = 0;
           
            
            foreach (Employee emp in employees)
            {
                var percentage=((float)emp.HoursWorked / totalHours) * 100;
                string percentageText = $"{percentage:0.0}%";
                float sweepAngle = ((float)emp.HoursWorked / totalHours) * 360;
                Color color =  Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256)) ;
              
                   
                Brush pieBrush = new SolidBrush(color);
                graphics.FillPie(pieBrush, rect, startAngle, sweepAngle);
               
           
                graphics.FillRectangle(pieBrush, legendX, legendY + (i * 30), 20, 20);
              
                graphics.DrawString(emp.Name+ " " + percentageText, font, brush, legendX + 25, legendY + (i * 30));
                i++;
                startAngle += sweepAngle;
            }
            bitmap.Save(stream, ImageFormat.Png);
            graphics.Dispose();
            bitmap.Dispose();

        }


    }
}
