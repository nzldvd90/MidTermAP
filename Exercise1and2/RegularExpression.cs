using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Diagnostics;

namespace Exercise1
{
    /// <summary>
    /// Regular Expression class for string matching
    /// </summary>
    public class RegularExpression
    {
        #region Private variables

        // Variable indicates if current regex is compiled
        private bool _isCompiled = false;

        // Original regular expression passed to the constructor
        string _originalRegularExpression;

        // Formatted version of the regular expression
        string _formattedRegularExpression;

        // Regular expression in postifx form
        string _postfixRegularExpression;
        
        // Non-Deterministic automata parser
        NDAutomata _NDAutomata;

        // Deterministic Automata parser
        DAutomata _DAutomata;

        // Optimized Deterministic automata parser
        DAutomata _optimizedDAutomata;

        // Regular expression parser
        RegularExpressionParser _parser;

        // Compiler class
        ShallowCompiler _compiler;

        #endregion

        #region Constructior

        /// <summary>
        /// Create a new regular expression parser using these steps:
        /// 1) format the regular expresison
        /// 2) postfix the regex for efficiency reason (see http://www.cs.man.ac.uk/~pjj/cs2121/fix.html for the algorithm details)
        /// 3) Construction of Non-Deterministic automata using Thompson's Construction Algorithm (see page 153 of Dragon Book for algorithm details)
        /// 4) Convert Non-Deterministic Automata to and equivalent Deterministic Automata.
        /// 5) Optimize the Deterministic Automata by removing dead-transitions/states using (Minimize Automata Algorithm).
        /// </summary>
        /// <param name="regularExpress">The regular expression to create</param>
        public RegularExpression(string regularExpress)
        {

            _parser = new RegularExpressionParser();

            try
            {
                Debug.WriteLine("");
                Stopwatch t = new Stopwatch();
                t.Reset();
                t.Start();
                // Save the original expression
                _originalRegularExpression = regularExpress;

                // Obtaint the explicit regex expression
                t.Reset();
                t.Start();
                _formattedRegularExpression = _parser.ParseRegEx(_originalRegularExpression); // it it terminates the string is sintactically correct
                t.Stop();

                Debug.WriteLine("Regular expression parsed in " + ((double)t.ElapsedTicks / 10000.0).ToString("0.00000"));

                // Optimize the string from infix to postfix equivalent expression
                _postfixRegularExpression = RegularExpressionParser.ConvertToPostfix(_formattedRegularExpression); // just to apply an efficient LL1 grammar

                // Regular expression is now in postfix mode: create a Non-Deterministic automata
                t.Reset();
                t.Start();
                _NDAutomata = AutomataWrapper.CreateNFAutomata(_postfixRegularExpression);
                t.Stop();

                Debug.WriteLine("NFA generated in " + ((double)t.ElapsedTicks / 10000.0).ToString("0.00000"));

                // Reduce the number of states and transitions creating an equivalent Deterministic Automata
                t.Reset();
                t.Start();
                _DAutomata = AutomataWrapper.CreateDAutomata(_NDAutomata);
                t.Stop();

                Debug.WriteLine("DFA generated in " + ((double)t.ElapsedTicks / 10000.0).ToString("0.00000"));

                // Optimize the Deterministic automata
                t.Reset();
                t.Start();
                _optimizedDAutomata = AutomataWrapper.MinimizeDAutomata(_DAutomata);
                t.Stop();

                Debug.WriteLine("Optimized DFA generated in " + ((double)t.ElapsedTicks / 10000.0).ToString("0.00000"));
            }
            catch (RegularExpressionParser.RegularExpressionParserException e)
            {
                throw e;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get a bool indicates if exists the runnable class
        /// </summary>
        public bool IsCompiled
        {
            get { return _isCompiled; }
        }

        /// <summary>
        /// Original regular expression
        /// </summary>
        public string OriginalExpression
        {
            get { return _originalRegularExpression; }
        }

        /// <summary>
        /// Regular expression in Infix form
        /// </summary>
        public string FormattedExpression
        {
            get { return _formattedRegularExpression; }
        }

        /// <summary>
        /// Regular expression in Postfix form
        /// </summary>
        public string PostfixExpression
        {
            get { return _postfixRegularExpression; }
        }

        /// <summary>
        /// Non Deterministic automata states count
        /// </summary>
        public int NDStateCount
        {
            get { return _NDAutomata.States.Length; }
        }

        /// <summary>
        /// Deterministic automata states count
        /// </summary>
        public int DStateCount
        {
            get { return _DAutomata.States.Length; }
        }

        /// <summary>
        /// Optimized (final) automata states count
        /// </summary>
        public int OptimizedDStateCount
        {
            get { return _optimizedDAutomata.States.Length; }
        }

        /// <summary>
        /// Get the optimized automata that parse this regular expression
        /// </summary>
        public DAutomata ParserAutomata
        {
            get { return _optimizedDAutomata; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Compile the deterministi automata into a compiled C# runnable class
        /// </summary>
        /// <returns>The source code for the class</returns>
        public string Compile()
        {
            // Compile C-Sharp code
            _compiler = new ShallowCompiler();
            try
            {
                var generateClass = _compiler.CompileCode(_optimizedDAutomata);
                _isCompiled = true;
                Console.WriteLine("Code compiled successfully");
                return generateClass.ToString();
            }
            catch (ShallowCompiler.CompilerException e)
            {
                Console.WriteLine("Error occurred during compilation : \r\n" + e.ToString());
                throw e;
            }

        }

        /// <summary>
        /// Check if an input string matchs this regular expression
        /// </summary>
        /// <param name="inputString">String to match with this regular expression</param>
        /// <param name="timer">Stopwatch timer that is resetted and started before, and stopped after, running IsMatch in compiled class</param>
        /// <returns></returns>
        public bool IsMatch(string inputString, Stopwatch timer = null)
        {
            // If a previous call of the method Compile()
            if (_isCompiled)
                return ExecuteCompiledClass(inputString, timer);
            return IsMatch(ref inputString, _optimizedDAutomata.StartState, 0);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Internal call for IsMatch call that check if the string matchs this regular expression
        /// </summary>
        /// <param name="inputString">String to match with this regular expression</param>
        /// <param name="startState">Start state (usualy the automata start state)</param>
        /// <param name="startFrom">Start index for the inputString</param>
        /// <returns>true if inputString matchs this regular expression</returns>
        private static bool IsMatch(ref string inputString, DAutomata.DAutomataState startState, int startFrom=0)
        {
            DAutomata.DAutomataState toState = null;
            DAutomata.DAutomataState stateCurr = startState;

            DAutomata.DAutomataState jollyState = null;
            for (int i = startFrom; i < inputString.Length; i++)
            {
                jollyState = stateCurr.GetTransition(RegularExpressionParser.MetaCharsTranslations.JollyCharTrans);
                if (jollyState != null)
                    // ambiguous state: current char can match also with "." pattern!
                    // Let's check recoursivelly if jollyState is a valid transition from this start point
                    // (rembering that * operator is "greedy" and can also match a char before "." operator)
                    if (IsMatch(ref inputString, jollyState, i + 1)) // backtrack implementation
                        return true;

                char chInputSymbol = inputString[i];
                toState = stateCurr.GetTransition(chInputSymbol.ToString()); // get next transition that match this char
                if (toState == null) return false;
                stateCurr = toState;
            }
            return stateCurr.IsFinal;
        }

        /// <summary>
        /// Execute IsMatch() method in the Compiled Class
        /// </summary>
        /// <param name="inputString">String to match</param>
        /// <param name="timer">Stopwatch timer to start before and stop after the execution of the IsMatch() method</param>
        /// <returns></returns>
        private bool ExecuteCompiledClass(string inputString, Stopwatch timer = null)
        {
            try
            {
                return _compiler.Run(inputString, timer);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion
    }
}
