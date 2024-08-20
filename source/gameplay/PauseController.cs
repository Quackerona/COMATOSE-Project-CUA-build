using Godot;
using System;

public partial class PauseController : Node
{
	AudioStreamPlayer pauseMusic;

	SubViewportContainer pauseViewportContainer;
	Viewport pauseViewport;

	bool paused;

	public override void _Ready()
	{
		pauseMusic = GetNode<AudioStreamPlayer>("../Pause");

		pauseViewportContainer = GetNode<SubViewportContainer>("../PauseViewportContainer");
		pauseViewport = pauseViewportContainer.GetNode<Viewport>("PauseViewport");

		pauseViewport.GetNode<PauseButton>("Panel/ButtonContainer/Resume").onClick += Resume;
		pauseViewport.GetNode<PauseButton>("Panel/ButtonContainer/Restart").onClick += Restart;
		pauseViewport.GetNode<PauseButton>("Panel/ButtonContainer/Botplay").onClick += Botplay;
		pauseViewport.GetNode<PauseButton>("Panel/ButtonContainer/Quit").onClick += Quit;
		pauseViewport.GetNode<HSlider>("Panel/ButtonContainer/Volume/HSlider").ValueChanged += Volume;
	}

    public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_accept"))
		{
			if (paused)
				Resume();
			else
				Pause();
		}
	}

	void Pause()
	{
		Input.MouseMode = Input.MouseModeEnum.Visible;

		if (!pauseMusic.Playing)
			pauseMusic.Play();

		paused = true;
		GetTree().Paused = true;

		pauseViewportContainer.Show();
	}

	void Resume()
	{
		Input.MouseMode = Input.MouseModeEnum.Hidden;

		pauseMusic.Stop();

		paused = false;
		GetTree().Paused = false;

		pauseViewportContainer.Hide();
	}

	void Restart()
	{
		pauseMusic.Stop();

		Resume();
		GetTree().ChangeSceneToFile("res://scenes/songs/" + SongUtility.SONG.name + ".tscn");
	}

	void Botplay()
	{
		Input.MouseMode = Input.MouseModeEnum.Hidden;

		pauseMusic.Stop();

		Resume();
		for (int i = 0; i < SongUtility.SONG.keys; i++)
		{
			StrumNote strumNote = (StrumNote)GameController.instance.strumNotes[i];
			
			strumNote.auto = !strumNote.auto;
		}
	}

	void Quit()
	{
		pauseMusic.Stop();

		Resume();
		Input.MouseMode = Input.MouseModeEnum.Visible;
		AudioServer.SetBusVolumeDb(0, 0);
		GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
	}

	void Volume(double value)
	{
		AudioServer.SetBusVolumeDb(0, (float)value);
	}

	public override void _Notification(int what)
    {
		switch (what)
		{
			case (int)MainLoop.NotificationApplicationFocusOut:
				Pause();
				break;
		}
    }
}
