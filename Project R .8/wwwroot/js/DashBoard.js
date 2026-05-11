const maxRPM = 8000;
const maxSpeed = 360;
let rpm = 0;
let gear = 1;
let pickedgear = 1;
let acceleration = false;
let speed = 0;

const gearRanges = {
    1: { red: [7000, 8000], green: [4000, 7000], orange: [0, 4000] },
    2: { red: [7000, 8000], green: [4000, 7000], orange: [0, 4000] },
    3: { red: [7000, 8000], green: [4000, 7000], orange: [0, 4000] },
    4: { red: [7000, 8000], green: [4000, 7000], orange: [0, 4000] },
    5: { red: [7000, 8000], green: [4000, 7000], orange: [0, 4000] },
    6: { red: [6000, 7000], green: [4000, 6000], orange: [0, 4000] },
};
const gearRatios = [1 / 4, 1 / 2.5, 1 / 1.5, 1, 1 / 0.8, 1 / 0.7];
function drawRpm(rpm, gear) {
    const ctx = DashBoard.getContext('2d');
    ctx.clearRect(0, 0, DashBoard.width, DashBoard.height);

    const ranges = gearRanges[gear];

    ctx.fillStyle = "#000000";
    ctx.fillRect(0, 0, DashBoard.width, DashBoard.height);

    ctx.fillStyle = "rgba(139, 110, 26, 0.5)";
    ctx.fillRect(0, 50, (ranges.orange[1] / maxRPM) * DashBoard.width, 50);
    ctx.fillStyle = "rgba(6, 151, 23, 0.5)";
    ctx.fillRect((ranges.green[0] / maxRPM) * DashBoard.width, 50, ((ranges.green[1] - ranges.green[0]) / maxRPM) * DashBoard.width, 50);
    ctx.fillStyle = "rgba(123, 4, 3, 0.5)";
    ctx.fillRect((ranges.red[0] / maxRPM) * DashBoard.width, 50, ((ranges.red[1] - ranges.red[0]) / maxRPM) * DashBoard.width, 50);

    if (rpm < ranges.orange[1]) {
        ctx.fillStyle = "#8b6e1a";
        ctx.fillRect(0, 50, (rpm / maxRPM) * DashBoard.width, 50);
    }
    else if (rpm < ranges.green[1]) {
        ctx.fillStyle = "#8b6e1a";
        ctx.fillRect(0, 50, (ranges.orange[1] / maxRPM) * DashBoard.width, 50);

        ctx.fillStyle = "#069717";
        ctx.fillRect((ranges.green[0] / maxRPM) * DashBoard.width, 50, ((rpm - ranges.green[0]) / maxRPM) * DashBoard.width, 50);
    }
    else if (rpm <= ranges.red[1]) {
        ctx.fillStyle = "#8b6e1a";
        ctx.fillRect(0, 50, (ranges.orange[1] / maxRPM) * DashBoard.width, 50);

        ctx.fillStyle = "#069717";
        ctx.fillRect((ranges.green[0] / maxRPM) * DashBoard.width, 50, ((ranges.green[1] - ranges.green[0]) / maxRPM) * DashBoard.width, 50);

        ctx.fillStyle = "#7B0403";
        ctx.fillRect((ranges.red[0] / maxRPM) * DashBoard.width, 50, ((rpm - ranges.red[0]) / maxRPM) * DashBoard.width, 50);
    }
}

function drawSpeed(speed) {
    const SpeedoMeter = document.getElementById("SpeedoMeter");

    const tctx = SpeedoMeter.getContext('2d');
    tctx.clearRect(0, 0, SpeedoMeter.width, SpeedoMeter.height);

    tctx.fillStyle = "#000000";
    tctx.fillRect(0, 0, SpeedoMeter.width, SpeedoMeter.height);

    tctx.fillStyle = "rgba(255, 255, 255, 0.5)";
    tctx.fillRect(0, 50, SpeedoMeter.width, 50);

    tctx.fillStyle = "#ffffff";
    tctx.fillRect(0, 50, (speed / maxSpeed) * SpeedoMeter.width, 50);
}

//key detection
window.keys = {};

window.addEventListener("keydown", (e) => {
    window.keys[e.code] = true;
});

window.addEventListener("keyup", (e) => {
    window.keys[e.code] = false;
});

function update(dt) {
    //acceleration
    if (window.keys["KeyW"]) {
        acceleration = true;
    }
    if (!window.keys["KeyW"]) {
        acceleration = false;
    }
    //braking
    if (window.keys["KeyS"]) {
        rpm -= 4000 * dt;
    }
    //clutch
    if (window.keys["Space"]) {
        //Gear selection
        if (window.keys["Numpad1"]) {
            pickedgear = 1;
        }
        else if (window.keys["Numpad2"]) {
            pickedgear = 2;
        }
        else if (window.keys["Numpad3"]) {
            pickedgear = 3;
        }
        else if (window.keys["Numpad4"]) {
            pickedgear = 4;
        }
        else if (window.keys["Numpad5"]) {
            pickedgear = 5;
        }
        else if (window.keys["Numpad6"]) {
            pickedgear = 6;
        }
        acceleration = false;
        clutchDown(pickedgear);
    }

    if (acceleration) {
        if (rpm <= 4000) {
            rpm += 1750 * dt;
        }
        else if ((rpm > 4000) && (rpm <= 7000)) {
            rpm += (Math.pow(7000 - rpm, 0.93) + 200) * dt;
        }
        else {
            rpm += Math.pow(maxRPM - rpm, 0.9) * dt;
        }
    }
    else {
        rpm -= Math.max(Math.pow(gear, 1.3) * 200 * dt, (400 * dt));  //natural rpm drop
    }
    //rpm limits
    if (gear == 6) {
        rpm = Math.min(rpm, 7000);
    }
    else {
        rpm = Math.min(rpm, maxRPM);
    }
    rpm = Math.max(rpm, 0);

    const wheelCircumference = 2.0;
    const gearRatio = gearRatios[gear - 1];
    if (rpm <= 800) {
        speed = 0;
    }
    else {
        speed = (((rpm - 800 / 60) * gearRatio * wheelCircumference) * dt) / 1000;
        speed = speed * 3.6 * 300;
    }
    speed = Math.min(speed, maxSpeed)
}

function clutchDown(pickedgear) {
    var trpm = rpm;
    if (pickedgear > gear) {    //Gear up
        trpm = rpm - ((pickedgear - gear) * 1700);
        if (trpm < 2300) {      //Check If Player Has Enough rpm 
            trpm = rpm;
        }
        else {
            rpm = trpm;
            gear = pickedgear;
        }
    }
    else {      //Gear Down
        trpm += ((gear - pickedgear) * 1700);
        rpm = Math.min(trpm, maxRPM);
        gear = pickedgear;
    }
}

function writeStats() {
    const gearDis = document.getElementById("CurrentGear");
    gearDis.innerText = "Gear: " + gear;

    const rpmDis = document.getElementById("CurrentRpm");
    rpmDis.innerText = "Rpm: " + Math.floor(rpm);

    const speedDis = document.getElementById("CurrentSpeed");
    speedDis.innerText = "Speed: " + Math.floor(speed);
}

function render(rpm, gear) {
    drawRpm(rpm, gear);
    drawSpeed(speed);
}

let lastTime = performance.now();
function gameLoop(currentTime) {
    const deltaTime = (currentTime - lastTime) / 1000;
    lastTime = currentTime;

    update(deltaTime);
    render(rpm, gear);
    writeStats();

    requestAnimationFrame(gameLoop);
}

requestAnimationFrame(gameLoop);
