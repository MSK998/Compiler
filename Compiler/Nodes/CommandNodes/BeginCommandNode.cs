using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Nodes
{
    class BeginCommandNode :ICommandNode
    {
        public ICommandNode Command { get; }

        public Position Position { get; }

        public BeginCommandNode(ICommandNode command, Position position)
        {
            Command = command;
            Position = position;
        }
    }
}
