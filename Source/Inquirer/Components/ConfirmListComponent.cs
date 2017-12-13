﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InquirerCS.Interfaces;
using InquirerCS.Traits;

namespace InquirerCS.Components
{
    public class ConfirmListComponent<TList, TResult> : IConfirmComponent<TList> where TList : IEnumerable<TResult>
    {
        private IConvertToStringTrait<TResult> _convert;
        private IConsole _console;

        public ConfirmListComponent(IConvertToStringTrait<TResult> convert, IConsole console)
        {
            _convert = convert;
            _console = console;
        }

        public bool Confirm(TList result)
        {
            _console.Clear();

            StringBuilder sb = new StringBuilder();

            sb.Append($"Are you sure? [y/n] : ");

            sb.Append("[");
            sb.Append(string.Join(", ", result.Select(item => _convert.Convert.Run(item))));
            sb.Append("]");

            _console.WriteLine(sb.ToString());
            ConsoleKeyInfo key = default(ConsoleKeyInfo);
            do
            {
                key = _console.ReadKey();
                _console.SetCursorPosition(0, Console.CursorTop);
            }
            while (key.Key != ConsoleKey.Y && key.Key != ConsoleKey.N && key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Escape);

            if (key.Key == ConsoleKey.N || key.Key == ConsoleKey.Escape)
            {
                return true;
            }

            return false;
        }
    }
}
