﻿using System;
using System.Collections.Generic;
using InquirerCS.Interfaces;
using InquirerCS.Traits;

namespace InquirerCS.Components
{
    public class DisplaySelectableChoices<TResult> : IRenderChoices<TResult>
    {
        private const int _CURSOR_OFFSET = 2;
        private IConvertToStringTrait<TResult> _convert;
        private List<Selectable<TResult>> _choices;

        public DisplaySelectableChoices(List<Selectable<TResult>> choices, IConvertToStringTrait<TResult> convert)
        {
            _choices = choices;
            _convert = convert;
        }

        public void Render()
        {
            int index = 0;
            foreach (Selectable<TResult> choice in _choices)
            {
                ConsoleHelper.PositionWriteLine($"     {_convert.Convert.Run(choice.Item)}", 0, index + _CURSOR_OFFSET);
                ConsoleHelper.PositionWriteLine(choice.IsSelected ? "*" : " ", 3, index + _CURSOR_OFFSET);
                index++;
            }
        }

        public void Select(int index)
        {
            ConsoleHelper.PositionWriteLine($"->   {_convert.Convert.Run(_choices[index].Item)}", 0, index + _CURSOR_OFFSET, ConsoleColor.DarkYellow);
            ConsoleHelper.PositionWriteLine(_choices[index].IsSelected ? "*" : " ", 3, index + _CURSOR_OFFSET, ConsoleColor.DarkYellow);
        }
    }
}