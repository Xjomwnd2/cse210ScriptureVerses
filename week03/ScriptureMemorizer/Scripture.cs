using System;
using System.Collections.Generic;
using System.Linq;

public class Scripture
{
    public Reference Reference { get; private set; }
    private List<Word> Words;

    public Scripture(Reference reference, string text)
    {
        Reference = reference;
        Words = text.Split(' ').Select(word => new Word(word)).ToList();
    }

    public string GetDisplayText()
    {
        return $"{Reference.ToString()} - {string.Join(" ", Words.Select(w => w.GetDisplayText()))}";
    }

    public void HideRandomWords(int count)
    {
        Random rand = new Random();
        var visibleWords = Words.Where(w => !w.IsHidden).ToList();

        if (visibleWords.Count == 0) return;

        int wordsToHide = Math.Min(count, visibleWords.Count);
        for (int i = 0; i < wordsToHide; i++)
        {
            int index = rand.Next(visibleWords.Count);
            visibleWords[index].Hide();
            visibleWords.RemoveAt(index);
        }
    }

    public bool IsFullyHidden()
    {
        return Words.All(w => w.IsHidden);
    }
}
