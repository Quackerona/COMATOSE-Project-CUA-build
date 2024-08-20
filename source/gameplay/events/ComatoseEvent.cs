using Godot;
using System;

public partial class ComatoseEvent : Node, EventInterface
{
    ColorRect red;

    ColorRect checker;

    ColorRect black;
    Label title;
    Label credit;

    ColorRect bg;
    
    float bloomValue;
	ShaderMaterial bloom;

    bool shake;
    Random randomShakeVal;

    ShaderMaterial newShader;

    public void AfterStart()
    {
        red = GameController.instance.gameViewport.GetNode<ColorRect>("Red");

        checker = GameController.instance.gameViewport.GetNode<ColorRect>("Checker");

        black = GameController.instance.gameViewport.GetNode<ColorRect>("BeginningLayer/Black");
        title = GameController.instance.gameViewport.GetNode<Label>("BeginningLayer/Title");
        credit = GameController.instance.gameViewport.GetNode<Label>("BeginningLayer/Credit");

        bg = GameController.instance.gameViewport.GetNode<ColorRect>("Bg");

        bloom = (ShaderMaterial)GetNode<ColorRect>("../BloomShader/ColorRect").Material;

        GameController.instance.gameCam.Position = new Vector2(640, 360);

        randomShakeVal = new Random();

        newShader = new ShaderMaterial();
        newShader.Shader = ResourceLoader.Load<Shader>("res://assets/shaders/Evil.gdshader");

        ShaderMaterial oldShader = (ShaderMaterial)checker.Material;

        checker.Material = newShader; // cache
        checker.Material = oldShader;
    }

    public void AfterUpdate(double delta)
    {
        bloomValue = Mathf.Lerp(bloomValue, 0, (float)delta * 3f);
		bloom.SetShaderParameter("intensity", bloomValue);

        if (shake)
        {
            float intensity = 4f;

            GameController.instance.gameCam.Position = new Vector2(640 + GetRandomNumber(-intensity, intensity), 360 + GetRandomNumber(-intensity, intensity));
            GameController.instance.hudCam.Position = new Vector2(640 + GetRandomNumber(-intensity, intensity), 360 + GetRandomNumber(-intensity, intensity));
        }
    }

    public void OnStep()
    {
        switch (SongUtility.curStep)
        {
            case 16:
                title.Show();
                credit.Show();
                break;
            case 48:
                {
                    using Tween tween = CreateTween();
                    tween.TweenProperty(title, "modulate:a", 0, 0.3);
                    tween.Parallel().TweenProperty(credit, "modulate:a", 0, 0.3);
                }

                GameController.instance.strumNotes[0].Show();
                break;
            case 50:
                GameController.instance.gameCam.Zoom += new Vector2(0.03f, 0.03f);
                GameController.instance.hudCam.Zoom += new Vector2(0.03f, 0.03f);

                GameController.instance.strumNotes[1].Show();
                break;
            case 52:
                GameController.instance.gameCam.Zoom += new Vector2(0.03f, 0.03f);
                GameController.instance.hudCam.Zoom += new Vector2(0.03f, 0.03f);

                GameController.instance.strumNotes[2].Show();
                break;
            case 54:
                GameController.instance.gameCam.Zoom += new Vector2(0.03f, 0.03f);
                GameController.instance.hudCam.Zoom += new Vector2(0.03f, 0.03f);

                GameController.instance.strumNotes[3].Show();
                break;
            case 56:
                GameController.instance.gameCam.Zoom += new Vector2(0.03f, 0.03f);
                GameController.instance.hudCam.Zoom += new Vector2(0.03f, 0.03f);

                GameController.instance.healthBar.Show();
                GameController.instance.score.Show();
                break;
            case 60:
                {
                    using Tween tween = CreateTween();
                    tween.TweenProperty(black, "modulate:a", 0, 0.2);
                }

                GameController.instance.gameCamZoom = new Vector2(0.5f, 0.5f);
                break;
            case 68:
                {
                    using Tween tween = CreateTween();
                    tween.TweenProperty(GameController.instance.player, "modulate", new Color("white"), 0.4);
                }
                break;
            case 189:
                {
                    using Tween tween = CreateTween();
                    tween.TweenProperty(GameController.instance.gameCam, "zoom", new Vector2(2, 2), 0.4).SetTrans(Tween.TransitionType.Circ);
                    tween.Parallel().TweenProperty(GameController.instance.player, "modulate", new Color("black"), 0.2).SetTrans(Tween.TransitionType.Circ);
                }
                break;
            case 192:
                bg.Hide();

                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);
                GameController.instance.player.Modulate = new Color("white");
                checker.Show();

                GameController.instance.playerScript.animPostFix = "-alt";
                shake = true;

                bloomValue = 5f;
                break;
            case 256:
                GameController.instance.player.Hide();
                checker.Hide();

                shake = false;
                break;
            case 258:
                GameController.instance.player.Show();
                break;
            case 260:
                shake = true;

                checker.Show();
                bloomValue = 5f;

                GameController.instance.gameCam.Zoom += new Vector2(1f, 1f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);

                break;
            case 320:
                bg.Show();

                {
                    using Tween tween = CreateTween();
                    tween.TweenProperty(red, "modulate:a", 1, 1);
                    tween.Parallel().TweenProperty(checker, "modulate:a", 0, 1);
                    tween.Parallel().TweenProperty(GameController.instance.player, "modulate", new Color("black"), 1);
                    tween.Parallel().TweenProperty(bg, "color", new Color("black"), 1);
                }

                GameController.instance.playerScript.animPostFix = "";

                shake = false;
                break;
            case 336:
                {
                    using Tween tween = CreateTween();
                    tween.TweenProperty(GameController.instance.player, "modulate", new Color(1, 0, 0), 1);
                }

                SongUtility.ChangeBpm(134);

                GameController.instance.gameCam.Zoom += new Vector2(0.3f, 0.3f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);

                shake = true;
                break;
            case 460:
                {
                    using Tween tween = CreateTween();
                    tween.TweenProperty(red, "modulate:a", 0, 0.2);
                    tween.TweenProperty(GameController.instance.player, "modulate:a", 0, 0.2);
                }
                break;
            case 464:
                SongUtility.ChangeBpm(148);

                shake = true;

                checker.Modulate = new Color("white");
                bloomValue = 5f;

                GameController.instance.player.Modulate = new Color("white");
                GameController.instance.playerScript.animPostFix = "-alt";

                GameController.instance.gameCam.Zoom += new Vector2(1f, 1f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);

                break;
            case 528:
                checker.Hide();

                shake = false;

                break;
            case 532:
                shake = true;

                checker.Show();
                bloomValue = 5f;

                GameController.instance.gameCam.Zoom += new Vector2(1f, 1f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);

                break;
            case 584:
                shake = false;

                GameController.instance.player.Hide();
                checker.Hide();

                checker.Material = newShader;

                break;
            case 596:
                shake = true;

                GameController.instance.player.Show();
                GameController.instance.player.Modulate = new Color(1, 0, 0);
                red.Modulate = new Color("white");

                GameController.instance.playerScript.animPostFix = "";

                GameController.instance.gameCam.Zoom += new Vector2(0.3f, 0.3f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);
                break;
            case 660:
                GameController.instance.gameCam.Zoom += new Vector2(0.3f, 0.3f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);
                break;
            case 666:
                GameController.instance.gameCam.Zoom += new Vector2(0.3f, 0.3f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);
                break;
            case 672:
                GameController.instance.gameCam.Zoom += new Vector2(0.3f, 0.3f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);
                break;
            case 676:
                GameController.instance.gameCam.Zoom += new Vector2(0.3f, 0.3f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);
                break;
            case 682:
                GameController.instance.gameCam.Zoom += new Vector2(0.3f, 0.3f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);
                break;
            case 688:
                GameController.instance.gameCam.Zoom += new Vector2(0.3f, 0.3f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);
                break;
            case 692:
                GameController.instance.gameCam.Zoom += new Vector2(0.3f, 0.3f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);
                break;
            case 698:
                GameController.instance.gameCam.Zoom += new Vector2(0.3f, 0.3f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);
                break;
            case 704:
                GameController.instance.gameCam.Zoom += new Vector2(0.3f, 0.3f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);
                break;
            case 708:
                GameController.instance.gameCam.Zoom += new Vector2(0.3f, 0.3f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);
                break;
            case 714:
                GameController.instance.gameCam.Zoom += new Vector2(0.3f, 0.3f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);
                break;
            case 720:
                GameController.instance.gameCam.Zoom += new Vector2(0.3f, 0.3f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);
                break;
            case 724:
                GameController.instance.player.Modulate = new Color("white");
                red.Hide();
                GameController.instance.playerScript.animPostFix = "-alt";
                black.Modulate = new Color("white");
                break;
            case 725:
                black.Hide();
                break;
            case 726:
                GameController.instance.gameCam.Zoom += new Vector2(0.3f, 0.3f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);
                break;
            case 727:
                GameController.instance.gameCam.Zoom += new Vector2(0.3f, 0.3f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);
                break;
            case 728:
                SongUtility.ChangeBpm(160);

                GameController.instance.gameCam.Zoom += new Vector2(0.3f, 0.3f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);
                break;
            case 788:
                GameController.instance.player.Hide();
                break;
            case 792:
                SongUtility.ChangeBpm(180);

                GameController.instance.player.Show();

                GameController.instance.gameCam.Zoom += new Vector2(0.3f, 0.3f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);

                checker.Show();

                bloomValue = 5f;
                break;
            case 912:
                GameController.instance.gameCam.Zoom += new Vector2(0.3f, 0.3f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);
                break;
            case 913:
                GameController.instance.gameCam.Zoom += new Vector2(0.3f, 0.3f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);
                break;
            case 914:
                GameController.instance.gameCam.Zoom += new Vector2(0.3f, 0.3f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);
                break;
            case 915:
                GameController.instance.gameCam.Zoom += new Vector2(0.3f, 0.3f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);
                break;
            case 916:
                GameController.instance.gameCam.Zoom += new Vector2(0.3f, 0.3f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);
                break;
            case 917:
                GameController.instance.gameCam.Zoom += new Vector2(0.3f, 0.3f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);
                break;
            case 918:
                GameController.instance.gameCam.Zoom += new Vector2(0.3f, 0.3f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);
                break;
            case 919:
                GameController.instance.gameCam.Zoom += new Vector2(0.3f, 0.3f);
                GameController.instance.hudCam.Zoom += new Vector2(0.2f, 0.2f);
                break;
            case 920:
                shake = false;
                GameController.instance.player.Show();
                for (int i = 0; i < SongUtility.SONG.keys; i++)
                {
                    StrumNote strumNote = (StrumNote)GameController.instance.strumNotes[i];
                    
                    strumNote.playAnim = true;
                }

                checker.Hide();

                GameController.instance.hudViewportContainer.Hide();
                break;
        }
    }

    public void OnBeat()
    {
        if (SongUtility.curBeat >= 48 && SongUtility.curBeat < 64)
        {
            GameController.instance.gameCam.Zoom += new Vector2(0.08f, 0.08f);
            GameController.instance.hudCam.Zoom += new Vector2(0.05f, 0.05f);
        }
        else if (SongUtility.curBeat >= 65 && SongUtility.curBeat < 80)
        {
            GameController.instance.gameCam.Zoom += new Vector2(0.08f, 0.08f);
            GameController.instance.hudCam.Zoom += new Vector2(0.05f, 0.05f);
        }
        else if (SongUtility.curBeat >= 116 && SongUtility.curBeat < 132)
        {
            GameController.instance.gameCam.Zoom += new Vector2(0.08f, 0.08f);
            GameController.instance.hudCam.Zoom += new Vector2(0.05f, 0.05f);
        }
        else if (SongUtility.curBeat >= 133 && SongUtility.curBeat < 146)
        {
            GameController.instance.gameCam.Zoom += new Vector2(0.08f, 0.08f);
            GameController.instance.hudCam.Zoom += new Vector2(0.05f, 0.05f);
        }
        else if (SongUtility.curBeat >= 182 && SongUtility.curBeat < 197)
        {
            GameController.instance.gameCam.Zoom += new Vector2(0.08f, 0.08f);
            GameController.instance.hudCam.Zoom += new Vector2(0.05f, 0.05f);
        }
        else if (SongUtility.curBeat >= 198 && SongUtility.curBeat < 230)
        {
            GameController.instance.gameCam.Zoom += new Vector2(0.1f, 0.1f);
            GameController.instance.hudCam.Zoom += new Vector2(0.07f, 0.07f);
        }
    }

    public float GetRandomNumber(float minimum, float maximum)
    { 
        return (float)randomShakeVal.NextDouble() * (maximum - minimum) + minimum;
    }
}
