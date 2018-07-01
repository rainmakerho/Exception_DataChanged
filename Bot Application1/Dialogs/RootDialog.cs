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
    public class RootDialog : IDialog<object>
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
            await context.PostAsync($"You sent {activity.Text} which was {length} characters@RootDialog");

            //save root state
            await context.PostAsync($"save dialog state@RootDialog");
            using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, context.Activity.AsMessageActivity()))
            {
                var botDataStore = scope.Resolve<IBotDataStore<BotData>>();
                var key = Address.FromActivity(context.Activity);
                var botUserData = await botDataStore.LoadAsync(key, BotStoreType.BotUserData, default(CancellationToken));
                var dialogData = botUserData.GetProperty<string>("dialog");
                botUserData.SetProperty<string>("dialog", "RootDialog:MessageReceivedAsync");
                await botDataStore.SaveAsync(key, BotStoreType.BotUserData, botUserData, default(CancellationToken));
                await botDataStore.FlushAsync(key, default(CancellationToken));
            }

            if (activity.Text == "dialog1")
            {
                context.Call(new Dialog1(), ResumeDialog1Async);
            }
            else
            {
                context.Wait(MessageReceivedAsync);
            }

            
        }


        private async Task ResumeDialog1Async(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            await context.PostAsync($"Now @RootDialog");
            context.Wait(MessageReceivedAsync);
        }
    }
}