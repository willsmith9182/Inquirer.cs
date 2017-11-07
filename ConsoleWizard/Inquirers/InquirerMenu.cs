﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConsoleWizard
{
    public class InquirerMenu<TAnswers> where TAnswers : class, new()
    {
        private string _header;
        private Inquirer<TAnswers> _inquirer;
        private List<Tuple<string, Action>> _options = new List<Tuple<string, Action>>();

        public InquirerMenu(string header, Inquirer<TAnswers> inquirer)
        {
            _inquirer = inquirer;
            _header = header;
        }

        public InquirerMenu<TAnswers> AddOption(string description, Action option)
        {
            _options.Add(new Tuple<string, Action>(description, option));
            return this;
        }

        public void Prompt()
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame[] stackFrames = stackTrace.GetFrames();
            StackFrame callingFrame = stackFrames[1];

            Console.Clear();
            ConsoleHelper.WriteLine(_header + " :");
            ConsoleHelper.WriteLine();

            ConsoleHelper.WriteLine("  " + DisplayChoice(0), ConsoleColor.DarkYellow);
            for (int i = 1; i < _options.Count; i++)
            {
                ConsoleHelper.WriteLine("  " + DisplayChoice(i));
            }

            Console.CursorVisible = false;

            int boundryTop = Console.CursorTop - _options.Count;
            int boundryBottom = boundryTop + _options.Count - 1;

            ConsoleHelper.PositionWrite("→", 0, boundryTop);

            bool move = true;
            while (move)
            {
                int y = Console.CursorTop;

                bool isCanceled = false;
                var key = ConsoleHelper.ReadKey(out isCanceled);
                if (isCanceled)
                {
                    if (_inquirer.History.Count > 0)
                    {
                        var method = _inquirer.History.Pop();
                        method.Invoke(null, null);
                    }

                    Environment.Exit(0);
                }

                Console.SetCursorPosition(0, y);
                ConsoleHelper.Write("  " + DisplayChoice(y - boundryTop));
                Console.SetCursorPosition(0, y);

                switch (key)
                {
                    case (ConsoleKey.UpArrow):
                        {
                            if (y > boundryTop)
                            {
                                y -= 1;
                            }

                            break;
                        }

                    case (ConsoleKey.DownArrow):
                        {
                            if (y < boundryBottom)
                            {
                                y += 1;
                            }

                            break;
                        }

                    case (ConsoleKey.Enter):
                        {
                            Console.CursorVisible = true;
                            var answer = _options[Console.CursorTop - boundryTop];
                            move = false;
                            _inquirer.History.Push(callingFrame.GetMethod());
                            answer.Item2();
                            return;
                        }
                }

                ConsoleHelper.PositionWrite("  " + DisplayChoice(y - boundryTop), 0, y, ConsoleColor.DarkYellow);
                ConsoleHelper.PositionWrite("→", 0, y);
                Console.SetCursorPosition(0, y);
            }
        }

        protected string DisplayChoice(int index)
        {
            return $"{_options[index].Item1}";
        }
    }
}