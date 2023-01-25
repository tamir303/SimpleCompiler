using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace myCompiler
{
        class Evaluator
    {

        private readonly ExpressionSyntax _root;
        private Dictionary<SyntaxKind, Func<int, int, int>> _arithmeticOpt  = new Dictionary<SyntaxKind, Func<int, int, int>>();

        public Evaluator(ExpressionSyntax root)
        {
            this._root = root;
            setArithmeticalOptions();
        }

        private void setArithmeticalOptions()
        {
            _arithmeticOpt[SyntaxKind.PlusToken] = (x, y) => x + y;
            _arithmeticOpt[SyntaxKind.MinusToken] = (x, y) => x - y;
            _arithmeticOpt[SyntaxKind.StarToken] = (x, y) => x * y;
            _arithmeticOpt[SyntaxKind.SlashToken] = (x, y) => x / y;
            Debug.Assert(!_arithmeticOpt.Keys.Equals(ArithmeticalOpts.SupportedArithmeticOpts), "Operations lists of Evaluator and Parser aren't equal!");
        }

        public int Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private int EvaluateExpression(ExpressionSyntax root)
        {
            if (root is NumberExpressionSyntax n)
                return (int) n.NumberToken.Value;
            
            if (root is BinaryExpressionSyntax b)
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);
                if (_arithmeticOpt.ContainsKey(b.OperatorToken.Kind))
                    return _arithmeticOpt[b.OperatorToken.Kind](left, right);
                else
                    throw new Exception($"Unexcepted binary operator {b.OperatorToken.Kind}");
            }

            if (root is ParenthesizedExpressionSyntax p)
                return EvaluateExpression(p.Expression);

            throw new Exception($"Unexcepted node {root.Kind}");
        }
    }
}