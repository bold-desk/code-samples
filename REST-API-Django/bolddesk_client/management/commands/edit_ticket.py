from django.core.management.base import BaseCommand
import requests
import traceback
import json


class Command(BaseCommand):
    help = 'Edit ticket fields in BoldDesk via API (sample).'

    session = requests.Session()

    def add_arguments(self, parser):
        parser.add_argument('--domain', default='YOUR_DOMAIN', help='BoldDesk domain')
        parser.add_argument('--api-key', dest='api_key', default='YOUR_API_KEY', help='API key')
        parser.add_argument('--id', dest='ticket_id', required=True, help='Ticket ID')
        parser.add_argument('--fields', dest='fields', default='{"priorityId": 2}', help='JSON object of fields to update')

    def handle(self, *args, **options):
        domain = options['domain']
        api_key = options['api_key']
        ticket_id = options['ticket_id']
        fields_raw = options['fields']

        try:
            fields = json.loads(fields_raw)
        except Exception:
            self.stderr.write('ERROR: --fields must be valid JSON')
            return

        url = f'https://{domain}/api/v1/tickets/{ticket_id}/update_fields?skipDependencyValidation=true'
        self.session.headers.update({'Accept': 'application/json', 'x-api-key': api_key})

        payload = {'fields': fields}

        try:
            r = self.session.put(url, json=payload)
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
