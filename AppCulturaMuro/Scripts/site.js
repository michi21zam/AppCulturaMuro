// Tiempo relativo
function relativeTime(dateStr) {
    var date = new Date(dateStr);
    var now = new Date();
    var diff = Math.floor((now - date) / 1000);

    if (diff < 60) return 'just now';
    if (diff < 3600) return Math.floor(diff / 60) + ' min ago';
    if (diff < 86400) return Math.floor(diff / 3600) + 'h ago';
    if (diff < 604800) return Math.floor(diff / 86400) + 'd ago';

    return date.toLocaleDateString('en-US', { month: 'short', day: 'numeric' });
}
function clearSearch() {
    document.getElementById('searchInput').value = '';
    window.location.href = '/Home/Index';
}

document.addEventListener('DOMContentLoaded', function () {

    // Aplicar tiempo relativo
    document.querySelectorAll('.cm-relative-time').forEach(function (el) {
        var date = el.getAttribute('data-date');
        if (date) el.textContent = relativeTime(date);
    });

    // Card entrance animation
    var cards = document.querySelectorAll('.cm-post-card');
    cards.forEach(function (card, i) {
        card.style.opacity = '0';
        card.style.transform = 'translateY(16px)';
        card.style.transition = 'opacity .35s ease ' + (i * 0.05) + 's, transform .35s ease ' + (i * 0.05) + 's';
        setTimeout(function () {
            card.style.opacity = '1';
            card.style.transform = 'translateY(0)';
        }, 50 + (i * 50));
    });
});