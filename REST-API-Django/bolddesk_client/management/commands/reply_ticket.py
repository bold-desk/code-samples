from django.core.management.base import BaseCommand
import requests
import traceback


class Command(BaseCommand):
    help = 'Reply to a ticket in BoldDesk via API (sample).'

    session = requests.Session()

    def add_arguments(self, parser):
        parser.add_argument('--domain', default='YOUR_DOMAIN', help='BoldDesk domain')
        parser.add_argument('--api-key', dest='api_key', default='YOUR_API_KEY', help='API key')
        parser.add_argument('--id', dest='ticket_id', required=True, help='Ticket ID')
        parser.add_argument('--description', default='sample description', help='Reply description')
        parser.add_argument('--reply-on-behalf', dest='reply_on_behalf', action='store_true', help='Reply on behalf of requester')

    def handle(self, *args, **options):
        domain = options['domain']
        api_key = options['api_key']
        ticket_id = options['ticket_id']
        description = options['description']
        reply_on_behalf = options['reply_on_behalf']

        url = f'https://{domain}/api/v1/tickets/{ticket_id}/updates'
        self.session.headers.update({'Accept': 'application/json', 'x-api-key': api_key})

        payload = {
            'description': description,
            'replyOnBehalfOfRequester': bool(reply_on_behalf)
        }

        try:
            r = self.session.post(url, json=payload)
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
