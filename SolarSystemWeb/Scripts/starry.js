﻿function Starry(canvas, count, sparkCount) {

    var elements = [];
    var sizes = { micro: 0.2, mini: 0.4, medium: 0.6, big: 0.8, max: 1.0 };
    var delay = 1000 / 30;
    var opacity = .8;

    function randomInt(a, b) {
        return Math.floor(Math.random() * (b - a + 1) + a);
    }

    function randomFloatAround(num) {
        var plusminus = randomInt(0, 1000) % 2,
            val = num;
        if (plusminus)
            val += 0.1;
        else
            val -= 0.1;
        return parseFloat(val.toFixed(1));
    }

    function randomColor(alpha) {
        return 'rgba(' + randomInt(0, 255) + ',' + randomInt(0, 255) + ',' + randomInt(0, 255) + ',' + alpha + ')';
    }

    function star(x, y, size, alpha) {
        var radius = sizes[size];
        var context = canvas.getContext('2d');

        var gradient = context.createRadialGradient(x, y, 0, x + radius, y + radius, radius * 2);
        gradient.addColorStop(0, randomColor(alpha));
        gradient.addColorStop(1, randomColor(alpha));

        context.beginPath();
        context.clearRect(x - radius - 1, y - radius - 1, radius * 2 + 2, radius * 2 + 2);
        context.closePath();

        context.beginPath();
        context.arc(x, y, radius, 0, 2 * Math.PI);
        context.fillStyle = gradient;
        context.fill();

        return {
            'x': x,
            'y': y,
            'size': size,
            'alpha': alpha
        };
    }

    function generate(starsCount, opacity) {
        var keys = Object.keys(sizes);

        for (var i = 0; i < starsCount; i++) {
            var x = randomInt(2, canvas.offsetWidth - 2),
                y = randomInt(2, canvas.offsetHeight - 2),
                size = keys[randomInt(0, keys.length - 1)];

            elements.push(star(x, y, size, opacity));
        }
    }

    function spark(numberOfStarsToAnimate) {

        for (var i = 0; i < numberOfStarsToAnimate; i++) {
            var id = randomInt(0, elements.length - 1),
                obj = elements[id],
                newAlpha = obj.alpha;
            do {
                newAlpha = randomFloatAround(obj.alpha);
            } while (newAlpha < .3 || newAlpha > 1)

            elements[id] = star(obj.x, obj.y, obj.size, newAlpha);
        }

        requestAnimFrame(function () {
            spark(numberOfStarsToAnimate);
        });
    }

    generate(count, opacity);

    var requestAnimFrame = (function () {
        return window.requestAnimationFrame || window.webkitRequestAnimationFrame
            || window.mozRequestAnimationFrame || window.oRequestAnimationFrame
            || window.msRequestAnimationFrame || function (callback) {
                window.setTimeout(callback, delay);
            };
    })();

    this.drawStars = function () {        
        spark(sparkCount);
    }
}
