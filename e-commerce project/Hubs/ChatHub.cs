using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_project.Hubs
{
	public class ChatHub : Hub
	{
		public async Task Send(string message)
		{
			await this.Clients.All.SendAsync("Send", message);
		}
		public async Task NotifyAdmin(string message)
		{
			await this.Clients.Others.SendAsync("NotifyAdmin", message);
		}
		public override async Task OnConnectedAsync()
		{
			await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} вошел в чат");
			await base.OnConnectedAsync();
		}
		public override async Task OnDisconnectedAsync(Exception exception)
		{
			await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} покинул в чат");
			await base.OnDisconnectedAsync(exception);
		}
	}
}
