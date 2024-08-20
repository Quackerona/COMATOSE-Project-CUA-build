using System.Collections.Generic;
using Godot;

public class BPMChangeEvent
{
	public int beatTime { get; set; }
	public float songTime { get; set; }
}

public class SongUtility
{
	public static SongData SONG { get; set; }

	public static List<string> difficulties { get; } = new List<string>(){"easy", "normal", "hard"};
	public static float bpms { get; private set; }
	public static float stepCrochet { get; private set; }
    public static float songPosition { get; set; }

	// Player Stats
	static float _health;
	public static float health { get {return _health;} set {
		_health = Mathf.Clamp(value, 0, 2);
	}}
	public static int score { get; set; }
	public static int misses { get; set; }
	public static int combo { get; set; }
	public static List<float> accuracy { get; set; } = new List<float>(){100};

	// Song Metadata
	public static int curStep { get; private set; }
	public static int curBeat { get; private set; }

	public static BPMChangeEvent bpmChangeMap { get; set; }

	public static void Init()
	{
		bpms = 0;
		stepCrochet = 0;
		songPosition = 0;

		curStep = 0;
		curBeat = 0;

		bpmChangeMap = new BPMChangeEvent();
	}

	public static void ResetSongMetadata()
	{
		health = 1;
		score = 0;
		misses = 0;
		combo = 0;
		accuracy = new List<float>(){100};
	}

	public static void ChangeBpm(float newBpm)
	{
		bpms = 60000f / newBpm;
		stepCrochet = bpms / 4;

		bpmChangeMap.beatTime = curBeat;
		bpmChangeMap.songTime = songPosition;
	}

	public static void CalculateSongMetadata()
	{
		float rawBeat = bpmChangeMap.beatTime + (songPosition - bpmChangeMap.songTime) / bpms;

		curStep = (int)(rawBeat * 4f);
		curBeat = (int)rawBeat;
	}
}
