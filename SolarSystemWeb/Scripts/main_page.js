$().ready(function () {
    var canvas = document.getElementById("star_field");
    var starry = new Starry(canvas);

    starry.drawStars(3000, 30);
});