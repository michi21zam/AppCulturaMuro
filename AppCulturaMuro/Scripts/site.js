// Card entrance animation on load
document.addEventListener('DOMContentLoaded', function () {
    var cards = document.querySelectorAll('.cm-card');
    cards.forEach(function (card, i) {
        card.style.opacity = '0';
        card.style.transform = 'translateY(20px)';
        card.style.transition = 'opacity .4s ease ' + (i * 0.06) + 's, transform .4s ease ' + (i * 0.06) + 's';
        setTimeout(function () {
            card.style.opacity = '1';
            card.style.transform = 'translateY(0)';
        }, 50);
    });
});