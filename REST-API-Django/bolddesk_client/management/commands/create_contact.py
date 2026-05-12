from django.core.management.base import BaseCommand
import requests
import traceback


class Command(BaseCommand):
    help = 'Create a contact in BoldDesk via API (sample).'

    # Reuse a single Session instance (similar to HttpClient in C#)
    session = requests.Session()

    def add_arguments(self, parser):
        parser.add_argument('--domain', default='YOUR_DOMAIN', help='BoldDesk domain (e.g., yourcompany.bolddesk.com)')
        parser.add_argument('--api-key', dest='api_key', default='YOUR_API_KEY', help='API key')
        parser.add_argument('--name', dest='contact_name', default='James', help='Contact name')
        parser.add_argument('--email', dest='email', default='james@example.com', help='Email ID')
        parser.add_argument('--display-name', dest='display_name', default='Jade', help='Contact display name')

    def handle(self, *args, **options):
        domain = options['domain']
        api_path = '/api/v1/contacts'
        api_key = options['api_key']
        contact_name = options['contact_name']
        email = options['email']
        display_name = options['display_name']

        base_url = f'https://{domain}'.rstrip('/')
        url = f'{base_url}{api_path}'

        # Header Value for Content type + APIKEY Authentication (matches C# headers)
        self.session.headers.update({
            'Accept': 'application/json',
            'x-api-key': api_key
        })

        try:
            # Post data to create contact (matching C# property names as JSON keys)
            contact_data = {
                'contactName': contact_name,
                'emailId': email,
                'contactDisplayName': display_name
            }

            response = self.session.post(url, json=contact_data)

            # Read the response (like ReadAsStringAsync in C#)
            response_text = response.text

            # Confirm whether the response is success or not (like EnsureSuccessStatusCode)
            response.raise_for_status()

            self.stdout.write(self.style.SUCCESS('Result: ' + response_text))

        except requests.exceptions.HTTPError as ex:
            status_code = ex.response.status_code if ex.response is not None else None
            self.stderr.write('ERROR:')
            self.stderr.write(f'Status Code: {status_code}')
            self.stderr.write(f'Message: {str(ex)}')
            self.stderr.write('StackTrace:')
            self.stderr.write(traceback.format_exc())

        except Exception as ex:
            self.stderr.write('ERROR:')
            self.stderr.write('Status Code: N/A')
            self.stderr.write(f'Message: {str(ex)}')
            self.stderr.write('StackTrace:')
            self.stderr.write(traceback.format_exc())
