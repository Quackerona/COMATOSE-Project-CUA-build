using Godot;
using System;

public partial class PauseButton : Control
{
	Label label;
	bool hover;

	public Action onClick { get; set; }
	
	public override void _Ready()
	{
		label = GetNode<Label>("Label");

		label.MouseEntered += OnMouseEntered;
		label.MouseExited += OnMouseExited;
	}

    public override void _Process(double delta)
	{
		if (hover)
		{
			if (Input.IsActionJustPressed("leftClick"))
			    onClick.Invoke();
		}
	}

	void OnMouseEntered()
    {
        hover = true;
		label.LabelSettings.FontColor = new Color(1, 1, 0);
    }

	void OnMouseExited()
    {
        hover = false;
		label.LabelSettings.FontColor = new Color(1, 1, 1);
    }
}
