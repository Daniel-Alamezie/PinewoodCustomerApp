using Microsoft.AspNetCore.Mvc;
using PinewoodCustomerApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace PinewoodCustomerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {

        // Mocking a db using the list data structure for simplicity;
        ///<summary>
        /// We are using the list data structure here because we dont need to worry about other dependency set up and it
        /// behaves just like a simple SQL database would, giving us flexibily with CRUD operations.
        ///</summary>
        private static List<Customer> customers = new List<Customer>();

        // Allows us to get individual customer recoord
        [HttpGet("{id}")]
        public IActionResult GetCustomer(int id)
        {
            var customer = customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }


        // Returns the list of customers
        [HttpGet]
        public IActionResult GetCustomers()
        {
            return Ok(customers);
        }


        /// <summary>
        /// endpoint to add a new customer
        /// </summary>
        [HttpPost]
        public IActionResult AddCustomer(Customer newCustomer)
        {
            // If the list is empty then make the new customer entry the first one, setting the list to 1.
            if (customers.Count == 0)
            {
                newCustomer.Id = 1;
            }
            else
            {
                newCustomer.Id = customers.Max(c => c.Id) + 1; // adds a new customer using the id as the key and incrementing the id on each addition

            }
            customers.Add(newCustomer);

            // return the newly created customer on success.
            return Ok(newCustomer);
        }

        ///<summary>
        /// endpoint to update an existing customer
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(int id, Customer updatedCustomer)
        {
            var customer = customers.FirstOrDefault(c => c.Id == id); // find the customer with this id

            if (customer == null) return NotFound(); // return error 404 if the customer does not exist

            // Proceed to update the customer information if they exist.
            customer.FullName = updatedCustomer.FullName;
            customer.PhoneNumber = updatedCustomer.PhoneNumber;
            customer.Email = updatedCustomer.Email;

            // return the new customer updated;
            return Ok(customer);
        }

        ///<summary>
        /// endpoint to delete an existing customer if they exist
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            var customer = customers.FirstOrDefault(c => c.Id == id);
            if (customer == null) return NotFound(); // return error 404 if the customer does not exist.

            customers.Remove(customer); // remove the customer from the list if it exists.

            return Ok();

        }

    }
}
