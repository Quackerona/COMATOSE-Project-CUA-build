using Godot;
using System;

public partial class OptionsMenuBehavior : Node2D
{
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_cancel"))
			GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
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
