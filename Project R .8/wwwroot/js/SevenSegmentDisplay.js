const canvas = document.getElementById('SevenSegmentCanvas');
const ctx = canvas.getContext('2d');

// Binary patterns for digits 0-9 (Segments: abcdefg)
const digits = [
    0x7E, 0x30, 0x6D, 0x79, 0x33, 0x5B, 0x5F, 0x70, 0x7F, 0x7B
];

function drawSegment(x, y, width, height, isOn) {
    ctx.fillStyle = isOn ? "red" : "#220000"; // Bright red if on, dim for background
    ctx.fillRect(x, y, width, height);
}

function drawGear(number, offset) {
    const sW = 20; // Segment width (length)
    const sT = 5;  // Segment thickness
    const val = digits[number];

    ctx.fillStyle = "#000000";
    ctx.clearRect(0, 0, canvas.width, canvas.height); // Clear canvas

    // Draw segments A-G based on bitwise comparison
    ctx.fillRect(0, 0, canvas.width, canvas.height); // Clear canvas
    drawSegment(offset + sT, 0 + 5, sW, sT, val & 0x40);          // A (Top)
    drawSegment(offset + sT + sW, sT + 5, sT, sW, val & 0x20);   // B (Upper Right)
    drawSegment(offset + sT + sW, 2 * sT + sW + 5, sT, sW, val & 0x10); // C (Lower Right)
    drawSegment(offset + sT, 2 * sT + 2 * sW + 5, sW, sT, val & 0x08); // D (Bottom)
    drawSegment(offset, 2 * sT + sW + 5, sT, sW, val & 0x04);      // E (Lower Left)
    drawSegment(offset, sT + 5, sT, sW, val & 0x02);             // F (Upper Left)
    drawSegment(offset + sT, sT + sW + 5, sW, sT, val & 0x01);   // G (Middle)
}

 