using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class Createcontact
{
    static HttpClient client = new HttpClient(); // One HttpClient instance be reused.
                                                 //HttpClient is used to make WebAPI Request from console application.

    static async Task Main(string[] args)
    {
        string domain = "YOUR_DOMAIN"; //Your bolddesk domain name.

        string apiPath = "/api/v1/contacts"; //Your API path.

        string apiKey = "YOUR_API_KEY"; //Your API key.

        client.BaseAddress = new Uri("https://" + domain); //Generate API path to call.

        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); //Header Value for Content type.

        client.DefaultRequestHeaders.Add("x-api-key", apiKey); //For Authentication access.

        try
        {
            //Post data to create contacts.
            var contactData = new Contact
            {
                ContactName = "James",
                EmailId = "james@example.com",
                SecondaryEmailId = "larry@example.com",
                ContactDisplayName = "Jade",             //Sample Data's for Required fields..        
                ContactPhoneNo = "9876545789",
                ContactMobileNo = "9876512345",
                ContactAddress = "4th Lane, East America",
                ContactJobTitle = "Support Engineer",
                TimeZoneId = 1,
                LanguageId = 1,
                ContactNotes = "Sample Note",
                ContactExternalReferenceId = "1",
                ContactTag = "Sample_Tag",
                IsVerified = true
            };

            HttpResponseMessage response = await client.PostAsJsonAsync(apiPath, contactData).ConfigureAwait(false);//To send a POST request as an asynchronous operation to the specified Uri with the given value serialized as JSON.

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

public class Contact
{
    public string ContactName { get; set; }

    public string EmailId { get; set; }

    public string SecondaryEmailId { get; set; }

    public string ContactDisplayName { get; set; }

    public string ContactPhoneNo { get; set; }

    public string ContactMobileNo { get; set; }

    public string ContactAddress { get; set; }

    public string ContactJobTitle { get; set; }

    public int TimeZoneId { get; set; }

    public int LanguageId { get; set; }

    public string ContactNotes { get; set; }

    public string ContactExternalReferenceId { get; set; }

    public string ContactTag { get; set; }

    public bool IsVerified { get; set; }

}
