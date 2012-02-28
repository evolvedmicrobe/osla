using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ScicloneICP;
//using ScicloneLog;
//using ScicloneProject;
using System.Collections;
using Growth_Curve_Software;
using System.Runtime.InteropServices;
using System.Threading;

namespace Growth_Curve_Software
{
    public class Sciclone : BaseInstrumentClass
    {
        //ScicloneICP.CICPClass m_SciCloneICP;
        ScicloneICP._CICP m_SciCloneICP;
        ScicloneICP.enumICPStates currentState;
        //string ConfigurationFile = @"C:\Program Files\Zymark\Instruments\Sciclone\ICP\Configurations\ScicloneALH3000.txt";
        string pConfigurationFile = @"C:\Program Files\Zymark\Instruments\Sciclone\ICP\Configurations\ScicloneALH3000_renmaed.txt";

        public string ConfigurationFile
        {
            get { return pConfigurationFile; }
            set { pConfigurationFile = value; }
        }
        public override void InitializeFromParsedXML(System.Xml.XmlNode instrumentNode)
        {
            base.InitializeFromParsedXML(instrumentNode);
            FinishInitialization();
        }
        //most of this is ripped off from the example file from the people at Clara
        //However, also of note is that the app must be loaded explicitly by the program
        //before a method can be called or run, if this is not true, it will throw an error.        
        public Sciclone()
        {        }
        public override string  Name
        {
	        get { return "Sciclone"; }
        }
        
        public override void Initialize()
        {
            //Another two methods have to be called after this one, first to make sure that it loads the correct app
            //and that it initializes the system
            try
            {
                bool SimulationMode = false;
                if (currentState != enumICPStates.Initial)
                {
                    throw new Exception("Sciclone is already running");
                }
                    m_SciCloneICP = new CICPClass();

                    ScicloneICP.enumOperationalMode mymode;
                    if (SimulationMode)
                    {
                        mymode = ScicloneICP.enumOperationalMode.omSimulationMode;
                    }
                    else
                    {
                        mymode = ScicloneICP.enumOperationalMode.omRealMode;
                    }
                    //now create the coreDCC which is the core device of the sciclone
                    //the icp should determine the mode based on its setting
                    m_SciCloneICP.set_IICPAutomationServer_OperationalMode(ref mymode);
                    m_SciCloneICP.IICPAutomationServer_CreateCoreDCC();
                    string[] myArray=new string[20];
                    //Array temp = new Array();
                    //temp.Initialize();
                    Array ToPass= stringArToArray(myArray);
                    myArray.Initialize();
                    m_SciCloneICP.IICPAutomationServer_GetConfigurationsList(ref ToPass);
                    myArray = ArrayToStringAr(ToPass);
                   // foreach (object o in myArray)
                    //{
                        //MessageBox.Show(o.ToString() + "\n" + o.GetType().ToString());
                   // }
                    //need to set a method to have the user select and load 
                        
                    if (!SimulationMode)
                    {
                        //establish a connection with the serial port, which is the set / specified as a configuration parameter
                        //int he sciclone ALH 3000 server process
                        m_SciCloneICP.IICPAutomationServer_ConnectToSciclone();
                        //download configuration
                        //m_SciCloneICP.DownloadConfiguration();
                        //This instruction below was the one most recently used when it worked
                        //m_SciCloneICP.IICPAutomationServer_DownloadConfiguration();
                        m_SciCloneICP.IICPAutomationServer_LoadConfiguration(ConfigurationFile);
                    }
                    else
                    {
                       
                        m_SciCloneICP.IICPAutomationServer_LoadConfiguration(ConfigurationFile);
                        //throw new Exception("Simulation not up and running, no configuration loaded");
                    }
                    //now create controlling shell, which contain the instrument specific parts of the 
                    //Sciclone such as the head, z8, etc
                    m_SciCloneICP.IICPAutomationServer_CreateControllingShell();
                    //now connect to the auxilarry stuff (eg cavaro pumps, etc)
                    m_SciCloneICP.IICPAutomationServer_ConnectToAuxiliaryHW();

                    //create programming shell
                    //the programming shell is the component of the ICP that manages method creation
                    //and execution, and provides the main GUI for the ICP
                    m_SciCloneICP.IICPAutomationServer_CreateProgrammingShell();

                    enumGUIWindowState GUIState = enumGUIWindowState.wsNormal;
                    m_SciCloneICP.set_IICPAutomationServer_GUIWindowState(ref GUIState);
                    //now to initialize the software
                    m_SciCloneICP.IICPAutomationServer_Initialize();
                    //insert status ok here
                    StatusOK = true;
                    currentState = ScicloneICP.enumICPStates.Created;
                    //m_SciCloneICP.ExecutionStatus += new __CICP_ExecutionStatusEventHandler(m_SciCloneICP_ExecutionStatus);

            }
            
            catch (Exception thrown)
            {
                InstrumentError IE = new InstrumentError("Could not Initialize Sciclone\n\n"+thrown.Message, false, this);              
                throw IE;
            }
        }
        public void FinishInitialization()
        {
            //These two steps are required to have the machine work, but were not included
            //in the original example vb project, so I am putting them here
            try
            {
                LoadApplication();
                RunMethod("OnInitialization", "none");
                RunMethod("MoveToSafePosition", "none");
            }
            catch (Exception thrown)
            {
                throw new InstrumentError("Failed to Load and Initialize Application\n" + thrown.Message, false, this);
            }
        }
        private void LoadApplication()
        {
            //could load any application, but am using the standard for now
            try
            {
                string AppName = @"\MarxLab_Apps.app";
                //m_SciCloneICP.LoadApplication(AppName);
                m_SciCloneICP.IICPAutomationServer_LoadApplication(AppName);
            }
            catch (Exception thrown)
            {
                throw new InstrumentError("Could not load application for Sciclone, likely caused by running on uninitialized sciclone\n\nInner Message: " + thrown.Message, false, this);
            }
        }
        //Async Method details
        long sLastAsyncCompletionStatus;
        string sLastAsyncCompletionStatusDescription;
        bool bAsyncCallinProgress;
        void m_SciCloneICP_ExecutionStatus(ref int ErrorCode, ref string ErrorDescription)
        {
            bAsyncCallinProgress = false;
            sLastAsyncCompletionStatusDescription = ErrorDescription;
            sLastAsyncCompletionStatus = ErrorCode;//zero is okay
        }
        void PrepareToStartAsyncExecution()
        {
            sLastAsyncCompletionStatus = -9;//negative number is bad, indicates wasn't set on finish.  
            sLastAsyncCompletionStatusDescription = "Description not set by Sciclone";
            bAsyncCallinProgress = true;
        }
        void CleanUpAsyncExecution()
        {
            bAsyncCallinProgress = false;
        }        
        
        public string[] GetMethodsList()
        {
            string[] myArray = new string[120];
            Array ToPass = stringArToArray(myArray);
            myArray.Initialize();
            CICP mySci = (CICP) m_SciCloneICP;
            mySci.IICPAutomationServer_GetMethodsList(ref ToPass);
            myArray = ArrayToStringAr(ToPass);
            return myArray;
        }
        public string[] GetApplicationList()
        {
            string[] Applications = new string[20];
            Array ToPass = (Array)Applications;
            try
            {
                if (StatusOK)
                {
                    m_SciCloneICP.IICPAutomationServer_GetApplicationsList(ref ToPass);
                    Applications = ArrayToStringAr(ToPass);
                }
                else
                {
                    //Applications.Add("Instrument Not Initialized");
                }
                return Applications;
            }
            catch (Exception thrown)
            {
                StatusOK = false;
                InstrumentError IE = new InstrumentError("Could not get app list", false, this);
                throw IE;
            }
        }
        public bool RunMethod(string MethodName, string CommaSeperatedParameters)
        {
             clsMethod ToRun=null; 
            try
            {

               ToRun= GetMethodInformationAsMethod(MethodName);
                if (CommaSeperatedParameters.ToUpper() != "NONE")
                {
                    string[] Parameters = CommaSeperatedParameters.Split(',');
                    for (int i = 0; i < Parameters.Length; i++)
                    { Parameters[i] = Parameters[i].Trim(); }
                    if (Parameters.Length != ToRun.Count)
                    { throw new Exception("The number of parameters you had, did not match the methods"); }
                    for (int i = 1; i <= Parameters.Length; i++)
                    {
                        object j = (object)i;
                        //as far as I can tell, all the parameters as passed as strings.
                        clsParameter Param = (clsParameter)ToRun.get_Item(ref j);
                        if (Param.TypeOfParameter == enumTypeOfParameter.typNumericParameter)
                        {
                            try { Convert.ToDouble(Parameters[i - 1]); }
                            catch { throw new Exception("Your parameter type does not match a numeric value"); }
                        }
                        Param.set_ValueOfParameter(ref Parameters[i - 1]);
                    }
                }
                RunMethodPrivate(ToRun);
                try { Marshal.ReleaseComObject(ToRun); }
                catch { }
                ToRun = null;
                return true;
            }
            catch (Exception thrown)
            {
                try { Marshal.ReleaseComObject(ToRun); }
                catch { }
                ToRun = null;
                throw new Exception("Failure during method call\n\n" + thrown.Message);
                return false;
                
            }
            
        }
        private bool RunMethodPrivate(clsMethod Method)
        {
            try
            {
                PrepareToStartAsyncExecution();
                //m_SciCloneICP.ExecuteMethod(ref Method);
                m_SciCloneICP.IICPAutomationServer_ExecuteMethod(ref Method);
                while (bAsyncCallinProgress)
                {
                    Thread.Sleep(750);
                    Application.DoEvents();
                }
                if (sLastAsyncCompletionStatus != 0)//indicates an error
                {
                    throw new InstrumentError("Sciclone did not complete command correctly.\nError Code: " + sLastAsyncCompletionStatus.ToString() + "\nDescription: " + sLastAsyncCompletionStatusDescription, false, this);
                    StatusOK = false;
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception thrown)
            {
                StatusOK = false;
                throw new InstrumentError("Method " + Method.ToString() + " did not run correctly " + thrown.Message, false, this);
            }
        }
        public clsMethod GetMethodInformationAsMethod(string MethodName)
        {
            //In order to initialize the object I have to go through this nonsense first
            //otherwise no method is created
            ClsMethodVariant PlaceHolder = new ClsMethodVariant();
            clsMethod MethodReturned = (clsMethod)PlaceHolder;
            m_SciCloneICP.IICPAutomationServer_LoadMethod(MethodName, ref MethodReturned);
            return MethodReturned;
        }
        public string GetMethodInformationAsString(string MethodName)
        {
            clsMethod MethodReturned = GetMethodInformationAsMethod(MethodName);
            string OfMethod = MethodName + "(";
            if (MethodReturned.Count > 0)
            {
                for (int i = 1; i <= MethodReturned.Count; i++)
                {
                    object j = (object)i;
                    clsParameter myParam = MethodReturned.get_Item(ref j);
                    OfMethod += myParam.TypeOfParameter.ToString() + " " + myParam.NameOfParameter + ",";
                }
            }
            OfMethod.TrimEnd(',');
            OfMethod += ")";
            return OfMethod;
        }
        
        /// <summary>
        /// This sections contains methods necessary to communicate with the activeX control
        /// </summary>
        /// <param name="ToUse"></param>
        /// <returns></returns>
        private Array stringArToArray(string[] ToUse)
        {
            return (Array)ToUse;
        }
        private Array objectArToArray(object[] ToUse)
        { return (Array)ToUse; }
        private string[] ArrayToStringAr(Array ToConvert)
        {
            string[] myArray = new string[ToConvert.Length];
            ToConvert.CopyTo(myArray, 0);
            return myArray;
        }
        public void SetScicloneAsManuallyCorrected()
        {
            StatusOK = true;
        }
        //general BaseInstrument Methods
        public override bool AttemptRecovery(InstrumentError Error)
        {
            try
            {
                StatusOK = false;
                currentState = enumICPStates.Initial;
                CloseAndFreeUpResources();
                Initialize();
                FinishInitialization();
                StatusOK = true;
                return true;

            }
            catch (Exception thrown)
            {
                return false;
            }
        }
        ~Sciclone()
        {
            if (m_SciCloneICP != null)
            {
                CloseAndFreeUpResources();
            }
        }
        public override bool CloseAndFreeUpResources()
        {
            try
            {
                StatusOK = false;
                if (m_SciCloneICP != null)
                {
                    try
                    {
                        m_SciCloneICP.IICPAutomationServer_Shutdown();
                    }
                    catch { }
                    try
                    {
                        int val;
                        val = System.Runtime.InteropServices.Marshal.ReleaseComObject(m_SciCloneICP);
                    }
                    catch { }
                    m_SciCloneICP = null;
                }
                    //GC.Collect();
                //Thread.Sleep(500);
                string[] Tokill = new string[6] { "ScicloneICP", "SciPEM", "SciRabbitVexta", "Consumables", "CommDispatcher","CavroDeviceController" };
                foreach(string process in Tokill)
                {
                    
                    KillProcessAttempt(process);
                    
                }
                return true;
            }
            catch (Exception thrown)
            {
                return false;
            }
        }

        
    }

    public class ClsMethodVariant : clsMethod
    {
  

        #region _clsMethod Members

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public string MethodName
        {
            get { throw new NotImplementedException(); }
        }

        public clsParameter get_Item(ref object vntIndexKey)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

