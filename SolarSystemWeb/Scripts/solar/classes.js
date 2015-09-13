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

function Orbit(center, radius) {
    this.center = center;
    this.radius = radius;

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
            ctx.arc(this.center.x, this.center.y, this.radius, 0, Math.PI * 2, true);
            ctx.closePath();
            ctx.stroke();

            ctx.clearRect(this.planet.pos.x - this.planet.radius, this.planet.pos.y - this.planet.radius,
                this.planet.radius * 2, this.planet.radius * 2);
            this.planet.drawBorder();
        } else {
            ctx.lineWidth = 1;
            ctx.strokeStyle = 'rgba(0,192,255,0.5)';
            ctx.beginPath();
            ctx.arc(this.center.x, this.center.y, this.radius, 0, Math.PI * 2, true);
            ctx.closePath();
            ctx.stroke();
        }
    }

function Planet(orbit, radius, time) {
    this.pos = new Point(0, 0);
    this.orbit = orbit;
    this.radius = radius;
    this.speed = Math.PI * 2 / (time * 1000);
    this.angle = ~~(Math.random() * 360);

    this.parent = null;

    this.animate = true;
    this.name;
    this.tile;
    this.ctx;
    this.orbit.setProperty({ 'planet': this });
    this.orbitVisibility = true;
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
                this.pos.y = this.orbit.center.y + this.orbit.radius * Math.sin(this.angle);
                this.angle += this.speed * deltaTime;
            } else {
                this.pos.x = this.parent.pos.x + this.orbit.radius * Math.cos(this.angle);
                this.pos.y = this.parent.pos.y + this.orbit.radius * Math.sin(this.angle);
                this.angle += this.speed * deltaTime * 10;
            }
            
        }
        
        if (typeof this.tile !== 'undefined') {
            this.tile.draw(this.pos.x, this.pos.y, // Центр тайла
                this.orbit.center, this.radius);
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
    draw: function (x, y, p) {
        var ctx = this.ctx;
        ctx.save();
        ctx.translate(x, y);
        ctx.rotate(Math.atan2(p.y - y, p.x - x) + Math.PI / 2);
        this.ctx.drawImage(this.img, this.x, this.y, this.width, this.height, -12, -12, 24, 24);
        ctx.restore();
    }
};