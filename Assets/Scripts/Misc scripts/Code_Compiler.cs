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

    // Creating a method with a single line of code
    public static T Line<T> (string line) {
        try
        {
            string source_code = @"
                using System;
                using UnityEngine;

                public class Compiled_Class {
                    public " + typeof(T).ToString() + @" Compiled_Method () {
                        return " + line + @"
                    }
                }";
            //Console.Warning(null, source_code);
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
            CompilerResults compilerResults = code_provider.CompileAssemblyFromSource(compile_parameters, source_code);
            // Calling the selected method
            var typeInstance = compilerResults.CompiledAssembly.CreateInstance("Compiled_Class");
            MethodInfo method = typeInstance.GetType().GetMethod("Compiled_Method");
            return (T)method.Invoke(typeInstance, new object[] {}); 
        }
        catch (Exception)
        {
            Console.Error(null, "Runtime code compiler error");
        }
        return default(T);
    }
}