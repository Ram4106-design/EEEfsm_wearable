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
├── FSMSimulator/                # [NEW] C# Windows Forms Simulator
│   ├── FSMSimulator.csproj     # Project file (Open in Visual Studio)
│   ├── SimulatorForm.cs        # Main UI Implementation
│   ├── FSM_Wearable.cs         # Core FSM Logic
│   ├── Controls/               # Custom UI Controls (LEDs, Waveform)
│   └── TestCases.cs            # Automated Test Scenarios
│
├── C#/                          # Legacy Console Implementation
│   ├── fsm_wearable.cs         
│   └── Program.cs              
│
└── Verilog/                     # Verilog Hardware Implementation
    ├── fsm_wearable.v          # Synthesizable FSM module
    ├── tb_fsm_wearable.v       # Comprehensive testbench (36 tests)
    ├── compile.bat             # Windows compilation script
    └── compile.ps1             # PowerShell compilation script
```

## 🚀 Getting Started

### 🖥️ C# Visual Simulator (Recommended)

**Features:**
- Interactive Sensor Switches (S1-S6)
- Real-time State Diagram Visualization
- Live Waveform View
- Automated Test Runner

**How to Run:**
1. Open `FSMSimulator/FSMSimulator.csproj` in **Visual Studio 2022** (or newer).
2. Press **Start** (F5) to build and run.
3. *Alternatively*, if you have .NET SDK installed:
   ```bash
   cd FSMSimulator
   dotnet run
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

### C# Simulator Tests
- **Interactive Mode**: Manually toggle sensors and observe state changes.
- **Automated Mode**: Select from 5 predefined test cases (matching Verilog) and run them instantly.

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
- ✅ Software simulation in C# (Windows Forms GUI)
- ✅ Visual State Diagram & Waveforms

## 📄 License

This project is open source and available for educational and research purposes.

## 👥 Contributors

- Ram4106-design

---

**Note:** This FSM is designed for wearable health monitoring applications. Ensure proper sensor calibration and medical validation before deployment in real-world scenarios.
