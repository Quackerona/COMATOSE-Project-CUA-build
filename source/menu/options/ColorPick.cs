using Godot;
using System;

public partial class ColorPick : OptionModule
{
	ColorPicker colorPicker;
	ShaderMaterial bfColor;

	public override void _Ready()
	{
		LoadOption();

		bfColor = (ShaderMaterial)GetNode<AnimatedSprite2D>("../Bf").Material;

		colorPicker = GetNode<ColorPicker>("ColorPicker");
		colorPicker.ColorChanged += OnColorChanged;

		colorPicker.Color = (Color)Get();
		OnColorChanged(colorPicker.Color); // for some reason "colorPicker.Color" doesn't set off "ColorChanged"??
	}

    void OnColorChanged(Color color)
    {
        Set(color);
		JsonUtility.SaveSettings();
		
		bfColor.SetShaderParameter("u_replacement_color", color);
    }
}
