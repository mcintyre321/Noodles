using System.Collections.Generic;

namespace WebNoodle.Example.Models
{
    public class Home
    {
        private IList<Note> _notes = new List<Note>();
        public IEnumerable<Note> Notes { get { return _notes; } }
        public void AddNote(string note)
        {
            _notes.Add(new Note(){Text = note});
        }
    }

    public class Note
    {
        public string Text { get; set; }
        public bool Deleted { get; set; }
    }
}