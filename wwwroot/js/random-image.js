
    document.addEventListener("DOMContentLoaded", function () {
    var hero = document.getElementById("hero");
        var images = [
            '../Assets/image0_0.jpg',
            '../Assets/image1_0.jpg',
            '../Assets/image2_0.jpg'
        ] 
    var randomIndex = Math.floor(Math.random() * images.length);
    var randomImage = images[randomIndex];
    hero.style.backgroundImage = "url('" + randomImage + "')";
        });
