﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleWizard
{
    public class QuestionPagedList<T> : QuestionList<T>
    {
        private List<T> _pageChoices = new List<T>();

        private int _skipChoices = 0;

        internal QuestionPagedList(QuestionList<T> question) : base(question.Message)
        {
            ValidatationFn = question.ValidatationFn;
            ParseFn = question.ParseFn;
            ChoicesDisplayFn = question.ChoicesDisplayFn;
            Choices = question.Choices;
        }

        internal int PageSize { get; set; } = 0;

        internal override T Prompt()
        {
            Console.Clear();

            bool tryAgain = true;
            T answer = DefaultValue;
            _pageChoices = Choices.Skip(_skipChoices).Take(PageSize).ToList();

            while (tryAgain)
            {
                DisplayQuestion();

                Console.WriteLine();
                Console.WriteLine();

                DisplayChoices();

                Console.CursorVisible = false;

                int boundryTop = Console.CursorTop - _pageChoices.Count;
                int boundryBottom = boundryTop + _pageChoices.Count - 1;

                ConsoleHelper.PositionWrite("→", 0, boundryTop);

                while (true)
                {
                    int y = Console.CursorTop;
                    var key = Console.ReadKey().Key;

                    Console.SetCursorPosition(0, y);
                    ConsoleHelper.Write("  " + ChoicesDisplayFn(y - boundryTop, _pageChoices[y - boundryTop]));
                    Console.SetCursorPosition(0, y);

                    switch (key)
                    {
                        case (ConsoleKey.UpArrow):
                            {
                                if (y > boundryTop)
                                {
                                    y -= 1;
                                }
                                else
                                {
                                    if (_skipChoices - PageSize >= 0)
                                    {
                                        _skipChoices -= PageSize;
                                        return Prompt();
                                    }
                                }

                                break;
                            }

                        case (ConsoleKey.DownArrow):
                            {
                                if (y < boundryBottom)
                                {
                                    y += 1;
                                }
                                else
                                {
                                    if (_skipChoices + PageSize < Choices.Count)
                                    {
                                        _skipChoices += PageSize;
                                        return Prompt();
                                    }
                                }

                                break;
                            }

                        case (ConsoleKey.Enter):
                            {
                                Console.CursorVisible = true;
                                return _pageChoices[Console.CursorTop - boundryTop];
                            }
                    }

                    ConsoleHelper.PositionWrite("→", 0, y);
                    ConsoleHelper.PositionWrite("  " + ChoicesDisplayFn(y - boundryTop, _pageChoices[y - boundryTop]), 0, y, ConsoleColor.DarkYellow);
                }
            }

            Console.WriteLine();
            return answer;
        }

        private void DisplayChoices()
        {
            for (int i = 0; i < _pageChoices.Count; i++)
            {
                ConsoleHelper.WriteLine("  " + ChoicesDisplayFn(i + 1, _pageChoices[i]));
            }
        }
    }
}