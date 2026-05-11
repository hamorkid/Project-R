//make maps store data as an array and call it map data.
//contain map name spawn (X) (Y) and angle ...

//make a function outside gameloop to detect things that dont need to be detected periodically
//ie: lap count, checkpoints ...
skins = new Skins();
maps = new Maps();

myCanvas.width = window.innerWidth;
myCanvas.height = window.innerHeight;

SevenSegmentCanvas.width = 40;
SevenSegmentCanvas.height = 65;

const gearRanges = {
    1: { red: [7000, 8000], green: [4000, 7000], orange: [0, 4000] },
    2: { red: [7000, 8000], green: [4000, 7000], orange: [0, 4000] },
    3: { red: [7000, 8000], green: [4000, 7000], orange: [0, 4000] },
    4: { red: [7000, 8000], green: [4000, 7000], orange: [0, 4000] },
    5: { red: [7000, 8000], green: [4000, 7000], orange: [0, 4000] },
    6: { red: [6000, 7000], green: [4000, 6000], orange: [0, 4000] },
};

function drawRpm() {
    const DashBoard = document.getElementById("DashBoard");
    const ctx = DashBoard.getContext('2d');
    ctx.clearRect(0, 0, DashBoard.width, DashBoard.height);

    const ranges = gearRanges[car.gear];

    ctx.fillStyle = "#000000";
    ctx.fillRect(0, 0, DashBoard.width, DashBoard.height);

    ctx.fillStyle = "rgba(139, 110, 26, 0.5)";
    ctx.fillRect(0, 50, (ranges.orange[1] / car.maxRPM) * DashBoard.width, 50);
    ctx.fillStyle = "rgba(6, 151, 23, 0.5)";
    ctx.fillRect((ranges.green[0] / car.maxRPM) * DashBoard.width, 50, ((ranges.green[1] - ranges.green[0]) / car.maxRPM) * DashBoard.width, 50);
    ctx.fillStyle = "rgba(123, 4, 3, 0.5)";
    ctx.fillRect((ranges.red[0] / car.maxRPM) * DashBoard.width, 50, ((ranges.red[1] - ranges.red[0]) / car.maxRPM) * DashBoard.width, 50);

    if (car.rpm < ranges.orange[1]) {
        ctx.fillStyle = "#8b6e1a";
        ctx.fillRect(0, 50, (car.rpm / car.maxRPM) * DashBoard.width, 50);
    }
    else if (car.rpm < ranges.green[1]) {
        ctx.fillStyle = "#8b6e1a";
        ctx.fillRect(0, 50, (ranges.orange[1] / car.maxRPM) * DashBoard.width, 50);

        ctx.fillStyle = "#069717";
        ctx.fillRect((ranges.green[0] / car.maxRPM) * DashBoard.width, 50, ((car.rpm - ranges.green[0]) / car.maxRPM) * DashBoard.width, 50);
    }
    else if (car.rpm <= ranges.red[1]) {
        ctx.fillStyle = "#8b6e1a";
        ctx.fillRect(0, 50, (ranges.orange[1] / car.maxRPM) * DashBoard.width, 50);

        ctx.fillStyle = "#069717";
        ctx.fillRect((ranges.green[0] / car.maxRPM) * DashBoard.width, 50, ((ranges.green[1] - ranges.green[0]) / car.maxRPM) * DashBoard.width, 50);

        ctx.fillStyle = "#7B0403";
        ctx.fillRect((ranges.red[0] / car.maxRPM) * DashBoard.width, 50, ((car.rpm - ranges.red[0]) / car.maxRPM) * DashBoard.width, 50);
    }
}

function drawSpeed() {
    const SpeedoMeter = document.getElementById("SpeedoMeter");
    const tctx = SpeedoMeter.getContext('2d');

    tctx.clearRect(0, 0, SpeedoMeter.width, SpeedoMeter.height);

    tctx.fillStyle = "#000000";
    tctx.fillRect(0, 0, SpeedoMeter.width, SpeedoMeter.height);

    tctx.fillStyle = "rgba(255, 255, 255, 0.5)";
    tctx.fillRect(0, 50, SpeedoMeter.width, 50);

    tctx.fillStyle = "#ffffff";
    tctx.fillRect(0, 50, (car.speed / car.maxSpeed) * SpeedoMeter.width, 50);
}

//key detection
window.keys = {};

window.addEventListener("keydown", (e) => {
    window.keys[e.code] = true;
});

window.addEventListener("keyup", (e) => {
    window.keys[e.code] = false;
});
class Car {
    penalty = 0;
    //Drawing Variables
    x = 10750; //las vegas x: 12750
    y = 4200; //las vegas y: 1250

    //Car Variables
    speed = 0;
    angle = 0.67 * Math.PI; // las vegas angle: 0.23
    rpm = 0;
    gear = 1;
    pickedgear = 1;
    steer = 0;

    //Hard limits
    maxRPM = 8000;
    maxSpeed = 360;
    gearRatios = [1 / 4, 1 / 2.5, 1 / 1.5, 1, 1 / 0.8, 1 / 0.7];
    maxSteerAngle = Math.PI / 50;
    wheelBase = 5;


    Car() {
        generateCar();
    }

    update(dt) {
        //acceleration
        if (window.keys["KeyW"]) {
            this.rpm = this.acc(dt);
            if (this.penaltyMode()) {
                this.rpm = this.rpm;
            }
        }
        if (!window.keys["KeyW"]) {
            this.rpm = this.noAcceleration(dt);
        }
        //braking
        if (window.keys["KeyS"]) {
            this.rpm = this.brake(dt);
        }
        //clutch
        if (window.keys["Space"]) {
            //Gear selection
		console.log("Clutch");
            if (window.keys["KeyU"]) {
                this.pickedgear = 1;
            }
            else if (window.keys["KeyI"]) {
                this.pickedgear = 2;
            }
            else if (window.keys["KeyJ"]) {
                this.pickedgear = 3;
            }
            else if (window.keys["keyK"]) {
                this.pickedgear = 4;
            }
            else if (window.keys["KeyN"]) {
                this.pickedgear = 5;
            }
            else if (window.keys["KeyM"]) {
                this.pickedgear = 6;
            }
            this.acceleration = false;
            this.clutchDown(this.pickedgear);
        }
        //turning
        if (window.keys["KeyA"]) {
            this.steer = 1;
        }
        else if (window.keys["KeyD"]) {
            this.steer = -1;
        }
        if (!window.keys["KeyA"] && !window.keys["KeyD"]) {
            this.steer = 0;
        }
        

        this.rpmLimit();
        this.speed = this.calcSpeed(dt);
        this.moveCar(dt);
    }

    acc(dt) {
        if (this.rpm <= 4000) {
            this.rpm += 1750 * dt;
        }
        else if ((this.rpm > 4000) && (this.rpm <= 7000)) {
            this.rpm += (Math.pow(7000 - this.rpm, 0.93) + 200) * dt;
        }
        else {
            this.rpm += Math.pow(this.maxRPM - this.rpm, 0.9) * dt;
        }

        return this.rpm;
    }
    noAcceleration(dt) {
        return (this.rpm - Math.max(Math.pow(this.gear, 1.3) * 200 * dt, (400 * dt)));
    }
    penaltyMode() {
        if (((detectSurface() == "OffRoad") || window.keys["KeyA"] || window.keys["KeyD"]) && this.spped > 70) {
            return true;
        }
        return false
    }
    brake(dt) {
        return this.rpm - 4000 * dt;
    }
    rpmLimit() {
        if (this.gear == 6) {
            this.rpm = Math.min(this.rpm, 7000);
        }
        else {
            this.rpm = Math.min(this.rpm, this.maxRPM);
        }
        this.rpm = Math.max(this.rpm, 0);
    }

    clutchDown(pickedgear) {
        let trpm = this.rpm;
        if (pickedgear > this.gear) {    //Gear up
            trpm = this.rpm - ((pickedgear - this.gear) * 1700);
            if (trpm < 2300) {      //Check If Player Has Enough rpm 
                trpm = this.rpm;
            }
            else {
                this.rpm = trpm;
                this.gear = pickedgear;
            }
        }
        else {      //Gear Down
            trpm += ((this.gear - pickedgear) * 1700);
            this.rpm = Math.min(trpm, this.maxRPM);
            this.gear = pickedgear;
        }
    }

    turnPenalty(dt, speed) {
        if (window.keys["KeyA"] || window.keys["KeyD"]) {
            
            this.penalty += (speed * 0.3) * dt;
            return Math.max(this.penalty, 0);
        }
        this.penalty -= (speed * 0.3) * dt;
        return Math.max(this.penalty, 0);
    }

    moveCar(dt) {
        const speedFactor = 1 - (this.speed / this.maxSpeed) * 0.8;
        const steerAngle = this.steer * this.maxSteerAngle * speedFactor;        // steer is -1 or 1
        const turnRadius = this.wheelBase / Math.tan(steerAngle);
        const angularVelocity = this.speed / turnRadius;

        this.angle += angularVelocity * dt;
        this.y -= Math.cos(this.angle) * this.speed * dt * 2;
        this.x -= Math.sin(this.angle) * this.speed * dt * 2;
    }

    calcSpeed(dt) {
        const wheelCircumference = 2.0;
        var gearRatio = this.gearRatios[this.gear - 1];
        if (this.rpm <= 0) {
            this.speed = 0;
        }
        else { 
            this.speed = (((this.rpm - 800 / 60) * gearRatio * wheelCircumference) * dt) / 1000;
            this.speed = this.speed * 3.6 * 300;
        }    

        Math.min(this.speed, this.maxSpeed);
        this.speed -= this.turnPenalty(dt, this.speed);
        this.surfacePenalty(detectSurface());
        this.speed = Math.max(this.speed, 0);
        this.speed = Math.min(this.speed, this.maxSpeed);
        return this.speed
    }

    surfacePenalty(surface) {
	    if (surface == "Road") {
            
        }
        else if (surface == "Edge") {
            
        }
        else if (surface == "Curb") {
            
        
        }
        else if (surface == "OffRoad") {
            if (this.speed > 70) {
                const excess = this.rpm - 70; 
                this.rpm -= excess * 0.02;

                if (this.speed < 70) {
                    this.speed = 70;
                }
            }
	    }
        
    }
}

function detectSurface() {
    const map = document.getElementById('myCanvas');
    const ctx = map.getContext('2d', { willReadFrequently: true });

    const imgData = ctx.getImageData((myCanvas.width / 2) , (myCanvas.height / 2) , 1, 1).data;

    const r = imgData[0];
    const g = imgData[1];
    const b = imgData[2];
    function near(value, target, tolerance = 20) {
        return Math.abs(value - target) <= tolerance;
    }
    console.log(r, g, b)
    if (near(r, 49) && near(g, 49) && near(b, 49)) {
        return "Road";
    } else if (near(r, 255) && near(g, 255) && near(b, 255)) {
        return "Edge";
    } else if (near(r, 255) && near(g, 0) && near(b, 0)) {
        return "Curb";
    }
    //console.log(imgData);
    /*
    if (imgData == grayRoad) {
        return "Road";
    }
    else if (imgData == whiteEdges) {
        return "Edge";
    }
    else if (imgData == redCurbs) {
        return "Curb";
    } */
    return "OffRoad";
}

function generateCar() {
    const temp = document.getElementById('myCanvas');
    const ctx = temp.getContext('2d');
    ctx.imageSmoothingEnabled = false;
        
    ctx.save();
    ctx.translate(myCanvas.width / 2 , myCanvas.height / 2 );    // move origin to car center
    ctx.rotate(-car.angle);    // rotate around that  
    ctx.drawImage(carImg, -32,  -32, 96, 96);
    ctx.restore();

    //console.log("Car drawn at:", car.x, car.y, "with angle:", car.angle);

    carImg.onerror = function () {
        console.error("Image couldn't load. Check path:", carImg.src);
    };
}

function generateTrack(dt) {
    const map = document.getElementById('myCanvas');
    const ctx = map.getContext('2d');
    ctx.imageSmoothingEnabled = false;

    mapX = 0;
    mapY = 0;
    mapX -= car.x;
    mapY -= car.y;
   
    ctx.clearRect(0, 0, myCanvas.width, myCanvas.height);
    ctx.drawImage(track, mapX, mapY, myCanvas.width * 8, myCanvas.height * 8);        

    track.onerror = function () {
        console.error("Image couldn't load. Check path:", track.src);
    };
    
}

function writeSpeed(speed) {
    const ctx = document.getElementById("SpeedNumber");
    ctx.innerText = Math.round(speed);
}

function clearFrame() {
    const map = document.getElementById('myCanvas');
    const ctx = map.getContext('2d');
    const img = new Image();
    img.onload = function () {
        ctx.clearRect(0, 0, myCanvas.width, myCanvas.height);
    };
}

function generateDashBoard() {
    drawRpm();
    drawSpeed();
    drawGear(car.gear, 5);
    writeSpeed(car.speed);
}


car = new Car();
let lastTime = performance.now();
function gameLoop(currentTime) {
    const deltaTime = (currentTime - lastTime) / 1000;
    lastTime = currentTime;

    clearFrame()
    generateTrack(deltaTime);
    car.update(deltaTime);
    generateCar();
    generateDashBoard();

    requestAnimationFrame(gameLoop);
}

const track = new Image();
//track.src = maps.LAS_VEGAS;
track.src = maps.MAP;

const carImg = new Image();
carImg.src = skins.SELECTED-SKIN;

requestAnimationFrame(gameLoop);

