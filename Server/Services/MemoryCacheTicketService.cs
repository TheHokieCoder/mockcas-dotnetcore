namespace Server.Services
{
	using Microsoft.Extensions.Caching.Memory;
	using System;


	public class MemoryCacheTicketService : ITicketService
	{
		private readonly IMemoryCache _tickets;

		public string GetTicket(string ticket)
		{
			if (_tickets != null)
			{
				bool ticketFound = _tickets.TryGetValue(ticket, out string username);
				return ticketFound ? username : String.Empty;
			}
			else
			{
				throw new InvalidOperationException("Service ticket cannot be retrieved because the memory cache provider is not available.");
			}
		}

		public void InsertTicket(string ticket, string username)
		{
			if (_tickets != null)
			{
				_tickets.Set(ticket, username);
			}
			else
			{
				throw new InvalidOperationException("Service ticket cannot be stored because the memory cache provider is not available.");
			}
		}

		public MemoryCacheTicketService(IMemoryCache memoryCache)
		{
			_tickets = memoryCache;
		}
	}
}
