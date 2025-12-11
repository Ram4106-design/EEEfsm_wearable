`timescale 1ns/1ps

module tb_fsm_wearable;

    //==========================================================
    // TESTBENCH SIGNALS
    //==========================================================
    reg clk;
    reg reset_n;
    reg [5:0] S_input;
    wire [5:0] A_output;
    wire [1:0] state_out;
    
    // Test tracking
    integer test_count;
    integer pass_count;
    integer fail_count;
    reg [80:1] state_name;
    
    //==========================================================
    // INSTANTIATE DUT (Device Under Test)
    //==========================================================
    fsm_wearable uut (
        .clk(clk),
        .rst_n(reset_n),
        .sensor_inputs(S_input),
        .actuator_outputs(A_output),
        .state_code(state_out)
    );

     //==========================================================
    // VCD WAVEFORM DUMP
    //==========================================================
    initial begin
        // Create VCD file for waveform viewing
        $dumpfile("fsm_waveform.vcd");
        $dumpvars(0, tb_fsm_wearable);  // Level 0 = dump all signals
    end
    
    //==========================================================
    // CLOCK GENERATION (100 MHz = 10ns period)
    //==========================================================
    initial begin
        clk = 0;
        forever #5 clk = ~clk;  // Toggle every 5ns
    end
    
    //==========================================================
    // TASK: CHECK OUTPUT
    //==========================================================
    task check_output;
        input [5:0] expected;
        input [8*50:1] test_name;
        begin
            test_count = test_count + 1;
            #50;  // Wait for output to stabilize (5 clock cycles)
            
            if (A_output === expected) begin
                $display("[PASS] Test %0d: %s", test_count, test_name);
                $display("       Input: S=%b, Output: A=%b (Expected: %b)", 
                         S_input, A_output, expected);
                pass_count = pass_count + 1;
            end else begin
                $display("[FAIL] Test %0d: %s", test_count, test_name);
                $display("       Input: S=%b, Output: A=%b (Expected: %b)", 
                         S_input, A_output, expected);
                fail_count = fail_count + 1;
            end
            $display("-----------------------------------------------");
        end
    endtask
    
    //==========================================================
    // TASK: APPLY RESET
    //==========================================================
    task apply_reset;
        begin
            $display("\n[INFO] Applying reset...");
            reset_n = 0;
            S_input = 6'b000000;
            #30;
            reset_n = 1;
            #20;
            $display("[INFO] Reset complete. Starting tests...\n");
            $display("===============================================");
        end
    endtask
    
    //==========================================================
    // MAIN TEST SEQUENCE
    //==========================================================
    initial begin
        // Initialize counters
        test_count = 0;
        pass_count = 0;
        fail_count = 0;
        
        // Initialize signals
        clk = 0;
        reset_n = 1;
        S_input = 6'b000000;
        
        $display("\n");
        $display("***********************************************");
        $display("*  FSM WEARABLE DEHYDRATION DETECTION TEST   *");
        $display("***********************************************");
        
        // Apply reset
        apply_reset();
        
        //==========================================================
        // CATEGORY 1: IDLE STATE TESTS (P4 Priority)
        //==========================================================
        $display("\n=== CATEGORY 1: IDLE STATE (P4) ===\n");
        
        // Test 1: All sensors off
        S_input = 6'b000000;
        check_output(6'b000000, "IDLE - All sensors OFF");
        
        // Test 2: Only S1 active (single sensor, count=1)
        S_input = 6'b000001;
        check_output(6'b000000, "IDLE - Only S1 active (count=1)");
        
        //==========================================================
        // CATEGORY 2: LIGHT DEHYDRATION TESTS (P3 Priority)
        //==========================================================
        $display("\n=== CATEGORY 2: LIGHT DEHYDRATION (P3) ===\n");
        
        // Test 3: S1=1, S2=1 (count=2)
        S_input = 6'b000011;
        check_output(6'b101100, "LIGHT - S1 & S2 active (count=2)");
        
        //==========================================================
        // CATEGORY 3: ACTIVITY ALERT TESTS (P2 Priority)
        //==========================================================
        $display("\n=== CATEGORY 3: ACTIVITY ALERT (P2) ===\n");
        
        // Test 4: Only S6 active
        S_input = 6'b100000;
        check_output(6'b101110, "ACTIVITY - Only S6 active");
        
        //==========================================================
        // CATEGORY 4: SEVERE DEHYDRATION TESTS (P1 Priority)
        //==========================================================
        $display("\n=== CATEGORY 4: SEVERE DEHYDRATION (P1) ===\n");
        
        // Test 5: S5=1, S1=1 (P1 condition)
        S_input = 6'b010001;
        check_output(6'b111111, "SEVERE - S5 & S1 active");
        
        //==========================================================
        // TEST SUMMARY
        //==========================================================
        #100;
        $display("\n");
        $display("***********************************************");
        $display("*            TEST SUMMARY                     *");
        $display("***********************************************");
        $display("Total Tests:  %0d", test_count);
        $display("Passed:       %0d", pass_count);
        $display("Failed:       %0d", fail_count);
        $display("Success Rate: %0d%%", (pass_count * 100) / test_count);
        $display("***********************************************");
        
        if (fail_count == 0) begin
            $display("\n[SUCCESS] All tests passed!");
        end else begin
            $display("\n[WARNING] Some tests failed!");
        end
        
        $display("\n");
        $finish;
    end
    
    //==========================================================
    // CONTINUOUS MONITORING
    //==========================================================
    
    // 1. Logic to update the string name whenever the state changes
    always @(uut.current_state) begin
        case (uut.current_state)
            2'b00: state_name = "IDLE       ";
            2'b01: state_name = "LIGHT      ";
            2'b10: state_name = "SEVERE     ";
            2'b11: state_name = "ACTIVITY   ";
            default: state_name = "UNKNOWN    ";
        endcase
    end

    // 2. The monitor task simply prints the variable
    initial begin
        $display("\n=== CONTINUOUS STATE MONITORING ===");
        $display("Time(ns) | Reset | S_Input  | State | A_Output | State_Name");
        $display("---------|-------|----------|-------|----------|-------------");
        
        $monitor("%7t |    %b    | %b |  %b    | %b | %s", 
                 $time, reset_n, S_input, uut.current_state, A_output, state_name);
    end
    
    //==========================================================
    // TIMEOUT WATCHDOG
    //==========================================================
    initial begin
        #100000;  // 100 microseconds timeout
        $display("\n[ERROR] Simulation timeout!");
        $finish;
    end

endmodule