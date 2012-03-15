using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Reflection;

namespace Growth_Curve_Software
{

    public partial class MakeProtocols : Form
    {
        List<BaseInstrumentClass> InstrumentCollection;
        public MakeProtocols(List<BaseInstrumentClass> instrumentcollection)
        {
            InitializeComponent();
            InstrumentCollection = instrumentcollection;
            //hope this is thread safe, note that I will still be accessing the same objects as before,
            //this only accounts for changes to the collection itself            
        }
        private void MakeProtocols_Load(object sender, EventArgs e)
        {
            TurnInstrumentsIntoTree();
        }
        private void TurnInstrumentsIntoTree()
        {
            ArrayList MethodNamesToIgnore = new ArrayList();
            Type BaseType = typeof(BaseInstrumentClass);
            foreach (MethodInfo MI in BaseType.GetMethods())
            { MethodNamesToIgnore.Add(MI.Name); }
            foreach (BaseInstrumentClass Instr in InstrumentCollection)
            {
                //BaseInstrumentClass Instr = (BaseInstrumentClass)o;
                TreeNode InstrNode = new TreeNode(Instr.Name);
                Type InstType = Instr.GetType();
                foreach (MethodInfo MI in InstType.GetMethods())
                {
                    if (!MethodNamesToIgnore.Contains(MI.Name))
                    {
                        string NodeName = MI.Name + "(";
                        foreach (ParameterInfo PI in MI.GetParameters())
                        {
                            NodeName += PI.Name + ",";
                        }
                        NodeName = NodeName.TrimEnd(',') + ")";
                        InstrNode.Nodes.Add(new TreeNode(NodeName));
                    }
                }
                if (InstrNode.Nodes.Count != 0)
                { MethodsView.Nodes.Add(InstrNode); }
            }
        }
        private object GetInstrumentClass(string name)
        {
            foreach (object o in InstrumentCollection)
            {
                BaseInstrumentClass BC = (BaseInstrumentClass)o;
                if (BC.Name == name)
                {
                    return BC;
                }
            }
            return null;
        }
        private MethodInfo ReturnMethodInfoFromObject(string MethodName, object o)
        {
            Type t = o.GetType();
            foreach (MethodInfo MI in t.GetMethods())
            {
                if (MI.Name == MethodName)
                {
                    return MI;
                }
            }
            throw new Exception("Sought after method not found");
        }
        private void MethodsView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            lblMethodName.Visible = false;//this is a dummy label, only used to hold the string value, here for historical reasons
            TreeNode curSelection = MethodsView.SelectedNode;
            lblMethodName.Text = "";
            lblParametersRequired.Text = "";
            lblClickButtonProtocol.Text = "";
            tblMethodParameterView.DataSource = null;
            if (curSelection.Nodes.Count == 0)
            {
                string MethodName = curSelection.Text.ToString().Split('(')[0];
                string InstrumentName = curSelection.Parent.Text;
                var Instrument = GetInstrumentClass(InstrumentName);
                var Meth = ReturnMethodInfoFromObject(MethodName, Instrument);
                if (Meth.GetParameters().Length > 0)
                {
                    lblParametersRequired.Text = "Fill in the required arguments for the \n" + MethodName + " method below";
                    FillTableViewWithMethodInfo(InstrumentName, Meth);
                }
                else { lblParametersRequired.Text = "No parameters are required for the " + MethodName + " method"; }
                lblMethodName.Text = InstrumentName + "-" + Meth.Name;
                lblClickButtonProtocol.Text = "Click below to add this method\n:";
            }

        }
        private void FillTableViewWithMethodInfo(string InstrumentName, MethodInfo MI)
        {
            DataTable DT = new DataTable();
            foreach (ParameterInfo PI in MI.GetParameters())
            {
                //DataColumn DC = new DataColumn(PI.Name, PI.ParameterType);
                DataColumn DC = new DataColumn(PI.Name, System.Type.GetType("System.String"));
                DT.Columns.Add(DC);
            }
            DataRow DR = DT.NewRow();
            DT.Rows.Add(DR);
            tblMethodParameterView.DataSource = DT;
        }
        private void tblMethodParameterView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("The data type you entered does not match the type required by the method.  For example, if the parameter should be a number and you entered text, that won't work.  Try Placing a number instead of text, or delete the entry to make this error disappear", "Incorrect Parameter", MessageBoxButtons.OK, MessageBoxIcon.Hand);

        }
        private void btnAddMethod_Click(object sender, EventArgs e)
        {
            //This method is now greatly changed to accomodate the new format of having variables in there
            //mostly just a awful lot of type checking, input verification stuff

            if (lblMethodName.Text.Split('-').Length == 2 && MethodsView.SelectedNode!=null)
            {
                TreeNode curSelection = MethodsView.SelectedNode;
                ListBoxProtocolItem CurProtocolItem = new ListBoxProtocolItem();
                string instName = lblMethodName.Text.Split('-')[0];
                string methName = lblMethodName.Text.Split('-')[1];
                CurProtocolItem.Instruction.InstrumentName = instName;
                CurProtocolItem.Instruction.MethodName = methName;
                object[] Parameters;
                if (tblMethodParameterView.DataSource != null)
                {
                    DataTable DT = (DataTable)tblMethodParameterView.DataSource;
                    DataRow DR = DT.Rows[0];
                    Parameters = DR.ItemArray;//actual inputted values, as strings

                    //Now we will find out what this value is, to make sure it is all agreeable
                    string MethodName = curSelection.Text.ToString().Split('(')[0];
                    string InstrumentName = curSelection.Parent.Text;
                    var Instrument = GetInstrumentClass(InstrumentName);
                    var Meth = ReturnMethodInfoFromObject(MethodName, Instrument);

                    ParameterInfo[] ParametersinMeth=Meth.GetParameters();
                    Type[] ParameterTypes = new Type[ParametersinMeth.Length];
                    for (int j = 0; j < ParametersinMeth.Length; j++)
                    {
                        ParameterTypes[j]=ParametersinMeth[j].ParameterType;
                    }

                    if (ParameterTypes.Length != DR.ItemArray.Length)
                    { MessageBox.Show("Inputted parameters and expected parameters do not have an equal count, contact Nigel"); return; }

                    for (int i = 0; i < ParameterTypes.Length;i++ )
                    {
                        object o = Parameters[i];//actual inputted value, currently as a string
                        
                        if (o.ToString() == "")
                        {
                            MessageBox.Show("You need to supply more parameters before this method can be added", "Parameters Not Supplied", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return;
                        }
                       if (ParameterTypes[i] == System.Type.GetType("System.String"))
                        {
                           //check that there are real variables in the 
                            if (!ProtocolManager.CheckStringForValidVariables((string)Parameters[i], (IList)lstCurrentVariables.Items))
                            {
                                MessageBox.Show("You have entered a variable that does not exist","Bad Data Entry",MessageBoxButtons.OK,MessageBoxIcon.Error);

                            }
                            continue;//string types always get cleared
                        }                      
                        else//need to check if this is a variable, and if it is not, I neet to check that the method works out okay
                        {
                            Type ExpectedType = ParameterTypes[i];
                            //two cases, either they have entered a variable, or they have a value, to find out which
                            string VariableName;
                            bool ParameterIsVariable = ProtocolManager.GetVariableNameFromText((string)o, out VariableName);
                            if (ParameterIsVariable)
                            {
                                //first to check that such a variable exists
                               
                                ReferenceToProtocolVariable Variable = new ReferenceToProtocolVariable(VariableName);
                                if (!ProtocolManager.IsVariableInList(VariableName, (IList)lstCurrentVariables.Items)) 
                                { MessageBox.Show("You are using a variable name that does not exist in your protocol", "Bad Variable Name", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                                else
                                {
                                    ProtocolVariable VariableInList = new ProtocolVariable();
                                    foreach (object Varia in lstCurrentVariables.Items)
                                    { if (Varia.ToString() == VariableName) 
                                    { VariableInList = (ProtocolVariable)Varia; break; } }
                                
                                    //make sure the variable type and parameter type match
                                    if (VariableInList.DataType != ParameterTypes[i])
                                    {
                                        MessageBox.Show("You are using a variable that does not have the appropriate type for this parameter", "Bad Variable Type", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                    else
                                    {
                                        //replace the "parameter" with the reference to the variable
                                        Parameters[i] = Variable;
                                    }
                                }
                            }
                            else
                            {
                                //make sure I can actually convert the damn thing to what it should be eg "A"->"a"
                                try
                                {
                                    Parameters[i]=Convert.ChangeType((string)o, ParameterTypes[i]);
                                }
                                catch
                                {
                                    MessageBox.Show("Could not convert your entry to the proper type, please make sure that numbers are used where they should be", "Bad Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                        }
                    }
                }
                else
                {
                    Parameters = new object[0];
                }
                CurProtocolItem.Instruction.Parameters = Parameters;
                CurProtocolItem.InstructionNumber = GetNextInstructionNumber();
                lstProtocol.Items.Add(CurProtocolItem);
            }
        }
        private void informationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Problems, Question, Comments?\nContact: Nigel Delaney\nndelaney@fas.harvard.edu\n415-823-4767");
        }
        private int GetNextInstructionNumber()
        {
            int LastItemIndex = lstProtocol.Items.Count + 1;
            return LastItemIndex;
        }
        private void btnAddInstruction_Click(object sender, EventArgs e)
        {
            int LoopStart = 0;
            int LoopCount = 0;
            try
            {
                LoopStart = Convert.ToInt32(txtLoopStart.Text);
                if (LoopStart > lstProtocol.Items.Count)
                {
                    MessageBox.Show("You cannot start a loop at an instruction number that does not exist", "Bad entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch { MessageBox.Show("You did not enter a valid instruction number to start at", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            try
            {
                LoopCount = Convert.ToInt32(txtNumberOfLoops.Text);
            }
            catch { MessageBox.Show("You did not enter a valid number of loops", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            ListBoxRepeatItem NewLoop = new ListBoxRepeatItem();
            NewLoop.RepeatInstruction.StartInstruction = LoopStart;
            NewLoop.RefToActualInstruction = (ListBoxProtocolItem)lstProtocol.Items[LoopStart - 1];
            NewLoop.RepeatInstruction.TimesToRepeat = LoopCount;
            NewLoop.InstructionNumber = GetNextInstructionNumber();
            lstProtocol.Items.Add(NewLoop);
        }
        private void btnDelayInstruction_Click(object sender, EventArgs e)
        {
            int DelayTime = 0;
            try
            {
                DelayTime = Convert.ToInt32(txtDelayMinutes.Text);
                if (DelayTime <= 0) { throw new Exception(); }
            }
            catch { MessageBox.Show("The delay time you entered is not valid", "Invalid time", MessageBoxButtons.OK, MessageBoxIcon.Hand); return; }
            ListBoxDelayItem newDelay = new ListBoxDelayItem();
            newDelay.delay.minutes = DelayTime;
            newDelay.InstructionNumber = GetNextInstructionNumber();
            lstProtocol.Items.Add(newDelay);
        }
        private void MoveInstructionUp()
        {
            if (lstProtocol.SelectedIndex > 0)
            {
                int indexAt = lstProtocol.SelectedIndex;
                object ToMove = lstProtocol.Items[indexAt];
                object ToReplace = lstProtocol.Items[indexAt - 1];
                lstProtocol.Items[indexAt] = ToReplace;
                lstProtocol.Items[indexAt - 1] = ToMove;
                ReNumberProtocolSteps();
              
                lstProtocol.SelectedIndex = indexAt - 1;
                lstProtocol.SelectedItem = ToMove;
            }
        }
        private void ReNumberProtocolSteps()
        {
            int i = 0;
            ArrayList Temp = new ArrayList();
            foreach (object o in lstProtocol.Items)
            {
                ProtocolListBoxItem ProtocolItem = (ProtocolListBoxItem)o;
                ProtocolItem.InstructionNumber = ++i;
                Temp.Add(ProtocolItem);
            }
            //for some reason I have to re-add everything to make it paint correctly again
            lstProtocol.Items.Clear();
            lstProtocol.Refresh();
            lstProtocol.Items.AddRange(Temp.ToArray());
            //foreach (object o in Temp)
            //{ lstProtocol.Items.Add(o); }
        }
        private void btnDeleteProtocolItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstProtocol.SelectedIndex != -1)
                {
                    lstProtocol.Items.RemoveAt(lstProtocol.SelectedIndex);
                    ReNumberProtocolSteps();
                    lstProtocol.Refresh();
                }
            }
            catch{ShowGenericError(); }
        }
        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            MoveInstructionUp();
        }
        private void MoveInstructionDown()
        {
            if (lstProtocol.SelectedIndex != -1  && lstProtocol.SelectedIndex!=lstProtocol.Items.Count-1)
            {
                int indexAt = lstProtocol.SelectedIndex;
                object ToMove = lstProtocol.Items[indexAt];
                object ToReplace = lstProtocol.Items[indexAt + 1];
                lstProtocol.Items[indexAt] = ToReplace;
                lstProtocol.Items[indexAt + 1] = ToMove;
                ReNumberProtocolSteps();
                lstProtocol.Refresh();
                lstProtocol.SelectedIndex = indexAt + 1;
                lstProtocol.SelectedItem = ToMove;
            }
        }
        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            MoveInstructionDown();
        }
        private void lstProtocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ToolTip T = new ToolTip();
                T.SetToolTip(lstProtocol, lstProtocol.SelectedItem.ToString());
            }
            catch { }
        }
        private ArrayList ProtocolListBoxToProtocolInstructionList(ListBox BoxToRun)
        {
            //This method will convert the list box protocol into an actual protocol
            ArrayList CurrentProtocol = new ArrayList();
            try
            {
                foreach (object Item in BoxToRun.Items)
                {
                    ProtocolListBoxItem myInstruction = (ProtocolListBoxItem)Item;
                    CurrentProtocol.Add(myInstruction.ReturnInstructionObject());
                }
            }
            catch (Exception thrown) { MessageBox.Show("The protocol tree could not be read properly, and your protocol is corrupt.  Check your repeat indexing.\nError is: " + thrown.Message); }
            return CurrentProtocol;
        }
        private Protocol CreateProtocolFromForm()
        {
            Protocol newProtocol = new Protocol();
            if (txtProtocolName.Text == "" | txtEmails.Text == "")
            {
                throw new Exception("You must name your protocol and provide an email address");
            }
            if (lstProtocol.Items.Count == 0)
            {
                throw new Exception("There are no instructions in your protocol, please add them.");
            }
            newProtocol.ErrorEmailAddress = txtEmails.Text;
            newProtocol.ProtocolName = txtProtocolName.Text;
            newProtocol.Instructions = ProtocolListBoxToProtocolInstructionList(lstProtocol);
            foreach (object o in lstCurrentVariables.Items)
            {
                ProtocolVariable PV = (ProtocolVariable)o;
                newProtocol.Variables.Add(PV.ToString(), PV);
            }
            //newProtocol.Instructions = ProtocolConverter.ProtocolWithRepeatsToProtocolWithout(newProtocol.Instructions);
            return newProtocol;
        }
        private void saveProtocolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog SFD = new SaveFileDialog();
                SFD.DefaultExt = ".xml";
                SFD.Filter = "XML file (*.xml)|*.xml";
                SFD.AddExtension = true;
                DialogResult DR = SFD.ShowDialog();
                if (DR == DialogResult.OK)
                {
                    string Filename = SFD.FileName;
                    ProtocolConverter.ProtocolToXMLFile(CreateProtocolFromForm(), Filename);
                }
            }
            catch (Exception thrown)
            {
                MessageBox.Show("Could not save the file.\n\n" + thrown.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void loadProtocolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string direct = @"C:\Users\Nigel Delaney\Desktop\\";
            try
            {
                OpenFileDialog OFD = new OpenFileDialog();
                OFD.DefaultExt = ".xml";
                OFD.Filter="XML file (*.xml)|*.xml";
                DialogResult DR = OFD.ShowDialog();
                if (DR == DialogResult.OK)
                {
                    string filename = OFD.FileName;
                    Protocol NewProtocol = ProtocolConverter.XMLFileToProtocol(filename);
                    txtEmails.Text = NewProtocol.ErrorEmailAddress;
                    txtProtocolName.Text = NewProtocol.ProtocolName;
                    lstProtocol.Items.Clear();
                    lstCurrentVariables.Items.Clear();
                    foreach (ProtocolVariable PV in NewProtocol.Variables.Values)
                    {
                        lstCurrentVariables.Items.Add(PV);
                    }
                    ProtocolConverter.ProtocolItemsToListBoxProtocolItems(NewProtocol.Instructions, lstProtocol,false);
                }
            }
            catch (Exception thrown)
            { MessageBox.Show("Could not open file.  Error is:\n\n" + thrown.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void insertOtherProtocolAtEndOfThisOneToolStripMenuItem_Click(object sender, EventArgs e)
        {
             try
            {
                OpenFileDialog OFD = new OpenFileDialog();
                OFD.DefaultExt = ".xml";
                OFD.Filter="XML file (*.xml)|*.xml";
                DialogResult DR = OFD.ShowDialog();
                if (DR == DialogResult.OK)
                {
                    string filename = OFD.FileName;
                    Protocol NewProtocol = ProtocolConverter.XMLFileToProtocol(filename);
                    txtEmails.Text = NewProtocol.ErrorEmailAddress;
                    txtProtocolName.Text = NewProtocol.ProtocolName;
                    //to make sure they are unique I am making a new set
                    List<ProtocolVariable> NewVariables = new List<ProtocolVariable>();
                    foreach (ProtocolVariable PV in NewProtocol.Variables.Values)
                    {
                        NewVariables.Add(PV);
                    }
                    //not add the old if it doesn't match
                    foreach (object o in lstCurrentVariables.Items)
                    {
                        ProtocolVariable PV = (ProtocolVariable)o;
                        foreach (ProtocolVariable NewPV in NewVariables)
                        {
                            if (NewPV.Equals(PV))
                            {
                                goto Next;
                            }
                        }
                        NewVariables.Add(PV);
                    Next:
                        continue;
                    }
                    lstCurrentVariables.Items.Clear();
                    lstCurrentVariables.Items.AddRange(NewVariables.ToArray());
                    ProtocolConverter.ProtocolItemsToListBoxProtocolItems(NewProtocol.Instructions, lstProtocol,true);
                }
            }
            catch (Exception thrown)
            { MessageBox.Show("Could not open file.  Error is:\n\n" + thrown.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void lstProtocol_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Delete)
                {
                    if (lstProtocol.SelectedIndex != -1)
                    {
                        lstProtocol.Items.RemoveAt(lstProtocol.SelectedIndex);
                        ReNumberProtocolSteps();
                        lstProtocol.Refresh();
                    }
                }
                else if (e.KeyData == Keys.Up)
                {
                    //MoveInstructionUp();
                }
                else if (e.KeyData == Keys.Down)
                {
                    //MoveInstructionDown();
                }
            }
            catch { ShowGenericError(); }
        }
        //The variables tab
        private void lstVariableTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lstVariableTypes.SelectedIndex != -1)
                {
                    DataTable DT = new DataTable();
                    DataColumn DC = new DataColumn("Variable Name", System.Type.GetType("System.String"));
                    DT.Columns.Add(DC);
                    Type VariableType;
                    switch (lstVariableTypes.SelectedIndex)
                    {
                        case 0:
                            VariableType = System.Type.GetType("System.String");
                            break;
                        case 1:
                            VariableType = System.Type.GetType("System.Int32");
                            break;
                        case 2:
                            VariableType = System.Type.GetType("System.Double");
                            break;
                        default:
                            throw new Exception("Variable type was not one of the expected ones");
                            VariableType = typeof(Int32);
                            break;
                    }
                    DC = new DataColumn("Value", VariableType);
                    DT.Columns.Add(DC);
                    DataRow DR = DT.NewRow();
                    DT.Rows.Add(DR);
                    dataViewVariable.DataSource = DT;
                    lstCurrentVariables.SelectedIndex = -1;
                }
            }
            catch { ShowGenericError(); }
        }
        private void dataViewVariable_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("The data type you entered does not match the type required for the variable.  For example, if the parameter should be a number and you entered text, that won't work.  Try Placing a number instead of text, or delete the entry to make this error disappear", "Incorrect Parameter", MessageBoxButtons.OK, MessageBoxIcon.Hand);

        }
        private void btrnAddVariable_Click(object sender, EventArgs e)
        {
            try
            {
                ProtocolVariable PV = new ProtocolVariable();
                if (dataViewVariable.DataSource != null)
                {
                    DataTable DT = (DataTable)dataViewVariable.DataSource;

                    DataRow DR = DT.Rows[0];
                    foreach (object o in DR.ItemArray)
                    {
                        if (o.ToString() == "")
                        {
                            MessageBox.Show("You need to supply more information before this variable can be added", "Parameters not Supplied", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return;
                        }
                    }
                    PV.Name = DR[0] as string;
                    PV.Value = DR[1];
                    PV.DataType = DR[1].GetType();
                    if (lstVariableTypes.SelectedIndex != -1)//this is a new variable
                    {
                        foreach (object o in lstCurrentVariables.Items)
                        {
                            if (o.Equals(PV))
                            {
                                MessageBox.Show("The variable name you have choosen is not unique");
                                return;
                            }
                        }
                    }
                    else //we are changing the parameter of our old variable 
                    {
                        foreach (object o in lstCurrentVariables.Items)
                        {
                            if (o.Equals(PV))
                            {
                                PV = (ProtocolVariable)o;//change the reference to ensure that the items in the protocol all point to same object, and we don't use a new one
                                PV.Value = DR[1];
                                lstCurrentVariables.Items.Remove(o);
                                break;
                            }
                        }
                    }
                    lstCurrentVariables.Items.Add(PV);
                }
            }
            catch { ShowGenericError(); }

        }        
        private void lstCurrentVariables_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lstCurrentVariables.SelectedIndex != -1)
                {
                    DataTable DT = new DataTable();
                    DataColumn DC = new DataColumn("Variable Name", System.Type.GetType("System.String"));
                    DT.Columns.Add(DC);
                    ProtocolVariable SelectedVariable = (ProtocolVariable)lstCurrentVariables.SelectedItem;
                    DC = new DataColumn("Value", SelectedVariable.DataType);
                    DT.Columns.Add(DC);
                    DataRow DR = DT.NewRow();
                    DR[0] = SelectedVariable.Name;
                    DR[1] = SelectedVariable.Value;
                    DT.Rows.Add(DR);
                    dataViewVariable.DataSource = DT;
                    lstVariableTypes.SelectedIndex = -1;
                }
            }
            catch { ShowGenericError(); }
        }
        private void lstCurrentVariables_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Delete)
                {
                    //try to remove it, and if it works out, go ahead
                    if (lstCurrentVariables.SelectedIndex != -1 && lstProtocol.Items.Count > 0)
                    {
                        ProtocolVariable PVtoRemove = (ProtocolVariable)lstCurrentVariables.SelectedItem;
                        lstCurrentVariables.Items.RemoveAt(lstCurrentVariables.SelectedIndex);
                        Protocol NewProtocol = CreateProtocolFromForm();
                        //now check that this protocol works out
                        if (!ProtocolManager.VerifyProtocolVariablesReferences(NewProtocol))
                        {
                            MessageBox.Show("This variable is referenced inside the protocol, it cannot be removed", "Error Removing Variable", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //put it back
                            lstCurrentVariables.Items.Add(PVtoRemove);
                        }
                    }
                }
            }
            catch (Exception thrown)
            {
                MessageBox.Show("Could not perform action\n\n"+thrown.Message);
            }
        }
        private void ShowGenericError()
        {
            MessageBox.Show("Something Weird Happened, Contact Nigel", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

}
