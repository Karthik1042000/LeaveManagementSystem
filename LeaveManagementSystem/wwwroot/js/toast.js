function ToastMessage(heading, message, icon, bgColor) {
    $.toast({
        heading: heading,
        text: message,
        showHideTransition: 'fade', 
        icon: icon,
        hideAfter: false,
        position: 'top-right',
        stack: false,
        loader: true,
        textAlign: 'start',
        bgColor: bgColor,
        textColor: 'white',
        beforeShow: function (toast) {
            $(toast).addClass('toast-slide');
        },
        beforeHide: function (toast) {
            $(toast).removeClass('toast-slide');
            $(toast).addClass('toast-slide-hide');
        },
        afterHidden: function (toast) {
            $(toast).removeClass('toast-slide-hide');
        }
    });
}
