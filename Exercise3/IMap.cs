using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Exercise3
{
    // PROGRAMMING BY CONTRACT: this interface is documented using .NET standard documentation
    // that don't have REQUIRES and EFFECTS explicit tags but:
    // - the REQUIRES are implicit written in the <exception> documentation of each method signature
    // - the EFFECTS are described in the <return> documentation of each method signature
    /// <summary>
    /// Rapresent a generic set of association (Key, Value) where Key is of type T and Key is of type U.
    /// </summary>
    /// <typeparam name="T">Keys type.</typeparam>
    /// <typeparam name="U">Values type.</typeparam>
    public interface IMap<T, U> : ICollection<KeyValuePair<T, U>>, IEnumerable<KeyValuePair<T, U>>, IEnumerable
    {
        /// <summary>
        /// Get a collection of all the keys for this map
        /// </summary>
        ICollection<T> Keys { get; }

        /// <summary>
        /// Get a collection of all the values in this map
        /// </summary>
        ICollection<U> Values
        {
            get;
        }

        /// <summary>
        /// Get or Set the value of a specific key of this map
        /// </summary>
        /// <param name="key">Key of the element to get or set.</param>
        /// <returns>Element associated to the specified key.</returns>
        /// <exception cref="System.ArgumentNullException">key is null.</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Key not exists in this map.</exception>
        U this[T key] { get; set; }

        /// <summary>
        /// Add a new association (<para>key</para>, <para>value</para>) to the map.
        /// </summary>
        /// <param name="key">Key of the new pair (unique in the map).</param>
        /// <param name="value">Value of the new pair (not necessary unique).</param>
        /// <exception cref="System.ArgumentNullException">key is null.</exception>
        /// <exception cref="System.ArgumentException">A pair with the specified key already exists.</exception>
        void Add(T key, U value);

        /// <summary>
        /// Determinate if this map have at least one association to the specified key
        /// </summary>
        /// <param name="key">Key to find in this map.</param>
        /// <returns>true if exists an association for the provided key; false otherwise</returns>
        /// <exception cref="System.ArgumentNullException">key is null.</exception>
        bool ContainsKey(T key);

        /// <summary>
        /// Allow to remove the (key, value) pair associated to the provided key.
        /// </summary>
        /// <param name="key">Key of the pair to remove.</param>
        /// <returns>true if key associated pair has been removed; false otherwise.
        /// This method returns false even if key does not exists.
        /// See also <seealso cref="ContainsKey"/> to check if key is in this map.</returns>
        /// <exception cref="System.ArgumentNullException">key is null.</exception>
        bool Remove(T key);

        /// <summary>
        /// Try to get the value associated to the specified key.
        /// </summary>
        /// <param name="key">Key to find in the map.</param>
        /// <param name="value">If <para>key</para> has been found in the map this parameter is set
        /// with the <para>value</para> associated to the <para>key</para>; default value of the param otherwise.
        /// This param is passed without initialization.</param>
        /// <returns>true if exists an association for the specified <para>key</para></returns>
        /// <exception cref="System.ArgumentNullException">key is null.</exception>
        bool TryGetValue(T key, out U value);
    }
}
