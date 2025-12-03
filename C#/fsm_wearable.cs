	using System;
		
		namespace WearableDehydrationDetection
		{
			/// <summary>
			/// Finite State Machine for Wearable Dehydration Detection System
			/// </summary>
			public class FSM_Wearable
			{
				// ============================================
				// STATE DEFINITIONS
				// ============================================
				private enum State
				{
					IDLE = 0,           // 00: Normal operation
					LIGHT_DEHY = 1,     // 01: Early dehydration
					SEVERE_DEHY = 2,    // 10: Critical dehydration
					ACTIVITY_ALERT = 3  // 11: High activity
				}
				
				// Current FSM state
				private State currentState;
				
				// 2-stage synchronizer buffers (for metastability prevention)
				private int[] syncBuffer1 = new int[6];
				private int[] syncBuffer2 = new int[6];
				
				// Sampling rate (100 ms)
				private const int SAMPLING_RATE_MS = 100;
				
				// ============================================
				// CONSTRUCTOR
				// ============================================
				public FSM_Wearable()
				{
					currentState = State.IDLE;
					ResetSynchronizer();
				}
				
				// ============================================
				// SYNCHRONIZER RESET
				// ============================================
				private void ResetSynchronizer()
				{
					for(int i=0; i<6; i++) syncBuffer1[i] = 0;
					for(int i=0; i<6; i++) syncBuffer2[i] = 0;
				}
				
				// ============================================
				// 2-STAGE INPUT SYNCHRONIZER
				// ============================================
				private int[] SynchronizeInputs(int[] sensorInputs)
				{
					// Stage 1: First D flip-flop
					for (int i = 0; i < 6; i++)
					syncBuffer1[i] = sensorInputs[i];
					
					// Stage 2: Second D flip-flop
					for (int i = 0; i < 6; i++)
					syncBuffer2[i] = syncBuffer1[i];
					
					return syncBuffer2;
				}
				
				// ============================================
				// COUNT ACTIVE SENSORS (for P3 priority)
				// ============================================
				private int CountActiveSensors(int[] syncedInputs)
				{
					int count = 0;
					for (int i = 0; i < 5; i++) // S1-S5 only
					count += syncedInputs[i];
					return count;
				}
				
				// ============================================
				// PRIORITY DETECTION LOGIC
				// ============================================
				private bool DetectPriorityP1(int[] syncedInputs)
				{
					// P1: S5=1 AND any of (S1 OR S2 OR S3 OR S4)=1
					return syncedInputs[4] == 1 && 
					(syncedInputs[0] == 1 || syncedInputs[1] == 1 || 
					syncedInputs[2] == 1 || syncedInputs[3] == 1);
				}
				
				private bool DetectPriorityP2(int[] syncedInputs)
				{
					// P2: S6=1
					return syncedInputs[5] == 1;
				}
				
				private bool DetectPriorityP3(int[] syncedInputs)
				{
					// P3: sum(S1..S5) >= 2
					return CountActiveSensors(syncedInputs) >= 2;
				}
				
				// ============================================
				// NEXT STATE LOGIC (Combinational)
				// ============================================
				private State GetNextState(int[] syncedInputs)
				{
					// Strict priority hierarchy: P1 > P2 > P3 > P4
					if (DetectPriorityP1(syncedInputs))
					return State.SEVERE_DEHY;      // Priority 1
					
					else if (DetectPriorityP2(syncedInputs))
					return State.ACTIVITY_ALERT;   // Priority 2
					
					else if (DetectPriorityP3(syncedInputs))
					return State.LIGHT_DEHY;       // Priority 3
					
					else
					return State.IDLE;             // Priority 4 (default)
				}
				
				// ============================================
				// MOORE OUTPUT ENCODER
				// ============================================
				private int[] GetActuatorOutputs(State state)
				{
					int[] outputs = new int[6]; // A1-A6
					
					switch (state)
					{
						case State.IDLE:
						for(int i=0; i<6; i++) outputs[i] = 0;        // 000000
						break;
						
						case State.LIGHT_DEHY:
						outputs = new int[] {1, 0, 1, 1, 0, 0}; // 101100
						break;
						
						case State.SEVERE_DEHY:
						for(int i=0; i<6; i++) outputs[i] = 1;        // 111111
						break;
						
						case State.ACTIVITY_ALERT:
						outputs = new int[] {1, 0, 1, 1, 1, 0}; // 101110
						break;
						
						default:
						for(int i=0; i<6; i++) outputs[i] = 0;        // Safe default
						break;
					}
					
					return outputs;
				}
				
				// ============================================
				// MAIN PROCESSING FUNCTION (called every 100ms)
				// ============================================
				public int[] ProcessSensorData(int[] sensorInputs)
				{
					// Step 1: Synchronize inputs (2-stage)
					int[] syncedInputs = SynchronizeInputs(sensorInputs);
					
					// Step 2: Determine next state
					State nextState = GetNextState(syncedInputs);
					
					// Step 3: Update state register
					currentState = nextState;
					
					// Step 4: Generate actuator outputs (Moore style)
					int[] actuatorOutputs = GetActuatorOutputs(currentState);
					
					return actuatorOutputs;
				}
				
				// ============================================
				// MONITORING FUNCTIONS
				// ============================================
				public string GetCurrentStateName()
				{
					return currentState.ToString();
				}
				
				public int GetCurrentStateCode()
				{
					return (int)currentState;
				}
			}
		}
