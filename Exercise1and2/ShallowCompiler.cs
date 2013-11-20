using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Collections;
using System.Diagnostics;

namespace Exercise1
{
    /// <summary>
    /// Class that offers a list of useful methods to generate CLR "Assembly" of an automata structure.
    /// </summary>
    public class ShallowCompiler
    {
        #region Private variables

        // Assembly form CLR runnable class
        private Assembly assembly = null;

        // Result class from compilation
        private ICompilable compiledClass = null;

        #endregion

        #region Inner class

        /// <summary>
        /// Compiler Exception class
        /// </summary>
        public class CompilerException : Exception
        {
            #region Constructor

            /// <summary>
            /// CompilerException constructor
            /// </summary>
            /// <param name="er">Error collection</param>
            public CompilerException(CompilerErrorCollection er)
            {
                Errors = er;
            }

            #endregion

            #region Public variable

            /// <summary>
            /// Gets the errors list
            /// </summary>
            public CompilerErrorCollection Errors;

            #endregion
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Compile a class from source code
        /// </summary>
        /// <param name="automata">A Deterministic automata to convert into an automata class</param>
        /// <returns>true if compile success</returns>
        /// <exception cref="ShallowCompiler.CompilerError">Compiler errors occurs</exception>
        public string CompileCode(DAutomata automata)
        {
            // C# compiler helper class
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();

            // Compiler parameters
            CompilerParameters parameters = new CompilerParameters();

            // generate in memory compilation (not an EXE/DLL file)
            parameters.GenerateInMemory = true;
            parameters.GenerateExecutable = false;

            // Add all the DLL reference to perform the compilation
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Data.dll");
            parameters.ReferencedAssemblies.Add("System.Core.dll");
            parameters.ReferencedAssemblies.Add("MidTermAP.dll");

            // Generate the automata class source code!
            StringBuilder generateClass = new StringBuilder();

            DAutomata.DAutomataState[] allStates = automata.States;
            string[] inputSymbols = automata.Chars;

            generateClass.AppendLine("using System;");
            //generateClass.AppendLine("using System.Collections.Generic;");
            //generateClass.AppendLine("using System.ComponentModel;");
            generateClass.AppendLine("using System.Data;");
            //generateClass.AppendLine("using System.Drawing;");
            //generateClass.AppendLine("using System.Text;");
            generateClass.AppendLine("using Exercise1;");
            generateClass.AppendLine("");
            generateClass.AppendLine("namespace Exercise1");
            generateClass.AppendLine("{");
            generateClass.AppendLine("   public class MyRegExExecutor: Exercise1.ICompilable");
            generateClass.AppendLine("   {");
            generateClass.AppendLine("");
            generateClass.AppendLine("      public bool IsMatch(string str)");
            generateClass.AppendLine("      {");
            generateClass.AppendLine("         return IsMatch(ref str, " + automata.StartState.ID + ", 0);");
            generateClass.AppendLine("      }");
            generateClass.AppendLine("");
            generateClass.AppendLine("      private bool IsMatch(ref string str, int stState = 0, int stIndex = 0)");
            generateClass.AppendLine("      {");
            generateClass.AppendLine("         int curState = stState;");
            generateClass.AppendLine("         for(int i = stIndex; i<str.Length; i++)");
            generateClass.AppendLine("         {");
            generateClass.AppendLine("            switch(curState)");
            generateClass.AppendLine("            {");
            foreach (var st in allStates)
            {
                generateClass.AppendLine("               case " + st.ID + ":");
                generateClass.AppendLine("                  switch(str[i])");
                generateClass.AppendLine("                  {");
                foreach (string chr in inputSymbols)
                {
                    DAutomata.DAutomataState nextS = st.GetTransition(chr);
                    if (nextS == null || chr == RegularExpressionParser.MetaCharsTranslations.JollyCharTrans) continue;
                    generateClass.AppendLine("                     case '" + (chr[0] == '\\' ? "\\" : chr[0].ToString()) + "':");
                    var jollyTrans = st.GetTransition(RegularExpressionParser.MetaCharsTranslations.JollyCharTrans);
                    if (jollyTrans != null)
                    {

                        generateClass.AppendLine("                        if(IsMatch(ref str, " + jollyTrans.ID + ", i+1)) return true;");
                    }

                    if(st.ID!=nextS.ID) generateClass.AppendLine("                        curState = " + nextS.ID + ";");
                    generateClass.AppendLine("                        break;");
                }

                DAutomata.DAutomataState dotS = st.GetTransition(RegularExpressionParser.MetaCharsTranslations.JollyCharTrans);
                generateClass.AppendLine("                      default:");
                if (dotS == null)
                    generateClass.AppendLine("                        return false;");
                else
                {
                    generateClass.AppendLine("                        curState = " + dotS.ID + ";");
                    generateClass.AppendLine("                        break;");
                }
                generateClass.AppendLine("                  }");
                generateClass.AppendLine("                  break;");
            }
            generateClass.AppendLine("            }");
            generateClass.AppendLine("         }");
            string orStates = "";
            foreach (var st in automata.States)
            {
                if (st.IsFinal)
                {
                    if (orStates != "") orStates += " || ";
                    orStates += "curState == " + st.ID;
                }
            }
            generateClass.AppendLine("         return (" + orStates + ");");
            generateClass.AppendLine("      }");
            generateClass.AppendLine("   }");
            generateClass.AppendLine("}");

            // Compile the generated source code
            CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, generateClass.ToString());

            // Catch possible compiler errors
            if (!results.Errors.HasErrors)
            {
                // no errors => get the copiled class "MyRegExExecutor"
                this.assembly = results.CompiledAssembly;
                compiledClass = (ICompilable)Activator.CreateInstance(assembly.GetTypes()[0]);
                return generateClass.ToString();
            }
            else
            {
                // there are some compile errors. throws it!
                throw new CompilerException(results.Errors);
            }
        }

        /// <summary>
        /// Run the compiled class
        /// </summary>
        /// <param name="input">input string to match with this compiled version of </param>
        /// <param name="timer">timer to start before and stop after IsMatch() call</param>
        /// <returns>true if compiled class matchs the input string</returns>
        public bool Run(string input, Stopwatch timer = null)
        {
            if (timer != null)
            {
                timer.Restart();
                timer.Start();
                bool result = compiledClass.IsMatch(input);
                timer.Stop();
                return result;
            }
            else
            {
                return compiledClass.IsMatch(input);
            }
        }

        #endregion
    }

    /// <summary>
    /// Public interface to copile an automata into class
    /// </summary>
    public interface ICompilable
    {
        /// <summary>
        /// API to call the "IsMatch()" method in the compiled class
        /// </summary>
        /// <param name="str">String to match fwith the compiled regular expression</param>
        /// <returns>true in str matchs this the compiled regular expression</returns>
        bool IsMatch(string str);
    }
}
