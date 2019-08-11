﻿/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Linq;
using magic.node;
using magic.signals.contracts;

namespace magic.lambda.equality
{
    [Slot(Name = "mte")]
    public class Mte : ISlot
    {
        readonly ISignaler _signaler;

        public Mte(IServiceProvider services)
        {
            _signaler = services.GetService(typeof(ISignaler)) as ISignaler;
        }

        public void Signal(Node input)
        {
            if (input.Children.Count() != 2)
                throw new ApplicationException("Operator [mte] requires exactly two values to compare to each other");

            _signaler.Signal("eval", input);

            var lhs = input.Children.First().Value;
            var rhs = input.Children.Skip(1).First().Value;

            if (lhs == null && rhs == null)
                input.Value = true;
            else if (lhs != null && rhs == null)
                input.Value = true;
            else if (lhs == null && rhs != null)
                input.Value = false;
            else
                input.Value = ((IComparable)lhs).CompareTo(rhs) >= 0;
        }
    }
}