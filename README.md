MidTermDavideAnzalone
=====================
My C# implementation of the mid term for Advanced Programming Exam.

# Exercise 1: A Regular expression parser and interpreter
I've wrapped the ambiguous grammar of the exercise in an equivalent LL1 grammar.
Parsing/interpreting steps are the follow:

1.  create a formatted version of the regex (expliciting the interval, and other).
2.  first optimization: I've converted the infix regex produced by the pre-parser into an equivalent one which is in post-fix mode. The Post Fix Mode is convenient to create the automata using a one-pass parser.
3.  building of the Non-Deterministic Automata as described at page 153 of the Dragon Book using the algorithm proposed by the book.
4.  wrapped the NFAutomata to the equivalent DFAutomata using the algorithm proposed in the book.
5.  optimized the DFA using [Minimizing Algorithm](http://www.cs.engr.uky.edu/~lewis/essays/compilers/min-fa.html)

The exercise has been developed using my implementation of DFA, NFA, EpsilonClosure,
Partitions, and other.
I suggest you to see the graphical scheme of all the classes used for parsing and interpreting the exercise Language for Regular Expression: you can find a blue print in Exercise1and2/ClassDiagram1.cd using Visual Studio IDE.

## Notes
According to the exercise ambiguity my version of IsMatch() corresponds to the System.Text.RegularExpressions.Regex.IsMatch() version of C# that retruns true if the strings matchs completely the Regular Expression.

# Exercise 2: Compile a Regular Expression and performance tests.
I've created a Compile() method in RegularExpression Class that can be called before IsMatch() call.
In this case (compiled version of the regex) the IsMatch() method invoke the IsMatch() method of the compiled class that is loaded and compiled usinc C# JIT.
I've generate a class file manually wrapping the MinimizedDFAutomata into a series of switchs-cases.
Then I used CSharpCodeProvider/Assembly classes of .NET to transform the produced source code into an executable class that implements ICompilable interface.

## Performances
Performances are pretty good: Tests mode execute the IsMatch() for the regular expression "([1-9][0-9]*|0)(,[0-9]*[1-9])?" 100 times that match all the float/int numbers on a 100K long number (imagine the first 100000 piGreco digits).
In my machine (OS: Windows 7, Processor: Inter(R) Core(TM) I7 (2,00GHz), RAM: 4GB) test returns 0.4455ms (avg time) for the compiled version (approx. 450 microsecs).

# Exercise 3
I've adopted the "Programming By Contract" approach. I've implemented the IMap Interface and i've write a BST Implementation of the map in the BSTMap Class.
Programming by contract approach use REQUIRES and EFFECTS "words" in the method comments to inform users of the initial conditions and the excepted behavior after a methods calls.
Unfortunatelly C# standard tags has not these "words" in the tag system so:

* Requires are explained in the <summary> tags
* Effects are explained in the <return> tags
* Throwed exception are explained in the <exception> tags

Please see ClassBluePrint.cd for the complete class scheme.

# Exercise 4
Just a simply SCALA language tutorial. Read it :)
