import requests
import traceback


class CreateContactGroup:
    # One Session instance should be reused (similar to a single HttpClient instance).
    session = requests.Session()

    @staticmethod
    def main():
        domain = "YOUR_DOMAIN"          # Your bolddesk domain name (e.g., "yourcompany.bolddesk.com")
        api_path = "/api/v1/contact_groups"  # Your API path.
        api_key = "YOUR_API_KEY"        # Your API key.

        base_url = f"https://{domain}".rstrip("/")
        url = f"{base_url}{api_path}"

        # Header Value for Content type + APIKEY Authentication (matches your C# headers)
        CreateContactGroup.session.headers.update({
            "Accept": "application/json",
            "x-api-key": api_key
        })

        try:
            # Post data to create contact group (sample data...)
            # C# property ContactGroupName -> typically serialized as "contactGroupName" in JSON.
            contact_group_data = {
                "contactGroupName": "Administration"
            }

            response = CreateContactGroup.session.post(url, json=contact_group_data)

            # To read the response (same as ReadAsStringAsync)
            response_text = response.text

            # Confirms whether the response is success or Not (like EnsureSuccessStatusCode)
            response.raise_for_status()

            print("Result:" + response_text)

        except requests.exceptions.HTTPError as ex:
            # HTTP error with response available
            status_code = ex.response.status_code if ex.response is not None else None
            print("ERROR:")
            print("Status Code:", status_code)
            print("Message:", str(ex))
            print("StackTrace:")
            print(traceback.format_exc())

        except Exception as ex:
            # Other errors (network, runtime, etc.)
            print("ERROR:")
            print("Status Code: N/A")
            print("Message:", str(ex))
            print("StackTrace:")
            print(traceback.format_exc())


if __name__ == "__main__":
    CreateContactGroup.main()