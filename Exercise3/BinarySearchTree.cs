using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exercise3
{
    // PROGRAMMING BY CONTRACT: this class is documented using .NET standard documentation
    // that don't have REQUIRES and EFFECTS explicit tags but:
    // - the REQUIRES are implicit written in the <exception> documentation of each method
    // - the EFFECTS are described in the <return> documentation of each method
    /// <summary>
    /// Implementation of <see cref="IMap"/> interface as generic binary search tree
    /// </summary>
    /// <typeparam name="T">Keys type</typeparam>
    /// <typeparam name="U">Values type</typeparam>
    public class BSTMap<T, U> : IMap<T, U> where T : IComparable
    {
        // README: Implementation is not required but for the Add/Remove/Clear methods
        // i've write a part of the implementation to show that _count local variable
        // is ever significative with the tree structure.

        #region Local static environment variables

        /// <summary>
        /// Pointer to the first element of the three
        /// </summary>
        KeyValuePair<T, U> _root;

        /// <summary>
        /// for efficiency reason mantains a partial count of the nodes
        /// integrity is guaranteed to the fact that Add() method to add a new
        /// Node is called from the tree structure and not from Node.
        /// Is Add/Remove success increment/decrement this counter.
        /// </summary>
        int _count = 0;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new empty tree
        /// </summary>
        public BSTMap()
        {
            // do nothing
        }

        public BSTMap(KeyValuePair<T, U> root)
        {
            // set the root
            _root = root;
            _count = 1;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add a new node (<para>key</para>, <para>value</para>) to this tree.
        /// </summary>
        /// <param name="key">Key of the new Node (unique in this tree).</param>
        /// <param name="value">Value of the new Node (not necessary unique).</param>
        /// <exception cref="System.ArgumentNullException">key is null.</exception>
        /// <exception cref="System.ArgumentException">A pair with the specified key already exists.</exception>
        public void Add(T key, U value)
        {
            this.Add(new KeyValuePair<T, U>(key, value));
        }

        /// <summary>
        /// <see cref="BSTTree.Add(T,U)"/>
        /// </summary>
        public void Add(KeyValuePair<T, U> item)
        {
            try
            {
                // Implementation of Tree Add: not required
                Count++;
                throw new NotImplementedException(); //Not required
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Allow to remove all the Nodes from this tree.
        /// </summary>
        public void Clear()
        {
            // Implementation of the tree clear
            Count = 0;
            throw new NotImplementedException(); //Not required
        }

        /// <summary>
        /// Determinate if this tree contains a Node having the specified key
        /// </summary>
        /// <param name="key">Key to find in this tree.</param>
        /// <returns>true if exists a Node having the provided key; false otherwise</returns>
        /// <exception cref="System.ArgumentNullException">key is null.</exception>
        public bool ContainsKey(T key)
        {
            throw new NotImplementedException(); //Not required
        }

        /// <summary>
        /// Determinate if this tree contains a specific Node
        /// </summary>
        /// <param name="item">Node to find in this tree.</param>
        /// <returns>true if Node exists in this tree; false otherwise</returns>
        /// <exception cref="System.ArgumentNullException">key is null.</exception>
        public bool Contains(KeyValuePair<T, U> item)
        {
            throw new NotImplementedException(); //Not required
        }

        /// <summary>
        /// Copy all the elements of this tree into a System.Array object, starting from a specific
        /// index of the destination array.
        /// </summary>
        /// <param name="array">Unidimentional array in which ekements must to be copied.
        /// System.Array indicization is zero-based.</param>
        /// <param name="arrayIndex">Zero-based index to use as start point for the copy.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">arrayIndex is negative.</exception>
        /// <exception cref="System.ArgumentNullException">array is null.</exception>
        public void CopyTo(KeyValuePair<T, U>[] array, int arrayIndex)
        {
            throw new NotImplementedException(); //Not required
        }

        /// <summary>
        /// Support simple interation of the elements of this tree
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<T, U>> GetEnumerator()
        {
            throw new NotImplementedException(); //Not required
        }

        /// <summary>
        /// <see cref="System.Collections.IEnumerable.GetEnumerator()"/>
        /// </summary>
        /// <returns>Non generic enumerator for the elements of this tree</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException(); //Not required
        }

        /// <summary>
        /// Allow to remove the (key, value) pair associated to the provided key.
        /// </summary>
        /// <param name="key">Key of the pair to remove.</param>
        /// <returns>true if key associated pair has been removed; false otherwise.
        /// This method returns false even if no-one Node have the specified key.
        /// Use <seealso cref="ContainsKey"/> to check if exists a Node having the
        /// specified key is in this tree.</returns>
        /// <exception cref="System.ArgumentNullException">key is null.</exception>
        public bool Remove(T key)
        {
            // private implementation of the remove node by key
            return RemoveNode(key);
        }

        /// <summary>
        /// Allow to remove the (key, value) pair associated to the provided key.
        /// </summary>
        /// <param name="item">pair to remove (only key is used).</param>
        /// <returns>true if key associated pair has been removed; false otherwise.
        /// This method returns false even if no-one Node have the specified key.
        /// Use <seealso cref="ContainsKey"/> to check if exists a Node having the
        /// specified key is in this tree.</returns>
        /// <exception cref="System.ArgumentNullException">key is null.</exception>
        public bool Remove(KeyValuePair<T, U> item)
        {
            // private implementation of the remove node
            return RemoveNode(item);
        }

        /// <summary>
        /// Get or Set the value of a specific key of this tree
        /// </summary>
        /// <param name="key">Key of the element to get or set.</param>
        /// <returns>Element associated to the specified key.</returns>
        /// <exception cref="System.ArgumentNullException">key is null.</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Key not exists in this map.</exception>
        public U this[T key]
        {
            get
            {
                // ArgumentNullException is throwed from SearchNode() method
                BSTNode<T,U> _node = SearchNode(key);
                if(_node==null) throw new KeyNotFoundException();
                return _node.Value;
            }
            set
            {
                // ArgumentNullException is throwed from SearchNode() method
                BSTNode<T, U> _node = SearchNode(key);
                if (_node == null) throw new KeyNotFoundException();
                SearchNode(key).Value = value;
            }
        }

        /// <summary>
        /// Try to get the value associated to the specified key.
        /// </summary>
        /// <param name="key">Key to find in the map.</param>
        /// <param name="value">If <para>key</para> has been found in the map this parameter is set
        /// with the <para>value</para> associated to the <para>key</para>; default value of the param otherwise.
        /// This param is passed without initialization.</param>
        /// <returns>true if exists an association for the specified <para>key</para></returns>
        /// <exception cref="System.ArgumentNullException">key is null.</exception>
        public bool TryGetValue(T key, out U value)
        {
            try
            {
                // ArgumentNullException is throwed from SearchNode() method
                BSTNode<T, U> _node = SearchNode(key);
                if (_node == null) throw new KeyNotFoundException();
                value = _node.Value;
                return true;
            }
            catch (Exception)
            {
                value = default(U);
                return false;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get the number of nodes of this tree
        /// </summary>
        public int Count
        {
            get { throw new NotImplementedException(); } //Not required
            private set { _count = value; }
        }

        /// <summary>
        /// Inherited from ICollection. Returns false because BSTMap is not a read only
        /// data structure
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Verify if tree is empty
        /// </summary>
        public bool Empty { 
            get { return Count==0;}
        }

        /// <summary>
        /// Get a sorted collection of all the keys in this tree
        /// </summary>
        public ICollection<T> Keys
        {
            get { throw new NotImplementedException(); } //Not required
        }

        /// <summary>
        /// Get a collection of all the values in this tree
        /// </summary>
        public ICollection<U> Values
        {
            get { throw new NotImplementedException(); } //Not required
        }
        
        #endregion

        #region Private methods

        /// <summary>
        /// Add a node to the tree
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private BSTNode<T, U> AddNode(T key, U value)
        {
            if (key == null) throw new ArgumentNullException("key is null");
            if (ContainsKey(key)) throw new ArgumentException("Key already exists");

            BSTNode<T, U> _newNode = new BSTNode<T, U>(key, value);
            _newNode.Left = null;
            _newNode.Right = null;
            
            // Add node implementation: not required

            throw new NotImplementedException();
        }

        /// <summary>
        /// Called after an insertion fo a node; id tree is not balanced
        /// apply the Balance-Tree algorithm to balance the depth.
        /// </summary>
        private void BalanceTree(){
            throw new NotImplementedException();
        }

        /// <summary>
        /// Private implementation of the Node remove
        /// </summary>
        /// <param name="item">Node to remove</param>
        /// <returns>true if item has been removed; false otherwise.
        /// <see cref="IMap.Remove(T)"/></returns>
        private bool RemoveNode(KeyValuePair<T, U> item)
        {
            try
            {
                if (!Empty)
                {
                    // implementation of remove node
                    Count--;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// Private implementation of the Node remove
        /// </summary>
        /// <param name="key">key of the element to remove</param>
        /// <returns>true if a node has been removed; false otherwise.
        /// <see cref="IMap.Remove(T)"/></returns>
        private bool RemoveNode(T key)
        {
            // WARMUP: if key is an int element "key==null" throws an exception, instead, key.Equals(null) return false;
            if (key.Equals(null)) throw new ArgumentNullException("key cannot be null");
            
            try
            {
                if (!Empty)
                {
                    // implementation of remove node: not required
                    Count--;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// Search a specific node of the tree by key.
        /// </summary>
        /// <param name="key">The key to search.</param>
        /// <returns>The node having key as key is exists; null otherwise</returns>
        /// <exception cref="System.ArgumentNullException">key is null.</exception>
        private BSTNode<T, U> SearchNode(T key)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Node: private inner class

        /// <summary>
        /// Rappresentation of a Binary Search Tree node having a pair (key, value) as content
        /// </summary>
        /// <typeparam name="K">Content key type</typeparam>
        /// <typeparam name="V">Content value type</typeparam>
        internal class BSTNode<K,V> where K : IComparable
        {
            #region Internal state

            /// <summary>
            /// Internal state of the node
            /// </summary>
            KeyValuePair<K, V> _content;

            /// <summary>
            /// Left tree: reference to another node
            /// </summary>
            BSTNode<K, V> _left;

            /// <summary>
            /// Right tree: reference to another node
            /// </summary>
            BSTNode<K, V> _right;

            #endregion

            #region Constructors

            /// <summary>
            /// Create a new node having content as internal state
            /// </summary>
            /// <param name="content">Pair (key, value) for the node</param>
            public BSTNode(KeyValuePair<K, V> content)
            {
                _content = content;
                Left = null;
                Right = null;
            }

            /// <summary>
            /// Create a new node having ad internal state (key, value)
            /// </summary>
            /// <param name="key">Key of the node</param>
            /// <param name="value">Value of the node</param>
            public BSTNode(K key, V value) : this(new KeyValuePair<K,V>(key,value))  { }

            #endregion

            #region Properties

            /// <summary>
            /// Get or set the left node of this node
            /// </summary>
            public BSTNode<K, V> Left
            {
                get { return _left; }
                // set is possible from BSTMap only, not from other external classes
                internal set { _left = value; }

            }

            /// <summary>
            /// Get or the rigth node of this node
            /// </summary>
            public BSTNode<K, V> Right
            {
                get { return _right; }
                // set is possible from BSTMap only, not from other external classes
                internal set { _right = value; }

            }

            /// <summary>
            /// Get the key of this node
            /// </summary>
            public K Key
            {
                get { return _content.Key; }
            }

            /// <summary>
            /// Get or set the value of this node
            /// </summary>
            public V Value
            {
                get { return _content.Value; }
                // set is possible from BSTMap only, not from other external classes
                internal set {
                    // _content.Value is read only for KeyValuePair. Create a new pair with the same key.
                    _content = new KeyValuePair<K,V>(_content.Key, value);
                }

            }

            #endregion
        }

        #endregion
    }
}
