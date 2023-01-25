using System;
using System.Collections.Generic;
using System.Linq;

namespace myCompiler
{
    enum SyntaxKind
    {
        NumberToken,
        WhitespaceToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        BadToken,
        EndOfFileToken,
        NumberExpression,
        BinaryExpression,
        ParenthesizedExpressionSyntax
    }

    abstract class ArithmeticalOpts 
    { 
        public static List<SyntaxKind> SupportedArithmeticOpts = (new SyntaxKind[] {SyntaxKind.PlusToken, SyntaxKind.MinusToken, SyntaxKind.StarToken, SyntaxKind.SlashToken}).ToList();
        public static List<SyntaxKind> PrimaryArithmeticOpts = (new SyntaxKind[] {SyntaxKind.StarToken, SyntaxKind.SlashToken}).ToList();
        public static List<SyntaxKind> SeconderyArithmeticOpts = (new SyntaxKind[] {SyntaxKind.PlusToken, SyntaxKind.MinusToken}).ToList();
    }

    abstract class SyntaxNode
    {
        public abstract SyntaxKind Kind { get; }

        public abstract IEnumerable<SyntaxNode> GetChildren();
    }

    class SyntaxToken : SyntaxNode
    {

        public SyntaxToken(SyntaxKind kind, int position, string text, object value)
        {
            Kind = kind;
            Position = position;
            Text = text;
            Value = value;
        }

        public override SyntaxKind Kind { get; }
        public int Position { get; }
        public string Text { get; }
        public object Value { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Enumerable.Empty<SyntaxNode>();
        }
    }

    sealed class SyntaxTree
    {
        public SyntaxTree(IEnumerable<string> diagnostics, ExpressionSyntax root, SyntaxToken endOfFileToken)
        {
            Diagnostics = diagnostics.ToArray();
            Root = root;
            EndOfFileToken = endOfFileToken;
        }

        public ExpressionSyntax Root { get; }
        public SyntaxToken EndOfFileToken { get; }
        public IReadOnlyList<string> Diagnostics { get; }

        public static SyntaxTree Parse(string text)
        {
            var parser = new Parser(text);
            return parser.Parse();
        }
    }

    abstract class ExpressionSyntax : SyntaxNode {}

    sealed class NumberExpressionSyntax : ExpressionSyntax
    {
        public NumberExpressionSyntax(SyntaxToken numbertoken)
        {
            NumberToken = numbertoken;
        }

        public override SyntaxKind Kind => SyntaxKind.NumberExpression;
        public SyntaxToken NumberToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return NumberToken;
        }
    }

    sealed class BinaryExpressionSyntax : ExpressionSyntax
    {
        public BinaryExpressionSyntax(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            Left = left;
            OperatorToken = operatorToken;
            Right = right;
        }

        public override SyntaxKind Kind => SyntaxKind.BinaryExpression;
        public ExpressionSyntax Left { get; }
        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntax Right { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Left;
            yield return OperatorToken;
            yield return Right;
        }
    }

    sealed class ParenthesizedExpressionSyntax : ExpressionSyntax
    {
        public ParenthesizedExpressionSyntax(SyntaxToken openParenthesisToken, ExpressionSyntax expression, SyntaxToken closeParenthesisToken)
        {
            OpenParenthesisToken = openParenthesisToken;
            Expression = expression;
            CloseParenthesisToken = closeParenthesisToken;
        }

        public override SyntaxKind Kind => SyntaxKind.ParenthesizedExpressionSyntax;
        public SyntaxToken OpenParenthesisToken { get; }
        public ExpressionSyntax Expression { get; }
        public SyntaxToken CloseParenthesisToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OpenParenthesisToken;
            yield return Expression;
            yield return CloseParenthesisToken;
        }
    }
}