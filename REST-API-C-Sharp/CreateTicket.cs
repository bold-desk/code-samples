using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public class CreateTicket
{
    static HttpClient client = new HttpClient(); // One HttpClient instance be reused.
                                                 //HttpClient is used to make WebAPI Request from console application.

    static async Task Main(string[] args)
    {
        string domain = "YOUR_DOMAIN"; //Your bolddesk domain name.

        string apiPath = "/api/v1/tickets"; //Your API path.

        string apiKey = "YOUR_API_KEY"; //Your API key.

        client.BaseAddress = new Uri("https://" + domain); //Generate API path to call.

        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); //Header Value for Content type.

        client.DefaultRequestHeaders.Add("x-api-key", apiKey); //For APIKEY Authentication.

        try
        {
            //Post data to create ticket.
            var ticketData = new Ticket    //Sample Data's for Required fields
            {
                BrandId = 8,
                Subject = "Sample",
                Description = "Create ticket sample",
                PriorityId = 2,             
            };

            HttpResponseMessage response = await client.PostAsJsonAsync(apiPath, ticketData).ConfigureAwait(false);//To send a POST request as an asynchronous operation to the specified Uri with the given value serialized as JSON.

            var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false); //To read the response.

            response.EnsureSuccessStatusCode(); //For Confirms whether the response is success or Not.

            Console.WriteLine(responseData); //Return the response.
        }

        //To catch the error message.
        catch (Exception ex)
        {
            Console.WriteLine("ERROR: ");
            Console.WriteLine("Status Code: " + ex.StatusCode); //Returns the error status.
            Console.WriteLine(ex.Message);//Returns the error message.
            Console.WriteLine(ex.StackTrace);//Returns the stack at the time uncaught exception occurs.  
        }

    }
}

public class Ticket
{
    public int BrandId { get; set; }

    public string Subject { get; set; }

    public string Description { get; set; }

    public int PriorityId { get; set; }
}
