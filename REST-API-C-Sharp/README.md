# BoldDesk REST API - C# Samples

This folder contains C# code samples for interacting with the BoldDesk REST API.

## Prerequisites

- .NET SDK
- HttpClient (built into .NET)

## Configuration

Before running any sample, update the following variables:

```csharp
string domain = "YOUR_DOMAIN";    // Your BoldDesk domain (e.g., "yourcompany.bolddesk.com")
string apiKey = "YOUR_API_KEY";    // Your API key
```

## Available Samples

### Tickets

| File | Description |
|------|-------------|
| `ListTickets.cs` | List tickets with filtering and pagination |
| `GetTickets.cs` | Get details of a specific ticket |
| `CreateTicket.cs` | Create a new ticket |
| `EditTicket.cs` | Update an existing ticket |
| `DeleteTicket.cs` | Delete a ticket |
| `ReplyTicket.cs` | Reply to a ticket |

### Contacts

| File | Description |
|------|-------------|
| `ListContacts.cs` | List all contacts |
| `GetContacts.cs` | Get details of a specific contact |
| `CreateContacts.cs` | Create a new contact |

### Contact Groups

| File | Description |
|------|-------------|
| `ListContactGroups.cs` | List all contact groups |
| `GetContactGroups.cs` | Get details of a specific contact group |
| `CreateContactGroups.cs` | Create a new contact group |

## Authentication

All API requests use the `x-api-key` header for authentication:

```csharp
client.DefaultRequestHeaders.Add("x-api-key", apiKey);
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
```

## Running a Sample

```bash
dotnet run
```

Or compile and run directly:

```bash
dotnet script ListTickets.cs
```