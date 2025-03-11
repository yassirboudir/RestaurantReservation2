document.addEventListener('DOMContentLoaded', function () {
    const track = document.querySelector('.carousel-track');
    const slides = Array.from(document.querySelectorAll('.carousel-slide'));
    const prevBtn = document.querySelector('.prev-btn');
    const nextBtn = document.querySelector('.next-btn');
    const dots = Array.from(document.querySelectorAll('.dot'));
    let currentIndex = 0;
    const slideCount = slides.length;
    const autoPlayInterval = 3000; // 3 seconds
    let autoPlayTimer;

    // Exit if elements are missing
    if (!track || slideCount === 0 || !prevBtn || !nextBtn || dots.length === 0) {
        return;
    }

    function updateSlidePosition() {
        track.style.transform = `translateX(-${currentIndex * 100}%)`;
        updateActiveDot();
    }

    function updateActiveDot() {
        dots.forEach(dot => dot.classList.remove('active'));
        dots[currentIndex].classList.add('active');
    }

    prevBtn.addEventListener('click', function () {
        currentIndex = currentIndex > 0 ? currentIndex - 1 : slideCount - 1;
        updateSlidePosition();
        resetAutoPlay();
    });

    nextBtn.addEventListener('click', function () {
        currentIndex = currentIndex < slideCount - 1 ? currentIndex + 1 : 0;
        updateSlidePosition();
        resetAutoPlay();
    });

    dots.forEach((dot, index) => {
        dot.addEventListener('click', function () {
            currentIndex = index;
            updateSlidePosition();
            resetAutoPlay();
        });
    });

    function autoPlay() {
        currentIndex = (currentIndex + 1) % slideCount;
        updateSlidePosition();
    }

    function startAutoPlay() {
        autoPlayTimer = setInterval(autoPlay, autoPlayInterval);
    }

    function resetAutoPlay() {
        clearInterval(autoPlayTimer);
        startAutoPlay();
    }

    updateSlidePosition();
    startAutoPlay();
});
