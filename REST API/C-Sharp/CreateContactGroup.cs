using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;


public class CreateContactGroup
{
    static HttpClient client = new HttpClient(); // One HttpClient instance be reused.
                                                 //HttpClient is used to make WebAPI Request from console application.

    static async Task Main(string[] args)
    {
        string domain = "YOUR_DOMAIN"; //Your bolddesk domain name.

        string apiPath = "/api/v1/contact_groups"; //Your API path.

        string apiKey = "YOUR_API_KEY"; //Your API key.

        client.BaseAddress = new Uri("https://" + domain); //Generate API path to call.

        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); //Header Value for Content type.

        client.DefaultRequestHeaders.Add("x-api-key", apiKey); //For APIKEY Authentication.

        try
        {
            //Post data to create contact groups.
            var contactGroupData = new ContactGroups
            {
                ContactGroupName = "Administration",   //sample data...
                ContactGroupDescription = "Sample Description",
                ContactGroupNotes = "Sample_Notes",
                ContactGroupExternalReferenceId = "1",
                ContactGroupAddress = "America",
                ContactGroupTag = "Sample_Tag",
                ContactGroupDomain = "yahodo.com, redhat.com",
                CustomFields = new Dictionary<string, object?>
                {
                    { "cf_configure": "2023-09-16" },
                    { "cf_contact_group_field_test": "test" },
                    { "cf_group_report": "test"}
                };
            };

            HttpResponseMessage response = await client.PostAsJsonAsync(apiPath, contactGroupData).ConfigureAwait(false);//To send a POST request as an asynchronous operation to the specified Uri with the given value serialized as JSON.

            var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false); //To read the response.

            response.EnsureSuccessStatusCode(); //For Confirms whether the response is success or Not.

            Console.WriteLine("Result:" + responseData); //Return the response.
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


public class ContactGroups
{
    public string ContactGroupName { get; set; }

    public string ContactGroupDescription { get; set; }

    public string ContactGroupNotes { get; set; }

    public string ContactGroupExternalReferenceId { get; set; }

    public string ContactGroupAddress { get; set; }

    public string ContactGroupTag { get; set; }

    public string ContactGroupDomain { get; set; }

    public Dictionary<string, object?>? CustomFields { get; set; }
}
