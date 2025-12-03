`timescale 1ns/1ps

module tb_fsm_wearable;

    //==========================================================
    // TESTBENCH SIGNALS
    //==========================================================
    reg clk;
    reg reset_n;
    reg [5:0] S_input;
    wire [5:0] A_output;
    
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
        .reset_n(reset_n),
        .S_input(S_input),
        .A_output(A_output)
    );

     //==========================================================
    // VCD WAVEFORM DUMP - TARUH DI SINI
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
    // TASK: RESET DURING STATE
    //==========================================================
    task reset_during_state;
        input [5:0] input_pattern;
        input [8*50:1] test_name;
        begin
            test_count = test_count + 1;
            
            // Apply input to enter a state
            S_input = input_pattern;
            #50;  // Wait to enter state
            
            // Apply reset
            reset_n = 0;
            S_input = 6'b000000;  // Clear inputs during reset
            #20;
            reset_n = 1;
            #50;  // Wait after reset
            
            if (A_output === 6'b000000) begin
                $display("[PASS] Test %0d: %s", test_count, test_name);
                pass_count = pass_count + 1;
            end else begin
                $display("[FAIL] Test %0d: %s", test_count, test_name);
                fail_count = fail_count + 1;
            end
            $display("-----------------------------------------------");
        end
    endtask
    
    //==========================================================
    // TASK: CHECK SYNC DELAY
    //==========================================================
    task check_sync_delay;
        input [5:0] input_pattern;
        input [8*50:1] test_name;
        begin
            test_count = test_count + 1;
            
            // Start from IDLE
            S_input = 6'b000000;
            #50;
            
            // Apply input and check before sync completes
            S_input = input_pattern;
            #10;  // Check after 10ns (before sync complete)
            
            if (A_output !== 6'b111111) begin
                $display("[PASS] Test %0d: %s", test_count, test_name);
                pass_count = pass_count + 1;
            end else begin
                $display("[FAIL] Test %0d: %s", test_count, test_name);
                fail_count = fail_count + 1;
            end
            
            #50;  // Wait for sync to complete
            $display("-----------------------------------------------");
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
        
        // Test 3: Only S2 active (single sensor, count=1)
        S_input = 6'b000010;
        check_output(6'b000000, "IDLE - Only S2 active (count=1)");
        
        // Test 4: Only S3 active (single sensor, count=1)
        S_input = 6'b000100;
        check_output(6'b000000, "IDLE - Only S3 active (count=1)");
        
        // Test 5: Only S4 active (single sensor, count=1)
        S_input = 6'b001000;
        check_output(6'b000000, "IDLE - Only S4 active (count=1)");
        
        //==========================================================
        // CATEGORY 2: LIGHT DEHYDRATION TESTS (P3 Priority)
        //==========================================================
        $display("\n=== CATEGORY 2: LIGHT DEHYDRATION (P3) ===\n");
        
        // Test 6: S1=1, S2=1 (count=2)
        S_input = 6'b000011;
        check_output(6'b101100, "LIGHT - S1 & S2 active (count=2)");
        
        // Test 7: S1=1, S3=1 (count=2)
        S_input = 6'b000101;
        check_output(6'b101100, "LIGHT - S1 & S3 active (count=2)");
        
        // Test 8: S2=1, S4=1 (count=2)
        S_input = 6'b001010;
        check_output(6'b101100, "LIGHT - S2 & S4 active (count=2)");
        
        // Test 9: S1=1, S2=1, S3=1 (count=3)
        S_input = 6'b000111;
        check_output(6'b101100, "LIGHT - S1, S2, S3 active (count=3)");
        
        // Test 10: S1=1, S2=1, S3=1, S4=1 (count=4)
        S_input = 6'b001111;
        check_output(6'b101100, "LIGHT - Four sensors active (count=4)");
        
        //==========================================================
        // CATEGORY 3: ACTIVITY ALERT TESTS (P2 Priority)
        //==========================================================
        $display("\n=== CATEGORY 3: ACTIVITY ALERT (P2) ===\n");
        
        // Test 11: Only S6 active
        S_input = 6'b100000;
        check_output(6'b101110, "ACTIVITY - Only S6 active");
        
        // Test 12: S6=1, S1=1 (activity with one sensor)
        S_input = 6'b100001;
        check_output(6'b101110, "ACTIVITY - S6 & S1 active");
        
        // Test 13: S6=1, S1=1, S2=1 (activity overrides P3)
        S_input = 6'b100011;
        check_output(6'b101110, "ACTIVITY - S6 overrides LIGHT condition");
        
        // Test 14: S6=1, multiple sensors (P2 > P3)
        S_input = 6'b100111;
        check_output(6'b101110, "ACTIVITY - P2 priority over P3");
        
        //==========================================================
        // CATEGORY 4: SEVERE DEHYDRATION TESTS (P1 Priority)
        //==========================================================
        $display("\n=== CATEGORY 4: SEVERE DEHYDRATION (P1) ===\n");
        
        // Test 15: S5=1, S1=1 (P1 condition)
        S_input = 6'b010001;
        check_output(6'b111111, "SEVERE - S5 & S1 active");
        
        // Test 16: S5=1, S2=1 (P1 condition)
        S_input = 6'b010010;
        check_output(6'b111111, "SEVERE - S5 & S2 active");
        
        // Test 17: S5=1, S3=1 (P1 condition)
        S_input = 6'b010100;
        check_output(6'b111111, "SEVERE - S5 & S3 active");
        
        // Test 18: S5=1, S4=1 (P1 condition)
        S_input = 6'b011000;
        check_output(6'b111111, "SEVERE - S5 & S4 active");
        
        // Test 19: S5=1, S1=1, S2=1 (P1 with multiple)
        S_input = 6'b010011;
        check_output(6'b111111, "SEVERE - S5 with multiple sensors");
        
        // Test 20: S5=1 only (should NOT trigger P1)
        S_input = 6'b010000;
        check_output(6'b000000, "IDLE - S5 alone (no P1 trigger)");
        
        //==========================================================
        // CATEGORY 5: PRIORITY TESTING (P1 > P2 > P3 > P4)
        //==========================================================
        $display("\n=== CATEGORY 5: PRIORITY HIERARCHY ===\n");
        
        // Test 21: P1 > P2 (S5=1, S1=1, S6=1)
        S_input = 6'b110001;
        check_output(6'b111111, "Priority - P1 overrides P2");
        
        // Test 22: P1 > P3 (S5=1, S1=1, S2=1)
        S_input = 6'b010011;
        check_output(6'b111111, "Priority - P1 overrides P3");
        
        // Test 23: P2 > P3 (S6=1, S1=1, S2=1)
        S_input = 6'b100011;
        check_output(6'b101110, "Priority - P2 overrides P3");
        
        // Test 24: All conditions active (P1 highest)
        S_input = 6'b111111;
        check_output(6'b111111, "Priority - P1 wins when all active");
        
        // Test 25: P1 > P2 > P3 (S5=1, S2=1, S6=1)
        S_input = 6'b110010;
        check_output(6'b111111, "Priority - Complex P1>P2>P3");
        
        //==========================================================
        // CATEGORY 6: EDGE CASES
        //==========================================================
        $display("\n=== CATEGORY 6: EDGE CASES ===\n");
        
        // Test 26: Rapid state changes
        S_input = 6'b000000; #20;
        S_input = 6'b100000; #20;
        S_input = 6'b010001; #20;
        check_output(6'b111111, "Edge - Rapid state transitions");
        
        // Test 27: Return to IDLE after SEVERE
        S_input = 6'b010001; #50;
        S_input = 6'b000000;
        check_output(6'b000000, "Edge - SEVERE to IDLE transition");
        
        // Test 28: Bouncing between states
        S_input = 6'b000011; #50;  // LIGHT
        S_input = 6'b100000; #50;  // ACTIVITY
        S_input = 6'b000011;
        check_output(6'b101100, "Edge - LIGHT->ACTIVITY->LIGHT");
        
        //==========================================================
        // CATEGORY 7: RESET FUNCTIONALITY
        //==========================================================
        $display("\n=== CATEGORY 7: RESET TESTING ===\n");
        
        // Test 29: Reset during SEVERE state
        reset_during_state(6'b010001, "Reset during SEVERE state");
        
        // Test 30: Normal operation after reset
        S_input = 6'b100000;
        check_output(6'b101110, "Reset - Normal operation after reset");
        
        //==========================================================
        // CATEGORY 8: SYNCHRONIZER TESTING
        //==========================================================
        $display("\n=== CATEGORY 8: SYNCHRONIZER TESTING ===\n");
        
        // Test 31: Verify 2-stage synchronization delay
        check_sync_delay(6'b010001, "Synchronizer delay works");
        
        //==========================================================
        // CATEGORY 9: TRUTH TABLE SAMPLES
        //==========================================================
        $display("\n=== CATEGORY 9: TRUTH TABLE SAMPLES ===\n");
        
        // Test representative entries from paper's truth table
        
        // Row 1: 000000 -> IDLE (000000)
        S_input = 6'b000000;
        check_output(6'b000000, "Truth Table Row 1");
        
        // Row 2: 100000 -> ACT (101110)
        S_input = 6'b100000;
        check_output(6'b101110, "Truth Table Row 2");
        
        // Row 3: 010000 -> IDLE (not SEVERE, S5 alone)
        S_input = 6'b010000;
        check_output(6'b000000, "Truth Table Row 3");
        
        // Row 13: 000110 -> LIGHT (101100)
        S_input = 6'b000110;
        check_output(6'b101100, "Truth Table Row 13");
        
        // Row 64: 111111 -> SEVERE (111111)
        S_input = 6'b111111;
        check_output(6'b111111, "Truth Table Row 64");
        
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
            $display("\n[SUCCESS] All tests passed! ✓");
        end else begin
            $display("\n[WARNING] Some tests failed! ✗");
        end
        
        $display("\n");
        $finish;
    end
    
    //==========================================================
    // CONTINUOUS MONITORING
    //==========================================================
    
    // 1. Logic to update the string name whenever the state changes
    always @(uut.Q_current) begin
        case (uut.Q_current)
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
        
        // Notice we now pass 'state_name' directly, which fixes the error
        $monitor("%7t |    %b    | %b |  %b    | %b | %s", 
                 $time, reset_n, S_input, uut.Q_current, A_output, state_name);
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