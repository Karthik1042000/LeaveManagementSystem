function ToastMessage(heading, message, icon, bgColor) {
    $.toast({
        heading: heading,
        text: message,
        showHideTransition: 'fade', // Use fade transition
        icon: icon,
        hideAfter: false,
        position: 'top-right',
        stack: false,
        loader: true,
        textAlign: 'start',
        bgColor: bgColor,
        textColor: 'white',
        beforeShow: function (toast) {
            // Add the custom CSS class for slide animation directly to the toast element
            $(toast).addClass('toast-slide');
        },
        beforeHide: function (toast) {
            // Add the custom CSS class for slide hide animation
            $(toast).removeClass('toast-slide');
            $(toast).addClass('toast-slide-hide');
        },
        afterHidden: function (toast) {
            // Remove the slide hide animation class after the toast is hidden
            $(toast).removeClass('toast-slide-hide');
        }
    });
}
