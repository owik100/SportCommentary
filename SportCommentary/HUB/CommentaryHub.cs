using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using NuGet.Common;
using SportCommentaryDataAccess.DTO.SingleCommentary;

namespace SportCommentary.HUB
{
    public class CommentaryHub : Hub
    {
        private readonly static ConnectionManager _connections = new ConnectionManager();

        public async Task SendSingleComment(SingleCommentDTO newSingleComment)
        {
            int commentaryId = newSingleComment.CommentaryID;
            var connectedClientsToCommentay = _connections.GetConnections(commentaryId);
            if(connectedClientsToCommentay != null && connectedClientsToCommentay.Count() > 0)
            {
                foreach (var user in connectedClientsToCommentay)
                {
                    await Clients.Client(user).SendAsync("ReceiveSingleComment", newSingleComment);
                }
            }
        }

        public override Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            string commentryId = httpContext.Request.Query["commentryId"];

            _connections.Add(Convert.ToInt32(commentryId), Context.ConnectionId);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var httpContext = Context.GetHttpContext();
            string commentryId = httpContext.Request.Query["commentryId"];

            _connections.Remove(Convert.ToInt32(commentryId), Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
