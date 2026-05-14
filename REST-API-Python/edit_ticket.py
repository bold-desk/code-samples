import requests
import traceback

# One Session instance should be reused (similar to a single HttpClient instance).
session = requests.Session()


def main():
    domain = "YOUR_DOMAIN"  # Your bolddesk domain name (e.g., "yourcompany.bolddesk.com")
    api_key = "YOUR_API_KEY"  # Your API key
    ticket_id = "YOUR_TICKET_ID"  # <-- Replace with actual ticket id (e.g., "12345")

    # Your API path (same as C#)
    api_path = f"/api/v1/tickets/{ticket_id}/update_fields"

    # Query string (same as C#)
    params = {
        "skipDependencyValidation": "true"
    }

    base_url = f"https://{domain}".rstrip("/")
    url = f"{base_url}{api_path}"

    # Header Value for Content type + APIKEY Authentication (matches your C# headers)
    session.headers.update({
        "Accept": "application/json",
        "x-api-key": api_key
    })

    try:
        # PUT data to edit ticket fields (matches TicketUpdate with 'fields' dictionary)
        ticket_data = {
            "fields": {
                "priorityId": 2
            }
        }

        # Send PUT request with JSON body (equivalent to PutAsJsonAsync)
        response = session.put(url, params=params, json=ticket_data)

        # Read response text (like ReadAsStringAsync)
        response_text = response.text

        # Ensure success (like EnsureSuccessStatusCode)
        response.raise_for_status()

        # Print response
        print(response_text)

    except requests.exceptions.HTTPError as ex:
        status_code = ex.response.status_code if ex.response is not None else None
        body = ex.response.text if ex.response is not None else ""
        print("ERROR:")
        print("Status Code:", status_code)
        print("Message:", str(ex))
        print("Response Body:", body)
        print("StackTrace:")
        print(traceback.format_exc())

    except Exception as ex:
        print("ERROR:")
        print("Status Code: N/A")
        print("Message:", str(ex))
        print("StackTrace:")
        print(traceback.format_exc())


if __name__ == "__main__":
    main()