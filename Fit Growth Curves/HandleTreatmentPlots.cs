using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using ZedGraph;
using System.Linq;
using System.Drawing.Drawing2D;

namespace Fit_Growth_Curves
{

    public partial class FitterForm : Form
    {
        TextBox[] TreatmentTextBoxes = new TextBox[SelectablePlateMap.MAX_GROUP_ASSIGNMENTS];
        delegate double GetValueForTreatment(GrowthData x);
        void selectablePlateMap1_GroupsChanged(object sender, EventArgs e)
        {
            RemakeTreatmentGraph();
           
        }
        private void ClearTreatmentPlot()
        {
                GraphPane Graph = plotTreatments.GraphPane;
                Graph.CurveList.Clear();
        }
        private void chkTreat_CheckedChanged(object sender, EventArgs e)
        {
            RemakeTreatmentGraph();

        }
 
        private void RemakeTreatmentGraph()
        {
            ClearTreatmentPlot();
            if (rbtnTreatTimevOD.Checked)
            {
                UpdateTreatmentGraphWithGrowthData();
            }
            else
            {
                GetValueForTreatment valueGetter;
                string Title = "";
                if (rbtnTreatDoublingTime.Checked)
                {
                    valueGetter = (GrowthData x) => x.GrowthRate.DoublingTime;
                    Title = "Doubling Time";
                }
                else if (rbtnTreatGrowthRate.Checked)
                {
                    valueGetter = (GrowthData x) => x.GrowthRate.GrowthRate;
                    Title = "Growth Rate";
                }
                else if (rbtnTreatMaxOd.Checked)
                {
                    valueGetter = (GrowthData x) => x.ODValues.Max();
                    Title = "Max OD";
                }
                else if (rbtnTreatNumPoints.Checked)
                {
                    valueGetter = (GrowthData x) => x.GrowthRate.NumPoints;
                    Title = "Number of Points Used";
                }
                else if (rbtnTreatRSq.Checked)
                {
                    valueGetter = (GrowthData x) => x.GrowthRate.R2;
                    Title = "R2";
                }
                else if (rbtnTimeToOD.Checked)
                {
                    valueGetter = (GrowthData x) => x.HoursTillODReached(.02).GetValueOrDefault(0);
                    Title = "Estimated Time Till OD .02";
                }
                else //(rbtnTreatInitialOD.Checked)
                {
                    valueGetter = (GrowthData x) => x.ODValues[0];
                    Title = "Initial OD";
                }
                UpdateTreatmentGraphWithTreatmentData(valueGetter, Title);
            }
        }
        private Dictionary<string,GrowthData> GetDictionaryOfGrowthRateData()
        {
            Dictionary<string,GrowthData> CurrentDataSets=new Dictionary<string,GrowthData>();
            foreach(GrowthData gd in lstGrowthCurves.Items.Cast<GrowthData>())
            {
                CurrentDataSets[gd.ToString()]=gd;
            }
            return CurrentDataSets;
        }
        void UpdateTreatmentGraphWithGrowthData()
        {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    GraphPane Graph = plotTreatments.GraphPane;
                    Graph.CurveList.Clear();
                    Graph.Title.Text = "Growth Plots";
                    Graph.XAxis.Title.Text = "Hours";
                    if (chkTreatShowLog.Checked) { Graph.YAxis.Title.Text = "Log [OD600]"; }
                    else { Graph.YAxis.Title.Text = "OD600"; }
                    Graph.Legend.Position = LegendPos.InsideTopLeft;
                    Graph.Legend.FontSpec.Size = 8f;
                    Graph.Legend.IsHStack = true;
                    SymbolType SymboltoUse = SymbolType.Circle;
                    Dictionary<string, GrowthData> curData = GetDictionaryOfGrowthRateData();
                    for (int i = 1; i < SelectablePlateMap.MAX_GROUP_ASSIGNMENTS; i++)
                    {
                        Color groupColor;
                        var curNames = selectablePlateMap1.GetNamesOfWellsAssignedToGroup(i, out groupColor);
                        string GroupName="";
                        if (TreatmentTextBoxes[i] != null)
                        {
                            GroupName = TreatmentTextBoxes[i].Text;
                        }
                        if (GroupName == "")
                        {
                            GroupName = "Treatment: " + i.ToString();
                        }
                        foreach (string name in curNames)
                        {
                            if (curData.ContainsKey(name))
                            {
                                GrowthData GD = curData[name];
                                PointPairList XY;
                                if (chkTreatShowLog.Checked)
                                { XY = new PointPairList(GD.TimeValuesDecimal, GD.LogODValues); }
                                else
                                { XY = new PointPairList(GD.TimeValuesDecimal, GD.ODValues); }
                                Graph.AddCurve(GroupName, XY, groupColor, SymboltoUse);
                            }
                        }                       
                    }
                    if (chkTreatLegend.Checked)
                    { Graph.Legend.IsVisible = true; }
                    else { Graph.Legend.IsVisible = false; }
                    Graph.XAxis.Scale.MaxGrace = .05;
                    plotTreatments.AxisChange();
                    plotTreatments.Invalidate();
                    this.Cursor = Cursors.Default;
                }
                catch (Exception thrown)
                { MessageBox.Show("Could not make graph, talk to nigel.\n\nError is:\n" + thrown.Message, "Graph Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally { this.Cursor = Cursors.Default; }
        }
        void UpdateTreatmentGraphWithTreatmentData(GetValueForTreatment FunctionForData,string Title)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                GraphPane Graph = plotTreatments.GraphPane;
                Graph.CurveList.Clear();
                Graph.Title.Text = Title;
                Graph.XAxis.Title.Text = "Treatments";
                Graph.YAxis.Title.Text = "";
                Graph.Legend.Position = LegendPos.InsideTopLeft;
                Graph.Legend.FontSpec.Size = 8f;
                Graph.Legend.IsHStack = true;
                SymbolType SymboltoUse = SymbolType.Circle;
               
                Dictionary<string, GrowthData> curData = GetDictionaryOfGrowthRateData();
                for (int i = 1; i < SelectablePlateMap.MAX_GROUP_ASSIGNMENTS; i++)
                {
                    Color groupColor;
                    List<double> xVals = new List<double>();
                    var curNames = selectablePlateMap1.GetNamesOfWellsAssignedToGroup(i, out groupColor);
                    string GroupName = "";
                    if (TreatmentTextBoxes[i] != null)
                    {
                        GroupName = TreatmentTextBoxes[i].Text;
                    }
                    if (GroupName == "")
                    {
                        GroupName = "Treatment: " + i.ToString();
                    }
                    PointPairList XY=new PointPairList();
                    foreach (string name in curNames)
                    {
                        if (curData.ContainsKey(name))
                        {
                            GrowthData GD = curData[name];
                            XY.Add((double)i, FunctionForData(GD));
                        }
                    }
                    if (XY.Count > 0)
                    {
                        LineItem li=Graph.AddCurve(GroupName, XY, groupColor, SymboltoUse);
                        li.Line.IsVisible = false;
                        li.Symbol.Fill = new Fill(groupColor);                         
                    }                    
                }
                if (chkTreatLegend.Checked)
                { Graph.Legend.IsVisible = true; }
                else { Graph.Legend.IsVisible = false; }
                Graph.XAxis.Scale.MaxGrace = .05;
                plotTreatments.AxisChange();
                plotTreatments.Invalidate();
                this.Cursor = Cursors.Default;
            }
            catch (Exception thrown)
            { MessageBox.Show("Could not make graph, talk to nigel.\n\nError is:\n" + thrown.Message, "Graph Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally { this.Cursor = Cursors.Default; }
        }

    }

}