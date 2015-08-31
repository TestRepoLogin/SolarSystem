$().ready(function () {
    var canvas = document.getElementById("star_field");
    var starry = new Starry(canvas, 3000, 30);

    starry.drawStars();    
});
