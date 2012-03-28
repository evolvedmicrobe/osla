using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Collections;
using System.Xml;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;

namespace Growth_Curve_Software
{
    //This file contains all the code for Creating/Managing Protocols

    public interface ValidInstruction
    {
    }
    public enum ProtocolRunResult
    { AllProtocolsFinished, NoLoadedProtocols, Error }
    public enum ProtocolPriority
    {
        Low, Medium, High
    }
    [Serializable]
    public class ProtocolVariable
    {
        //this class will be a type of "variable" that the user 
        //can create in the protocol editor
        public Type DataType;
        public string Name;
        public object Value;
        public ProtocolVariable(string name, Type VariableType, object value)
        {
            Name = name;
            DataType = VariableType;
            Value = value;
        }
        public ProtocolVariable()
        {
        }
        public override string ToString()
        {
            return "%" + Name + "%";
        }
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is ProtocolVariable)
            {
                ProtocolVariable PV2 = (ProtocolVariable)obj;
                if (PV2.Name == Name) { return true; }
                else { return false; }
            }
            else
            {
                return base.Equals(obj);
            }
        }
        public static bool CheckIfAnObjectMatchesAType(object o, Type T)
        {
            try
            {
                Convert.ChangeType(o, T);
                return true;
            }
            catch { return false; }
        }
    }
    [Serializable]
    public struct ReferenceToProtocolVariable
    {
        //This just contains the name of a variable name
        public string VaribleName;
        public ReferenceToProtocolVariable(string varName)
        { VaribleName = varName; }
        public override string ToString()
        {
            return VaribleName;
        }
    }

    [Serializable]
    public class StaticProtocolItem : ValidInstruction, ICloneable//not really sure why I made a valid instruction interface
    {
        //this is an excecution step in the protocol, where the parameters 
        //are determined at the start of the run
        public string InstrumentName, MethodName;
        public object[] Parameters;
        public Protocol ContainingProtocol;
        public StaticProtocolItem()
        {
            Parameters = new object[0];
        }
        public override string ToString()
        {

            string ToReturn = "";
            ToReturn = MethodName + " (";
            if (Parameters != null)
            {
                foreach (object o in Parameters) { if (o != null) ToReturn += o.ToString() + ","; }
            }
            return ToReturn.TrimEnd(',') + ")";
        }
        public StaticProtocolItem Clone()
        {
            StaticProtocolItem SP = new StaticProtocolItem();
            SP.MethodName = this.MethodName;
            SP.Parameters = this.Parameters;
            SP.InstrumentName = this.InstrumentName;
            SP.ContainingProtocol = this.ContainingProtocol;
            return SP;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

    }
    [Serializable]
    public class DelayTime
    {
        public int minutes;
        public override string ToString()
        {
            return "Pause for " + minutes.ToString() + " minutes";
        }
    }
    [Serializable]
    public class LoopInstruction
    {//this is an instruction that leads to a repeat
        public int StartInstruction;
        public int TimesToRepeat;
    }
    public class ProtocolListBoxItem
    {
        public int InstructionNumber;
        public virtual object ReturnInstructionObject()
        {
            return false;
        }
    }
    public class ListBoxProtocolItem : ProtocolListBoxItem
    {
        public StaticProtocolItem Instruction;
        public ListBoxProtocolItem()
        {
            Instruction = new StaticProtocolItem();
        }
        public override string ToString()
        {
            return InstructionNumber.ToString() + " : " + Instruction.ToString();
        }
        public override object ReturnInstructionObject()
        {
            return Instruction;
        }
    }
    public class ListBoxDelayItem : ProtocolListBoxItem
    {
        public DelayTime delay;
        public ListBoxDelayItem()
        {
            delay = new DelayTime();
        }
        public override string ToString()
        {
            return "Pause for " + delay.minutes.ToString() + " minutes";
        }
        public override object ReturnInstructionObject()
        {
            return delay;
        }
    }
    public class ListBoxRepeatItem : ProtocolListBoxItem
    {
        public LoopInstruction RepeatInstruction;
        public ProtocolListBoxItem RefToActualInstruction;
        public ListBoxRepeatItem()
        {
            RepeatInstruction = new LoopInstruction();
        }
        public override string ToString()
        {
            return "Repeat from instruction " + RepeatInstruction.StartInstruction.ToString() + " for " + RepeatInstruction.TimesToRepeat.ToString() + " times";
        }
        public override object ReturnInstructionObject()
        {
            return RepeatInstruction;
        }
    }
    [Serializable]
    public class Protocol
    {
        //This class will contain all the instructions for a particular protocol
        public ArrayList Instructions;//list of instructions in the protocol to return, this is private at all times
        public Dictionary<string, ProtocolVariable> Variables;
        public DateTime NextExecutionTimePoint;//when the next execution point needs to  be run (for example, if there is a 45 minute delay, this time will be in 45 minutes)
        public string ProtocolName;//the name of the protocol
        public string ErrorEmailAddress;//if more then one email, can be seperated by semicolon
        public string ErrorPhoneNumber;
        public int NextItemToRun;//the index in the protocol array where the next item should run
        public ProtocolPriority Priority;//not actually used right now, will be implemented in the future
        public Protocol()
        {
            //empty constructor, needs to be filled in
            Instructions = new ArrayList();
            ProtocolName = "";
            Variables = new Dictionary<string, ProtocolVariable>();
        }
        public StaticProtocolItem ReplaceVariablesWithValues(StaticProtocolItem ToRun)
        {
            //This method will take a protocol item that has variables, and replace it with actual values,
            //It should be called before the method is run
            StaticProtocolItem NewProtocolItem = ToRun.Clone();
            NewProtocolItem.ContainingProtocol = this;
            for (int i = 0; i < NewProtocolItem.Parameters.Length; i++)
            {
                object o = ToRun.Parameters[i];
                //check if it is a string
                if (o is string)
                {
                    string VariableName;
                    //now have to go through and remove all of the variables, replacing them with other strings
                    String myval = (String)o;
                    while (true)
                    {
                        bool variableInString = ProtocolManager.GetVariableNameFromText(myval, out VariableName);
                        if (variableInString)
                        {
                            string ToReplace = Variables[VariableName].Value.ToString();
                            myval = myval.Replace(VariableName, ToReplace);//swap out the value and searchagain
                        }
                        else
                        { break; }
                    }
                    ToRun.Parameters[i] = myval;
                }
                else if (o is ReferenceToProtocolVariable)
                {
                    ReferenceToProtocolVariable RefToVariable = (ReferenceToProtocolVariable)o;
                    ToRun.Parameters[i] = Variables[RefToVariable.ToString()].Value;
                }
            }
            return NewProtocolItem;
        }
        public override string ToString()
        {
            try { return ProtocolName + " -- " + NextExecutionTimePoint.ToString(); }
            catch { }
            return ProtocolName;
        }
    }
    [Serializable]
    public delegate void ProtocolPauseEventHandler(TimeSpan TS);

    [Serializable]
    public class ProtocolManager
    {
        int CallHourStart = 23;
        int CallHourEnd = 8;
        //Static methods
        public static bool IsVariableInList(string VariableName, IList ListOfVariables)
        {
            //Search a collection to see if a string is in it
            foreach (object o in ListOfVariables)
            {
                ProtocolVariable PV = (ProtocolVariable)o;
                if (PV.ToString() == VariableName)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool CheckStringForValidVariables(string Text, IList ListOfVariables)
        {
            //this method will check if the variables in a string actually exists, for example if the string is
            //2,%Variable2%,%Variable3%, it will ensure that these variables exist, if not returns false
            Regex VariableFinder = new Regex("%[^%]*%");
            string VariableName;
            foreach (Match myVariable in VariableFinder.Matches(Text))
            {
                VariableName = myVariable.Groups[0].ToString();
                bool InCollection = IsVariableInList(VariableName, ListOfVariables);
                if (!InCollection) { return false; }
            }
            return true;

        }
        public static bool GetVariableNameFromText(string Text, out string VariableName)
        {
            //returns true if the text contains a variable name, and then outputs that name
            Regex VariableFinder = new Regex("%[^%]*%");
            Match myVariable = VariableFinder.Match(Text);
            if (myVariable.Success)
            {
                VariableName = myVariable.Groups[0].ToString();
                return true;
            }
            else
            {
                VariableName = "";
                return false;
            }
        }
        public static List<string> GetAllVaraibleNamesFromText(string Text)
        {
            List<string> toReturn = new List<string>();
            Regex VariableFinder = new Regex("%[^%]*%");
            foreach (Match hit in VariableFinder.Matches(Text))
            {
                toReturn.Add(hit.Groups[0].ToString());
            }
            return toReturn;
        }
        public static bool VerifyProtocolVariablesReferences(Protocol ProtocolToRun)
        {
            //This is a method that will ensure that all of the references to variables in a protocol work out,
            //it will go through the instructions and check that the variables that are referenced exist
            foreach (object o in ProtocolToRun.Instructions)
            {
                if (o is StaticProtocolItem)
                {
                    StaticProtocolItem SP = (StaticProtocolItem)o;
                    //now to move through and check all
                    foreach (object Param in SP.Parameters)
                    {
                        if (Param is string)
                        {
                            //check all variable names in a string
                            List<string> VariableNames = GetAllVaraibleNamesFromText((string)Param);
                            foreach (string vName in VariableNames) { if (!ProtocolToRun.Variables.ContainsKey(vName)) { return false; } }
                        }
                        else if (Param is ReferenceToProtocolVariable)
                        {
                            ReferenceToProtocolVariable CurRef = (ReferenceToProtocolVariable)Param;
                            if (!ProtocolToRun.Variables.ContainsKey(CurRef.ToString())) { return false; }
                        }
                    }
                }
            }
            return true;
        }

        //instance goodies
        public List<Protocol> Protocols;//This will be the collection of all protocols that are in use,it should not be altered outside of this class
        public Protocol CurrentProtocolInUse;//
        public InstrumentManager manager;
        public ProtocolManager(InstrumentManager manager)
        {
            this.manager = manager;
            Protocols = new List<Protocol>();
        }
        public void AddProtocol(Protocol ProtocolToAdd)
        {
            ProtocolToAdd.NextExecutionTimePoint = DateTime.Now;
            if (Protocols.Count == 0)//check if this is the most current protocol
            {
                CurrentProtocolInUse = ProtocolToAdd;
            }
            Protocols.Add(ProtocolToAdd);

            UpdateAlarmProtocols();
        }
        public void UpdateAlarmProtocols()
        {
            List<string> newNames = Protocols.Select(x => x.ProtocolName).ToList();
            Alarm a = manager.GiveAlarmReference();
            if (a != null && a.Connected) a.SetProtocolNames(newNames);

        }
        public void RemoveProtocol(Protocol ProtocolToRemove)
        {
            if (Protocols.Count == 1)
            {//empty out the current protocol in use
                CurrentProtocolInUse = null;
                Protocols.Remove(ProtocolToRemove);
            }
            else
            {
                if (CurrentProtocolInUse == ProtocolToRemove)
                {
                    CurrentProtocolInUse = null;
                    Protocols.Remove(ProtocolToRemove);
                    FindMilliSecondsUntilNextRunAndChangeCurrentProtocol();
                }
                else
                {
                    Protocols.Remove(ProtocolToRemove);
                }
            }
            UpdateAlarmProtocols();
        }
        private double FindMilliSecondsUntilNextRunAndChangeCurrentProtocol()
        {
            //this will loop through all the Protocols in the list, find the next item to execute
            //and then run that item from the list
            if (Protocols.Count < 1)
            {
                throw new Exception("No loaded protocols were available when the delay was requested");
                //should throw an error here
            }
            DateTime CurNextTimePoint;
            if (CurrentProtocolInUse == null)//this happens if the protocol was deleted
            {
                CurNextTimePoint = DateTime.Now.AddYears(100);//makes sure another protocol always runs sooner
            }
            else
            {
                CurNextTimePoint = CurrentProtocolInUse.NextExecutionTimePoint;
            }
            foreach (Protocol Prot in Protocols)
            {
                if (Prot.NextExecutionTimePoint.CompareTo(CurNextTimePoint) == -1)//make sure -1 is lower
                {
                    CurrentProtocolInUse = Prot;
                    CurNextTimePoint = Prot.NextExecutionTimePoint;
                }
            }
            //now return the time until the next run in milliseconds
            TimeSpan CurDelay = CurNextTimePoint.Subtract(DateTime.Now);
            return CurDelay.TotalMilliseconds;
        }
        public object GetNextProtocolObject()
        {
            //this will return one of three objects, an instruction, a delay time (double), or null if protocols are done            
            if (Protocols.Count == 0)
            {//Once we are out of protocols, then that is it. Return null so that the program knows that there are no protocols
                return null;
            }
            else if (CurrentProtocolInUse == null)
            {
                double MS = FindMilliSecondsUntilNextRunAndChangeCurrentProtocol();
                if (MS > 0) { return MS; } //this should always return a positive ms value
            }
            //Below is a postfix operation
            int IndexOfNextProtInst = CurrentProtocolInUse.NextItemToRun++;//move up one instruction index,         
            //now to make sure there is another instruction
            if (CurrentProtocolInUse.Instructions.Count <= IndexOfNextProtInst)
            {
                //soon I will check for the protocol to be run next, based on the protocols next execution time
                //by setting the time to now plus several years, I guarantee other protocols will be much earlier then this one            
                //Removed later, not sure why here 

                //then we have finished this protocol, and it should be removed from the list
                RemoveProtocol(CurrentProtocolInUse);
                //note that it is still the current protocol in use though!!
                //SO I THINKL THIS CODE BELOW IS INCORRECT, STILL NEED TO PASS ON INSTRUCTION
                if (Protocols.Count == 0)
                {
                    //then this was the last protocol in the list, so return null
                    return null;
                }
                else
                {
                    return GetNextProtocolObject();
                }
            }
            else if (CurrentProtocolInUse.Instructions[IndexOfNextProtInst].GetType() == typeof(DelayTime))
            {
                //in this case this protocol run has ended
                DelayTime Delay = (DelayTime)CurrentProtocolInUse.Instructions[IndexOfNextProtInst];
                //determine the time when the next protocol should be run
                DateTime NextRunTime = DateTime.Now;
                NextRunTime = NextRunTime.AddMinutes(Delay.minutes);
                CurrentProtocolInUse.NextExecutionTimePoint = NextRunTime;
                double MillisecondDelay = FindMilliSecondsUntilNextRunAndChangeCurrentProtocol();
                //now if this time is negative, no delay should be used, and we should directly proceed to the next protocol
                if (MillisecondDelay <= 1)
                {
                    //recursively grab next instruction
                    return GetNextProtocolObject();
                }
                else { return MillisecondDelay; }
            }
            else if (CurrentProtocolInUse.Instructions[IndexOfNextProtInst].GetType() == typeof(StaticProtocolItem))
            {
                //in this case we simply return the protocol item
                return CurrentProtocolInUse.ReplaceVariablesWithValues((StaticProtocolItem)CurrentProtocolInUse.Instructions[IndexOfNextProtInst]);
                //after converting it to replace the variables with values
            }
            else
            {
                //finally the catch all behavior, kind of a bummer if this happens
                throw new Exception("The NextProtocol method received some unexpected behavior");
            }
        }
        private SmtpClient createSmtpClient()
        {
            //IF THIS FAILS, IT IS LIKELY DUE TO THE MCAFEE VIRUS SCANNER
            //CHANGE THE ACCESS PROTECTION TO ALLOW AN EXCEPTION FOR THE PROGRAM
            SmtpClient ToSend = new SmtpClient("smtp.gmail.com");//need to fill this in
            ToSend.UseDefaultCredentials = false;
            ToSend.Port = 587;
            ToSend.EnableSsl = true;
            ToSend.Credentials = new NetworkCredential("cjmarxlab@gmail.com", "marxlab3079");
            return ToSend;

        }
        public void ReportToAllUsers(string message = "The robot has an error, and has stopped working")
        {
            EmailErrorMessageToAllUsers(message);
            if (ShouldCall()) { CallAllUsers(); }
        }
        public void EmailErrorMessageToAllUsers(string ErrorMessage = "The robot has an error, and has stopped working")
        {
            foreach (object o in Protocols)
            {
                Protocol ProtocolForEmails = (Protocol)o;
                string[] emails = ProtocolForEmails.ErrorEmailAddress.Split(';');
                foreach (string emailaddress in emails)
                {
                    //IF THIS FAILS, IT IS LIKELY DUE TO THE MCAFEE VIRUS SCANNER
                    //CHANGE THE ACCESS PROTECTION TO ALLOW AN EXCEPTION FOR THE PROGRAM
                    String senderAddress = "cjmarxlab@gmail.com";
                    try
                    {
                        MailMessage email = new MailMessage(senderAddress, emailaddress, "Robot Alert", ErrorMessage);
                        SmtpClient ToSend = createSmtpClient();
                        ToSend.Send(email);
                    }
                    catch { }
                }
            }
        }
        public void CallAllUsers()
        {
            Alarm a = manager.GiveAlarmReference();
            foreach (object o in Protocols)
            {
                Protocol ProtocolNumbers = (Protocol)o;
                string[] phonenumbers = ProtocolNumbers.ErrorPhoneNumber.Split(';');
                foreach (string number in phonenumbers)
                {
                    // Todo use boolean return to implement multiple call attempts
                     a.CallConnects(number);
                }
            }
        }
        private bool ShouldCall()
        {
            DateTime now = System.DateTime.Now;
            int nd = now.Day;
            int nt = now.Hour;
            return nt >= CallHourStart || nt <= CallHourEnd;
        }
        public double GetMilliSecondsTillNextRunTime()
        {
            return this.FindMilliSecondsUntilNextRunAndChangeCurrentProtocol();
        }
    }
    public class ProtocolConverter
    {
        private static object ReturnValue(string ValueType, string Value)
        {
            switch (ValueType)
            {
                case "System.String":
                    return Value;
                    break;
                case "System.Int32":
                    return Convert.ToInt32(Value);
                    break;
                case "System.Double":
                    return Convert.ToDouble(Value);
                    break;
                case "System.Single":
                    return Convert.ToSingle(Value);
                    break;
                case "System.DateTime":
                    return Convert.ToDateTime(Value);
                    break;
                case "System.Boolean":
                    return Convert.ToBoolean(Value);
                case "Growth_Curve_Software.ReferenceToProtocolVariable":
                    return new ReferenceToProtocolVariable(Value);
                default:
                    throw new Exception("Could not identify value type during parsing, value type was: " + ValueType);
            }
        }
        public static Protocol XMLFileToProtocol(string Filename)
        {
            //Starting to approach the point where I need to break this up....

            Protocol NewProtocol = new Protocol();
            XmlDocument XmlDoc = new XmlDocument();
            XmlTextReader XReader = new XmlTextReader(Filename);//http://dn.codegear.com/article/32384
            XmlDoc.Load(XReader);
            //first node is xml, second is the protocol, this is assumed and should be the case
            XmlNode ProtocolXML = XmlDoc.ChildNodes[1];
            NewProtocol.ProtocolName = ProtocolXML.ChildNodes[0].InnerText;
            NewProtocol.ErrorEmailAddress = ProtocolXML.ChildNodes[1].InnerText;
            Assembly assembly = Assembly.GetExecutingAssembly();

            XmlNode VariablesNode = ProtocolXML.ChildNodes[2];
            foreach (XmlNode variable in VariablesNode.ChildNodes)
            {
                string Name = variable.ChildNodes[0].InnerText;
                string variableTypeString = variable.ChildNodes[1].Attributes[0].Value;
                Type VariableType = System.Type.GetType(variableTypeString);
                object valueAsString = variable.ChildNodes[1].InnerText;
                var Value = Convert.ChangeType(valueAsString, VariableType);
                ProtocolVariable PV = new ProtocolVariable(Name, VariableType, Value);
                NewProtocol.Variables.Add(PV.ToString(), PV);

            }

            XmlNode instructionsNode = ProtocolXML.ChildNodes[3];
            foreach (XmlNode instruct in instructionsNode.ChildNodes)
            {
                string DataType = instruct.Attributes[0].Value;
                Type InstructionType = assembly.GetType(DataType);
                object ProtocolInstruction = Activator.CreateInstance(InstructionType);
                string ValueType, Value;
                foreach (XmlNode Property in instruct.ChildNodes)
                {
                    if (Property.Name == "Parameters")//this means I have a static protocol item
                    {
                        ArrayList AL = new ArrayList();
                        object[] ParameterArray;
                        if (Property.ChildNodes.Count == 0) { ParameterArray = new object[0]; }
                        else
                        {
                            ParameterArray = new object[Property.ChildNodes.Count];
                            int ParameterIndex = 0;
                            foreach (XmlNode Parameter in Property.ChildNodes)
                            {
                                ValueType = Parameter.Attributes[0].Value;
                                Value = Parameter.InnerText;
                                ParameterArray[ParameterIndex] = ReturnValue(ValueType, Value);
                                ParameterIndex++;
                            }
                        }
                        StaticProtocolItem ProtocolItem = (StaticProtocolItem)ProtocolInstruction;
                        ProtocolItem.Parameters = ParameterArray;
                        ProtocolItem.ContainingProtocol = NewProtocol;
                    }
                    else
                    {
                        FieldInfo FI = ProtocolInstruction.GetType().GetField(Property.Name);
                        ValueType = Property.Attributes[0].Value;
                        Value = Property.InnerText;
                        object ValuetoSet = ReturnValue(ValueType, Value);
                        FI.SetValue(ProtocolInstruction, ValuetoSet);
                    }
                }
                NewProtocol.Instructions.Add(ProtocolInstruction);
            }
            XReader.Close();
            return NewProtocol;
        }
        public static void ProtocolToXMLFile(Protocol curProtocol, string Filename)
        {

            XmlTextWriter XWriter = new XmlTextWriter(Filename, Encoding.ASCII);
            XWriter.Formatting = Formatting.Indented;
            XWriter.WriteStartDocument();
            XWriter.WriteStartElement("Protocol");

            //first to write the name
            XWriter.WriteStartElement("ProtocolName");
            XWriter.WriteString(curProtocol.ProtocolName);
            XWriter.WriteEndElement();
            //now the error emails
            XWriter.WriteStartElement("ErrorEmailAddress");
            XWriter.WriteValue(curProtocol.ErrorEmailAddress);
            XWriter.WriteEndElement();

            //Here we have code to loop through and drop the variables in
            XWriter.WriteStartElement("Variables");
            foreach (string varName in curProtocol.Variables.Keys)
            {
                ProtocolVariable PV = curProtocol.Variables[varName];
                XWriter.WriteStartElement("Variable");
                XWriter.WriteStartElement("Name");
                XWriter.WriteString(PV.Name);
                XWriter.WriteEndElement();
                XWriter.WriteStartElement("Value");
                XWriter.WriteStartAttribute("Type");
                XWriter.WriteValue(PV.DataType.ToString());
                XWriter.WriteEndAttribute();
                XWriter.WriteValue(PV.Value.ToString());
                XWriter.WriteEndElement();
                XWriter.WriteEndElement();
            }
            XWriter.WriteEndElement();//end variables

            XWriter.WriteStartElement("Instructions");
            //now to loop through all the instructions and make it happen
            foreach (object o in curProtocol.Instructions)
            {
                XWriter.WriteStartElement("Instruction");
                XWriter.WriteStartAttribute("InstType");
                XWriter.WriteValue(o.GetType().ToString());
                XWriter.WriteEndAttribute();
                //now to unload the properties here
                //System.Windows.Forms.MessageBox.Show(o.GetType().GetProperties().Length.ToString());
                var toTest = Type.GetType("System.Object[]");
                foreach (FieldInfo PI in o.GetType().GetFields())
                {
                    if (PI.FieldType.ToString() == "System.Object[]") //the parameters are always passed as an object array, this is kind of silly now since all my methods only take one argument, but in the future who knows.
                    {
                        object[] Params = (object[])PI.GetValue(o);
                        XWriter.WriteStartElement("Parameters");
                        foreach (object Param in Params)
                        {
                            XWriter.WriteStartElement("Parameter");
                            XWriter.WriteStartAttribute("Type");
                            XWriter.WriteValue(Param.GetType().ToString());
                            XWriter.WriteEndAttribute();
                            //XWriter.WriteValue(Param);
                            //switched to accomodate new types
                            XWriter.WriteString(Param.ToString());
                            XWriter.WriteEndElement();

                        }
                        XWriter.WriteEndElement();
                    }
                    else if (PI.FieldType == typeof(Protocol)) continue;//Ignore the containing protocol bit
                    else
                    {
                        XWriter.WriteStartElement(PI.Name);
                        XWriter.WriteStartAttribute("Type");
                        XWriter.WriteValue(PI.FieldType.ToString());
                        XWriter.WriteEndAttribute();
                        XWriter.WriteValue(PI.GetValue(o));//should throw an error if not a valid type
                        XWriter.WriteEndElement();
                    }
                }
                XWriter.WriteEndElement();
            }
            XWriter.WriteEndElement();

            XWriter.WriteEndElement();
            XWriter.WriteEndDocument();
            XWriter.Close();
        }
        private static ArrayList ProtocolWithRepsRecurscion(ArrayList Instructions, LoopInstruction Repeat, int RepeatIndex)
        {
            //this protocol should be called by the one below it to expand out a loop
            ArrayList ProtToAppend = new ArrayList();//this will be returned
            for (int j = 0; j < Repeat.TimesToRepeat; j++)
            {
                for (int i = Repeat.StartInstruction - 1; i < RepeatIndex; i++)
                {
                    object Item = Instructions[i];
                    if (Item is LoopInstruction)
                    {
                        LoopInstruction NewRepeat = (LoopInstruction)Item;
                        ArrayList NewListToAdd = ProtocolWithRepsRecurscion(Instructions, NewRepeat, i);
                        ProtToAppend.AddRange(NewListToAdd);
                    }
                    else
                    {
                        ProtToAppend.Add(Item);
                    }

                }
            }
            return ProtToAppend;
        }
        public static ArrayList ProtocolWithRepeatsToProtocolWithout(ArrayList Instructions)
        {
            //This method will convert an arraylist that contains protocols with loop instructions into an arraylist
            //that contains no repeat instructions, should combine it with the method above later
            ArrayList CurrentProtocol = new ArrayList();
            try
            {
                int currentIndex = 0;
                foreach (object Item in Instructions)
                {
                    if (Item is LoopInstruction)
                    {
                        LoopInstruction Repeat = (LoopInstruction)Item;
                        ArrayList NewList = ProtocolWithRepsRecurscion(Instructions, Repeat, currentIndex);
                        CurrentProtocol.AddRange(NewList);
                    }
                    else
                    {
                        CurrentProtocol.Add(Item);
                    }
                    currentIndex++;
                }
            }
            catch (Exception thrown) { throw new Exception("Could not convert the protocol, inner error is:\n" + thrown.Message); }
            return CurrentProtocol;
        }
        public static void ProtocolItemsToListBoxProtocolItems(ArrayList Instructions, System.Windows.Forms.ListBox ListBoxToFill, bool AddToCurrent)
        {
            int index = 1;
            if (AddToCurrent)
            {
                index = ListBoxToFill.Items.Count + 1;
            }
            foreach (object Instruction in Instructions)
            {
                if (Instruction is StaticProtocolItem)
                {
                    ListBoxProtocolItem inst = new ListBoxProtocolItem();
                    inst.InstructionNumber = index;
                    inst.Instruction = (StaticProtocolItem)Instruction;
                    ListBoxToFill.Items.Add(inst);
                }
                else if (Instruction is LoopInstruction)
                {
                    ListBoxRepeatItem rep = new ListBoxRepeatItem();
                    rep.InstructionNumber = index;
                    rep.RepeatInstruction = (LoopInstruction)Instruction;
                    ListBoxToFill.Items.Add(rep);
                }
                else if (Instruction is DelayTime)
                {
                    ListBoxDelayItem del = new ListBoxDelayItem();
                    del.delay = (DelayTime)Instruction;
                    del.InstructionNumber = index;
                    ListBoxToFill.Items.Add(del);
                }
                index++;
            }
        }
    }
    public class ProtocolEventCaller
    {
        public event ProtocolPauseEventHandler onProtocolPause;
        public ProtocolEventCaller()
        {
        }
        public void FirePauseEvent(TimeSpan curDelay)
        {
            if (onProtocolPause != null)
            {
                onProtocolPause(curDelay);
            }
        }

    }
}
