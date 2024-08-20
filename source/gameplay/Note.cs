using Godot;
using System;

public partial class Note : Node2D
{
	public NoteData noteData { get; set; }

	public bool pressed { get; set; } // Mainly for hold notes.
	float endHoldStrum;
	Sprite2D note;
	Sprite2D holdPiece;
	Sprite2D holdEnd;

	public AnimatedSprite2D strumNote { get; set; } // What strum note does this note belong in?
	public StrumNote strumNoteScript { get; set; }


	public override void _Ready()
	{
		note = GetNode<Sprite2D>("Note");
		holdPiece = GetNode<Sprite2D>("Piece");
		holdEnd = GetNode<Sprite2D>("End");
		if (JsonUtility.userData.downScroll)
		    holdEnd.FlipV = true;
	}

    public override void _Process(double delta)
    {
        if (pressed)
			GoodNoteHitHold();
    }

    public void SetupStrum(AnimatedSprite2D strum)
	{
		strumNote = strum;
		strumNoteScript = (StrumNote)strumNote;
	}

	public void SetupHold()
	{
		float length = noteData.length / SongUtility.stepCrochet - 1;

		endHoldStrum = SongUtility.stepCrochet * (length - 1) + SongUtility.stepCrochet;

		float translatedScale = SongUtility.stepCrochet / 30.8f * SongUtility.SONG.speed * length;

		holdPiece.Scale = new Vector2(1, translatedScale);

		float currentSize = 44 * translatedScale;

		Vector2 newPiecePosition = new Vector2(0, currentSize / 2);
		Vector2 newEndPosition = new Vector2(0, currentSize + 44 / 2);
		
		if (JsonUtility.userData.downScroll)
		{
			newPiecePosition = -newPiecePosition;
			newEndPosition = -newEndPosition;
		}

		holdPiece.Position = newPiecePosition;
		holdEnd.Position = newEndPosition;
	}

	void ResetHold()
	{
		pressed = false;
		note.Show();

		holdPiece.Scale = new Vector2(1, 1);
		holdEnd.Scale = new Vector2(1, 1);
		
		holdPiece.Position = new Vector2(0, 0);
		holdEnd.Position = new Vector2(0, 0);
	}

	void DisposeNote()
	{
		strumNoteScript.DisposeHitable();
		NotePool.Put(this);
	}

	public void GoodNoteHit()
	{
		if (noteData.length == 0)
			DisposeNote();
		else
		{
			note.Hide();
			pressed = true;
		}
	}

	void GoodNoteHitHold() // Almost a direct copy of SetupHold lol
	{
		float newLength = noteData.length * ((endHoldStrum - (SongUtility.songPosition - noteData.strumTime)) / endHoldStrum) / SongUtility.stepCrochet - 1;

		if (newLength < 0.1f)
		{
			ResetHold();
			DisposeNote();

			return;
		}

		float translatedScale = SongUtility.stepCrochet / 30.8f * SongUtility.SONG.speed * newLength;

		holdPiece.Scale = new Vector2(1, translatedScale);

		float currentSize = 44 * translatedScale;

		Vector2 newPiecePosition = new Vector2(0, currentSize / 2);
		Vector2 newEndPosition = new Vector2(0, currentSize + 4 / 2);

		if (JsonUtility.userData.downScroll)
		{			
			newPiecePosition = -newPiecePosition;
			newEndPosition = -newEndPosition;
		}

		holdPiece.Position = newPiecePosition;
		holdEnd.Position = newEndPosition;
	}

	public void MissNote()
	{
		if (noteData.length > 0)
			ResetHold();
			
		DisposeNote();
	}
}
