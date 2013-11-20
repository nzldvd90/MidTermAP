using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exercise1
{
    /// <summary>
    /// Rapresent a Non-Deterministic automata
    /// </summary>
    public class NDAutomata : IAutomata<NDAutomata.NDAutomataState>
    {
        #region Private variables

        // Static unique id for automata states
        private static int _curIDState = 0;

        // Start state for this automata
        private NDAutomataState _startState;

        // "static" array of states
        NDAutomataState[] _arrStates = null;

        // "static" array of symbols
        string[] _arrChars = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Create and initialize a new automata
        /// </summary>
        /// <param name="startState">Start state fot this automata</param>
        public NDAutomata(NDAutomataState startState)
        {
            InitializeStartState(startState);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Build all the internal states and the charSet and set stState as initial state
        /// </summary>
        /// <param name="stState">Initial state</param>
        public void InitializeStartState(NDAutomataState stState)
        {
            List<NDAutomataState> _lstStates = new List<NDAutomataState>();

            foreach(var state in GenerateStatesFromStartState(stState)){
                if (!_lstStates.Contains(state)) _lstStates.Add(state);
            }

            _curIDState = 0;
            NDAutomataState initialState = _lstStates[_lstStates.IndexOf(stState)];
            if (initialState == null)
                throw new Exception("Start state " + stState.ToString() + " is not an automata state");

            initialState.ID = NDAutomataState.getNextID();
            foreach (var state in _lstStates)
            {
                if (state != initialState) state.ID = NDAutomataState.getNextID();
            }
            _startState = initialState;
            _arrStates = _lstStates.ToArray();
            _arrChars = GetAllChars();
        }

        #endregion

        #region Automata Subclasses

        /// <summary>
        /// Rapresent a transition from char to all the next states
        /// </summary>
        public class NDTransition
        {
            KeyValuePair<string, List<NDAutomataState>> _internalState;
            public NDTransition(string matchedChar)
            {
                _internalState = new KeyValuePair<string, List<NDAutomataState>>(matchedChar, new List<NDAutomataState>());
            }

            public string MatchedChar
            {
                get { return _internalState.Key; }
            }

            public List<NDAutomataState> NextStates
            {
                get { return _internalState.Value; }
            }

            public NDTransition AddNextState(NDAutomataState newNext)
            {
                if (!_internalState.Value.Contains(newNext))
                    _internalState.Value.Add(newNext);
                return this;
            }

            public NDTransition RemoveNextState(NDAutomataState oldNext)
            {
                if (_internalState.Value.Contains(oldNext))
                    _internalState.Value.Remove(oldNext);
                return this;
            }

            public override bool Equals(object obj)
            {
                //try
                //{
                    NDTransition _temp = (NDTransition)obj;
                    return _temp.MatchedChar == this.MatchedChar;
                /*}
                catch
                {
                    return false;
                }*/
            }

            public override string ToString()
            {
                return this.MatchedChar;
            }

            // to suppress  the unoverride warning
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        /// <summary>
        /// Rapresent a Non-Deterministic automata state
        /// </summary>
        public class NDAutomataState : IAutomataState<NDAutomataState>
        {
            static int _staticId = 0;
            // Set of transition objects
            private List<NDTransition> _transitionFunction;
            private int _id = 0;
            private bool _isFinal = false;

            public NDAutomataState()
            {
                _transitionFunction = new List<NDTransition>();
                _id = _staticId++;
            }

            public int ID
            {
                get { return _id; }
                set { _id = value; }
            }

            public NDTransition[] Transitions
            {
                get { return _transitionFunction.ToArray(); }
            }

            public NDAutomataState AddTransition(string cMatchedChar, NDAutomataState sNextState)
            {
                NDTransition newT = new NDTransition(cMatchedChar);
                if (_transitionFunction.Contains(newT)) // already exists an association for this char in this state
                {
                    _transitionFunction[_transitionFunction.IndexOf(newT)].AddNextState(sNextState);
                }
                else // first association for this char
                {
                    _transitionFunction.Add(newT.AddNextState(sNextState));
                }
                return this;

            }

            public List<NDAutomataState> GetTransitions(string sCharToMatch)
            {
                NDTransition _temp = new NDTransition(sCharToMatch);
                if (_transitionFunction.Contains(_temp))
                {
                    return _transitionFunction[_transitionFunction.IndexOf(_temp)].NextStates;
                }
                return null;
            }

            public void RemoveTransition(string sInputSymbol)
            {
                NDTransition _temp = new NDTransition(sInputSymbol);
                try
                {
                    NDTransition _tempTransition = _transitionFunction[_transitionFunction.IndexOf(_temp)];
                    _transitionFunction.Remove(_tempTransition);
                }
                catch { }
            }

            public int ReplaceTransitionState(NDAutomataState stateOld, NDAutomataState stateNew)
            {
                int nReplacementCount = 0;
                List<NDAutomataState> setTrans = null;
                foreach (var de in _transitionFunction)
                {
                    setTrans = de.NextStates;
                    if (setTrans.Contains(stateOld))
                    {
                        de.RemoveNextState(stateOld);
                        de.AddNextState(stateNew);
                        nReplacementCount++;
                    }
                }
                return nReplacementCount;
            }


            public bool IsDeadState()
            {
                if (IsFinal) return false; // terminal states are not dead states

                if (_transitionFunction.Count == 0) return false; // if no chars are matched with this state it's not a dead state

                List<NDAutomataState> setToState = null;
                foreach (var de in _transitionFunction)
                {
                    setToState = de.NextStates;

                    foreach (var state in setToState)  // in a DFA, it should only iterate once
                    {
                        if (!state.Equals(this)) return false;
                    }
                }

                return true;
            }

            public List<string> GetAllChars()
            {
                List<string> _result = new List<string>();
                foreach (var st in _transitionFunction)
                {
                    if (!_result.Contains(st.MatchedChar)) _result.Add(st.MatchedChar);
                }
                return _result;
            }

            public override string ToString()
            {
                string s = "s" + this.ID.ToString();
                if (this.IsFinal)
                {
                    s = "{" + s + "}";
                }
                return s;
            }

            public override bool Equals(object obj)
            {
                NDAutomataState _temp = (NDAutomataState)obj;
                return _temp.ID == this.ID;
            }

            // to suppress  the unoverride warning
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public bool IsFinal
            {
                get { return _isFinal; }
                set { _isFinal = value; }
            }

            NDAutomataState GetSingleTransition(string cMatchedChar)
            {
                try
                {
                    NDTransition _temp = new NDTransition(cMatchedChar);
                    if (_transitionFunction.Contains(_temp))
                    {
                        return _transitionFunction[_transitionFunction.IndexOf(_temp)].NextStates[0];
                    }
                    return null;
                }
                catch
                {
                    return null;
                }
            }

            internal static int getNextID()
            {
                return _curIDState++;
            }
        }

        #endregion

        #region Property

        /// <summary>
        /// Initial automata state
        /// </summary>
        public NDAutomataState StartState
        {
            get { return _startState; }
        }

        /// <summary>
        /// Gets all automata states
        /// </summary>
        public NDAutomata.NDAutomataState[] States
        {
            get { return _arrStates; }
        }

        /// <summary>
        /// Gets all the automata chars
        /// </summary>
        public string[] Chars
        {
            get { return _arrChars; }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Generate all the automata states from a start state
        /// </summary>
        /// <param name="stateStart">Initial state</param>
        /// <returns></returns>
        static internal NDAutomata.NDAutomataState[] GenerateStatesFromStartState(Exercise1.NDAutomata.NDAutomataState stateStart)
        {
            List<NDAutomata.NDAutomataState> lstResults = new List<NDAutomata.NDAutomataState>();
            Stack<NDAutomata.NDAutomataState> unprocessedStates = new Stack<NDAutomata.NDAutomataState>();

            unprocessedStates.Push(stateStart);

            while (unprocessedStates.Count > 0)
            {
                NDAutomata.NDAutomataState state = unprocessedStates.Pop();
                if (!lstResults.Contains(state)) lstResults.Add(state);

                foreach (var curSymbol in state.GetAllChars())
                {
                    List<NDAutomata.NDAutomataState> arrTrans = state.GetTransitions(curSymbol);
                    if (arrTrans != null)
                    {
                        foreach (NDAutomata.NDAutomataState stateEpsilon in arrTrans)
                        {
                            if (!lstResults.Contains(stateEpsilon)) unprocessedStates.Push(stateEpsilon);
                        }
                    }
                }
            }   
            return lstResults.ToArray();
        }

        /// <summary>
        /// Get an unique id from the last ResetID() call
        /// </summary>
        /// <returns>Unique ID</returns>
        private int GetNextId()
        {
            return _curIDState++;
        }

        /// <summary>
        /// Gets the internal char set for this automata
        /// </summary>
        /// <returns></returns>
        private string[] GetAllChars()
        {
            List<string> _result = new List<string>();
            foreach (var state in _arrStates)
            {
                _result = _result.Union(state.GetAllChars()).ToList();
            }

            return _result.ToArray();
        }

        #endregion
    }
}
