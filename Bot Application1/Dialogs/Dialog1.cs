using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;

namespace Bot_Application1.Dialogs
{
    [Serializable]
    public class Dialog1 : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;

            // return our reply to the user
            await context.PostAsync($"You sent {activity.Text} which was {length} characters@Dialog1");

            await context.PostAsync($"save dialog state@Dialog1");
            //save dialog state
            using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, context.Activity.AsMessageActivity()))
            {
                var botDataStore = scope.Resolve<IBotDataStore<BotData>>();
                var key = Address.FromActivity(context.Activity);
                var botUserData = await botDataStore.LoadAsync(key, BotStoreType.BotUserData, default(CancellationToken));
                var dialogData = botUserData.GetProperty<string>("dialog");
                botUserData.SetProperty<string>("dialog", "Dialog1:MessageReceivedAsync");
                await botDataStore.SaveAsync(key, BotStoreType.BotUserData, botUserData, default(CancellationToken));
                //await botDataStore.FlushAsync(key, default(CancellationToken));
            }

            if (activity.Text == "root")
            {
                context.Done(activity.Text);
            }
            else
            {
                context.Wait(MessageReceivedAsync);
            }
            
        }
    }
}