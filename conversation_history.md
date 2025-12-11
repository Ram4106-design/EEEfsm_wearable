# Chat Session History: FSM Simulator Setup & Debugging
**Date:** December 11, 2025

## Summary of Activities

1.  **Initial Setup & SDK Installation**
    *   Attempted to run the FSM Simulator but found no system-wide .NET SDK.
    *   Downloaded and installed .NET 8.0 SDK locally in the project folder (`.dotnet/`).
    *   Verified installation (Version 8.0.416).

2.  **Debugging & Fixes**
    *   **Crash 1 (Startup):** Fixed `NullReferenceException` in `SimulatorForm.Designer.cs` caused by missing initialization of `cboTests` and `btnRunTest`.
    *   **Crash 2 (Visuals):** Fixed `ArgumentException` in `StateNode.cs` by enabling `SupportsTransparentBackColor` style for transparent controls.

3.  **Cleanup**
    *   Identified and deleted unused/temporary files:
        *   `FSMSimulator/dotnet-install.ps1`
        *   `FSMSimulator/output.txt`
        *   `C#/FSM_Wearable.exe`
        *   `Verilog/fsm_sim`, `*.vvp`, `*.vcd`, `*.log`
    *   Cleaned up git repository by removing accidentally committed `bin` and `obj` folders.
    *   Added `.gitignore` to prevent future clutter.

4.  **Documentation**
    *   Updated `README.md` with specific instructions for running:
        *   **C# Simulator**: Using Visual Studio or local `dotnet run`.
        *   **Verilog Simulation**: Using the `compile.ps1` script for Windows.
    *   Created `walkthrough.md` with detailed steps.

5.  **Version Control**
    *   Pushed all changes, fixes, and documentation to GitHub.
