module fsm_wearable (
    input wire clk,
    input wire rst_n,
    input wire [5:0] sensor_inputs,
    output reg [5:0] actuator_outputs,
    output reg [1:0] state_code
);

    // State Encoding
    localparam IDLE           = 2'b00;
    localparam LIGHT_DEHY     = 2'b01;
    localparam SEVERE_DEHY    = 2'b10;
    localparam ACTIVITY_ALERT = 2'b11;

    // Internal Signals
    reg [1:0] current_state, next_state;
    reg [5:0] sync_buffer1;
    reg [5:0] sync_buffer2; // Synced inputs

    // 2-Stage Synchronizer
    always @(posedge clk or negedge rst_n) begin
        if (!rst_n) begin
            sync_buffer1 <= 6'b0;
            sync_buffer2 <= 6'b0;
        end else begin
            sync_buffer1 <= sensor_inputs;
            sync_buffer2 <= sync_buffer1;
        end
    end

    // Priority Detection Logic
    wire p1_detect, p2_detect, p3_detect;
    wire [2:0] active_sensor_count;

    // P1: S5=1 AND any of (S1 OR S2 OR S3 OR S4)=1
    assign p1_detect = sync_buffer2[4] && (sync_buffer2[0] || sync_buffer2[1] || sync_buffer2[2] || sync_buffer2[3]);

    // P2: S6=1
    assign p2_detect = sync_buffer2[5];

    // P3: sum(S1..S5) >= 2
    // We need to count bits set in S1-S5 (indices 0-4)
    assign active_sensor_count = sync_buffer2[0] + sync_buffer2[1] + sync_buffer2[2] + sync_buffer2[3] + sync_buffer2[4];
    assign p3_detect = (active_sensor_count >= 3'd2);

    // Next State Logic (Combinational)
    always @(*) begin
        // Strict priority hierarchy: P1 > P2 > P3 > P4
        if (p1_detect)
            next_state = SEVERE_DEHY;
        else if (p2_detect)
            next_state = ACTIVITY_ALERT;
        else if (p3_detect)
            next_state = LIGHT_DEHY;
        else
            next_state = IDLE;
    end

    // State Register Update
    always @(posedge clk or negedge rst_n) begin
        if (!rst_n)
            current_state <= IDLE;
        else
            current_state <= next_state;
    end

    // Output Logic (Moore)
    always @(*) begin
        state_code = current_state;
        case (current_state)
            IDLE:           actuator_outputs = 6'b000000;
            LIGHT_DEHY:     actuator_outputs = 6'b101100; // A1, A3, A4 ON
            SEVERE_DEHY:    actuator_outputs = 6'b111111; // All ON
            ACTIVITY_ALERT: actuator_outputs = 6'b101110; // A1, A3, A4, A5 ON
            default:        actuator_outputs = 6'b000000;
        endcase
    end

endmodule