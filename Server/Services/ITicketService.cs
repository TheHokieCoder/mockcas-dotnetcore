namespace Server.Services
{
	public interface ITicketService
	{
		string GetTicket(string ticket);
		void InsertTicket(string ticket, string username);
	}
}
