using DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using WebApplicationWeb.Models;

namespace WebApplicationWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            List<Employee> employees = new List<Employee>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7117/");
            HttpResponseMessage response = await client.GetAsync("api/employees");
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                employees = JsonConvert.DeserializeObject<List<Employee>>(result);
            }
            return View(employees);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Employee employee = await GetEmployeeById(id);
            return View(employee);
        }

        private static async Task<Employee> GetEmployeeById(int id)
        {
            Employee employee = new Employee();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7117/");
            HttpResponseMessage response = await client.GetAsync($"api/employees/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                employee = JsonConvert.DeserializeObject<Employee>(result);
            }

            return employee;
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(Employee employee)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7117/");
            var response = await client.PostAsJsonAsync("api/employees", employee);
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7117/");
            HttpResponseMessage response = await client.DeleteAsync($"api/employees/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Employee employee = await GetEmployeeById(id);
            return View(employee);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Employee employee)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7117/");
            var response = await client.PutAsJsonAsync($"api/employees/{employee.Id}", employee);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}