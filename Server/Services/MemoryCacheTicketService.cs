namespace Server.Services
{
	using Microsoft.Extensions.Caching.Memory;
	using System;


	/// <summary>
	///		A ticket service implemented using ASP.NET Core's memory cache service.
	/// </summary>
	public class MemoryCacheTicketService : ITicketService
	{
		private readonly IMemoryCache _tickets;

		/// <summary>
		///		Returns the username associated with the specified service ticket.
		/// </summary>
		/// <param name="ticket">
		///		The service ticket to retrieve from the collection
		/// </param>
		/// <returns>
		///		The username of the authenticated user
		/// </returns>
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

		/// <summary>
		///		Inserts the specified service ticket into the collection along with a username.
		/// </summary>
		/// <param name="ticket">
		///		The service ticket that was issued for the authenticated user
		/// </param>
		/// <param name="username">
		///		The username of the authenticated user
		/// </param>
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

		/// <summary>
		///		Default constructor that receives an instance of the memory cache provider via dependency injection.
		/// </summary>
		/// <param name="memoryCache">
		///		An instance of the memory cache provider
		/// </param>
		public MemoryCacheTicketService(IMemoryCache memoryCache)
		{
			_tickets = memoryCache;
		}
	}
}
