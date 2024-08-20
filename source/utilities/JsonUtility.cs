using System;
using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;

[Serializable]
public class NoteData
{
    public float strumTime { get; set; }
    public int id { get; set; }
    public float length { get; set; }

} 

[Serializable]
public class SongData
{
    public string name { get; set; }
    public float bpm { get; set; }
    public float speed { get; set; }
	public int keys { get; set; } = 4;
    public List<NoteData> notes { get; set; }
}

[Serializable]
public class UserData
{
    // Gameplay
    public bool downScroll { get; set; }
    public bool middleScroll { get; set; }
    public bool ghostTapping { get; set; } = true;

    public float songVolume { get; set; }

    int _fps = 60;
    public int fps { get {return _fps;} set {
        _fps = value;
        Engine.MaxFps = value;
    }}

    bool _vSync;
    public bool vSync { get{return _vSync;} set{
        _vSync = value;

        if (value) DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Enabled);
		else DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Disabled);
    }}

    // Appearance
    bool _fullscreen;
    public bool fullscreen { get {return _fullscreen;} set {
        _fullscreen = value;

        if (value) DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		else DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
    }}

    // Controls
    public InputEventKey arrowLeft { get; set; }
    public InputEventKey arrowDown { get; set; }
    public InputEventKey arrowUp { get; set; }
    public InputEventKey arrowRight { get; set; }
}

class JsonUtility
{
    public static UserData userData { get; private set; }

    public static void LoadSettings() // Loads settings
	{
        if (FileAccess.FileExists("user://UserData.json"))
		{
            using FileAccess settings = FileAccess.Open("user://UserData.json", FileAccess.ModeFlags.Read);

		    userData = JsonConvert.DeserializeObject<UserData>(settings.GetAsText());
        }
        else
            userData = new UserData();

        LoadKeybind();
	}

    static void LoadKeybind() // Loads in keybinds from saved settings
    {
        if (userData.arrowLeft != null)
        {
            InputMap.ActionEraseEvents("arrowLeft");
			InputMap.ActionAddEvent("arrowLeft", userData.arrowLeft);
        }

        if (userData.arrowDown != null)
        {
            InputMap.ActionEraseEvents("arrowDown");
			InputMap.ActionAddEvent("arrowDown", userData.arrowDown);
        }

        if (userData.arrowUp != null)
        {
            InputMap.ActionEraseEvents("arrowUp");
			InputMap.ActionAddEvent("arrowUp", userData.arrowUp);
        }

        if (userData.arrowRight != null)
        {
            InputMap.ActionEraseEvents("arrowRight");
			InputMap.ActionAddEvent("arrowRight", userData.arrowRight);
        }
    }

    public static void SaveSettings() // Save settings
	{
		using FileAccess settings = FileAccess.Open("user://UserData.json", FileAccess.ModeFlags.Write);

        settings.StoreString(JsonConvert.SerializeObject(userData));
	}

    public static void LoadSongContainer(string songName, int difficulty = 1) // Loads chart
	{
		using FileAccess chartData = FileAccess.Open("res://assets/songs/" + songName + "/" + songName + "-" + SongUtility.difficulties[difficulty] + ".json", FileAccess.ModeFlags.Read);

		SongUtility.SONG = JsonConvert.DeserializeObject<SongData>(chartData.GetAsText());
	}
}