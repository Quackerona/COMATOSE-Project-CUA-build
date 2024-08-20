using Godot;
using System;

public partial class Number : OptionModule
{
	[Export(PropertyHint.Enum, "int,float,double")]
	int numType;

	HSlider slider;
	Label valueLabel;

	public override void _Ready()
	{
		LoadOption();

		slider = GetNode<HSlider>("HSlider");
		slider.ValueChanged += Change;

		if (HasNode("Value"))
			valueLabel = GetNode<Label>("Value");

		switch (numType)
		{
			case 0:
				slider.Value = (int)Get();
				break;
			case 1:
				slider.Value = (float)Get();
				break;
			case 2:
				slider.Value = (double)Get();
				break;
		}
	}

    void Change(double value)
    {
		dynamic convertedValue = 0;
		switch (numType)
		{
			case 0:
				convertedValue = (int)value;
				break;
			case 1:
				convertedValue = (float)value;
				break;
			case 2:
				convertedValue = value;
				break;
		}

		Set(convertedValue);
		JsonUtility.SaveSettings();

		if (valueLabel != null)
        	valueLabel.Text = value.ToString();
    }

}
