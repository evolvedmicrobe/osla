using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;

namespace Fit_Growth_Curves
{
    partial class FitterForm 
    {
        int[] TonyCheckerBoard = new int[30];
        int[] NigelSingleSampleBoard = new int[60];
        private void tabBlankRemoval_Paint(object sender, PaintEventArgs e)
        {
            ResetPlateDeletionProtocol();
            //first to fill the instruction rtf box
            txtDeleteBlanksTab.Rtf = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fswiss\fcharset0 Arial;}}{\*\generator Msftedit 5.41.21.2508;}\viewkind4\uc1\pard\qc\b\f0\fs20 About This Tab\par\par\pard\tab\b0 Often times when we collect data from plates used with the robots, many of the wells are left blank as controls.  The particular layout any individual uses can vary though.  After loading 96 well plate data, you can come to this tab to delete the blanks if the layout you have used in your plates matches one of the layouts shown here, simply click the appropriate button under the layout of choice.  Red indicates sample wells, while white indicates blanks.\par}";
            //now to make the appropriate blank layouts 
            try
            {
                if (lstGrowthCurves.Items.Count == 96 || lstGrowthCurves.Items.Count == 48)
                {
                    Plate_Tools PT;
                    if (!Is_48WellDataLoaded)
                    {
                        TonyCheckerBoard = new int[30];
                        NigelSingleSampleBoard = new int[60];
                        PT = new Plate_Tools(this, false);
                        //First to make Tony's Checkerboard
                        PT = new Plate_Tools(this, 20, true, false);
                        int StartPos = PT.CellNameToInts["B2"];//the outside is left empty
                        int BottomPos = PT.CellNameToInts["G11"];
                        int[] PositionsinCheckerboard = PT.ReturnIndexesInRectangle(StartPos, BottomPos);
                        double[] Data = new double[96];//this will hold a 0 if blank, 1 otherwise
                        int TonyCheckerBoardPosCounter = 0;
                        foreach (int z in PositionsinCheckerboard)
                        {
                            if (((z / 12) % 2) == 0)  //row A,C,D,F,H
                            {
                                if (z % 2 == 0)
                                {
                                    Data[z] = 1;
                                    TonyCheckerBoard[TonyCheckerBoardPosCounter] = z;
                                    TonyCheckerBoardPosCounter++;
                                }
                            }
                            else
                            {
                                if (z % 2 == 1)
                                {
                                    Data[z] = 1;
                                    TonyCheckerBoard[TonyCheckerBoardPosCounter] = z;
                                    TonyCheckerBoardPosCounter++;
                                }
                            }
                        }
                        //now to make the color map
                        PT.makeSquares(Data);
                        Graphics graphicsObj = e.Graphics;
                        //graphicsObj.DrawImage(PT.TheColoredSquaresBM, tabBlankRemoval.Left + 30, tabBlankRemoval.Top + 30, PT.TheColoredSquaresBM.Width, PT.TheColoredSquaresBM.Height);
                        graphicsObj.DrawImage(PT.TheColoredSquaresBM, btnRemoveTonyCheckerboard.Location.X, tabBlankRemoval.Top + 30, PT.TheColoredSquaresBM.Width, PT.TheColoredSquaresBM.Height);
                        //Now to make Nigel's Checkerboard, believe this is just whole glob in middle
                        Data = new double[96];
                        NigelSingleSampleBoard = PT.ReturnIndexesInRectangle(StartPos, BottomPos);
                        foreach (int z in NigelSingleSampleBoard)
                        {
                            Data[z] = 1;
                        }
                        PT.makeSquares(Data);
                        //graphicsObj.DrawImage(PT.TheColoredSquaresBM, tabBlankRemoval.Left + 400, tabBlankRemoval.Top + 30, PT.TheColoredSquaresBM.Width, PT.TheColoredSquaresBM.Height);
                        graphicsObj.DrawImage(PT.TheColoredSquaresBM, btnRemoveNigelBlanks.Location.X, tabBlankRemoval.Top + 30, PT.TheColoredSquaresBM.Width, PT.TheColoredSquaresBM.Height);
                    }
                    else
                    {
                        //First to make Tony's Checkerboard
                        PT = new Plate_Tools(this, 20, true, true);
                        TonyCheckerBoard = new int[12];
                        NigelSingleSampleBoard = new int[24];
                        int StartPos = PT.CellNameToInts["B2"];//the outside is left empty
                        int BottomPos = PT.CellNameToInts["E7"];
                        int[] PositionsinCheckerboard = PT.ReturnIndexesInRectangle(StartPos, BottomPos);
                        double[] Data = new double[48];//this will hold a 0 if blank, 1 otherwise
                        int TonyCheckerBoardPosCounter = 0;
                        foreach (int z in PositionsinCheckerboard)
                        {
                            if (((z / 8) % 2) == 0)  //row A,C,D,F,H
                            {
                                if (z % 2 == 0)
                                {
                                    Data[z] = 1;
                                    TonyCheckerBoard[TonyCheckerBoardPosCounter] = z;
                                    TonyCheckerBoardPosCounter++;
                                }
                            }
                            else
                            {
                                if (z % 2 == 1)
                                {
                                    Data[z] = 1;
                                    TonyCheckerBoard[TonyCheckerBoardPosCounter] = z;
                                    TonyCheckerBoardPosCounter++;
                                }
                            }
                        }
                        //now to make the color map
                        PT.makeSquares(Data);
                        Graphics graphicsObj = e.Graphics;
                        //graphicsObj.DrawImage(PT.TheColoredSquaresBM, tabBlankRemoval.Left + 30, tabBlankRemoval.Top + 30, PT.TheColoredSquaresBM.Width, PT.TheColoredSquaresBM.Height);
                        graphicsObj.DrawImage(PT.TheColoredSquaresBM, btnRemoveTonyCheckerboard.Location.X, tabBlankRemoval.Top + 30, PT.TheColoredSquaresBM.Width, PT.TheColoredSquaresBM.Height);
                        //Now to make Nigel's Checkerboard, believe this is just whole glob in middle
                        Data = new double[48];
                        NigelSingleSampleBoard = PT.ReturnIndexesInRectangle(StartPos, BottomPos);
                        foreach (int z in NigelSingleSampleBoard)
                        {
                            Data[z] = 1;
                        }
                        PT.makeSquares(Data);
                        //graphicsObj.DrawImage(PT.TheColoredSquaresBM, tabBlankRemoval.Left + 400, tabBlankRemoval.Top + 30, PT.TheColoredSquaresBM.Width, PT.TheColoredSquaresBM.Height);
                        graphicsObj.DrawImage(PT.TheColoredSquaresBM, btnRemoveNigelBlanks.Location.X, tabBlankRemoval.Top + 30, PT.TheColoredSquaresBM.Width, PT.TheColoredSquaresBM.Height);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Could not layout blanks", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ResetPlateDeletionProtocol()
        {
                toDeletePlateMap.ClearAllAssignments(false);
                foreach (GrowthData gd in lstGrowthCurves.Items.Cast<GrowthData>())
                {
                    string Name = gd.ToString();
                    if (toDeletePlateMap.isValidWellName(Name))
                    {
                        toDeletePlateMap.AssignWellToGroup(Name, 1);
                    }
                }
                toDeletePlateMap.RecreateImage();
        }
        void toDeletePlateMap_IndividualWellChanged(object sender, EventArgs e)
        {
            SelectablePlateEventArgs ev = (SelectablePlateEventArgs)e;
            if (ev.GroupAssignedTo == 0)
            {
                string WellID=ev.WellID;
                for (int index = 0; index < lstGrowthCurves.Items.Count; index++)
                {
                    object o = lstGrowthCurves.Items[index];
                    if (((GrowthData)o).ToString() == WellID)
                    {
                        DeleteCurve(index);
                        toDeletePlateMap.RecreateImage();
                        break;
                    }
                }
            }
        }
        private void DeleteBlanks(int[] SampleIndexes)
        {     
            if (lstGrowthCurves.Items.Count == 96 || lstGrowthCurves.Items.Count==48)
            {
                Plate_Tools PT;
                if (Is_48WellDataLoaded)
                {
                    PT = new Plate_Tools(this, true);
                }
                else
                {
                    PT = new Plate_Tools(this, false);
                }
              int[] toDelete = PT.GetInverseSquares(SampleIndexes);
              for (int i = toDelete.Length - 1; i > -1; i--)
                {
                    //MessageBox.Show(lstGrowthCurves.Items.Count.ToString()+" "+BlankIndexes[i].ToString());
                    lstGrowthCurves.Items.RemoveAt(toDelete[i]);
                    lstGrowthCurvesMirror.Items.RemoveAt(toDelete[i]);
                    lstMultiplePlots.Items.RemoveAt(toDelete[i]);
                }
            }
            else
            {
                MessageBox.Show("Could not delete the blanks.\nYou do not have 96 or 48 entries, are you working with plate data?");
            }           
        }
        private void AvgFirstDataPointsSubtractAndDeleteBlanks(int[] SampleIndexes)
        {
            if (lstGrowthCurves.Items.Count == 96 || lstGrowthCurves.Items.Count == 48)
            {
                Plate_Tools PT;
                if (Is_48WellDataLoaded)
                {
                    PT = new Plate_Tools(this, true);
                }
                else
                {
                    PT = new Plate_Tools(this, false);
                }
                int[] BlankIndexes = PT.GetInverseSquares(SampleIndexes);
                double BlankSum = 0;
                double BlankCount =(double) BlankIndexes.Length;
                foreach (int i in BlankIndexes)
                {
                    GrowthData GD = (GrowthData)lstGrowthCurves.Items[i];
                    BlankSum += GD.ODValues[0];//Lets hope that these are in the right order, should be!
                }
                double Blank = BlankSum / BlankCount;
                SubtractBlankFromAll(Blank);
                lblBlankInfo.Text = "Blank Used Was: " + Blank.ToString();
                DeleteBlanks(SampleIndexes);
            }
            else
            {
                MessageBox.Show("Could not delete the blanks.\nYou do not have 96 entries, are you working with plate data?");
            }
        }
        private void AvgAllDataPointsSubtractAndDeleteBlanks(int[] SampleIndexes)
        {
            if (lstGrowthCurves.Items.Count == 96 || lstGrowthCurves.Items.Count == 48)
            {
                Plate_Tools PT;
                if (Is_48WellDataLoaded)
                {
                    PT = new Plate_Tools(this, true);
                }
                else
                {
                    PT = new Plate_Tools(this, false);
                }
                int[] BlankIndexes = PT.GetInverseSquares(SampleIndexes);
                double BlankSum = 0;
                double BlankCount = 0;
                foreach (int i in BlankIndexes)
                {
                    GrowthData GD = (GrowthData)lstGrowthCurves.Items[i];
                    foreach (double dbl in GD.ODValues)
                    {
                        BlankSum += dbl;
                        BlankCount+=1;
                    }
                }
                double Blank = BlankSum / BlankCount;
                SubtractBlankFromAll(Blank);
                lblBlankInfo.Text = "Blank Used Was: " + Blank.ToString();
                DeleteBlanks(SampleIndexes);
            }
            else
            {
                MessageBox.Show("Could not delete the blanks.\nYou do not have 96 entries, are you working with plate data?");
            }
        }
        private void AvgAllBlanksAndSubtractAtEachTimePoint(int[] SampleIndexes)
        {
            if (lstGrowthCurves.Items.Count == 96 || lstGrowthCurves.Items.Count == 48)
            {
                Plate_Tools PT;
                if (Is_48WellDataLoaded)
                {
                    PT = new Plate_Tools(this, true);
                }
                else
                {
                    PT = new Plate_Tools(this, false);
                }
                int NumberOfReads=((GrowthData)lstGrowthCurves.Items[0]).ODValues.Length;
                double[] BlanksAtTimePoints = new double[NumberOfReads];
                int[] BlankIndexes = PT.GetInverseSquares(SampleIndexes);
                for (int read = 0; read < NumberOfReads; read++)
                {
                    double BlankSum = 0;
                    double BlankCount = (double)BlankIndexes.Length;
                    foreach (int i in BlankIndexes)
                    {                        
                        GrowthData GD = (GrowthData)lstGrowthCurves.Items[i];
                        BlankSum += GD.ODValues[read];                        
                    }
                    BlanksAtTimePoints[read] = BlankSum / BlankCount;
                }
                SubtractTimeSeriesBlankFromAll(BlanksAtTimePoints);
                lblBlankInfo.Text = "Blank used varied by time point";
                DeleteBlanks(SampleIndexes);
            }
            else
            {
                MessageBox.Show("Could not delete the blanks.\nYou do not have 96 entries, are you working with plate data?");
            }
        }

        private void btnRemoveTonyCheckerboard_Click(object sender, EventArgs e)
        { 
            this.Cursor=Cursors.WaitCursor;
            DeleteBlanks(TonyCheckerBoard);
            this.Cursor=Cursors.Default;
        }
        private void btnAvgDataPoint1AndDeleteBlanks_Click(object sender, EventArgs e)
        {
            //this method will average the first datapoint, subtract it from them all,
            //and then delete the blanks
            this.Cursor = Cursors.WaitCursor;
            AvgFirstDataPointsSubtractAndDeleteBlanks(TonyCheckerBoard);
            this.Cursor = Cursors.Default;
        }
        private void btnAvgAllAndDeleteTony_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            AvgAllDataPointsSubtractAndDeleteBlanks(TonyCheckerBoard);
            this.Cursor = Cursors.Default;
        }
        private void btnRemoveNigelBlanks_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            DeleteBlanks(NigelSingleSampleBoard);
            this.Cursor = Cursors.Default;
        }
        private void btnAvgFirstRemoveNigelBlanks_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            AvgFirstDataPointsSubtractAndDeleteBlanks(NigelSingleSampleBoard);
            this.Cursor = Cursors.Default;
        }
        private void btnAvgAllRemoveNigelBlanks_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            AvgAllDataPointsSubtractAndDeleteBlanks(NigelSingleSampleBoard);
            this.Cursor = Cursors.Default;
        }
        private void btnAvgTimeSeries_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            AvgAllBlanksAndSubtractAtEachTimePoint(TonyCheckerBoard);
            this.Cursor = Cursors.Default;

        }
        private void btnAvgtimePointsDeleteNigel_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            AvgAllBlanksAndSubtractAtEachTimePoint(NigelSingleSampleBoard);
            this.Cursor = Cursors.Default;
        }
    }
}
