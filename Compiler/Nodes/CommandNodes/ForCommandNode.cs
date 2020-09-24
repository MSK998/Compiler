using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Nodes
{
    public class ForCommandNode : ICommandNode
    {
        public ICommandNode Assign { get; }

        public IExpressionNode Expression { get; }

        public ICommandNode Command { get; }

        public Position Position { get; }

        public ForCommandNode(ICommandNode assign, IExpressionNode expression, ICommandNode command, Position position)
        {
            Assign = assign;
            Expression = expression;
            Command = command;
            Position = position;
        }
    }
}
