using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exercise3
{
    // PLEASE UNCOMMENT THE FOLLOWING CLASS WHEN TO USE.
    // THE REASON IS THAT I WANT TO SHOW THAT BSTMap CLASS IS TYPE SAFE
    // AND I'VE CREATE SOME WRONG TREE STRUCTURE TO SHOW THAT TYPES
    // ARE CHECKED AT COMPILE TIME.

    /* << REMOVE this line to uncomment! (click me and ctrl+X)
    
    class BSTMapDemostration
    {
        void ExampleMethod()
        {
            // Let's create 2 empty BSTMap from (string to int) and from (float to object)
            BSTMap<string, int> map1 = new BSTMap<string, int>();
            BSTMap<float, object> map3 = new BSTMap<float, object>();

            // Let's create a BSTMap from string to int initialized with a value
            BSTMap<string, int> map4 = new BSTMap<string, int>(new KeyValuePair<string, int>("three", 3));

            // Let's create a WRONG instansiation of the map3 class
            BSTMap<string, int> map5 = new BSTMap<string, int>(new KeyValuePair<int, string>(3, "tree"));
            // i'm assigning to (string, int) BSTMap ^^^^^^^ a pair (int, string) ^^^^^^^^.
            // This cause a compile-time error

            // Now let's try to see id compiler accept a non-comparable type for the BSTMap initialization
            BSTMap<object, int> map6 = new BSTMap<object, int>();
            // as you can see object is not allowed ^^^ because it's not comparable nativelly

            // So BSTMap is Type-Safe!

            // Now try to check is methods are type type safe.
            map1.Add("ciao", "hello"); // map1 is (string, int) map!!

            // Now try to check is methods are type type safe.
            map1.Remove(1); // map1 is (string, int) map, i can remove using keys, not index!

            // I can get a key using bracket notation array[key].
            var valCiao = map1["ciao"];

            // By concluding the exercise requirment let's see some examples of iterable functions
            foreach (var key in map1.Keys)
            {
                // Do something with SORTED keys
            }

            foreach (var value in map1.Values)
            {
                // Do something with values
            }

            foreach (var value in map1) // implicit iterator inherited from IEnumerable
            {
                var x1 = value.Key;
                var x2 = value.Value;
                // Do something with keys and values
            }

            // Explicit iterator
            var cursor = map1.GetEnumerator();
            while (cursor.MoveNext())
            {
                var curEl = cursor.Current;
                var curKey = curEl.Key;
                var curValue = curEl.Value;
            }

            // Dimostration end.
        }

    }
    /**/
}
