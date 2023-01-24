# SimpleCompiler

This is my fun project of creating a simple compiler.
Right now it supports arithmetic operations < + - * / > including dealing with invalid tokens and process a command into a treelike syntax.

1 + 2 * 3 - 4 / 5
└──BinaryExpression
    ├──BinaryExpression
    │   ├──BinaryExpression
    │   │   ├──BinaryExpression
    │   │   │   ├──NumberExpression
    │   │   │   │   └──NumberToken 1
    │   │   │   ├──PlusToken
    │   │   │   └──NumberExpression
    │   │   │       └──NumberToken 2
    │   │   ├──StarToken
    │   │   └──NumberExpression
    │   │       └──NumberToken 3
    │   ├──MinusToken
    │   └──NumberExpression
    │       └──NumberToken 4
    ├──SlashToken
    └──NumberExpression
        └──NumberToken 5
1
