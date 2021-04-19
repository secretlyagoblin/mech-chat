using CaveExplorer.Interfaces;
using Ink.Runtime;
using Ink;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CaveExplorer
{
    class Storyland
    {
        private Story _story;

        public Storyland(string story)
        {
            _story = new Ink.Runtime.Story(story);
        }

        public bool CanContinue => _story.canContinue;

        private Dictionary<string, Step> _storedMasks = new Dictionary<string, Step>();

        

        public Step StepForward()
        {
            var storyText = _story.Continue();

            // Masking tags are in the format #IN_identifier
            // If we encounter this tag, we advance the story
            // and save the text and tags for later
            while (ThereIsAMaskingTag(out var mask))
            {
                _story.currentTags.Remove(mask);
                _storedMasks.Add(
                    mask.Split('_')[1], // Store just the tag
                    new Step(storyText, _story.currentTags));

                if (!CanContinue) return Step.Empty();
                storyText = _story.Continue();
            }

            storyText = ApplyMaskIfRelevant(storyText);

            var tags = GetTags();

            //futher rules can be inserted here

            return new Step(storyText, tags);
        }

        private bool ThereIsAMaskingTag(out string mask)
        {
            foreach (var tag in _story.currentTags)
            {
                if (tag.Contains("IN_"))
                {
                    mask = tag;

                    return true;
                }
            }

            mask = null;

            return false;
        }

        private string ApplyMaskIfRelevant(string str)
        {
            str = str.Replace("\n", "");
            
            if (str.Contains("OUT_"))
                return _storedMasks[str.Split('_')[1]].Text;

            return str;
        }

        private List<string> GetTags()
        {
            var tags = new List<string>(_story.currentTags.Count);

            foreach (var tag in _story.currentTags)
            {
                if (tag.Contains("OUT_")) tags.AddRange(_storedMasks[tag.Split('_')[1]].Tags);
                else tags.Add(tag);
            }

            return tags;
        }

        public bool ShowChoices(out List<Choice> choices)
        {
            choices = new List<Choice>();

            if (_story.currentChoices.Count == 0) return false;

            for (int i = 0; i < _story.currentChoices.Count; ++i)
            {
                var choice = _story.currentChoices[i];

                choices.Add(new Choice(i, ApplyMaskIfRelevant(choice.text)));
            }
            return true;
        }

        public void Choose(int i)
        {
            _story.ChooseChoiceIndex(i);
        }
    }

    public class Step
    {
        public Step(string text,List<string> tags)
        {
            Text = text;
            Tags = tags;
        }

        public static Step Empty() => new Step("", new List<string>(0));

        public string Text { get; }
        public List<string> Tags { get; }
    }

    public class Choice
    {
        public Choice(int index, string content)
        {
            Index = index;
            Content = content;
        }

        public int Index { get; }
        public string Content { get; }
    }
}
