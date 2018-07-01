# Exception_DataChanged

Microsoft Bot Framework: Exception: The data is changed Demo Project

Demo IBotDataStore Call FlushAsync or not in Dialog

```csharp
using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, context.Activity.AsMessageActivity()))
{
	var botDataStore = scope.Resolve<IBotDataStore<BotData>>();
	var key = Address.FromActivity(context.Activity);
	var botUserData = await botDataStore.LoadAsync(key, BotStoreType.BotUserData, default(CancellationToken));
	botUserData.SetProperty<string>("dialog", "RootDialog:MessageReceivedAsync");
	await botDataStore.SaveAsync(key, BotStoreType.BotUserData, botUserData, default(CancellationToken));
    //await botDataStore.FlushAsync(key, default(CancellationToken));
}
```

You can ref [IBotDataStore.FlushAsync Exception: The data is changed ](https://rainmakerho.github.io/2018/07/01/2018020/) for detail
