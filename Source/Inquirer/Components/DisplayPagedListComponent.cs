﻿using System;
using InquirerCS.Interfaces;

namespace InquirerCS.Components
{
    public class DisplayPagedListChoices<TResult> : IRenderChoices<TResult>
    {
        private const int _CURSOR_OFFSET = 2;

        private IConvertToStringComponent<TResult> _convertToStringComponent;

        private IPagingComponent<TResult> _pagingComponent;

        public DisplayPagedListChoices(IPagingComponent<TResult> pagingComponent, IConvertToStringComponent<TResult> convertToString)
        {
            _pagingComponent = pagingComponent;
            _convertToStringComponent = convertToString;
        }

        public void Render()
        {
            int index = 0;
            foreach (TResult choice in _pagingComponent.CurrentPage)
            {
                ConsoleHelper.PositionWriteLine($"   {_convertToStringComponent.Run(choice)}", 0, index + _CURSOR_OFFSET);
                index++;
            }
        }

        public void Select(int index)
        {
            ConsoleHelper.PositionWriteLine($"-> {_convertToStringComponent.Run(_pagingComponent.CurrentPage[index])}", 0, index + _CURSOR_OFFSET, ConsoleColor.DarkYellow);
        }
    }
}
