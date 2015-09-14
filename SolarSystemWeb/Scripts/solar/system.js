"use strict";
var App = function (canvas, planetsInfo, infoCallback) {
    this.planets = [];
    this.canvas = canvas;
    this.ctx;
    this.mouse;
    this.infoCallback = infoCallback;

    this.width = canvas.width;
    this.height = canvas.height;
    this._resources = {};
         
    this.init(planetsInfo);
};

App.prototype = {
    
    init: function (planetsInfo) {
        
        if (!this.canvas.getContext('2d')) {
            document.body.innerHTML = '<center>No support 2d context.</center>';
            return false;
        }
        this.ctx = this.canvas.getContext('2d');
        this.ctx.font = '16px monospace';
        this.ctx.textAlign = 'center';

        var globalCenter = new Point(this.canvas.width / 2, this.canvas.height / 2);
        this.mouse = new MouseController(this.canvas);

        var that = this;

        var findById = function (arr, id) {
            
            for (var i = 0; i < arr.length; i++) {
                if (arr[i].id == id) {
                    return arr[i];
                }
            }
        }

        var im = {
            store: this._resources,
            imagesAdded: 0,
            imagesLoaded: 0,
            add: function (url, name) {                
                var self = this;
                var image = new Image();                

                image.onload = function () {                    
                    self.imagesLoaded++;                    
                    
                    var planet = findById(that.planets, name);                                       
                    var tile = planet.isSun ? new Tile(that.ctx, this._resources['sun'], 0, 0, 100, 100) :
                                                      new Tile(that.ctx, that._resources[planet.id], 0, 0, this.width, this.height);

                    planet.setProperty({ tile: tile }, true);

                    if (self.imagesAdded == self.imagesLoaded) {
                        self.app.render(new Date());
                    }
                }                
                image.src = url;
                this.store[name] = image;
                this.imagesAdded++;                
            },
            app: this
        }


        var time = 40;
        var sunId = -1;       

        for (var i = 0; i < planetsInfo.length; ++i) {

            im.add('/Image/ShowPng/' + planetsInfo[i].id, planetsInfo[i].id);            

            var distance = planetsInfo[i].isSun ? 0 : 10 + planetsInfo[i].distance * 32;

            if (planetsInfo[i].isSun) {
                sunId = planetsInfo[i].id;
            }

            var orbit = new Orbit(globalCenter.clone(), distance).setProperty({ ctx: this.ctx, mouse: this.mouse }, true);
            var planet = new Planet(orbit, planetsInfo[i].radius, time).setProperty({
                id: planetsInfo[i].id,
                name: planetsInfo[i].name,
                needShow: planetsInfo[i].needShow,
                ownerId: planetsInfo[i].ownerId,
                ctx: this.ctx
            }, true);
            this.planets.push(planet);
            console.log(planet.name + ' ' + planetsInfo[i].radius.toFixed(1) + ' ' + planet.radius.toFixed(1));
            
            time += 20;
        }        
        

        for (var i = 0; i < this.planets.length; ++i) {
            if (this.planets[i].ownerId !== sunId) {
                var owner = null;
                for (var j = 0; j < this.planets.length; ++j) {
                    if (this.planets[i].ownerId == this.planets[j].id) {
                        owner = this.planets[j];
                        this.planets[i].setProperty({ parent: this.planets[j] }, true);                  
                        this.planets[i].orbit.setProperty({ center: this.planets[j].pos }, true);                  
                        break;
                    }
                }
            }
        }

    },
    
    setPlanetVisibility: function(id, flag) {
        for (var i = 0; i < this.planets.length; i++) {
            if (this.planets[i].id == id) {
                this.planets[i].setProperty({ needShow: flag }, true);
                break;
            }
        }
    },

    setOrbitVisibility: function (flag) {
        for (var i = 0; i < this.planets.length; i++) {            
                this.planets[i].setOrbitVisibility(flag);                           
        }
    },

    render: function (lastTime) {        
        var curTime = new Date();
        var self = this,
            ctx = this.ctx,
            planets = this.planets,
            mouse = this.mouse;
        requestAnimationFrame(function () {
            self.render(curTime);
        });
        ctx.clearRect(0, 0, this.width, this.height);

        var showInfo = -1;
        for (var i = 0, il = planets.length; i < il; ++i) {
            if (!planets[i].needShow)
                continue;

            if (planets[i].orbitVisibility)
                planets[i].orbit.draw();

            planets[i].render(curTime - lastTime);
            if (Math.abs(planets[i].pos.x - mouse.pos.x) < planets[i].radius
                && Math.abs(planets[i].pos.y - mouse.pos.y) < planets[i].radius) {
                showInfo = i;
                if (mouse.pressed) {
                    planets[i].animate = planets[i].animate ? false : true;
                    infoCallback(planets[showInfo].id);
                }
            }
        }

        if (showInfo > -1) {
            planets[showInfo].showInfo();
            planets[showInfo].drawBorder();
            document.body.style.cursor = 'pointer';            
        } else {
            document.body.style.cursor = 'default';
        }
        mouse.pressed = false;
    }    
}