using System.Collections.Generic;
using Godot;

class NotePool
{
    static int maxCount;

    static Dictionary<int, List<Node2D>> pool;
    public static List<Node2D> activeNotes { get; private set; }

    public static void Init(int maxPerNote)
    {
        maxCount = maxPerNote;

        pool = new Dictionary<int, List<Node2D>>();
        activeNotes = new List<Node2D>();
    }

    public static Node2D Get(int id)
    {
        int normalizedId = id % SongUtility.SONG.keys;

        if (!pool.ContainsKey(normalizedId))
            pool.Add(normalizedId, new List<Node2D>(maxCount));

        List<Node2D> noteList = pool[normalizedId];
        foreach (Node2D note in noteList)
        {
            if (note.ProcessMode == Node.ProcessModeEnum.Disabled)
            {
                note.ProcessMode = Node.ProcessModeEnum.Inherit;
                note.Show();

                activeNotes.Add(note);
                return note;
            }
        }

        Node2D newNote = (Node2D)ResourceLoader.Load<PackedScene>("res://scenes/objects/notes/" + normalizedId + ".tscn").Instantiate();

        activeNotes.Add(newNote);
        if (noteList.Count < maxCount)
            noteList.Add(newNote);
        return newNote;
    }

    public static void Put(Node2D note)
    {
        activeNotes.Remove(note);
        
        Note noteScript = (Note)note;

        int normalizedId = noteScript.noteData.id % SongUtility.SONG.keys;

        List<Node2D> noteList = pool[normalizedId];

        if (!noteList.Contains(note))
            note.Free();
        else
        {
            note.Hide();
            note.ProcessMode = Node.ProcessModeEnum.Disabled;
        }
    }
}