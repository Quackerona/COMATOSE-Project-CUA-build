using Discord;
using Godot;

public partial class DiscordSDK : Node
{
	Discord.Discord discordSDK;
	
	// Rich Presence
	Activity activity;
	ActivityManager activityManager;
	public override void _Ready()
	{
		try{
			discordSDK = new Discord.Discord(1258987099705835571, (ulong)Discord.CreateFlags.NoRequireDiscord);

			activityManager = discordSDK.GetActivityManager();
		}
		catch{}

		activity = new Activity {
			Type = ActivityType.Playing,
			Assets = {
				LargeImage = "https://s12.gifyu.com/images/StseI.gif"
			}
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (discordSDK != null)
			discordSDK.RunCallbacks();
	}

	public void ChangePresence(string Details = "", string State = "")
	{
		if (discordSDK != null)
		{
			activity.State = State;
			activity.Details = Details;

			activityManager.UpdateActivity(activity, UpdateActivityCallback);
		}
	}
    // callbacks

    void UpdateActivityCallback(Result result)
    {
        
    }
}