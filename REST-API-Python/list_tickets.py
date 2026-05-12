import requests
import traceback

# One Session instance should be reused (similar to a single HttpClient instance).
session = requests.Session()


def main():
    domain = "YOUR_DOMAIN"   # Your bolddesk domain name (e.g., "yourcompany.bolddesk.com")
    api_key = "YOUR_API_KEY" # Your API key

    # Your API path (same as C#): /api/v1/tickets
    api_path = "/api/v1/tickets"

    # Query parameters (same as C#):
    # FilterId=1&BrandIds=3&Page=1&PerPage=20&orderBy=ticketId
    #
    # Note: In your pasted C# snippet, '&amp;' is HTML encoding for '&'.
    # Using params dict avoids encoding issues.
    params = {
        "FilterId": 1,
        "BrandIds": 3,
        "Page": 1,
        "PerPage": 20,
        "orderBy": "ticketId"
    }

    base_url = f"https://{domain}".rstrip("/")
    url = f"{base_url}{api_path}"

    # Header Value for Content type + APIKEY Authentication (matches your C# headers)
    session.headers.update({
        "Accept": "application/json",
        "x-api-key": api_key
    })

    try:
        # Send GET request (equivalent to client.GetAsync(apiPath))
        response = session.get(url, params=params)

        # Read response text (like ReadAsStringAsync)
        response_text = response.text

        # Ensure success (like EnsureSuccessStatusCode)
        response.raise_for_status()

        # Print response (same as C#)
        print("Result: " + response_text)

        # If you prefer JSON output (only if API returns JSON), use:
        # print(response.json())

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