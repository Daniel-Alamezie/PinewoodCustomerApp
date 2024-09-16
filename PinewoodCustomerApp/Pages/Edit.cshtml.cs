using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PinewoodCustomerApp.Models;

namespace PinewoodCustomerApp.Pages
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public EditModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public Customer Customer { get; set; }

        /// <summary>
        /// Handles the page load and fetches the customer by ID.
        /// </summary>
        public async Task<IActionResult> OnGet(int id)
        {
            // Fetch the customer by ID from the API
            Customer = await _httpClient.GetFromJsonAsync<Customer>($"https://localhost:7040/api/customer/{id}");

            if (Customer == null)
            {
                return RedirectToPage("/Index"); // Redirect to index if customer not found
            }

            return Page();
        }

        /// <summary>
        /// Handles the form submission and updates the customer details.
        /// </summary>
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Send the updated customer details to the API using Customer.Id
            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7040/api/customer/{Customer.Id}", Customer);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Index"); // Redirect back to the customer list after successful update
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed to update customer.");
                return Page();
            }
        }
    }
}
