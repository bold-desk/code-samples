using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class ReplyTicket
{
    static HttpClient client = new HttpClient(); // One HttpClient instance be reused.
                                                 //HttpClient is used to make WebAPI Request from console application.

    static async Task Main(string[] args)
    {
        string domain = "YOUR_DOMAIN"; //Your bolddesk domain name.

        string apiPath = "/api/v1/tickets/{ticketId}/updates"; //Your API path.

        string apiKey = "YOUR_API_KEY"; //Your API key.

        client.BaseAddress = new Uri("https://" + domain); //Generate API path to call.

        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); //Header Value for Content type.

        client.DefaultRequestHeaders.Add("x-api-key", apiKey); //For APIKEY Authentication.

        try
        {
            //Post data to reply ticket.
            var ticketData = new TicketUpdate    //Sample Data's to reply ticket.
            {
                Description = "sample description",
                Attachments = "tokenValue_1, tokenValue_2",
                AtMentionUserIds = new List<int> { 1, 2 },
                lastUpdateID = 12,
                lastRefreshDate = "2022-02-21T08:04:19.785Z",
                TimeSpent = 5,
                MessageTags = "tag1,tag2",
                UpdatedByUserIdOrEmailId = "12",
                SkimEmailNotification = true,
                TicketStatusId = 1,
                Cc = new List<int> { 1, 2 },
                ReplyOnBehalfOfRequester = false,
                DontAppendOnBehalfOfRequesterMessage = true
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

public class TicketUpdate
{
    public string Description { get; set; } = string.Empty;

    public string Attachments { get; set; }

    public List<int> AtMentionUserIds { get; set; }

    public long lastUpdateID { get; set; }

    public string lastRefreshDate { get; set; }

    public int TimeSpent { get; set; }

    public string MessageTags { get; set; }

    public string UpdatedByUserIdOrEmailId { get; set; }

    public bool SkimEmailNotification { get; set; }

    public int TicketStatusId { get; set; }

    public List<int> Cc { get; set; }

    public bool ReplyOnBehalfOfRequester { get; set; }

    public bool DontAppendOnBehalfOfRequesterMessage { get; set; }
}
