using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.IO;
using System.Reflection;
using System;
using Lib;
public class CommonInterfaceExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string code = @"
    using System;
    using Lib;
    public class Test:ITest
    {
        public int A()
        {
            return 224;
        }
    }";
        List<MetadataReference> metaDatas = new List<MetadataReference>();
        metaDatas.Add(MetadataReference.CreateFromFile("System.dll"));
        metaDatas.Add(MetadataReference.CreateFromFile("System.Runtime.dll"));
        metaDatas.Add(MetadataReference.CreateFromFile("Lib.dll"));

        Debug.Log("Commence");
        //Debug.Log("Commence");
        var syntaxTree = CSharpSyntaxTree.ParseText(code, CSharpParseOptions.Default);
        var compileOptions = new CSharpCompilationOptions(Microsoft.CodeAnalysis.OutputKind.DynamicallyLinkedLibrary);
        var compilation = CSharpCompilation.Create("assembly.dll", new List<SyntaxTree> { syntaxTree }, metaDatas, compileOptions);
        Debug.Log("Compilation created");
        MemoryStream ms = new MemoryStream();
        
        var result=compilation.Emit(ms);
        Debug.Log(result.Success);
        foreach (var h in result.Diagnostics)
        {
            Debug.Log(h);
        }
        ms.Seek(0, SeekOrigin.Begin);
        Assembly assembly = Assembly.Load(ms.GetBuffer());
        Debug.Log("Assembly loaded");
        Type type = assembly.GetType("Test");
        var obj = Activator.CreateInstance(type);
        var res = ((ITest)obj).A();
        Debug.Log($"Result: {res.ToString()}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
