using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public class ListContacts
{
    static HttpClient client = new HttpClient(); // One HttpClient instance be reused.
                                                 //HttpClient is used to make WebAPI Request from console application.

    static async Task Main(string[] args)
    {
        string domain = "YOUR_DOMAIN"; //Your bolddesk domain name.

        string apiPath = "/api/v1/contacts?Page=1&PerPage=20&RequiresCounts=true&OrderBy=contactId"; //Your API path.

        string apiKey = "YOUR_API_KEY"; //Your API key.

        client.BaseAddress = new Uri("https://" + domain); //Generate API path to call.

        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); //Header Value for Content type.

        client.DefaultRequestHeaders.Add("x-api-key", apiKey); //For APIKEY Authentication.

        try
        {

            HttpResponseMessage response = await client.GetAsync(apiPath).ConfigureAwait(false);//To send a GET request as an asynchronous operation to the specified Uri.

            var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false); //To read the response.

            response.EnsureSuccessStatusCode(); //For Confirms whether the response is success or Not.

            Console.WriteLine("Result: " + responseData); //Return the response.
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
