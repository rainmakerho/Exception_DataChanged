using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Bot_Application1
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static ConversationReference _ConversationReference;
        protected void Application_Start()
        {
            var store = new InMemoryDataStore();
            Conversation.UpdateContainer(
                       builder =>
                       {
                           //builder.Register(c => new CachingBotDataStore(store,
                           //   CachingBotDataStoreConsistencyPolicy.LastWriteWins))
                           //   .As<IBotDataStore<BotData>>()
                           //   .AsSelf()
                           //   .SingleInstance();
                           //
                           //builder.Register(c => new CachingBotDataStore(store,
                           //           CachingBotDataStoreConsistencyPolicy.LastWriteWins))
                           //           .As<IBotDataStore<BotData>>()
                           //           .AsSelf()
                           //           .SingleInstance();
                           //builder
                           //  .RegisterType<InMemoryDataStore>()
                           //  .As<IBotDataStore<BotData>>()
                           //  .Keyed<IBotDataStore<BotData>>(typeof(ConnectorStore))
                           //  .SingleInstance();

                         
                       });

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
