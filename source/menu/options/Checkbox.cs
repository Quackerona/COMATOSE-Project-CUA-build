using Godot;
using System;
using System.Reflection;

public partial class Checkbox : OptionModule
{
	CheckBox checkBox;

	public override void _Ready()
	{
		LoadOption();

		checkBox = GetNode<CheckBox>("CheckBox");
		checkBox.Toggled += Toggle;

		checkBox.ButtonPressed = (bool)Get(); // Checks if the state of this checkbox matches that of the corresponding UserData variable
	}

    void Toggle(bool toggledOn)
    {
		Set(toggledOn);
		JsonUtility.SaveSettings();
    }

}
