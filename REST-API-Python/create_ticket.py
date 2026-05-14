import requests
import traceback

# One Session instance should be reused (similar to a single HttpClient instance).
session = requests.Session()


def main():
    domain = "YOUR_DOMAIN"         # Your bolddesk domain name (e.g., "yourcompany.bolddesk.com")
    api_path = "/api/v1/tickets"   # Your API path
    api_key = "YOUR_API_KEY"       # Your API key

    base_url = f"https://{domain}".rstrip("/")
    url = f"{base_url}{api_path}"

    # Header Value for Content type + APIKEY Authentication (matches your C# headers)
    session.headers.update({
        "Accept": "application/json",
        "x-api-key": api_key
    })

    try:
        # Post data to create ticket (sample required fields)
        #
        # NOTE on JSON casing:
        # In C#, System.Text.Json often serializes PascalCase properties as camelCase by default.
        # BrandId -> brandId, PriorityId -> priorityId, etc.
        ticket_data = {
            "brandId": 8,
            "subject": "Sample",
            "description": "Create ticket sample",
            "priorityId": 2
        }

        # Send POST request with JSON body
        response = session.post(url, json=ticket_data)

        # Read response text (like ReadAsStringAsync)
        response_text = response.text

        # Ensure success (like EnsureSuccessStatusCode)
        response.raise_for_status()

        # Print response
        print(response_text)

    except requests.exceptions.HTTPError as ex:
        # HTTP error with status code + response body
        status_code = ex.response.status_code if ex.response is not None else None
        body = ex.response.text if ex.response is not None else ""
        print("ERROR:")
        print("Status Code:", status_code)
        print("Message:", str(ex))
        print("Response Body:", body)
        print("StackTrace:")
        print(traceback.format_exc())

    except Exception as ex:
        # Other errors (network issues, runtime errors, etc.)
        print("ERROR:")
        print("Status Code: N/A")
        print("Message:", str(ex))
        print("StackTrace:")
        print(traceback.format_exc())


if __name__ == "__main__":
    main()