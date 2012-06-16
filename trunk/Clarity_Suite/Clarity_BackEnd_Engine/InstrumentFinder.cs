using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Clarity
{
    /// <summary>
    /// This class is used to examine the parent namespace and return all the classes
    /// that are derived from BaseInstrumentClass, it is used to build a list of classes that can then
    /// be used
    /// </summary>
    public class InstrumentFinder
    {
        /// <summary>
        /// Gets a list of all of the classes derived from the baseInstrument class
        /// in the currently executing assembly
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        public static List<BaseInstrumentClass> GetAllInstrumentClasses()//string nameSpace)
        {
            Type TypeToFind = typeof(BaseInstrumentClass);
            //create an Assembly and use its GetExecutingAssembly Method
            //http://msdn2.microsoft.com/en-us/library/system.reflection.assembly.getexecutingassembly.aspx
            Assembly asm = TypeToFind.Assembly;  
            string nameSpace = TypeToFind.Namespace; 
            //create a list for the instrument classes to be returned
            List<BaseInstrumentClass> instrumentClasses = new List<BaseInstrumentClass>();
            List<Type> baseTypes = findTypesFromDLLs(Environment.CurrentDirectory,TypeToFind);
            foreach (Type type in baseTypes) //asm.GetTypes())
            {
                //check if it is in the same namespace and if so create an instance of it
                if (type.Namespace == nameSpace && !type.IsAbstract && type.IsSubclassOf(TypeToFind))
                {
                    //if it is a derived class, let's make a copy of it and put it in the list
                    ConstructorInfo ci = type.GetConstructor(new Type[] { });
                    BaseInstrumentClass InstrumentClass = (BaseInstrumentClass)ci.Invoke(new Object[] { });
                    //NOW TO CALL THE SET NAME FUNCTION, MUST BE DONE TO INSURE PROPER OPERATION!
                    instrumentClasses.Add(InstrumentClass);        
                }
            }
            //now loop through all the classes returned above and add
            //them to our classesName list
            return instrumentClasses;
        }
        /// <summary>
        /// This method looks through a path and finds all Dll files to load 
        /// anything that is derived from the given type to find.
        /// </summary>
        /// <param name="searchPath">Path to search</param>
        /// <param name="typeToFind">Type to find</param>
        /// <returns>A list of types found in the .dll</returns>
        private static System.Collections.Generic.List<System.Type> findTypesFromDLLs(string searchPath, System.Type typeToFind)
        {
            System.Collections.Generic.List<System.Type> TypesFound = new System.Collections.Generic.List<System.Type>();
            string[] files = System.IO.Directory.GetFiles(searchPath, "*.dll");
            string[] files2 = System.IO.Directory.GetFiles(searchPath);
            List<Type> toReturn = new List<Type>();
            for (int i = 0; i < files.Length; i++)
            {
                string cFile = files[i];
                //Now to try and load the .dll file, first verify it is a 
                try
                {
                    System.Reflection.AssemblyName.GetAssemblyName(cFile);
                    Assembly toLoad = System.Reflection.Assembly.LoadFile(cFile);
                    System.Type[] types = toLoad.GetTypes();
                    foreach (Type t in types)
                    {
                        if (!t.IsAbstract && t.IsSubclassOf(typeToFind) && !t.Equals(typeToFind))
                        {
                            toReturn.Add(t);
                        }
                    }
                }
                catch { }//not a .NET assembly presumably, we could log errors here if desired

            }
            return toReturn;
        }
        

    }
}
