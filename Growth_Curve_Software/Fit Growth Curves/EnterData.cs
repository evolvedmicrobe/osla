using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Fit_Growth_Curves
{

    public partial class FitterForm : Form
    {
        DataTable DT;
        private void MakeEnterDataTable()
        {
            DT = new DataTable();
            DataColumn col = new DataColumn("Sample Date/Time", System.Type.GetType("System.DateTime"));
            
            DT.Columns.Add(col);
            col = new DataColumn("OD Value", System.Type.GetType("System.Double"));
            DT.Columns.Add(col);
            DataView.DataSource = DT;
            
        }
        private void DataView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.ColumnIndex == 0) { MessageBox.Show("Not a valid Date Time"); }
            else { MessageBox.Show("You must enter a number"); }
        }
      

        private void btnInsertTime_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow row = DT.NewRow();
                row[0] = PickSampleTime.Text.ToString();
                DT.Rows.Add(row);
            }
            catch(Exception thrown)
            {
                MessageBox.Show("Could not add the time in the box, please check the value.\nThe actual error was: "+thrown.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            DT.Rows.Clear();
        }
        private void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                //reset values for matrices
                int lastRowEmpty = 0;
                if (txtDataName.Text == "") { MessageBox.Show("You must name this dataset"); }
                else
                {
                    MessageBox.Show(DT.Rows[DT.Rows.Count - 1][1].ToString());
                    if (DT.Rows[DT.Rows.Count - 1][1].ToString() == "")
                    { lastRowEmpty = 1; }//This will be used to not read the last row if the user put no data there
                    DateTime[] acTimeValues = new DateTime[DT.Rows.Count - lastRowEmpty];
                    double[] absDATA = new double[DT.Rows.Count - lastRowEmpty];
                    for (int i = 0; i < DT.Rows.Count - lastRowEmpty; i++)
                    {
                        acTimeValues[i] = Convert.ToDateTime(DT.Rows[i][0]);//add the time value
                        absDATA[i] = Convert.ToDouble(DT.Rows[i][1]);//Add the OD Value
                    }//add od value
                    UpdateDateTimesInFile(acTimeValues);
                    GrowthData GR = new GrowthData(txtDataName.Text, acTimeValues, absDATA, false);
                    lstGrowthCurves.Items.Add(GR);
                    lstGrowthCurvesMirror.Items.Add(GR.ToString());
                    lstMultiplePlots.Items.Add(GR.ToString());
                    lstGrowthCurves.SelectedIndex = lstGrowthCurves.Items.Count - 1;
                    tabMainTab.SelectedIndex = 0;
                }
            }

            catch (Exception thrown)
            {
                MessageBox.Show("Error in entering data\n\n check that all your values were real numbers.\n\n" + thrown.Message);
            }

        }
        }   
    
    }