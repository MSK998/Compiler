using Compiler.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Tokenization
{
    /// <summary>
    /// A tokenizer for the reader language
    /// </summary>
    public class Tokenizer
    {
        /// <summary>
        /// The error reporter
        /// </summary>
        public ErrorReporter Reporter { get; set; }

        /// <summary>
        /// The reader getting the characters from the file
        /// </summary>
        private IFileReader Reader { get; }

        /// <summary>
        /// The characters currently in the token
        /// </summary>
        private StringBuilder TokenSpelling { get; } = new StringBuilder();

        /// <summary>
        /// Createa a new tokenizer
        /// </summary>
        /// <param name="reader">The reader to get characters from the file</param>
        /// <param name="reporter">The error reporter to use</param>
        public Tokenizer(IFileReader reader, ErrorReporter reporter)
        {
            Reader = reader;
            Reporter = reporter;
        }

        /// <summary>
        /// Gets all the tokens from the file
        /// </summary>
        /// <returns>A list of all the tokens in the file in the order they appear</returns>
        public List<Token> GetAllTokens()
        {
            List<Token> tokens = new List<Token>();
            Token token = GetNextToken();
            while (token.Type != TokenType.EndOfText)
            {
                tokens.Add(token);
                token = GetNextToken();
            }
            tokens.Add(token);
            Reader.Close();
            return tokens;
        }

        /// <summary>
        /// Scan the next token
        /// </summary>
        /// <returns>True if and only if there is another token in the file</returns>
        private Token GetNextToken()
        {
            // Skip forward over any white space and comments
            SkipSeparators();

            // Remember the starting position of the token
            Position tokenStartPosition = Reader.CurrentPosition;

            // Scan the token and work out its type
            TokenType tokenType = ScanToken();

            // Create the token
            Token token = new Token(tokenType, TokenSpelling.ToString());
            Debugger.Write($"Scanned {token}");

            // Report an error if necessary
            if (tokenType == TokenType.Error)
            {
                // Report the error here
                Reporter.ReportError($"Error reading token {tokenStartPosition}");
            }

            return token;
        }

        /// <summary>
        /// Skip forward until the next character is not whitespace or a comment
        /// </summary>
        private void SkipSeparators()
        {
            while (Reader.Current == '#' || IsWhiteSpace(Reader.Current))
            {
                if (Reader.Current == '#')
                    Reader.SkipRestOfLine();

                else
                    Reader.MoveNext();
            }
        }

        /// <summary>
        /// Find the next token
        /// </summary>
        /// <returns>The type of the next token</returns>
        /// <remarks>Sets tokenSpelling to be the characters in the token</remarks>
        private TokenType ScanToken()
        {
            TokenSpelling.Clear();

            if (Reader.Current == '_' || char.IsLetter(Reader.Current))
            {
                TakeIt();
                while (Reader.Current == '_')
                {
                    TakeIt();
                }
                if (char.IsLetter(Reader.Current))
                {
                    TakeIt();
                    while (char.IsLetterOrDigit(Reader.Current))
                    {
                        TakeIt();
                    }
                }

                if (TokenTypes.IsKeyword(TokenSpelling))
                    return TokenTypes.GetTokenForKeyword(TokenSpelling);

                else
                {
                    return TokenType.Identifier;
                }
            }

            //integer
            else if (char.IsDigit(Reader.Current))
            {

                TakeIt();
                while (char.IsDigit(Reader.Current))
                {
                    TakeIt();
                }
                return TokenType.IntLiteral;

            }

            else if (IsGraphic(Reader.Current) || char.IsLetterOrDigit(Reader.Current))
            {
                TakeIt();
                return TokenType.Graphic;
            }


            else if (Reader.Current == '\'')
            {
                //Read ' 
                TakeIt();
                //Read the character
                TakeIt();
                while (char.IsLetterOrDigit(Reader.Current) || IsGraphic(Reader.Current))
                {
                    TakeIt();
                }

                if (Reader.Current == '\'')
                {
                    TakeIt();
                    return TokenType.CharLiteral;
                }
                else
                {
                    return TokenType.Error;
                }
            }


            //operator
            else if (IsOperator(Reader.Current))
            {
                //Read operator
                TakeIt();
                return TokenType.Operator;
            }

            else if (Reader.Current == ':')
            {
                //Read ":"
                //Check for :=
                TakeIt();
                if (Reader.Current == '=')
                {
                    TakeIt();
                    return TokenType.Becomes;
                }
                else
                {
                    return TokenType.Colon;
                }
            }

            else if (Reader.Current == ';')
            {
                //Read ;
                TakeIt();
                return TokenType.Semicolon;
            }

            else if (Reader.Current == '~')
            {
                //Read ~
                TakeIt();
                return TokenType.Is;
            }

            else if (Reader.Current == '(')
            {
                //Read (
                TakeIt();
                return TokenType.LeftBracket;
            }

            else if (Reader.Current == ')')
            {
                //Read )
                TakeIt();
                return TokenType.RightBracket;
            }


            if (Reader.Current == default(char))
            {
                // Read the end of the file
                TakeIt();
                return TokenType.EndOfText;
            }
            else
            {
                // Encountered a character we weren't expecting
                TakeIt();
                return TokenType.Error;
            }
        }

        /// <summary>
        /// Appends the current character to the current token then moves to the next character
        /// </summary>
        private void TakeIt()
        {
            TokenSpelling.Append(Reader.Current);
            Reader.MoveNext();
        }

        /// <summary>
        /// Checks whether a character is white space
        /// </summary>
        /// <param name="c">The character to check</param>
        /// <returns>True if and only if c is a whitespace character</returns>
        private static bool IsWhiteSpace(char c)
        {
            return c == ' ' || c == '\t' || c == '\n';
        }

        /// <summary>
        /// Checks whether a character is an operator
        /// </summary>
        /// <param name="c">The character to check</param>
        /// <returns>True if and only if the character is an operator in the language</returns>
        private static bool IsOperator(char c)
        {
            switch (c)
            {
                case '+':
                case '-':
                case '*':
                case '/':
                case '<':
                case '>':
                case '=':
                case '\\':
                case '!':
                    return true;
                default:
                    return false;
            }
        }

        private static bool IsGraphic(char c)
        {
            switch (c)
            {
                case ' ':
                case '?':
                case '.':
                    return true;
                default:
                    return false;
            }
        }
    }
}
