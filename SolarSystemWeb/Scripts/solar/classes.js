"use strict";

if (!window.requestAnimationFrame) {
    window.requestAnimationFrame = (function () {
        return window.webkitRequestAnimationFrame ||
        window.mozRequestAnimationFrame ||
        window.oRequestAnimationFrame ||
        window.msRequestAnimationFrame ||
        function (callback, element) {
            window.setTimeout(callback, 1000 / 60);
        };
    })();
}

function Base() {

}

Base.prototype = {
    setProperty: function (object, add) {
        if (add !== true)
            add = false;
        for (var key in object) {
            if (object.hasOwnProperty(key)) {
                if (typeof this[key] !== 'undefined' || add) {
                    this[key] = object[key];
                }
            }
        }
        return this;
    }
}

function MouseController(element) {
    this.element = null;
    this.pos = new Point(0, 0);
    this.pressed = false;

    if (typeof element !== 'undefined') {
        this.watch(element);
    }
};

MouseController.prototype = {
    watch: function (element) {
        var self = this;
        this.element = element;
        this.element.addEventListener('mousemove', function (e) {
            self.move(e);
        }, true);
        this.element.addEventListener('mousedown', function (e) {
            self.down(e);
        }, true);
        this.element.addEventListener('mouseup', function (e) {
            self.up(e);
        }, true);
    },

    move: function (e) {
        this.pos.set(e.offsetX || e.layerX, e.offsetY || e.layerY);
    },

    down: function (e) {
        this.move(e);
        this.pressed = true;
    },

    up: function (e) {
        this.move(e);
        this.pressed = false;
    }
};

function Orbit(center, radius, secondradius) {
    this.center = center;
    this.radius = radius;
    this.secondradius = secondradius || radius;

    this.planet = null;
    this.ctx = null;
    this.mouse = null;    
};

Orbit.prototype = Object.create(Base.prototype);

Orbit.prototype.draw =
    function () {
        var ctx = this.ctx;
        var hover = this.mouse && Math.abs(this.mouse.pos.getDis(this.center) - this.radius) < 12;
        if (hover) {
            ctx.lineWidth = 2;
            ctx.strokeStyle = 'rgb(0,192,255)';

            ctx.beginPath();
            ctx.ellipse(this.center.x, this.center.y, this.radius, this.secondradius, 0, 0, 2 * Math.PI);

            ctx.stroke();

            ctx.clearRect(this.planet.pos.x - this.planet.radius, this.planet.pos.y - this.planet.radius,
                this.planet.radius * 2, this.planet.radius * 2);
            this.planet.drawBorder();
        } else {
            ctx.lineWidth = 1;
            ctx.strokeStyle = 'rgba(0,192,255,0.5)';
            ctx.beginPath();
            ctx.ellipse(this.center.x, this.center.y, this.radius, this.secondradius, 0,  0, 2 * Math.PI);                           
            
            ctx.stroke();
        }
    }

function Planet(orbit, radius, secondRadius, time) {    
    this.pos = new Point(0, 0);
    this.orbit = orbit;
    this.radius = radius;
    this.secondradius = secondRadius || radius; 
    this.speed = Math.PI * 2 / (time * 1000);
    this.angle = ~~(Math.random() * 360);

    this.parent = null;

    this.animate = true;
    this.name;
    this.tile;
    this.ctx;
    this.orbit.setProperty({ 'planet': this });
    this.orbitVisibility = true;

    var getImageSize = function (rad) {
        var sizesMap = [
            {
                imageSizesRange: { min: 9, max: 9.5 },
                realSizesRange: { min: 0, max: 200 }       // икар
            },
            {
                imageSizesRange: { min: 10, max: 11 },
                realSizesRange: { min: 200, max: 500 }       // церера
            },
            {
                imageSizesRange: { min: 12, max: 12 },
                realSizesRange: { min: 500, max: 1000 }       // 
            },
            {
                imageSizesRange: { min: 13, max: 15 },
                realSizesRange: { min: 1000, max: 1500 }   // плутон
            },
            {
                imageSizesRange: { min: 16, max: 18 },
                realSizesRange: { min: 1500, max: 2000 }   // луна
            },
            {
                imageSizesRange: { min: 20, max: 23 },
                realSizesRange: { min: 2000, max: 2750 }   // меркурий
            },
            {
                imageSizesRange: { min: 24, max: 28 },
                realSizesRange: { min: 2750, max: 3500 }   // марс
            },
            {
                imageSizesRange: { min: 33, max: 35 },
                realSizesRange: { min: 3500, max: 7000 }   // земля
            },
            {
                imageSizesRange: { min: 42, max: 43 },
                realSizesRange: { min: 24000, max: 26000 }   // уран
            },
            {
                imageSizesRange: { min: 50, max: 55 },
                realSizesRange: { min: 55000, max: 70000 }   // юпитер
            },
            {
                imageSizesRange: { min: 80, max: null },
                realSizesRange: { min: 500000, max: null }   // солнце
            },
        ];

        var res = 30;

        for (var i = 0; i < sizesMap.length; ++i) {
            if (rad > sizesMap[i].realSizesRange.min &&
                sizesMap[i].realSizesRange.max === null)
                return sizesMap[i].imageSizesRange.min / 2.7;

            if (rad > sizesMap[i].realSizesRange.min &&
                rad <= sizesMap[i].realSizesRange.max) {
                var deltaReal = sizesMap[i].realSizesRange.max - sizesMap[i].realSizesRange.min;
                var deltaImage = sizesMap[i].imageSizesRange.max - sizesMap[i].imageSizesRange.min;
                var deltaSize = rad - sizesMap[i].realSizesRange.min;
                var imageAddition = deltaSize / deltaReal * deltaImage;

                return (sizesMap[i].imageSizesRange.min + imageAddition) / 2.7;
            }
        }

        return res;
    }
    
    this.radius = getImageSize(radius);

    var getOrbitRadius = function (rad) {
        var sizesMap = [
            {
                imageSizesRange: { min: 16, max: 55 },
                realSizesRange: { min: 15000, max: 2000000 }       // спутники
            },
            {
                imageSizesRange: { min: 10, max: 115 },
                realSizesRange: { min: 20000000, max: 160000000 }       // меркурий, венера, земля
            },
            {
                imageSizesRange: { min: 127, max: 180 },
                realSizesRange: { min: 160000000, max: 600000000 }       // марс и астероиды
            },
            {
                imageSizesRange: { min: 200, max: 215 },
                realSizesRange: { min: 700000000, max: 800000000 }       // юпитер
            },
            {
                imageSizesRange: { min: 280, max: 285 },
                realSizesRange: { min: 1000000000, max: 1600000000 }   // сатурн
            },
            {
                imageSizesRange: { min: 315, max: 320 },
                realSizesRange: { min: 2000000000, max: 3200000000 }   // уран
            },
            {
                imageSizesRange: { min: 340, max: 345 },
                realSizesRange: { min: 4000000000, max: 4600000000 }   // нептун
            },
            {
                imageSizesRange: { min: 370, max: 375 },
                realSizesRange: { min: 4900000000, max: null }   // плутон
            },           
        ];

        if (rad === 0)
            return 0;

        var res = 50;

        for (var i = 0; i < sizesMap.length; ++i) {
            
            if (rad > sizesMap[i].realSizesRange.min &&
                sizesMap[i].realSizesRange.max === null)
                return sizesMap[i].imageSizesRange.min;

            if (rad > sizesMap[i].realSizesRange.min &&
                rad <= sizesMap[i].realSizesRange.max) {
                var deltaReal = sizesMap[i].realSizesRange.max - sizesMap[i].realSizesRange.min;
                var deltaImage = sizesMap[i].imageSizesRange.max - sizesMap[i].imageSizesRange.min;
                var deltaSize = rad - sizesMap[i].realSizesRange.min;
                var imageAddition = deltaSize / deltaReal * deltaImage;

                return (sizesMap[i].imageSizesRange.min + imageAddition);
            }
        }

        return res;
    }

    this.realDistance = this.orbit.radius = getOrbitRadius(orbit.radius);
    this.secondradius = this.orbit.secondradius = getOrbitRadius(orbit.secondradius);
};

Planet.prototype = Object.create(Base.prototype);

Planet.prototype.drawBorder =
    function () {
        var ctx = this.ctx;
        ctx.lineWidth = 2;
        ctx.strokeStyle = 'rgb(0,192,255)';
        ctx.beginPath();
        ctx.arc(this.pos.x, this.pos.y, this.radius * 1.1, 0, Math.PI * 2, true);
        ctx.closePath();
        ctx.stroke();
    },

Planet.prototype.showInfo =
    function () {
        var x = this.pos.x + this.radius * 0.7;
        var y = this.pos.y + this.radius * 0.9;

        var ctx = this.ctx;
        ctx.fillStyle = '#002244';
        ctx.fillRect(x, y, 100, 24);
        ctx.fillStyle = '#0ff';
        ctx.fillText(this.name, x + 50, y + 17);
    },

Planet.prototype.setOrbitVisibility = function(flag) {
    this.orbitVisibility = flag ? true : false;
}

Planet.prototype.render =
    function (deltaTime) {
        if (this.animate) {
            if (!this.parent) {
                this.pos.x = this.orbit.center.x + this.orbit.radius * Math.cos(this.angle);
                this.pos.y = this.orbit.center.y + this.orbit.secondradius * Math.sin(this.angle);
                this.angle += this.speed * deltaTime;
            } else {
                this.pos.x = this.parent.pos.x + this.orbit.radius * Math.cos(this.angle);
                this.pos.y = this.parent.pos.y + this.orbit.secondradius * Math.sin(this.angle);
                this.angle += this.speed * deltaTime * 10;
            }
            
        }
        
        if (typeof this.tile !== 'undefined') {
            this.tile.draw(this.pos.x, this.pos.y, this.orbit.center, this.radius);
        }
    }

function Point(x, y) {
    this.x;
    this.y;

    this.set(x, y);
};

Point.prototype = Object.create(Base.prototype);

Point.prototype.set =
    function (x, y) {
        this.x = x || 0;
        this.y = y || 0;
    }

Point.prototype.getDis =
    function (other) {
        return Math.sqrt(Math.pow(other.x - this.x, 2) + Math.pow(other.y - this.y, 2));
    }

Point.prototype.clone =
    function () {
        return new Point(this.x, this.y);
    }

function Tile(ctx, img, x, y, w, h) {
    this.ctx = ctx;
    this.img = img;
    this.x = x;
    this.y = y;
    this.width = w;
    this.height = h;
};

Tile.prototype = {
    draw: function (x, y, p, radius) {
        var ctx = this.ctx;
        ctx.save();
        ctx.translate(x, y);
        ctx.rotate(Math.atan2(p.y - y, p.x - x) + Math.PI / 2);
        this.ctx.drawImage(this.img, this.x, this.y, this.width, this.height, -radius, -radius, 2*radius, 2*radius);
        ctx.restore();
    }
};