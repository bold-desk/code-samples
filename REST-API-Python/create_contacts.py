import requests
import traceback


# One Session instance should be reused (similar to a single HttpClient instance).
session = requests.Session()


def main():
    domain = "YOUR_DOMAIN"          # Your bolddesk domain name (e.g., "yourcompany.bolddesk.com")
    api_path = "/api/v1/contacts"   # Your API path.
    api_key = "YOUR_API_KEY"        # Your API key.

    base_url = f"https://{domain}".rstrip("/")
    url = f"{base_url}{api_path}"

    # Header Value for Content type + APIKEY Authentication (matches your C# headers)
    session.headers.update({
        "Accept": "application/json",
        "x-api-key": api_key
    })

    try:
        # Post data to create contacts (sample data for required fields)
        #
        # NOTE on JSON casing:
        # C# System.Text.Json commonly serializes PascalCase properties as camelCase by default.
        # So ContactName -> contactName, EmailId -> emailId, ContactDisplayName -> contactDisplayName
        contact_data = {
            "contactName": "James",
            "emailId": "james@example.com",
            "contactDisplayName": "Jade"
        }

        response = session.post(url, json=contact_data)

        # Read the response (like ReadAsStringAsync)
        response_text = response.text

        # Ensure success (like EnsureSuccessStatusCode)
        response.raise_for_status()

        print(response_text)

    except requests.exceptions.HTTPError as ex:
        # HTTP error with status code and response body
        status_code = ex.response.status_code if ex.response is not None else None
        body = ex.response.text if ex.response is not None else ""
        print("ERROR:")
        print("Status Code:", status_code)
        print("Message:", str(ex))
        print("Response Body:", body)
        print("StackTrace:")
        print(traceback.format_exc())

    except Exception as ex:
        # Any other exception (network errors, runtime issues, etc.)
        print("ERROR:")
        print("Status Code: N/A")
        print("Message:", str(ex))
        print("StackTrace:")
        print(traceback.format_exc())


if __name__ == "__main__":
    main()