using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

// https://forum.unity.com/threads/compiling-c-at-runtime.376611/
// Methods for compiling C# code at runtime
public class Code_Compiler
{
    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    // Compiling source code and returning compiler results
    private static CompilerResults Compile (string source_code)
    {
        Console.Log(null, source_code);
        // Adding compile parameters
        CompilerParameters compile_parameters = new CompilerParameters{
            GenerateExecutable = false, 
            GenerateInMemory = true
        };
        // Adding assemblies
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            compile_parameters.ReferencedAssemblies.Add(assembly.Location);
        // Compiling code
        var code_provider = new CSharpCodeProvider();
        CompilerResults compiler_results = code_provider.CompileAssemblyFromSource(compile_parameters, source_code);
        // Printing out error messages
        if(compiler_results.Errors != null && compiler_results.Errors.Count > 0)
            foreach(var error in compiler_results.Errors)
                Console.Warning(null, "Runtime Compiler error: " + error.ToString());
        // Returning compiler results for further processing
        return compiler_results;
    }

    // Returns a value from a single line of code
    public static T Return<T> (string line) {
        try
        {
            CompilerResults compiler_results = Compile(@"
                using System;
                using System.Linq;
                using UnityEngine;

                public class Compiled_Class {
                    public " + typeof(T).ToString() + @" Compiled_Method () {
                        return " + line + @"
                    }
                }
            ");
            // Calling the selected method
            var instance = compiler_results.CompiledAssembly.CreateInstance("Compiled_Class");
            MethodInfo method = instance.GetType().GetMethod("Compiled_Method");
            return (T)method.Invoke(instance, null); 
        }
        catch (Exception)
        {
            Console.Error(null, "Runtime code compiler error");
        }
        return default(T);
    }

    // Returns a compiled method with one parameter of a selected type
    public static KeyValuePair<System.Object, MethodInfo> Create_Parameter<T> (string code, T variable = default(T)) {
        try
        {
            CompilerResults compiler_results = Compile(@"
                using System;
                using System.Linq;
                using UnityEngine;

                public class Compiled_Class {
                    public void Compiled_Method (" + typeof(T).ToString() + @" variable = default(" + typeof(T).ToString() + @")) {
                        " + code + @"
                    }
                }
            ");
            // Calling the selected method
            var instance = compiler_results.CompiledAssembly.CreateInstance("Compiled_Class");
            MethodInfo method = instance.GetType().GetMethod("Compiled_Method");
            return new KeyValuePair<System.Object, MethodInfo>(instance, method);
        }
        catch (Exception)
        {
            Console.Error(null, "Runtime code compiler error");
            return new KeyValuePair<System.Object, MethodInfo>(null, null);
        }
    }
}