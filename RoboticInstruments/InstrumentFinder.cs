using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Growth_Curve_Software
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
            foreach (Type type in asm.GetTypes())
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

    }
}
