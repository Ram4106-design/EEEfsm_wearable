# PowerShell script to compile Verilog using a virtual drive workaround
# This avoids the space-in-path issue by creating a temporary drive mapping

Write-Host "Setting up virtual drive to avoid path spaces..." -ForegroundColor Cyan

# Create a temporary drive letter mapping (if not already exists)
if (-not (Test-Path "I:\")) {
    subst I: "D:\Program Files\iverilog"
    Write-Host "Created virtual drive I: -> D:\Program Files\iverilog" -ForegroundColor Green
}

# Temporarily add the virtual drive bin to PATH
$env:PATH = "I:\bin;$env:PATH"

# Compile using the virtual drive path
Write-Host "Compiling Verilog files..." -ForegroundColor Cyan
& "I:\bin\iverilog.exe" -o fsm_sim tb_fsm_wearable.v fsm_wearable.v

if ($LASTEXITCODE -eq 0) {
    Write-Host "`nCompilation successful! Output: fsm_sim" -ForegroundColor Green
    Write-Host "Run simulation with: vvp fsm_sim" -ForegroundColor Cyan
    Write-Host "View waveform with: gtkwave fsm.vcd" -ForegroundColor Cyan
} else {
    Write-Host "`nCompilation failed!" -ForegroundColor Red
}

# Note: The subst mapping persists until reboot or manual removal
# To remove: subst I: /D
