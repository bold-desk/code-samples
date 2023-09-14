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
                BrandId = 3,
                Subject = "Sample ticket",
                CategoryId = 6321,
                IsVisibileInCustomerPortal = true,
                RequesterId = 13346,
                Description = "<p>Sample ticket for testing</p>",
                AgentId = 4120,
                GroupId = 693,
                Attachments = "tokenValue_1, tokenValue_2",
                PriorityId = 2,
                DueDate = "2022-02-16T12:38:29.766Z",
                TypeId = 1,
                Tag = "21st-june,1212,1/0,0.8777419652677063",
                IsSpam = false,
                RequesterEmailId = "Jhon123@gmail.com",
                RequesterName = "Jhon",
                ContactGroupId = 1,
                PhoneNumber = "9562314786",
                CustomFields = new Dictionary<string, object?>()
                {
                    { "cf_ticket_radio": true },
                    { "cf_ticket_singleline": "Sample Single Line" }
                };
            };

            var ticketData1 = new Ticket1
            {
                BrandId = 3,
                Subject = "Sample ticket1",
                Description = "<p>Create sample ticket</p>",
                PriorityId = 2,
                RequesterName = "Mathan",
                CC = [12131, 13187, 9055],
                ContactGroupId = 959 ,
                WatchersUserId = "11956,6355,347",
                IsDataConsentGiven = false,
                AtMentionedUserIds = [13187, 347, 9055],
                SkipEmailNotification = true,
                LinkTypeId = 1,
                LinkedTicketId = 63457,
            };

            var ticketData2 = new Ticket2
            {
                BrandId = 4,
                Subject = "Sample Ticket2",
                Description = "<p>Testing sample ticket</p>",
                PriorityId = 2,
                RequesterName = "Kavin",
                CategoryId = 5654,
                IsVisibileInCustomerPortal = true,
                RequesterId =  13348,
                AgentId = 12623,
                AtMentionedUserIds = [347, 11956, 765],
                SkipEmailNotification = false,
                LinkTypeId = 1,
                LinkedTicketId = 63458,
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

    public string RequesterName { get; set; }

    public long CategoryId { get; set; }

    public bool IsVisibileInCustomerPortal { get; set; }

    public long RequesterId { get; set; }

    public long AgentId { get; set; }

    public int GroupId { get; set; }

    public string Attachments { get; set; }

    public string DueDate { get; set; }

    public long TypeId { get; set; }

    public string Tag { get; set; }

    public bool IsSpam { get; set; }

    public string RequesterEmailId { get; set; }

    public long ContactGroupId { get; set; }

    public string PhoneNumber { get; set; }

    public Dictionary<string, object?>? CustomFields { get; set; }
}

public class Ticket1
{
    public int BrandId { get; set; }

    public string Subject { get; set; }

    public string Description { get; set; }

    public int PriorityId { get; set; }

    public string RequesterName { get; set; }

    public List<long>? CC { get; set; }

    public long? ContactGroupId { get; set; }

    public string? WatchersUserId { get; set; }

    public bool? IsDataConsentGiven { get; set; }

    public List<long>? AtMentionedUserIds { get; set; }

    public bool SkipEmailNotification { get; set; }

    public int? LinkTypeId { get; set; }

    public long? LinkedTicketId { get; set; }
}

public class Ticket2
{
    public int BrandId { get; set; }

    public string Subject { get; set; }

    public string Description { get; set; }

    public int PriorityId { get; set; }

    public string RequesterName { get; set; }

    public long CategoryId { get; set; }

    public bool IsVisibileInCustomerPortal { get; set; }

    public long RequesterId { get; set; }

    public long AgentId { get; set; }

    public List<long>? AtMentionedUserIds { get; set; }

    public bool SkipEmailNotification { get; set; }

    public int? LinkTypeId { get; set; }

    public long? LinkedTicketId { get; set; }
}
