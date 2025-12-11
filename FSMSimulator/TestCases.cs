namespace FSMSimulator
{
    public class TestCase
    {
        public string Name { get; set; } = "";
        public bool[] Inputs { get; set; } = new bool[6];
        public State ExpectedState { get; set; }
        public bool[] ExpectedOutputs { get; set; } = new bool[6];
    }

    public static class TestCases
    {
        public static List<TestCase> AllTests = new List<TestCase>
        {
            new TestCase 
            { 
                Name = "1. IDLE - All OFF", 
                Inputs = new bool[] { false, false, false, false, false, false },
                ExpectedState = State.IDLE,
                ExpectedOutputs = new bool[] { false, false, false, false, false, false }
            },
            new TestCase 
            { 
                Name = "2. IDLE - S1 Only", 
                Inputs = new bool[] { true, false, false, false, false, false },
                ExpectedState = State.IDLE,
                ExpectedOutputs = new bool[] { false, false, false, false, false, false }
            },
            new TestCase 
            { 
                Name = "3. LIGHT - S1 & S2", 
                Inputs = new bool[] { true, true, false, false, false, false },
                ExpectedState = State.LIGHT_DEHY,
                ExpectedOutputs = new bool[] { true, false, true, true, false, false } // A1, A3, A4
            },
            new TestCase 
            { 
                Name = "4. ACTIVITY - S6 Only", 
                Inputs = new bool[] { false, false, false, false, false, true },
                ExpectedState = State.ACTIVITY_ALERT,
                ExpectedOutputs = new bool[] { true, false, true, true, true, false } // A1, A3, A4, A5
            },
            new TestCase 
            { 
                Name = "5. SEVERE - S5 & S1", 
                Inputs = new bool[] { true, false, false, false, true, false },
                ExpectedState = State.SEVERE_DEHY,
                ExpectedOutputs = new bool[] { true, true, true, true, true, true } // All ON
            }
        };
    }
}
