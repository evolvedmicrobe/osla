using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.IO;
using System.Reflection;
namespace Clarity
{
    
    [Serializable]
    public abstract class BaseInstrumentClass
    {
        /// <summary>
        /// This variable indicates whether the instrument is able to 
        /// perform commands or not, for most scenarios, if this is set to false
        /// any method calls to the instrument will throw errors.
        /// </summary>
        public bool StatusOK = false;

        /// <summary>
        /// The name of the instrument, should be unique
        /// </summary>
        public virtual string Name
        {
            get{return this.GetType().Name;} 
        }
        public override string ToString()
        {
            return Name;
        }
        /// <summary>
        /// This is an optional method that allows for some instruments to try
        /// and self-diagnose and correct a mistake based on the error produced.
        /// 
        /// This method should attempt to resolve the issue, if it cannot, then it should return a value indicating as much
        /// </summary>
        /// <param name="Error"></param>
        /// <returns></returns>
        public abstract bool AttemptRecovery(InstrumentError Error);
        //
        /// <summary>
        /// This method should initalize any resources the instrument needs, and not the constructor.
        /// Errors can be trapped arsising from this method.
        /// </summary>
        public virtual void Initialize()
        {
            return;
        }

        /// <summary>
        /// This should be a generic "Fix and reinitialize" method called after 
        /// the device fails.  
        /// </summary>
        /// <returns></returns>
        [UserCallableMethod()]
        public virtual bool AttemptRecovery()
        {
            return AttemptRecovery(new InstrumentError("none", false, this));
        }
        /// <summary>
        /// This method should be implemented by each class and handle releasing unmanaged resources,
        /// COM objects, etc. that are created for the instrument to run.  It is essential if an instrument wants to call 
        /// a "clean" start, and often involves direct process kills on COM objects that have lost their way.
        /// </summary>
        /// <returns></returns>
        public abstract bool CloseAndFreeUpResources();
        /// <summary>
        /// This is a method called to force certain instruments to free up resources that they
        /// are connecting to in a seperate process
        /// </summary>
        /// <param name="ProcessNameWithoutExeEnding">The name of the instrument to kill</param>
        public static void KillProcessAttempt(string ProcessNameWithoutExeEnding)
        {
            try
            {
                Process[] processList = Process.GetProcesses();
                var x = from p in processList select p.ProcessName;
                if (x.Contains(ProcessNameWithoutExeEnding))
                {
                    string output = String.Empty;
                    System.Diagnostics.Process proc = new Process();
                    ProcessStartInfo myStartInfo = new ProcessStartInfo();
                    myStartInfo.RedirectStandardInput = false;
                    myStartInfo.UseShellExecute = false;
                    myStartInfo.RedirectStandardOutput = false;
                    myStartInfo.Arguments = "/IM " + ProcessNameWithoutExeEnding + ".exe";
                    myStartInfo.CreateNoWindow = true;
                    //TODO: Change task killing so it is not specific to computer
                    myStartInfo.FileName = @"C:\WINDOWS\SYSTEM32\TASKKILL.EXE ";
                    proc.StartInfo = myStartInfo;
                    proc.Start();
                    proc.WaitForExit(3000);
                    output = proc.StandardOutput.ReadToEnd();

                }
            }
            catch { }
        } 
        /// <summary>
        /// This is a generic method that will set any instance variables based on 
        /// the xml node that corresponds to the instrument, in this case it does nothing 
        /// but call its initalization method
        /// </summary>
        /// <param name="instrumentNode"></param>
        public virtual void InitializeFromParsedXML(XmlNode instrumentNode)
        {
            //First remove anything old            
            SetPropertiesByXML(instrumentNode, this);
            this.Initialize();          
        }
        /// <summary>
        /// This is a generic method that will take the initialization values provided by
        /// the xml and use them to set instance variables, the xml childnodes must corresponds to variable
        /// names
        /// </summary>
        /// <param name="xml"></param>
        public static void SetPropertiesByXML(XmlNode nodeToGetValuesFrom, object toSet)
        {
            try
            {
                foreach (XmlNode childNode in nodeToGetValuesFrom)
                {
                    if (toSet == null)
                    {
                        throw new Exception("XML node is being used to set a null variable, the node is \n" + childNode.ToString());

                    }
                    if (childNode.NodeType == XmlNodeType.Comment)
                    {
                        continue;
                    }
                    
                        string propertyName = childNode.Name;
                        try
                        {
                            Type thisType = toSet.GetType();
                            //get the variable type info
                            XmlNode typeNode = childNode.Attributes.RemoveNamedItem("Type");
                            if (typeNode == null)
                            {
                                throw new Exception("Variable Type not set in xml, please declare the variable type for all "
                                    + " variables used for " + toSet.ToString());
                            }
                            Type VariableType = System.Type.GetType(typeNode.Value);
                            var Value = Convert.ChangeType(childNode.InnerText, VariableType);
                            //now get the property and change it
                            var prop = thisType.GetProperty(propertyName);
                            if (prop == null)
                            {
                                throw new Exception(toSet.ToString() + " does not have a property called " + propertyName
                                    + "\n so the xml file needs to be fixed");
                            }
                            prop.SetValue(toSet, Value, null);
                        }
                        catch (Exception thrown)
                        {
                            throw new Exception("Could not parse XML Node: " + propertyName + "\n" + thrown.Message); 
                        }
                
                }
            }
            catch (Exception thrown)
            {
                Exception newExcept=new Exception("Could not instantiate class based on XML " + thrown.Message,thrown);
                throw newExcept;
            }
        }
        /// <summary>
        /// This method tells the entire software suite where to 
        /// find the configuration file
        /// </summary>
        /// <returns></returns>
        public static string GetXMLSettingsFile()
        {
            string direc= Directory.GetCurrentDirectory();
            string file = direc + "\\ConfigurationFile.xml";
            return file;
        }
        /// <summary>
        /// Some instruments might want to know when the engine that runs the protocol execution
        /// has a certain event.  In particular, for long shutdowns the instrument might want to power 
        /// itself down and then restart later.  This allows instruments to register for events with 
        /// this type of information.
        /// </summary>
        /// <param name="PEC"></param>
        public virtual void RegisterEventsWithProtocolManager(ProtocolEventCaller PEC) { }
    
    }
    }


