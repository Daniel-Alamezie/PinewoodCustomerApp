using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PinewoodCustomerApp.Models;

namespace PinewoodCustomerApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HttpClient _httpClient; 

        public List<Customer> Customers { get; set; } = new List<Customer>();


        /// <summary>
        /// All ui property bindings here
        /// </summary>
        [BindProperty]
        public Customer NewCustomer { get; set; }


        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task OnGet()
        {
            try
            {
                // Fetch all customers from the API
                Customers = await _httpClient.GetFromJsonAsync<List<Customer>>("https://localhost:7040/api/customer");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching customers.");
            }
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7040/api/customer", NewCustomer);

            if (response.IsSuccessStatusCode)
            {
                // After adding the customer, reload the page to reflect changes
                return RedirectToPage();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed to add customer.");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            // make a request to the delete endpoint
            var response = await _httpClient.DeleteAsync($"https://localhost:7040/api/customer/{id}");

            if (response.IsSuccessStatusCode)
            {
                // Reload the page to reflect the updated customer list after deletion
                return RedirectToPage();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed to delete customer !"); // show a failed to delete error 

                return Page();
            }
        }
    }
}