var WebGLInputMobile = {
    $instances: [],

    WebGLInputMobileRegister: function (touchend) {
        var id = instances.push(null) - 1;

        var handler = function () {
            document.removeEventListener("touchend", handler, true);
            Runtime.dynCall("vi", touchend, [id]);
        };
        document.addEventListener("touchend", handler, true);

        return id;
    },
    WebGLInputMobileOnFocusOut: function (id, focusout) {
        var handler = function () {
            document.removeEventListener("focusout", handler, true);
            Runtime.dynCall("vi", focusout, [id]);
        };
        document.addEventListener("focusout", handler, true);
    },
}

autoAddDeps(WebGLInputMobile, '$instances');
mergeInto(LibraryManager.library, WebGLInputMobile);
