using Godot;
using System;

public partial class StateButton : Label
{
	[Export]
	string state;

	bool hover;

	public override void _Ready()
	{
		MouseEntered += OnMouseEntered;
		MouseExited += OnMouseExited;
	}


    public override void _Process(double delta)
	{
		if (hover)
		{
			if (Input.IsActionJustPressed("leftClick"))
				GetTree().ChangeSceneToFile("res://scenes/" + state + ".tscn");
		}
	}

	void OnMouseEntered()
    {
		hover = true;
        Modulate = new Color("yellow");
    }

	void OnMouseExited()
    {
		hover = false;
        Modulate = new Color("white");
    }
}
