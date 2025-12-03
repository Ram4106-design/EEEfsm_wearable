using System;
using WearableDehydrationDetection;

namespace WearableDehydrationDetection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Wearable Dehydration Detection FSM Simulation");
            Console.WriteLine("=============================================");

            FSM_Wearable fsm = new FSM_Wearable();
            
            // Test Case 1: IDLE (All sensors 0)
            Console.WriteLine("\nTest Case 1: All sensors 0 (Expected: IDLE)");
            int[] inputs1 = { 0, 0, 0, 0, 0, 0 };
            RunTest(fsm, inputs1);

            // Test Case 2: LIGHT_DEHY (S1=1, S2=1 -> sum >= 2)
            Console.WriteLine("\nTest Case 2: S1=1, S2=1 (Expected: LIGHT_DEHY)");
            int[] inputs2 = { 1, 1, 0, 0, 0, 0 };
            RunTest(fsm, inputs2);

            // Test Case 3: SEVERE_DEHY (S5=1, S1=1 -> P1 condition)
            Console.WriteLine("\nTest Case 3: S5=1, S1=1 (Expected: SEVERE_DEHY)");
            int[] inputs3 = { 1, 0, 0, 0, 1, 0 };
            RunTest(fsm, inputs3);

            // Test Case 4: ACTIVITY_ALERT (S6=1 -> P2 condition)
            Console.WriteLine("\nTest Case 4: S6=1 (Expected: ACTIVITY_ALERT)");
            int[] inputs4 = { 0, 0, 0, 0, 0, 1 };
            RunTest(fsm, inputs4);
        }

        static void RunTest(FSM_Wearable fsm, int[] inputs)
        {
            Console.WriteLine("Inputs: [" + string.Join(", ", inputs) + "]");
            int[] outputs = fsm.ProcessSensorData(inputs);
            Console.WriteLine("Current State: " + fsm.GetCurrentStateName() + " (" + fsm.GetCurrentStateCode() + ")");
            Console.WriteLine("Actuator Outputs: [" + string.Join(", ", outputs) + "]");
        }
    }
}
