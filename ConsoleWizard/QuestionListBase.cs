﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleWizard
{
    public abstract class QuestionListBase<T> : QuestionBase<T>
    {
        public List<T> Choices { get; set; }

        public QuestionListBase(string message) : base(message)
        {
        }
    }

    public abstract class QuestionMultipleListBase<TList,T> : QuestionBase<TList> where TList : List<T>  
    {
        public TList Choices { get; set; }

        public QuestionMultipleListBase(string message) : base(message)
        {
        }
    }
}
