from django.core.management.base import BaseCommand
import requests
import traceback


class Command(BaseCommand):
    help = 'Get a contact group by ID from BoldDesk API.'

    session = requests.Session()

    def add_arguments(self, parser):
        parser.add_argument('--domain', default='YOUR_DOMAIN', help='BoldDesk domain')
        parser.add_argument('--api-key', dest='api_key', default='YOUR_API_KEY', help='API key')
        parser.add_argument('--id', dest='group_id', required=True, help='Contact group ID')

    def handle(self, *args, **options):
        domain = options['domain']
        api_key = options['api_key']
        group_id = options['group_id']

        url = f'https://{domain}/api/v1/contact_groups/{group_id}'
        self.session.headers.update({'Accept': 'application/json', 'x-api-key': api_key})

        try:
            r = self.session.get(url)
            text = r.text
            r.raise_for_status()
            self.stdout.write(self.style.SUCCESS('Result: ' + text))
        except requests.exceptions.HTTPError as ex:
            status = ex.response.status_code if ex.response is not None else None
            self.stderr.write('ERROR:')
            self.stderr.write(f'Status Code: {status}')
            self.stderr.write(str(ex))
            self.stderr.write(traceback.format_exc())
        except Exception as ex:
            self.stderr.write('ERROR:')
            self.stderr.write(str(ex))
            self.stderr.write(traceback.format_exc())
