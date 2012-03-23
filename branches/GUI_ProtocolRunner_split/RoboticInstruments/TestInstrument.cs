using System;

namespace Growth_Curve_Software
{
    public class TestInstrument : BaseInstrumentClass
    {
        bool[] table;
        public override string Name
        {
            get
            {
                return "TEST";
            }
        }
        public int Current = 2;

        public TestInstrument()
        {
            this.StatusOK = true;
            table = new bool[4];
        }
        [UserCallableMethod()]
        public bool MoveNext()
        {

            for (int i = Current + 1; i < table.Length; i++)
            {
                if (table[i])
                    if (i % Current == 0)
                    {
                        table[i] = false;
                    }
            }

            for (int i = Current + 1; i < table.Length; i++)
            {
                if (table[i])
                {
                    Current = i;
                    return true;
                }
            }
            return false;
        }
        
        [UserCallableMethod()]
        public override bool AttemptRecovery(InstrumentError Error)
        {
            this.StatusOK = true;
            return true;
        }

        public override bool CloseAndFreeUpResources()
        {
            StatusOK = false;
            return true;
        }

        public override void Initialize()
        {
            this.StatusOK = true;
            this.Initialize(1000);
        }

        public override void Initialize(int table_size)
        {
            table = new bool[table_size];
            for (int i = 2; i < table.Length; i++)
            {
                table[i] = true;
            }
        }
    }
}


