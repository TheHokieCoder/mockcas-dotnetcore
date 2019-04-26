namespace Server.Services
{
	/// <summary>
	///		An interface for implementing a service that manages service tickets for the CAS server.
	/// </summary>
	public interface ITicketService
	{
		/// <summary>
		///		Returns the username associated with the specified service ticket.
		/// </summary>
		/// <param name="ticket">
		///		The service ticket to retrieve from the collection
		/// </param>
		/// <returns>
		///		The username of the authenticated user
		/// </returns>
		string GetTicket(string ticket);
		/// <summary>
		///		Inserts the specified service ticket into the collection along with a username.
		/// </summary>
		/// <param name="ticket">
		///		The service ticket that was issued for the authenticated user
		/// </param>
		/// <param name="username">
		///		The username of the authenticated user
		/// </param>
		void InsertTicket(string ticket, string username);
	}
}
