using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        List<Scripture> scriptures = LoadScriptures("scriptures.txt");
        Random random = new Random();
        
        // Pick a random scripture
        Scripture scripture = scriptures[random.Next(scriptures.Count)];

        while (!scripture.AllWordsHidden())
        {
            Console.Clear();
            Console.WriteLine(scripture.GetDisplayText());
            Console.WriteLine("\nPress Enter to hide words or type 'quit' to exit.");

            string input = Console.ReadLine().Trim().ToLower();
            if (input == "quit") break;
            
            scripture.HideRandomWords(3); // Hide 3 words at a time
        }

        Console.Clear();
        Console.WriteLine(scripture.GetDisplayText());
        Console.WriteLine("\nAll words are hidden. Memorization complete!");
    }

    static List<Scripture> LoadScriptures(string filename)
    {
        List<Scripture> scriptures = new List<Scripture>();
        if (File.Exists(filename))
        {
            string[] lines = File.ReadAllLines(filename);
            foreach (string line in lines)
            {
                string[] parts = line.Split('|'); // Format: "Reference|Scripture text"
                if (parts.Length == 2)
                {
                    scriptures.Add(new Scripture(new Reference(parts[0]), parts[1]));
                }
            }
        }
        return scriptures;
    }
}

class Reference
{
    public string Book { get; }
    public int StartVerse { get; }
    public int? EndVerse { get; } // Nullable for single verses

    public Reference(string reference)
    {
        string[] parts = reference.Split(' ');
        Book = parts[0];
        string[] verses = parts[1].Split('-');
        StartVerse = int.Parse(verses[0]);
        EndVerse = verses.Length > 1 ? int.Parse(verses[1]) : (int?)null;
    }

    public override string ToString()
    {
        return EndVerse.HasValue ? $"{Book} {StartVerse}-{EndVerse}" : $"{Book} {StartVerse}";
    }
}

class Scripture
{
    private Reference _reference;
    private List<Word> _words;
    private Random _random = new Random();

    public Scripture(Reference reference, string text)
    {
        _reference = reference;
        _words = text.Split(' ').Select(word => new Word(word)).ToList();
    }

    public string GetDisplayText()
    {
        return $"{_reference}: {string.Join(" ", _words.Select(w => w.Display))}";
    }

    public void HideRandomWords(int count)
    {
        var visibleWords = _words.Where(w => !w.IsHidden).ToList();
        if (visibleWords.Count == 0) return;

        for (int i = 0; i < count && visibleWords.Count > 0; i++)
        {
            int index = _random.Next(visibleWords.Count);
            visibleWords[index].Hide();
            visibleWords.RemoveAt(index);
        }
    }

    public bool AllWordsHidden() => _words.All(w => w.IsHidden);
}

class Word
{
    private string _original;
    public bool IsHidden { get; private set; }
    
    public Word(string text)
    {
        _original = text;
        IsHidden = false;
    }

    public string Display => IsHidden ? new string('_', _original.Length) : _original;

    public void Hide() => IsHidden = true;
}
