using Godot;
using System;

public partial class MainMenuBehavior : Node2D
{
    public override void _EnterTree()
    {
		JsonUtility.LoadSettings();
    }

	public override void _Notification(int what)
    {
		switch (what)
		{
			case (int)MainLoop.NotificationApplicationFocusOut:
				GetTree().Paused = true;
				break;

			case (int)MainLoop.NotificationApplicationFocusIn:
				GetTree().Paused = false;
				break;
		}
    }
}
