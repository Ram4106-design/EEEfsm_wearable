# Wearable Dehydration Detection FSM

A Finite State Machine (FSM) implementation for wearable dehydration detection systems, available in both C# (software simulation) and Verilog (hardware implementation).

## 📋 Overview

This project implements a priority-based FSM that monitors sensor inputs and controls actuators based on dehydration levels and activity states.

### States
- **IDLE** (00): Normal operation
- **LIGHT_DEHY** (01): Early dehydration detected
- **SEVERE_DEHY** (10): Critical dehydration detected
- **ACTIVITY_ALERT** (11): High activity detected

### Priority Hierarchy
1. **P1 (Highest)**: Severe Dehydration - S5=1 AND any of (S1 OR S2 OR S3 OR S4)=1
2. **P2**: Activity Alert - S6=1
3. **P3**: Light Dehydration - sum(S1..S5) >= 2
4. **P4 (Lowest)**: IDLE - Default state

## 🗂️ Project Structure

```
EEEfsm_wearable/
├── C#/                          # C# Software Implementation
│   ├── fsm_wearable.cs         # FSM class implementation
│   ├── Program.cs              # Test program with 4 test cases
│   ├── FSM_Wearable.exe        # Compiled executable
│   └── output.txt              # Sample output
│
└── Verilog/                     # Verilog Hardware Implementation
    ├── fsm_wearable.v          # Synthesizable FSM module
    ├── tb_fsm_wearable.v       # Comprehensive testbench (36 tests)
    ├── compile.bat             # Windows compilation script
    └── compile.ps1             # PowerShell compilation script
```

## 🚀 Getting Started

### C# Implementation

**Requirements:**
- .NET Framework 4.0 or higher (or .NET Core/5+)

**Compile:**
```bash
csc /out:FSM_Wearable.exe fsm_wearable.cs Program.cs
```

**Run:**
```bash
.\FSM_Wearable.exe
```

**Expected Output:**
```
Test Case 1: All sensors 0 (Expected: IDLE)
Inputs: [0, 0, 0, 0, 0, 0]
Current State: IDLE (0)
Actuator Outputs: [0, 0, 0, 0, 0, 0]
...
```

### Verilog Implementation

**Requirements:**
- Icarus Verilog (`iverilog` and `vvp`)

**Compile and Run:**
```bash
iverilog -o fsm_test.vvp fsm_wearable.v tb_fsm_wearable.v
vvp fsm_test.vvp
```

**View Waveforms:**
```bash
gtkwave fsm_waveform.vcd
```

## 📊 Input/Output Mapping

### Inputs (6 sensors)
| Sensor | Index | Description |
|--------|-------|-------------|
| S1 | 0 | Sensor 1 |
| S2 | 1 | Sensor 2 |
| S3 | 2 | Sensor 3 |
| S4 | 3 | Sensor 4 |
| S5 | 4 | Sensor 5 (Critical) |
| S6 | 5 | Activity Sensor |

### Outputs (6 actuators)
| State | A1 | A2 | A3 | A4 | A5 | A6 | Binary |
|-------|----|----|----|----|----|----|--------|
| IDLE | 0 | 0 | 0 | 0 | 0 | 0 | 000000 |
| LIGHT_DEHY | 1 | 0 | 1 | 1 | 0 | 0 | 101100 |
| SEVERE_DEHY | 1 | 1 | 1 | 1 | 1 | 1 | 111111 |
| ACTIVITY_ALERT | 1 | 0 | 1 | 1 | 1 | 0 | 101110 |

## 🔧 Hardware Implementation (EasyEDA)

For hardware implementation on FPGA/CPLD, refer to the Verilog implementation. The design includes:
- 2-stage synchronizer for metastability prevention
- Combinational next-state logic
- Moore-style output encoding
- Active-low reset

**Recommended Chips:**
- Xilinx XC9572XL (CPLD)
- Lattice iCE40 (FPGA)

## ✅ Testing

### C# Tests
- 4 basic test cases covering all states

### Verilog Tests
- 36 comprehensive test cases including:
  - IDLE state tests
  - Light dehydration tests
  - Activity alert tests
  - Severe dehydration tests
  - Priority hierarchy verification
  - Edge cases
  - Reset functionality
  - Synchronizer verification
  - Truth table samples

## 📝 Features

- ✅ Priority-based state transitions
- ✅ 2-stage input synchronization (Verilog)
- ✅ Moore FSM architecture
- ✅ Comprehensive test coverage
- ✅ Hardware-ready Verilog implementation
- ✅ Software simulation in C#

## 📄 License

This project is open source and available for educational and research purposes.

## 👥 Contributors

- Ram4106-design

---

**Note:** This FSM is designed for wearable health monitoring applications. Ensure proper sensor calibration and medical validation before deployment in real-world scenarios.
