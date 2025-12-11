using System;
using System.Linq;

namespace FSMSimulator
{
    public enum State
    {
        IDLE = 0,
        LIGHT_DEHY = 1,
        SEVERE_DEHY = 2,
        ACTIVITY_ALERT = 3
    }

    public class FSM_Wearable
    {
        // Inputs
        public bool[] SensorInputs { get; private set; } = new bool[6]; // S1-S6 (indices 0-5)
        public bool Reset_n { get; set; } = true;

        // Outputs
        public bool[] ActuatorOutputs { get; private set; } = new bool[6]; // A1-A6
        public State CurrentState { get; private set; }
        public State NextState { get; private set; }

        // Internal State
        private int[] _syncBuffer1 = new int[6];
        private int[] _syncBuffer2 = new int[6]; // Synced inputs

        public FSM_Wearable()
        {
            Reset();
        }

        public void SetSensor(int index, bool value)
        {
            if (index >= 0 && index < 6)
            {
                SensorInputs[index] = value;
            }
        }

        public void Reset()
        {
            CurrentState = State.IDLE;
            NextState = State.IDLE;
            Array.Clear(_syncBuffer1, 0, _syncBuffer1.Length);
            Array.Clear(_syncBuffer2, 0, _syncBuffer2.Length);
            Array.Clear(ActuatorOutputs, 0, ActuatorOutputs.Length);
        }

        public void ClockTick()
        {
            // 1. Synchronizer Logic
            if (!Reset_n)
            {
                Array.Clear(_syncBuffer1, 0, _syncBuffer1.Length);
                Array.Clear(_syncBuffer2, 0, _syncBuffer2.Length);
                CurrentState = State.IDLE;
            }
            else
            {
                // Shift inputs through synchronizer
                for (int i = 0; i < 6; i++)
                {
                    _syncBuffer2[i] = _syncBuffer1[i];
                    _syncBuffer1[i] = SensorInputs[i] ? 1 : 0;
                }
                
                // Update State
                CurrentState = NextState;
            }

            // 2. Combinational Logic (Next State & Output)
            EvaluateCombinationalLogic();
        }

        private void EvaluateCombinationalLogic()
        {
            // Priority Detection Logic based on SyncBuffer2
            bool p1_detect = false;
            bool p2_detect = false;
            bool p3_detect = false;

            // P1: S5=1 AND any of (S1 OR S2 OR S3 OR S4)=1
            // Indices: S1=0, S2=1, S3=2, S4=3, S5=4, S6=5
            bool s5 = _syncBuffer2[4] == 1;
            bool any_s1_s4 = (_syncBuffer2[0] == 1) || (_syncBuffer2[1] == 1) || (_syncBuffer2[2] == 1) || (_syncBuffer2[3] == 1);
            p1_detect = s5 && any_s1_s4;

            // P2: S6=1
            p2_detect = _syncBuffer2[5] == 1;

            // P3: sum(S1..S5) >= 2
            int active_sensor_count = 0;
            for (int i = 0; i < 5; i++)
            {
                if (_syncBuffer2[i] == 1) active_sensor_count++;
            }
            p3_detect = active_sensor_count >= 2;

            // Next State Logic
            if (p1_detect)
                NextState = State.SEVERE_DEHY;
            else if (p2_detect)
                NextState = State.ACTIVITY_ALERT;
            else if (p3_detect)
                NextState = State.LIGHT_DEHY;
            else
                NextState = State.IDLE;

            // Output Logic (Moore)
            // Reset outputs
            Array.Clear(ActuatorOutputs, 0, ActuatorOutputs.Length);

            switch (CurrentState)
            {
                case State.IDLE:
                    // 000000
                    break;
                case State.LIGHT_DEHY:
                    // 101100 -> A1, A3, A4 ON (Indices 0, 2, 3)
                    // Verilog: 6'b101100 -> MSB is A6? No, usually [5:0] means A6..A1 or A1..A6?
                    // Let's check Verilog usage:
                    // LIGHT_DEHY: actuator_outputs = 6'b101100; // A1, A3, A4 ON
                    // If [5:0], bit 0 is LSB.
                    // Usually 6'b101100 means bit 5=1, 4=0, 3=1, 2=1, 1=0, 0=0.
                    // If A1 is LSB (bit 0), then A1=0.
                    // But comment says "A1, A3, A4 ON".
                    // If A1 is MSB (bit 5), then A1=1.
                    // Let's assume standard Verilog bit order: [5] is MSB, [0] is LSB.
                    // 6'b101100:
                    // [5]=1, [4]=0, [3]=1, [2]=1, [1]=0, [0]=0
                    // If mapping is A6, A5, A4, A3, A2, A1
                    // Then A6=1, A4=1, A3=1.
                    // But comment says "A1, A3, A4".
                    // This implies the mapping might be reversed or specific.
                    // Let's look at SEVERE: 6'b111111 -> All ON. Consistent.
                    // Let's look at ACTIVITY: 6'b101110 -> A1, A3, A4, A5 ON.
                    // [5]=1, [4]=0, [3]=1, [2]=1, [1]=1, [0]=0
                    // If A1..A6 mapping:
                    // If A1 is bit 5? 1. A3 is bit 3? 1. A4 is bit 2? 1. A5 is bit 1? 1.
                    // That matches "A1, A3, A4, A5".
                    // So Mapping is:
                    // Bit 5 = A1
                    // Bit 4 = A2
                    // Bit 3 = A3
                    // Bit 2 = A4
                    // Bit 1 = A5
                    // Bit 0 = A6
                    // Let's re-verify LIGHT: 6'b101100
                    // Bit 5 (A1) = 1. OK.
                    // Bit 4 (A2) = 0. OK.
                    // Bit 3 (A3) = 1. OK.
                    // Bit 2 (A4) = 1. OK.
                    // Bit 1 (A5) = 0. OK.
                    // Bit 0 (A6) = 0. OK.
                    // Matches "A1, A3, A4 ON".
                    
                    // So mapping: Index 0 = A1, Index 1 = A2 ... Index 5 = A6.
                    // ActuatorOutputs[0] corresponds to A1 (Bit 5)
                    ActuatorOutputs[0] = true; // A1
                    ActuatorOutputs[2] = true; // A3
                    ActuatorOutputs[3] = true; // A4
                    break;

                case State.SEVERE_DEHY:
                    // 111111 -> All ON
                    for(int i=0; i<6; i++) ActuatorOutputs[i] = true;
                    break;

                case State.ACTIVITY_ALERT:
                    // 101110 -> A1, A3, A4, A5 ON
                    ActuatorOutputs[0] = true; // A1
                    ActuatorOutputs[2] = true; // A3
                    ActuatorOutputs[3] = true; // A4
                    ActuatorOutputs[4] = true; // A5
                    break;
            }
        }
    }
}
