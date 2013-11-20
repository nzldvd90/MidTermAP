using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exercise1
{
    /// <summary>
    /// Rappresenting a generic automata
    /// </summary>
    /// <typeparam name="T">Automata state type</typeparam>
    public interface IAutomata<T>
    {
        /// <summary>
        /// Gets all the automata chars
        /// </summary>
        string[] Chars { get; }

        /// <summary>
        /// Gets all the automata states
        /// </summary>
        T[] States { get; }

        /// <summary>
        /// Get the automata start state
        /// </summary>
        T StartState { get; }

        /// <summary>
        /// Get the automata start state
        /// </summary>
        void InitializeStartState(T startState);

    }

    /// <summary>
    /// Rapresent a generic automata state
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAutomataState<T>
    {
        /// <summary>
        /// Gets a boolean indicates if this state is a final state (accepting state)
        /// </summary>
        bool IsFinal { get; }
    }
}
