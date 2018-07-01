using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using Microsoft.Bot.Builder.ConnectorEx;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;

namespace Bot_Application1
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                Func<ChannelAccount, bool> isChatbot =
                    channelAcct => channelAcct.Id == message.Recipient.Id;
                if (message.MembersAdded.Any(isChatbot))
                {
                    var memberAdded = message.MembersAdded.FirstOrDefault();
                    WebApiApplication._ConversationReference = message.ToConversationReference();
                }
                
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }


        [Route("api/Messages/GetDialogData")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetDialogData()
        {
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            if (WebApiApplication._ConversationReference != null)
            {
                var message = WebApiApplication._ConversationReference.GetPostToBotMessage();
                using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, message.AsMessageActivity()))
                {
                    var botDataStore = scope.Resolve<IBotDataStore<BotData>>();
                    var key = Address.FromActivity(message);
                    var botUserData = await botDataStore.LoadAsync(key, BotStoreType.BotUserData, default(CancellationToken));
                    var dialogData = botUserData.GetProperty<string>("dialog");
                    resp.Content = new StringContent($"<html><body>Dialogs:{dialogData}</body></html>", System.Text.Encoding.UTF8, @"text/html");
                }
            }


            return resp;
        }
    }
}