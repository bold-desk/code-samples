# Django example: Create Contact Group (BoldDesk API)

This directory contains a minimal Django management command example that performs the same operation as the C# `CreateContactGroups.cs` sample: it POSTS a JSON payload to `/api/v1/contact_groups` with an `x-api-key` header.

Files:
- `bolddesk_client/management/commands/create_contact_group.py` — Django management command sample.
- `bolddesk_client/management/commands/create_contact.py` — Django management command sample for creating a contact.
- `bolddesk_client/management/commands/create_ticket.py` — Django management command sample for creating a ticket.
 - `bolddesk_client/management/commands/get_contact_group.py` — Get a contact group by ID.
 - `bolddesk_client/management/commands/list_contact_groups.py` — List contact groups.
 - `bolddesk_client/management/commands/get_contact.py` — Get a contact by ID.
 - `bolddesk_client/management/commands/list_contacts.py` — List contacts.
 - `bolddesk_client/management/commands/get_ticket.py` — Get ticket details by ID.
 - `bolddesk_client/management/commands/list_tickets.py` — List tickets.
 - `bolddesk_client/management/commands/edit_ticket.py` — Edit ticket fields.
 - `bolddesk_client/management/commands/delete_ticket.py` — Delete ticket.
 - `bolddesk_client/management/commands/reply_ticket.py` — Reply to a ticket.

Usage (within a Django project):

1. Copy the `bolddesk_client` directory into one of your Django project's apps and add the app name to `INSTALLED_APPS` in `settings.py`.
2. Install `requests` if you don't have it:

```bash
pip install requests
```

3. Run the commands (replace placeholders):

Create contact group:

```bash
python manage.py create_contact_group --domain yourcompany.bolddesk.com --api-key YOUR_API_KEY --name Administration
```

Create contact:

```bash
python manage.py create_contact --domain yourcompany.bolddesk.com --api-key YOUR_API_KEY --name James --email james@example.com --display-name Jade
```

Create ticket:

```bash
python manage.py create_ticket --domain yourcompany.bolddesk.com --api-key YOUR_API_KEY --brand-id 8 --subject "Sample" --description "Create ticket sample" --priority-id 2
```

Get a contact group:

```bash
python manage.py get_contact_group --domain yourcompany.bolddesk.com --api-key YOUR_API_KEY --id 123
```

List contact groups:

```bash
python manage.py list_contact_groups --domain yourcompany.bolddesk.com --api-key YOUR_API_KEY
```

Get a contact:

```bash
python manage.py get_contact --domain yourcompany.bolddesk.com --api-key YOUR_API_KEY --id 456
```

List contacts:

```bash
python manage.py list_contacts --domain yourcompany.bolddesk.com --api-key YOUR_API_KEY
```

Get ticket details:

```bash
python manage.py get_ticket --domain yourcompany.bolddesk.com --api-key YOUR_API_KEY --id 789
```

List tickets:

```bash
python manage.py list_tickets --domain yourcompany.bolddesk.com --api-key YOUR_API_KEY
```

Edit ticket fields (JSON for `--fields`):

```bash
python manage.py edit_ticket --domain yourcompany.bolddesk.com --api-key YOUR_API_KEY --id 789 --fields '{"priorityId":2}'
```

Delete ticket:

```bash
python manage.py delete_ticket --domain yourcompany.bolddesk.com --api-key YOUR_API_KEY --id 789
```

Reply to ticket:

```bash
python manage.py reply_ticket --domain yourcompany.bolddesk.com --api-key YOUR_API_KEY --id 789 --description "Thanks for contacting us" --reply-on-behalf
```

This mirrors the C# sample's behavior: reuses a session, sets `Accept: application/json` and `x-api-key` headers, posts `{'contactGroupName': 'Administration'}`, and prints the response or error details.
