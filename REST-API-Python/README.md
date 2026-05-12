# BoldDesk REST API - Python Samples

This folder contains Python code samples for interacting with the BoldDesk REST API.

## Prerequisites

- Python 3.x
- `requests` library (`pip install requests`)

## Configuration

Before running any sample, update the following variables in each file:

```python
domain = "YOUR_DOMAIN"   # Your BoldDesk domain (e.g., "yourcompany.bolddesk.com")
api_key = "YOUR_API_KEY"  # Your API key
```

## Available Samples

### Tickets

| File | Description |
|------|-------------|
| `list_tickets.py` | List tickets with filtering and pagination |
| `get_ticket_details.py` | Get details of a specific ticket |
| `create_ticket.py` | Create a new ticket |
| `edit_ticket.py` | Update an existing ticket |
| `delete_ticket.py` | Delete a ticket |
| `reply_ticket.py` | Reply to a ticket |

### Contacts

| File | Description |
|------|-------------|
| `list_contacts.py` | List all contacts |
| `get_contact.py` | Get details of a specific contact |
| `create_contacts.py` | Create a new contact |

### Contact Groups

| File | Description |
|------|-------------|
| `list_contact_groups.py` | List all contact groups |
| `get_contact_group.py` | Get details of a specific contact group |
| `create_contact_groups.py` | Create a new contact group |

## Authentication

All API requests require the `x-api-key` header with your API key:

```python
session.headers.update({
    "Accept": "application/json",
    "x-api-key": api_key
})
```

## Running a Sample

```bash
python list_tickets.py
```