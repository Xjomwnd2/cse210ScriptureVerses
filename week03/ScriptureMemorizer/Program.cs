// Program.cs
using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        // Create a scripture (you can change this to any scripture you want)
        Reference reference = new Reference("John", 3, 16, 17);
        string text = "For God so loved the world, that he gave his only begotten Son, that whosoever believeth in him should not perish, but have everlasting life. For God sent not his Son into the world to condemn the world; but that the world through him might be saved.";
        Scripture scripture = new Scripture(reference, text);

        bool isFinished = false;

        // Main program loop
        while (!isFinished)
        {
            // Clear console and display scripture
            Console.Clear();
            Console.WriteLine(scripture.GetDisplayText());
            Console.WriteLine();
            
            // Check if all words are hidden
            if (scripture.IsCompletelyHidden())
            {
                Console.WriteLine("You've completely memorized the scripture!");
                isFinished = true;
                continue;
            }

            // Prompt user for input
            Console.WriteLine("Press enter to continue or type 'quit' to exit:");
            string userInput = Console.ReadLine();

            // Check for quit command
            if (userInput.ToLower() == "quit")
            {
                isFinished = true;
            }
            else
            {
                // Hide some random words
                scripture.HideRandomWords(3);
            }
        }
    }
}

// Reference.cs
class Reference
{
    private string _book;
    private int _chapter;
    private int _verse;
    private int _endVerse;

    // Constructor for a single verse reference
    public Reference(string book, int chapter, int verse)
    {
        _book = book;
        _chapter = chapter;
        _verse = verse;
        _endVerse = verse; // Same as the start verse for single verse
    }

    // Constructor for verse range reference
    public Reference(string book, int chapter, int startVerse, int endVerse)
    {
        _book = book;
        _chapter = chapter;
        _verse = startVerse;
        _endVerse = endVerse;
    }

    // Return formatted reference
    public string GetDisplayText()
    {
        if (_verse == _endVerse)
        {
            return $"{_book} {_chapter}:{_verse}";
        }
        else
        {
            return $"{_book} {_chapter}:{_verse}-{_endVerse}";
        }
    }
}

// Word.cs
class Word
{
    private string _text;
    private bool _isHidden;

    public Word(string text)
    {
        _text = text;
        _isHidden = false;
    }

    public void Hide()
    {
        _isHidden = true;
    }

    public bool IsHidden()
    {
        return _isHidden;
    }

    public string GetDisplayText()
    {
        if (_isHidden)
        {
            // Return underscores matching the length of the word
            return new string('_', _text.Length);
        }
        else
        {
            return _text;
        }
    }
}

// Scripture.cs
class Scripture
{
    private Reference _reference;
    private List<Word> _words;

    public Scripture(Reference reference, string text)
    {
        _reference = reference;
        _words = new List<Word>();

        // Split the text into words and create Word objects
        string[] wordArray = text.Split(' ');
        foreach (string wordText in wordArray)
        {
            _words.Add(new Word(wordText));
        }
    }

    public void HideRandomWords(int numberToHide)
    {
        // Stretch challenge: Only select from words that are not already hidden
        Random random = new Random();
        int hiddenCount = 0;
        
        // First, check if we have enough unhidden words left
        int unhiddenWordsCount = _words.Count(w => !w.IsHidden());
        int wordsToHide = Math.Min(numberToHide, unhiddenWordsCount);
        
        // Hide the words
        while (hiddenCount < wordsToHide)
        {
            // Get a list of indices of unhidden words
            List<int> unhiddenIndices = new List<int>();
            for (int i = 0; i < _words.Count; i++)
            {
                if (!_words[i].IsHidden())
                {
                    unhiddenIndices.Add(i);
                }
            }
            
            // Get a random index from the unhidden words
            int randomIndex = unhiddenIndices[random.Next(unhiddenIndices.Count)];
            
            // Hide the word if it's not already hidden
            if (!_words[randomIndex].IsHidden())
            {
                _words[randomIndex].Hide();
                hiddenCount++;
            }
        }
    }

    public bool IsCompletelyHidden()
    {
        // Check if all words are hidden
        foreach (Word word in _words)
        {
            if (!word.IsHidden())
            {
                return false;
            }
        }
        return true;
    }

    public string GetDisplayText()
    {
        // Combine the reference and words into a display string
        string displayText = _reference.GetDisplayText() + ": ";
        
        foreach (Word word in _words)
        {
            displayText += word.GetDisplayText() + " ";
        }
        
        return displayText.Trim();
    }
}

// Extension method for LINQ-like functionality without requiring System.Linq
static class Extensions
{
    public static int Count<T>(this List<T> list, Func<T, bool> predicate)
    {
        int count = 0;
        foreach (T item in list)
        {
            if (predicate(item))
            {
                count++;
            }
        }
        return count;
    }
}