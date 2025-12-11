@echo off
echo ==========================================
echo      FSM Wearable Verilog Simulator
echo ==========================================

REM Set path to Icarus Verilog
set IVERILOG_PATH=D:\Program Files\iverilog\bin
set PATH=%IVERILOG_PATH%;%PATH%

echo.
echo [1/3] Compiling Verilog files...
iverilog -o fsm_sim tb_fsm_wearable.v fsm_wearable.v

if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] Compilation failed!
    pause
    exit /b %ERRORLEVEL%
)

echo [SUCCESS] Compilation complete.

echo.
echo [2/3] Running Simulation...
vvp fsm_sim

if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] Simulation failed!
    pause
    exit /b %ERRORLEVEL%
)

echo.
echo [3/3] Opening Waveform in GTKWave...
if exist fsm_waveform.vcd (
    start gtkwave fsm_waveform.vcd
) else (
    echo [ERROR] Waveform file 'fsm_waveform.vcd' not found!
)

echo.
echo Done.
pause
