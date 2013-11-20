using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Exercise1
{
    public class RegularExpressionParser
    {
        private bool _isConcatenateMode = false;
        private bool _isAlternateMode = false;
        private char _curChar = MetaChars._nullChar;
        private int _patternLength = -1;
        private string _patternString = String.Empty;
        private int _patternPosition = -1;
        private StringBuilder _stringBuilder = null;

        /// <summary>
        /// Enum to specify what kind of error occurs in parsing the regular exception
        /// </summary>
        public enum ParserError
        {
            ParenthesisMalformed,   // Malformed parentes like this (a(aaa*)
            EmptyParenthesis,       // Empty parentesys ()
            EmptyInterval,          // Empty interval []
            IntervalMismatch,       // Interval mismatch like in "[a-b" or "a-b"
            MissingPattern,         // Missing pattern like in "a|" or ""
            InvalidEscape,          // Invalid escape before a non-MetaChars symbols "\A"
            WrongInterval,          // "[C-A]" 
            EmptyString,            // No regex given
        }

        public class RegularExpressionParserException : Exception
        {
            public RegularExpressionParserException(ParserError errCode, int nErrorStart = -1, int nErrorLength = -1, string sFormattedString = "")
            {
                ErrorCode = errCode;
                ErrorStart = -1;
                ErrorLength = -1;
                FormattedString = sFormattedString;
            }

            public ParserError ErrorCode;
            public int ErrorStart = -1;
            public int ErrorLength = -1;
            public string FormattedString = String.Empty;

            public override string ToString()
            {
                return ErrorCode.ToString() + (ErrorStart != -1 ? " from position " + ErrorStart : "");
            }
        }

        public string ParseRegEx(string sPattern)
        {
            _curChar = MetaChars._nullChar;
            _isConcatenateMode = false;
            _isAlternateMode = false;
            _stringBuilder = new StringBuilder();
            _patternPosition = -1;
            _patternString = sPattern;
            _patternLength = sPattern.Length;

            if (sPattern.Length == 0)
            {
                throw new RegularExpressionParserException(ParserError.EmptyString);
            }

            _curChar = GetNextChar();

            char _literalStart = MetaChars._metaStringStart;
            char _literalEnd = MetaChars._metaStringEnd;
            string _emptyString = _literalStart.ToString() + _literalEnd.ToString();
            

            if (!(sPattern == _literalStart.ToString() || sPattern == _literalEnd.ToString() || sPattern ==_emptyString))
            {
                if (sPattern[0] == MetaChars._metaStringStart)
                    GoNextIfMatchToken(MetaChars._metaStringStart);
                if (_patternString[_patternLength - 1] == MetaChars._metaStringEnd)
                    _patternLength--;
            }

            while (_patternPosition < _patternLength)
            {
                switch (_curChar)
                {
                    case MetaChars.PatternAlternate:
                    case MetaChars.KleeneStar:
                    case MetaChars.OptionalPattern:
                        throw new RegularExpressionParserException(ParserError.MissingPattern, _patternPosition, 1);
                    case MetaChars.CloseParenthesys:
                        throw new RegularExpressionParserException(ParserError.ParenthesisMalformed, _patternPosition, 1);
                    default:
                        ParseExpression();
                        break;
                }
            }
            return _stringBuilder.ToString();
        }

        private char GetNextChar()
        {
            _patternPosition++;
            if (_patternPosition < _patternLength)
            {
                return _patternString[_patternPosition];
            }
            else
            {
                return MetaChars._nullChar;
            }
        }

        private bool GoNextIfMatchToken(char ch)
        {
            if (_curChar == ch)
            {
                _curChar = GetNextChar();
                return true;
            }
            return false;
        }


        private void ParseExpression()
        {
            while (GoNextIfMatchToken(MetaChars.EscapeChar))
            {
                AddConcatenateChar();
                if (!ExpectEscapeChar())
                {
                    throw new RegularExpressionParserException(ParserError.InvalidEscape, _patternPosition - 1, 1);
                }
                IsPostfixOperator();
                _isConcatenateMode = true;
            }

            while (GoNextIfMatchToken(MetaChars.PatternConcatenate))
            {
                AddConcatenateChar();
                _stringBuilder.Append(MetaChars.EscapeChar);
                _stringBuilder.Append(MetaChars.PatternConcatenate);
                IsPostfixOperator();
                _isConcatenateMode = true;
            }

            while (IsNonEscapeChar())
            {
                IsPostfixOperator();
                _isConcatenateMode = true;
                ParseExpression();
            }

            if (GoNextIfMatchToken(MetaChars.OpenParentesys))
            {
                int _EntryPos = _patternPosition - 1;
                AddConcatenateChar();
                _stringBuilder.Append(MetaChars.OpenParentesys);
                ParseExpression();
                if (!Expect(MetaChars.CloseParenthesys))
                {
                    throw new RegularExpressionParserException(ParserError.ParenthesisMalformed, _EntryPos, _patternPosition - _EntryPos);
                }
                _stringBuilder.Append(MetaChars.CloseParenthesys);

                int nLen = _patternPosition - _EntryPos;
                if (nLen == 2)
                {
                    throw new RegularExpressionParserException(ParserError.EmptyParenthesis, _EntryPos, _patternPosition - _EntryPos);
                }

                IsPostfixOperator();
                _isConcatenateMode = true;
                ParseExpression();
            }


            if (GoNextIfMatchToken(MetaChars.IntervalStart))
            {
                int _EntryPos = _patternPosition - 1;

                AddConcatenateChar();

                string sTmp = _stringBuilder.ToString();

                _stringBuilder = new StringBuilder(1024);
                _isAlternateMode = false;
                ParseCharecterSet();

                if (!Expect(MetaChars.IntervalEnd))
                {
                    throw new RegularExpressionParserException(ParserError.IntervalMismatch, _EntryPos, _patternPosition - _EntryPos);
                }

                int nLen = _patternPosition - _EntryPos;

                if (nLen == 2)  // "[]"
                {
                    throw new RegularExpressionParserException(ParserError.EmptyInterval, _EntryPos, _patternPosition - _EntryPos);
                }
                else
                {
                    string sCharset = _stringBuilder.ToString();
                    _stringBuilder = new StringBuilder();
                    _stringBuilder.Append(sTmp);
                    _stringBuilder.Append(MetaChars.OpenParentesys);
                    _stringBuilder.Append(sCharset /*ExpandRange(sCharset, nEntryPos) */   );
                    _stringBuilder.Append(MetaChars.CloseParenthesys);
                }

                IsPostfixOperator();

                _isConcatenateMode = true;

                ParseExpression();
            }

           if (GoNextIfMatchToken(MetaChars.PatternAlternate))
            {
                int _EntryPos = _patternPosition - 1;
                _isConcatenateMode = false;
                _stringBuilder.Append(MetaChars.PatternAlternate);
                ParseExpression();
                int nLen = _patternPosition - _EntryPos;
                if (nLen == 1)
                {
                    throw new RegularExpressionParserException(ParserError.MissingPattern, _EntryPos, _patternPosition - _EntryPos);
                }
                ParseExpression();
            }


        }
        private void ParseCharecterSet()
        {
            int nRangeFormStartAt = -1;
            int nStartAt = -1;
            int nLength = -1;

            // xx-xx form
            string sLeft = String.Empty;
            string sRange = String.Empty;
            string sRight = String.Empty;


            string sTmp = String.Empty;

            while (true)
            {
                sTmp = String.Empty;

                nStartAt = _curChar;

                if (GoNextIfMatchToken(MetaChars.EscapeChar))
                {
                    if ((sTmp = ExpectEscapeCharInBracket()) == String.Empty)
                    {
                        throw new RegularExpressionParserException(ParserError.InvalidEscape, _patternPosition - 1, 1);
                    }
                    nLength = 2;
                }

                if (sTmp == String.Empty)
                {
                    sTmp = IsNonEscapeCharInInterval();
                    nLength = 1;
                }

                if (sTmp == String.Empty)
                {
                    break;
                }

                if (sLeft == String.Empty)
                {
                    nRangeFormStartAt = nStartAt;
                    sLeft = sTmp;
                    AddAlternateChar();
                    _stringBuilder.Append(sTmp);
                    _isAlternateMode = true;
                    continue;
                }

                if (sRange == String.Empty)
                {
                    if (sTmp != MetaChars.IntervalRange.ToString())
                    {
                        nRangeFormStartAt = nStartAt;
                        sLeft = sTmp;
                        AddAlternateChar();
                        _stringBuilder.Append(sTmp);
                        _isAlternateMode = true;
                        continue;
                    }
                    else
                    {
                        sRange = sTmp;
                    }
                    continue;
                }

                sRight = sTmp;


                bool bOk = ExpandRange(sLeft, sRight);

                if (bOk == false)
                {
                    int nSubstringLen = (nStartAt + nLength) - nRangeFormStartAt;

                    throw new RegularExpressionParserException(ParserError.WrongInterval, nRangeFormStartAt, nSubstringLen);
                }
                sLeft = String.Empty;
                sRange = String.Empty;
                sRange = String.Empty;
            }

            if (sRange != String.Empty)
            {
                AddAlternateChar();
                _stringBuilder.Append(sRange);
                _isAlternateMode = true;
            }

        }
        private bool ExpandRange(string sLeft, string sRight)
        {
            char chLeft = (sLeft.Length > 1 ? sLeft[1] : sLeft[0]);
            char chRight = (sRight.Length > 1 ? sRight[1] : sRight[0]);

            if (chLeft > chRight)
            {
                return false;
            }

            chLeft++;
            while (chLeft <= chRight)
            {
                AddAlternateChar();

                switch (chLeft)
                {
                    case MetaChars.PatternAlternate:
                    case MetaChars.JollyChar:
                    case MetaChars.CloseParenthesys:
                    case MetaChars.PatternConcatenate:
                    case MetaChars.EscapeChar:
                    case MetaChars.KleeneStar:
                    case MetaChars.OptionalPattern:
                    case MetaChars.OpenParentesys:
                        _stringBuilder.Append(MetaChars.EscapeChar);
                        break;
                    default:
                        break;
                }

                _stringBuilder.Append(chLeft);
                _isAlternateMode = true;
                chLeft++;
            }

            return true;

        }

        private bool IsPostfixOperator()
        {
            switch (_curChar)
            {
                case MetaChars.KleeneStar:
                case MetaChars.OptionalPattern:
                    _stringBuilder.Append(_curChar);
                    return GoNextIfMatchToken(_curChar);
                default:
                    return false;
            }
        }
        private bool IsNonEscapeChar()
        {
            switch (_curChar)
            {
                case MetaChars.PatternAlternate:
                case MetaChars.IntervalStart:
                case MetaChars.CloseParenthesys:
                case MetaChars.EscapeChar:
                case MetaChars.OpenParentesys:
                case MetaChars.KleeneStar:
                case MetaChars.OptionalPattern:
                case MetaChars.PatternConcatenate:
                case MetaChars._nullChar:
                    return false;
                default:
                    AddConcatenateChar();
                    _stringBuilder.Append(_curChar);
                    GoNextIfMatchToken(_curChar);
                    break;
            }
            return true;
        }
        private string IsNonEscapeCharInInterval()
        {
            char ch = _curChar;

            switch (ch)
            {
                case MetaChars.IntervalEnd:
                case MetaChars.EscapeChar:
                case MetaChars._nullChar:
                    return String.Empty;
                case MetaChars.PatternAlternate:
                case MetaChars.JollyChar:
                case MetaChars.CloseParenthesys:
                case MetaChars.OpenParentesys:
                case MetaChars.KleeneStar:
                case MetaChars.OptionalPattern:
                case MetaChars.PatternConcatenate:
                    GoNextIfMatchToken(_curChar);
                    return MetaChars.EscapeChar.ToString() + ch.ToString();
                default:
                    GoNextIfMatchToken(_curChar);
                    return ch.ToString();
            }
        }
        private bool Expect(char ch)
        {
            if (GoNextIfMatchToken(ch))
            {
                return true;
            }
            return false;
        }
        private bool ExpectEscapeChar()
        {
            switch (_curChar)
            {
                case MetaChars.PatternAlternate:
                case MetaChars.JollyChar:
                case MetaChars.IntervalStart:
                case MetaChars.CloseParenthesys:
                case MetaChars.EscapeChar:
                case MetaChars.OpenParentesys:
                case MetaChars.KleeneStar:
                case MetaChars.OptionalPattern:
                    _stringBuilder.Append(MetaChars.EscapeChar);
                    _stringBuilder.Append(_curChar);
                    GoNextIfMatchToken(_curChar);
                    break;
                default:
                    return false;
            }
            return true;
        }
        private string ExpectEscapeCharInBracket()
        {
            char ch = _curChar;

            switch (_curChar)
            {
                case MetaChars.IntervalEnd:
                case MetaChars.EscapeChar:
                    GoNextIfMatchToken(_curChar);
                    return MetaChars.EscapeChar.ToString() + ch.ToString();
                default:
                    return String.Empty;
            }
        }

        private void AddConcatenateChar()
        {
            if (_isConcatenateMode)
            {
                _stringBuilder.Append(MetaChars.PatternConcatenate);
                _isConcatenateMode = false;
            }
        }

        private void AddAlternateChar()
        {
            if (_isAlternateMode)
            {
                _stringBuilder.Append(MetaChars.PatternAlternate);
                _isAlternateMode = false;
            }
        }


        /// <summary>
        /// Converts an infix regular expression to an equivalent postfix regular expression
        /// </summary>
        /// <param name="infixRegEx">Original regular expression in infix form</param>
        /// <returns>String rappresenting the regular expression in postfix form</returns>
        public static string ConvertToPostfix(string infixRegEx)
        {
            // Stack rappresenting the operands
            Stack<char> tempEvalStack = new Stack<char>();

            // Help queue for generating postfix-form regular expression
            Queue queuePostfix = new Queue();

            // Variable to easily manage the escape mode such as "\-", "\[", "\]", "\.", "\\"...
            bool isAnEscapeChar = false;

            // Read all chars in string sequentially
            foreach (var ch in infixRegEx)
            {
                if (!isAnEscapeChar)
                {
                    if (ch == MetaChars.EscapeChar)     // Found escape char
                    {
                        queuePostfix.Enqueue(ch);
                        isAnEscapeChar = true;
                        continue;
                    }
                }
                else                                    // Not an escape char
                {
                    queuePostfix.Enqueue(ch);
                    isAnEscapeChar = false;
                    continue;
                }
                switch (ch)
                {
                    case MetaChars.OpenParentesys:      // Handle the production RE -> (RE'
                        tempEvalStack.Push(ch);
                        break;
                    case MetaChars.CloseParenthesys:    // handle the production RE'' -> )RE
                        while (tempEvalStack.Peek() != MetaChars.OpenParentesys)
                            queuePostfix.Enqueue(tempEvalStack.Pop());
                        tempEvalStack.Pop();            // The while condition is false => the popped char is an '('
                        break;
                    default:
                        while (tempEvalStack.Count > 0)
                        {
                            if (CharPriority(tempEvalStack.Peek()) >= CharPriority(ch))
                                queuePostfix.Enqueue(tempEvalStack.Pop());
                            else
                                break;
                        }
                        tempEvalStack.Push(ch);
                        break;
                }
            }

            // Add stack operands to the postfix queue
            while (tempEvalStack.Count > 0) queuePostfix.Enqueue(tempEvalStack.Pop());

            // Generate the postfix normalized regular expression
            StringBuilder generatedPostfix = new StringBuilder();
            while (queuePostfix.Count > 0) generatedPostfix.Append(queuePostfix.Dequeue());

            // postfix regular espression as string
            return generatedPostfix.ToString();
        }

        /// <summary>
        /// Get the priority level of a string char
        /// </summary>
        /// <param name="charToCheck">A regular expression symbol</param>
        /// <returns>Priority level</returns>
        private static int CharPriority(char charToCheck)
        {
            switch (charToCheck)                        // The priority is this 
            {                                           //
                case MetaChars.OpenParentesys:          // 1.   (
                    return 1;                           //
                case MetaChars.PatternAlternate:        // 2.   |
                    return 2;                           //
                case MetaChars.PatternConcatenate:      // 3.   $
                    return 3;                           //
                case MetaChars.OptionalPattern:         // 4.   ? or *
                case MetaChars.KleeneStar:              //
                    return 4;                           //
                default:                                // 5.   others
                    return 5;
            }
        }

        /// <summary>
        /// Static class 
        /// </summary>
        public static class MetaChars
        {
            // Parser Special Char
            public const char _nullChar = '\0';         // Null symbol;
            public const char JollyChar = '.';          // Matchs any char
            public const char EscapeChar = '\\';        // Use MetaChars as normal char

            // Pattern Operators
            public const char PatternConcatenate = '§';       // AB...Z -> A$B$...$Z  concatenate patterns
            public const char PatternAlternate = '|';      // A|B|...|Z            alternate patterns
            public const char KleeneStar = '*';         // Kleeeny star operator
            public const char OptionalPattern = '?';    // optional pattern
            public const char OpenParentesys = '(';     // open parentesys
            public const char CloseParenthesys = ')';   // close parentesys
            public const char IntervalStart = '[';      // Interval open parentesys 
            public const char IntervalRange = '-';      // Interval range
            public const char IntervalEnd = ']';        // Interval close parentesys
            public const char _metaStringStart = '^';   // Begining of the string
            public const char _metaStringEnd = '$';     // End of string
        }

        public static class MetaCharsTranslations
        {
            public const string JollyCharTrans = ".";  // Translation for '.' jolly char
            public const string EpsilonChar = "Epsilon"; // Translation for epsilon char
        }

    }
}
