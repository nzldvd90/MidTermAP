using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Exercise1
{
    /// <summary>
    /// Class having a list of usefull methods to wrap (NFAutomata to DAutomata) and (DAutomata to optimized DAutomata)
    /// </summary>
    internal class AutomataWrapper
    {
        // OK
        /// <summary>
        /// Internal structure that associate an epsilon-closure to an automata state
        /// </summary>
        private Dictionary<DAutomata.DAutomataState, EpsilonClosure> mapDStateToEnclosure;

        // OK
        /// <summary>
        /// Create a new wrapper
        /// </summary>
        public AutomataWrapper()
        {
            mapDStateToEnclosure = new Dictionary<DAutomata.DAutomataState, EpsilonClosure>();
        }

        //OK
        /// <summary>
        /// Add a new association (DAutomataState, NDAutomataState[]) to the map
        /// </summary>
        /// <param name="stateDfa">Deterministic automata state</param>
        /// <param name="setEpsilonClosure">Epsilon closure</param>
        private void AddStateToMap(DAutomata.DAutomataState stateDfa, List<NDAutomata.NDAutomataState> setEpsilonClosure)
        {
            EpsilonClosure stateRecord = new EpsilonClosure();
            stateRecord.Closure = setEpsilonClosure;
            mapDStateToEnclosure[stateDfa] = stateRecord;
        }

        /// <summary>
        /// Get the mapped DAutomataState for current map.
        /// </summary>
        /// <param name="setEpsilonClosure">set of EpsiloClosure state as search criteria</param>
        /// <returns>DAutomataState if state is mapped; null otherwise</returns>
        private DAutomata.DAutomataState GetStateFromClosure(List<NDAutomata.NDAutomataState> setEpsilonClosure)
        {
            foreach (var stateRecord in mapDStateToEnclosure)
            {
                if (AreTheSameSet(stateRecord.Value.Closure, setEpsilonClosure)) return stateRecord.Key;
            }
            return null;
        }

        private bool AreTheSameSet(List<NDAutomata.NDAutomataState> list, List<NDAutomata.NDAutomataState> setEpsilonClosure)
        {
            if (list.Count != setEpsilonClosure.Count) return false;

            for (int i = 0; i < list.Count; i++)
            {
                if (!list.Contains(setEpsilonClosure[i])) return false;
            }

            return true;
        }

        private List<NDAutomata.NDAutomataState> GetClosureFromState(DAutomata.DAutomataState state)
        {
            return mapDStateToEnclosure[state].Closure;
        }

        /// <summary>
        /// Get the first unmarked state from the current map
        /// </summary>
        /// <returns>An unmarked state</returns>
        private bool GetUnmarkedState(out DAutomata.DAutomataState state)
        {
            foreach (var stateRecord in mapDStateToEnclosure)
            {
                if (!stateRecord.Value.Marked)
                {
                    state = stateRecord.Key;
                    return true;
                }
            }
            state = default(DAutomata.DAutomataState);
            return false;
        }

        /// <summary>
        /// Mark a state
        /// </summary>
        /// <param name="stateT">DAutomataState to set as Marked</param>
        private void MarkState(DAutomata.DAutomataState stateT)
        {
            mapDStateToEnclosure[stateT].Marked = true;
        }

        /// <summary>
        /// Return a list of all the states whitch have an enclosure in the map
        /// </summary>
        /// <returns>List of states; Empty list if no states are mapped</returns>
        private List<DAutomata.DAutomataState> GetDAutomataStates()
        {
            List<DAutomata.DAutomataState> setStates = new List<DAutomata.DAutomataState>();

            foreach (var objKey in mapDStateToEnclosure)
            {
                setStates.Add(objKey.Key);
            }

            return setStates;
        }

        /// <summary>
        /// Apply the Thompson’s Algorithm to create an automata from a regular expression in posfix form
        /// </summary>
        /// <param name="sRegExPosfix">Regulare expression in postfix form</param>
        /// <returns>Corrispondent Non Deterministic automata</returns>
        public static NDAutomata CreateNFAutomata(string sRegExPosfix)
        {
            Stack<NDAutomataEdge> NFAutomataStack = new Stack<NDAutomataEdge>();
            NDAutomataEdge expr, tempExpr1, tempExpr2, newExpr;
            bool inEscapeChar = false;

            foreach (char curChar in sRegExPosfix)
            {
                if (!inEscapeChar)
                {
                    if (curChar == RegularExpressionParser.MetaChars.EscapeChar)
                    {
                        inEscapeChar = true;
                        continue;
                    }
                }
                else
                {
                    newExpr = new NDAutomataEdge();
                    newExpr.FromState.AddTransition(curChar.ToString(), newExpr.ToState);

                    NFAutomataStack.Push(newExpr);

                    inEscapeChar = false;
                    continue;
                }

                switch (curChar)
                {
                    case RegularExpressionParser.MetaChars.KleeneStar:  // A*  Kleene star

                        newExpr = new NDAutomataEdge();
                        tempExpr1 = NFAutomataStack.Pop();

                        tempExpr1.ToState.AddTransition(RegularExpressionParser.MetaCharsTranslations.EpsilonChar, tempExpr1.FromState);
                        tempExpr1.ToState.AddTransition(RegularExpressionParser.MetaCharsTranslations.EpsilonChar, newExpr.ToState);

                        newExpr.FromState.AddTransition(RegularExpressionParser.MetaCharsTranslations.EpsilonChar, tempExpr1.FromState);
                        newExpr.FromState.AddTransition(RegularExpressionParser.MetaCharsTranslations.EpsilonChar, newExpr.ToState);

                        NFAutomataStack.Push(newExpr);

                        break;
                    case RegularExpressionParser.MetaChars.PatternAlternate:  // A|B
                        tempExpr2 = NFAutomataStack.Pop();
                        tempExpr1 = NFAutomataStack.Pop();

                        newExpr = new NDAutomataEdge();

                        tempExpr1.ToState.AddTransition(RegularExpressionParser.MetaCharsTranslations.EpsilonChar, newExpr.ToState);
                        tempExpr2.ToState.AddTransition(RegularExpressionParser.MetaCharsTranslations.EpsilonChar, newExpr.ToState);

                        newExpr.FromState.AddTransition(RegularExpressionParser.MetaCharsTranslations.EpsilonChar, tempExpr1.FromState);
                        newExpr.FromState.AddTransition(RegularExpressionParser.MetaCharsTranslations.EpsilonChar, tempExpr2.FromState);

                        NFAutomataStack.Push(newExpr);

                        break;

                    case RegularExpressionParser.MetaChars.PatternConcatenate:  // "a$b" (or "ab" in postfix form)
                        tempExpr2 = NFAutomataStack.Pop();
                        tempExpr1 = NFAutomataStack.Pop();

                        tempExpr1.ToState.AddTransition(RegularExpressionParser.MetaCharsTranslations.EpsilonChar, tempExpr2.FromState);

                        newExpr = new NDAutomataEdge(tempExpr1.FromState, tempExpr2.ToState);
                        NFAutomataStack.Push(newExpr);

                        break;

                    case RegularExpressionParser.MetaChars.OptionalPattern:  // A? => A|empty  
                        tempExpr1 = NFAutomataStack.Pop();
                        newExpr = new NDAutomataEdge();

                        newExpr.FromState.AddTransition(RegularExpressionParser.MetaCharsTranslations.EpsilonChar, tempExpr1.FromState);
                        newExpr.FromState.AddTransition(RegularExpressionParser.MetaCharsTranslations.EpsilonChar, newExpr.ToState);
                        tempExpr1.ToState.AddTransition(RegularExpressionParser.MetaCharsTranslations.EpsilonChar, newExpr.ToState);

                        NFAutomataStack.Push(newExpr);

                        break;
                    case RegularExpressionParser.MetaChars.JollyChar:
                        newExpr = new NDAutomataEdge();
                        newExpr.FromState.AddTransition(RegularExpressionParser.MetaCharsTranslations.JollyCharTrans, newExpr.ToState);
                        NFAutomataStack.Push(newExpr);
                        break;

                    default:
                        newExpr = new NDAutomataEdge();
                        newExpr.FromState.AddTransition(curChar.ToString(), newExpr.ToState);

                        NFAutomataStack.Push(newExpr);

                        break;
                }
            }

            expr = NFAutomataStack.Pop();  // pop the very last one.  YES, THERE SHOULD ONLY BE ONE LEFT AT THIS POINT
            expr.ToState.IsFinal = true;  // the very last state is the accepting state of the NFA

            NDAutomata.NDAutomataState startState = expr.FromState;
            NDAutomata newNFAutomata = new NDAutomata(startState);
            return newNFAutomata;  // retun the NFA

        }

        //OK
        /// <summary>
        /// Converts NDAutomata to DAutomata using "Subset Construction" algorithm.
        /// The algoritm is described at page 153 (Algoritm 3.20) of the Dragon Book.
        /// </summary>
        /// <param name="_NFAutomata">Non-Deterministic automata to convert</param>
        /// <returns>Equivalent Deterministic Automata</returns>
        public static DAutomata CreateDAutomata(NDAutomata _NFAutomata)
        {
            List<string> setAllInput = _NFAutomata.Chars.ToList();
            NDAutomata.NDAutomataState[] setAllState = _NFAutomata.States;
            AutomataWrapper helper = new AutomataWrapper();
            List<NDAutomata.NDAutomataState> setMove = null;
            List<NDAutomata.NDAutomataState> setEpsilonClosure = null;
            string charS = String.Empty;
            DAutomata.DAutomataState stateT = null;
            List<NDAutomata.NDAutomataState> setT = null;
            DAutomata.DAutomataState stateU = null;

            setAllInput.Remove(RegularExpressionParser.MetaCharsTranslations.EpsilonChar);
            setEpsilonClosure = GetEpsilonClosure(_NFAutomata.StartState);
            DAutomata.DAutomataState startDAutomataState = new DAutomata.DAutomataState();
            if (IsFinalGroup(setEpsilonClosure) == true) startDAutomataState.IsFinal = true;
            helper.AddStateToMap(startDAutomataState, setEpsilonClosure);

            while (helper.GetUnmarkedState(out stateT))
            {
                helper.MarkState(stateT);
                setT = helper.GetClosureFromState(stateT);
                foreach (object obj in setAllInput)
                {
                    charS = obj.ToString();
                    setMove = Move(setT, charS);
                    if (setMove.Count > 0)
                    {
                        setEpsilonClosure = GetEpsilonClosure(setMove);
                        stateU = helper.GetStateFromClosure(setEpsilonClosure);
                        if (stateU == null) // so set setEpsilonClosure must be a new one and we should crate a new DAutomata state
                        {
                            stateU = new DAutomata.DAutomataState();
                            if (IsFinalGroup(setEpsilonClosure) == true) stateU.IsFinal = true;
                            helper.AddStateToMap(stateU, setEpsilonClosure);  // add new state (as unmarked by default)
                        }
                        stateT.AddTransition(charS, stateU);
                    }
                }
            }

            return new DAutomata(startDAutomataState);
        }

        //OK
        /// <summary>
        /// Finds all state reachable from the specic state on Epsilon transition.
        /// For details see Dragon book on page 153.
        /// </summary>
        /// <param name="stateStart">State from which search begins</param>
        /// <returns>A set of all state reachable from teh startState on Epsilon transtion</returns>
        private static List<NDAutomata.NDAutomataState> GetEpsilonClosure(NDAutomata.NDAutomataState stateStart)
        {
            List<NDAutomata.NDAutomataState> setProcessed = new List<NDAutomata.NDAutomataState>();
            List<NDAutomata.NDAutomataState> setUnprocessed = new List<NDAutomata.NDAutomataState>();

            setUnprocessed.Add(stateStart);

            while (setUnprocessed.Count > 0)
            {
                NDAutomata.NDAutomataState state = (NDAutomata.NDAutomataState)setUnprocessed[0];
                List<NDAutomata.NDAutomataState> arrTrans = state.GetTransitions(RegularExpressionParser.MetaCharsTranslations.EpsilonChar);
                setProcessed.Add(state);
                setUnprocessed.Remove(state);

                if (arrTrans != null)
                {
                    foreach (NDAutomata.NDAutomataState stateEpsilon in arrTrans)
                    {
                        if (!setProcessed.Contains(stateEpsilon)) setUnprocessed.Add(stateEpsilon);
                    }
                }
            }

            return setProcessed;
        }

        //OK
        /// <summary>
        /// Get a list of all the reachable states from a list of states using Epsilon transition.
        /// For details see Dragon book on page 153.
        /// </summary>
        /// <param name="setState">Set of states to search from</param>
        /// <returns>Epsilon Enclosure</returns>
        private static List<NDAutomata.NDAutomataState> GetEpsilonClosure(List<NDAutomata.NDAutomataState> setStates)
        {
            List<NDAutomata.NDAutomataState> setAllEpsilonClosure = new List<NDAutomata.NDAutomataState>();
            foreach (NDAutomata.NDAutomataState state in setStates)
            {
                List<NDAutomata.NDAutomataState> setEpsilonClosure = GetEpsilonClosure(state);
                setAllEpsilonClosure = setAllEpsilonClosure.Union(setEpsilonClosure).ToList();
            }
            return setAllEpsilonClosure;
        }

        //OK
        /// <summary>
        /// Check if a group of NDAutomataStates contains a finals state
        /// </summary>
        /// <param name="group">Set of states</param>
        /// <returns>true if group contains a final states; false otherwise</returns>
        private static bool IsFinalGroup(List<NDAutomata.NDAutomataState> group)
        {
            foreach (NDAutomata.NDAutomataState state in group)
            {
                if (state.IsFinal) return true;
            }
            return false;
        }

        //OK
        /// <summary>
        /// Check if a group of DAutomataStates contains a finals state
        /// </summary>
        /// <param name="group">Set of states</param>
        /// <returns>true if group contains a final states; false otherwise</returns>
        private static bool GroupContainsFinalGroup(List<DAutomata.DAutomataState> setGroup)
        {
            foreach (DAutomata.DAutomataState state in setGroup)
            {
                if (state.IsFinal) return true;
            }
            return false;
        }

        //OK
        /// <summary>
        /// Collection of NDAutomataStates to which there is a transition on symbol charS for some state in states
        /// </summary>
        /// <param name="states">Collection of set to check in</param>
        /// <param name="chInputSymbol">Symbol to check</param>
        /// <returns>Set of Moves</returns>
        private static List<NDAutomata.NDAutomataState> Move(List<NDAutomata.NDAutomataState> states, string charSymbol)
        {
            List<NDAutomata.NDAutomataState> set = new List<NDAutomata.NDAutomataState>();

            foreach (NDAutomata.NDAutomataState obj in states)
            {
                List<NDAutomata.NDAutomataState> setMove = Move(obj, charSymbol);
                set = set.Union(setMove).ToList();
            }

            return set;
        }

        //OK
        /// <summary>
        /// Collection of NDAutomataStates to which there is a transition on symbol charS for state
        /// </summary>
        /// <param name="state">State to check in</param>
        /// <param name="chInputSymbol">Symbol to check</param>
        /// <returns>Set of Moves</returns>
        private static List<NDAutomata.NDAutomataState> Move(NDAutomata.NDAutomataState state, string charSymbol)
        {
            List<NDAutomata.NDAutomataState> collectionStates = new List<NDAutomata.NDAutomataState>();
            List<NDAutomata.NDAutomataState> transitions = state.GetTransitions(charSymbol);

            if (transitions != null) collectionStates.AddRange(transitions);

            return collectionStates;
        }

        //OK
        /// <summary>
        /// Minimize a Deterministic Automata using the minimize Algorithm (described here http://www.cs.engr.uky.edu/~lewis/essays/compilers/min-fa.html)
        /// </summary>
        /// <param name="_originalDAutomata">Deterministic Automata to reduce</param>
        /// <returns>Minimized Deterministic Automata</returns>
        public static DAutomata MinimizeDAutomata(DAutomata _originalDAutomata)
        {
            List<string> transSymbols = _originalDAutomata.Chars.ToList();
            List<DAutomata.DAutomataState> DAutomataStates = _originalDAutomata.States.ToList();
            DAutomata.DAutomataState startMinimizedState = null;

            var arr = PartitionGroupsDAutomata(DAutomataStates, transSymbols);

            foreach (var setGroup in arr)
            {

                // check final states SzeroDFA in the group
                bool finalStInGroup = GroupContainsFinalGroup(setGroup);
                bool startDFAinGroup = setGroup.Contains(_originalDAutomata.StartState);

                DAutomata.DAutomataState examinatedState = (DAutomata.DAutomataState)setGroup[0];

                if (startDFAinGroup) startMinimizedState = examinatedState;
                if (finalStInGroup) examinatedState.IsFinal = true;
                if (setGroup.Count == 1) continue;

                setGroup.Remove(examinatedState);

                int numOfReplace = 0;
                foreach (var stateToBeReplaced in setGroup)
                {
                    DAutomataStates.Remove(stateToBeReplaced);
                    foreach (var objState in DAutomataStates)
                    {
                        numOfReplace = numOfReplace + objState.ReplaceTransitionState(stateToBeReplaced, examinatedState);
                    }
                }
            }

            foreach (var state in DAutomataStates)
            {
                if (state.IsDeadState()) DAutomataStates.Remove(state);
            }

            DAutomata newOptimizedDFA = new DAutomata(startMinimizedState);
            return newOptimizedDFA;
        }

        //OK
        /// <summary>
        /// Implementation of group partition algorithm of a graph (automata).
        /// (see http://en.wikipedia.org/wiki/Graph_partition for algorithm details)
        /// </summary>
        /// <param name="DAutomataStates">The states to partite in groups</param>
        /// <param name="setInputSymbol">Automata char set to check equivalent automata paths</param>
        /// <returns>List of Automata group</returns>
        private static List<List<DAutomata.DAutomataState>> PartitionGroupsDAutomata(List<DAutomata.DAutomataState> DAutomataStates, List<string> setInputSymbol)
        {

            // Temporany list of the Deterministic Automata States
            List<List<DAutomata.DAutomataState>> tempGroups = new List<List<DAutomata.DAutomataState>>();

            // Transitions to explore
            Hashtable transitionsMap = new Hashtable();

            List<DAutomata.DAutomataState> emptySet = new List<DAutomata.DAutomataState>();
            List<DAutomata.DAutomataState> finalStates = new List<DAutomata.DAutomataState>();
            List<DAutomata.DAutomataState> otherStates = new List<DAutomata.DAutomataState>();

            // create 2 groups of states: Final and Non-Final states
            foreach (var state in DAutomataStates)
            {
                if (state.IsFinal)
                    finalStates.Add(state);
                else
                    otherStates.Add(state);
            }

            // create the first group (collection of all the Non-Final states)
            if (otherStates.Count > 0)
                tempGroups.Add(otherStates);

            // create the second group (collection of all the Final states)
            tempGroups.Add(finalStates);

            // here tempGroup have 1 or 2 groups.

            IEnumerator charsIterator = setInputSymbol.GetEnumerator();
            // foreach char
            while (charsIterator.MoveNext())
            {
                string charS = charsIterator.Current.ToString();

                int i = 0;
                while (i < tempGroups.Count)
                {
                    List<DAutomata.DAutomataState> setToBePartitioned = tempGroups[i];
                    i++;

                    // the set have less then 2 elements: skip this set
                    if (setToBePartitioned.Count == 0 || setToBePartitioned.Count == 1) continue;

                    // search for a set that can be divided into 2
                    foreach (var state in setToBePartitioned)
                    {
                        DAutomata.DAutomataState stateTransionTo = state.GetTransition(charS.ToString());
                        CreateOrAddToMap(transitionsMap, state, stateTransionTo == null ? emptySet : FindGroup(tempGroups, stateTransionTo));
                    }

                    if (transitionsMap.Count > 1) 
                    {
                        // exist a state that transit into different groups
                        tempGroups.Remove(setToBePartitioned);
                        foreach (DictionaryEntry setValue in transitionsMap)
                        {
                            tempGroups.Add((List<DAutomata.DAutomataState>)setValue.Value); 
                        }

                        // check again re-iterating for all char
                        i = 0;
                        charsIterator.Reset();
                    }
                    transitionsMap.Clear();
                }
            }

            return tempGroups;
        }

        //OK
        /// <summary>
        /// Merge an hash element if exist already an association or create a new association (setFound-->{state})
        /// </summary>
        /// <param name="hash">The hash table containig the map (group; {states})</param>
        /// <param name="state">State to add in the map</param>
        /// <param name="setFound">key of the map</param>
        private static void CreateOrAddToMap(Hashtable hash, DAutomata.DAutomataState state, List<DAutomata.DAutomataState> setFound)
        {

            if (!hash.ContainsKey(setFound))
            {
                var cur = new List<DAutomata.DAutomataState>();
                cur.Add(state);
                hash.Add(setFound, cur);
            }
            else
            {
                var cur = (List<DAutomata.DAutomataState>)hash[setFound];
                cur.Add(state);
                hash.Remove(setFound);
                hash.Add(setFound, cur);
            }
        }
        
        //OK
        /// <summary>
        /// Find a set in a list of groups for a specific state.
        /// </summary>
        /// <param name="groups">List of groups</param>
        /// <param name="state">State to search for</param>
        /// <returns>Set the state belongs to</returns>
        private static List<DAutomata.DAutomataState> FindGroup(List<List<DAutomata.DAutomataState>> groups, DAutomata.DAutomataState state)
        {
            foreach (var set in groups)
            {
                if (set.Contains(state)) return set;
            }
            return null;
        }

        #region Inner class

        //OK
        /// <summary>
        /// A class rappresenting an epsilon-enclosure.
        /// </summary>
        public class EpsilonClosure
        {
            List<NDAutomata.NDAutomataState> _enclosure = new List<NDAutomata.NDAutomataState>();
            bool _marked = false;

            /// <summary>
            /// Collection of NAutomataStates from which the DAutomataState comes to.
            /// </summary>
            public List<NDAutomata.NDAutomataState> Closure
            {
                get { return _enclosure; }
                set { _enclosure = value; }
            }

            /// <summary>
            /// Indicate if this enclosure has been processed.
            /// </summary>
            public bool Marked
            {
                get { return _marked; }
                set { _marked = value; }
            }

        }

        //OK
        /// <summary>
        /// Class for constructing the NFAutomata from the postfix regular expression
        /// </summary>
        public class NDAutomataEdge
        {
            #region Private variables

            // start state of the edge
            NDAutomata.NDAutomataState _fromState = null;

            // end state of the edge
            NDAutomata.NDAutomataState _toState = null;

            #endregion

            #region Constructors

            /// <summary>
            /// Build a new edge from 2 new NFAutomataState
            /// </summary>
            public NDAutomataEdge()
            {
                _fromState = new NDAutomata.NDAutomataState();
                _toState = new NDAutomata.NDAutomataState();
            }

            /// <summary>
            /// Build an edge from fromState to toState
            /// </summary>
            /// <param name="fromState">Start state for the edge</param>
            /// <param name="toState">End State of the edge</param>
            public NDAutomataEdge(NDAutomata.NDAutomataState fromState, NDAutomata.NDAutomataState toState)
            {
                _fromState = fromState;
                _toState = toState;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Get the start state of this edge
            /// </summary>
            public NDAutomata.NDAutomataState FromState
            {
                get { return _fromState; }
            }

            /// <summary>
            /// Get the final state of this edge
            /// </summary>
            public NDAutomata.NDAutomataState ToState
            {
                get { return _toState; }
            }

            #endregion
        }

        #endregion
    }
}
