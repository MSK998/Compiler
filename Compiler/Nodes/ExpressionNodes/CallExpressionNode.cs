using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Nodes
{
    public class CallExpressionNode : IExpressionNode
    {
        public IdentifierNode Identifier { get; }

        public IParameterNode Parameter { get; }

        public SimpleTypeDeclarationNode Type { get; set; }

        public Position Position { get { return Identifier.Position; } }

        public CallExpressionNode(IdentifierNode identifier, IParameterNode parameter)
        {
            Identifier = identifier;
            Parameter = parameter;
        }
    }
}

