using Godot;
using System;

public partial class StrumNote : AnimatedSprite2D
{
	[Export]
	public string action { get; set; }

	bool _auto;
	[Export]
	public bool auto { get{return _auto;} set {
		if (value)
			AnimationFinished += AssignAutoAnimation;
		else
			AnimationFinished -= AssignAutoAnimation;
		_auto = value;
	}} // Is it botplay?

	public bool playAnim { get; set; }

	Node2D hitable;
	Note hitableScript;

	public Character characterScript { get; set; }

	public override void _Process(double delta)
	{
		if (!auto)
		{
			if (Input.IsActionJustPressed(action))
			{
				if (hitable == null)
				{
					if (!JsonUtility.userData.ghostTapping)
						MissGhostTapping();

					Play("pressed");
				}
				else
				{
					Play("confirm");
					GoodNoteHit();
				}
			}

			if (Input.IsActionPressed(action))
			{
				if (!playAnim)
					characterScript.PlayAnimation(action);
			}

			if (Input.IsActionJustReleased(action))
			{
				if (hitable != null)
				    hitableScript.pressed = false;
					
				Play("static");
			}
		}
		else
		{
			if (hitable != null)
			{
				Play("confirm");

				if (!playAnim)
					characterScript.PlayAnimation(action);
				GoodNoteHit();
			}
		}
	}

	void GoodNoteHit()
	{
		if (hitableScript.noteData.id < SongUtility.SONG.keys)
		{
		    SongUtility.health += 0.06f;

			SetScore(true);
		}
			
		hitableScript.GoodNoteHit();
	}

	public void MissNote()
	{
		if (hitableScript.noteData.id < SongUtility.SONG.keys)
		{
			SongUtility.health -= 0.04f;
			++SongUtility.misses;

			SetScore(true);
		}

		characterScript.PlayAnimation(action + "-miss");

		hitableScript.MissNote();
	}

	public void MissGhostTapping()
	{
		SongUtility.score -= 200;
		SongUtility.health -= 0.04f;
		++SongUtility.misses;

		SetScore(false);
	}

	void SetScore(bool includeFactor) // Super accurate, but too punishing. I should probably nerf it later.
	{
		if (includeFactor)
		{
			float factor = 100f - Mathf.Abs(SongUtility.songPosition - hitableScript.noteData.strumTime);

			SongUtility.score += (int)factor;
			SongUtility.accuracy.Add(factor);
		}

		GameController.instance.UpdateInfo();

		if (SongUtility.health <= 0)
		{
			AudioServer.SetBusVolumeDb(0, 0);
			GetTree().ChangeSceneToFile("res://scenes/songs/" + SongUtility.SONG.name + ".tscn");
		}
	}

	public void AssignHitable(Node2D note)
	{
		if (hitable == null)
		{
			hitable = note;
			hitableScript = (Note)hitable;
		}
	}

	public void DisposeHitable()
	{
		hitable = null;
		hitableScript = null;
	}

	void AssignAutoAnimation() // For the "auto" variable. When "auto" is set, the strum animation must be completed automatically. Else, is completed manually.
	{
		Play("static");
	}
}
