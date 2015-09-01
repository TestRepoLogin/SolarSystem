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