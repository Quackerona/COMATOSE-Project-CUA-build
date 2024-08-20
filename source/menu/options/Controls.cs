using Godot;
using System;

public partial class Controls : OptionModule
{
	Panel panel;
	Label control;

	bool hover;
	bool pressed;

	public override void _Ready()
	{
		LoadOption();

		panel = GetNode<Panel>("Panel");
		panel.MouseEntered += () => hover = true;
		panel.MouseExited += () => hover = false;

		control = GetNode<Label>("Control");
		control.Text = OS.GetKeycodeString(((InputEventKey)InputMap.ActionGetEvents(setting)[0]).PhysicalKeycode).ToUpper(); // Gets the current keybind in Godot's InputMap
		control.ResetSize();
		control.Position = new Vector2(panel.Position.X + Mathf.RoundToInt(panel.Size.X / 2) - control.Size.X / 2, control.Position.Y);
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("leftClick"))
		{
			if (hover)
			{
			    pressed = true;
				panel.Modulate = new Color(100, 5, 1);
			}
			else
			{
				pressed = false;
				panel.Modulate = new Color(1, 1, 1);
			}
		}
	}

	public override void _Input(InputEvent @event)
    {
        base._Input(@event);

		if (pressed)
		{
			if (@event is InputEventKey key)
			{
				if (key.IsPressed())
				{
					InputMap.ActionEraseEvents(setting);
					InputMap.ActionAddEvent(setting, key);

					Set(key);
					JsonUtility.SaveSettings();

					control.Text = OS.GetKeycodeString(key.PhysicalKeycode).ToUpper();
					control.ResetSize();
					control.Position = new Vector2(panel.Position.X + Mathf.RoundToInt(panel.Size.X / 2) - control.Size.X / 2, control.Position.Y);
					pressed = false;

					panel.Modulate = new Color(1, 1, 1);
				}
			}
		}
    }
}
