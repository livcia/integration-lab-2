window.setupClickOutside = function(element, componentRef) {
    document.addEventListener('click', function(event) {
        const clickedMenu = event.target.closest('.menu-popup');
        const clickedBtn = event.target.closest('.btn-icon');
        if (clickedMenu) {
            return;
        }
        if (clickedBtn) {
            return;
        }
        componentRef.invokeMethodAsync('CloseMenu');
    });
};
