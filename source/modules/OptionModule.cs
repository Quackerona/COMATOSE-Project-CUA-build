using Godot;
using System;
using System.Reflection;

public partial class OptionModule : Control
{
	[Export]
	public string setting { get; set; }

	Type userDataType;
	PropertyInfo fieldInfo;
	
	public void LoadOption()
	{
		userDataType = typeof(UserData);
		fieldInfo = userDataType.GetProperty(setting);
	}

	public void Set(object value)
	{
		fieldInfo.SetValue(JsonUtility.userData, value);
	}

	public object Get()
	{
		return fieldInfo.GetValue(JsonUtility.userData);
	}
}
