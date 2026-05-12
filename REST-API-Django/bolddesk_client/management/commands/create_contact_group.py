from django.core.management.base import BaseCommand
import requests
import traceback


class Command(BaseCommand):
    help = 'Create a contact group in BoldDesk via API (sample).'

    session = requests.Session()

    def add_arguments(self, parser):
        parser.add_argument('--domain', default='YOUR_DOMAIN', help='BoldDesk domain (e.g., yourcompany.bolddesk.com)')
        parser.add_argument('--api-key', dest='api_key', default='YOUR_API_KEY', help='API key')
        parser.add_argument('--name', default='Administration', help='Contact group name')

    def handle(self, *args, **options):
        domain = options['domain']
        api_path = '/api/v1/contact_groups'
        api_key = options['api_key']
        name = options['name']

        base_url = f'https://{domain}'.rstrip('/')
        url = f'{base_url}{api_path}'

        # Header Value for Content type + APIKEY Authentication (matches C# headers)
        self.session.headers.update({
            'Accept': 'application/json',
            'x-api-key': api_key
        })

        try:
            # Post data to create contact group (sample data...)
            contact_group_data = {
                'contactGroupName': name
            }

            response = self.session.post(url, json=contact_group_data)

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
