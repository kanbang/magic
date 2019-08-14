﻿/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using magic.node;
using magic.signals.contracts;

namespace magic.lambda.change
{
    [Slot(Name = "unwrap-name")]
    public class UnwrapName : ISlot, IMeta
    {
        public void Signal(Node input)
        {
            var dest = input.Evaluate();
            foreach (var idx in dest)
            {
                var exp = idx.Evaluate();
                if (exp.Count() > 1)
                    throw new ApplicationException("Multiple sources found for [unwrap-value]");

                idx.Value = exp.FirstOrDefault()?.Name;
            }
        }

        public IEnumerable<Node> GetArguments()
        {
            yield return new Node(":", "x");
        }
    }
}
