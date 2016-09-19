// Write your Javascript code.

(function ($) {
    $.ajaxLoading = function (options) {
        // wrappers
        var loadingSuccess = function(data) {
            $.ajaxLoading.cleanup();
            if (options.success) options.success(data);
        };
        var loadingError = function() {
            $.ajaxLoading.cleanup();
            $.ajaxLoading.errorStart();
            if (options.error) options.error();
        };
        var loadingDone = function() {
            $.ajaxLoading.cleanup();
            if (options.done) options.done();
        };

        // override success/error/done with our wrappers
        var defaults = {
            success: loadingSuccess,
            error: loadingError,
            done: loadingDone
        };
        var statusCodeDefaults = {
            400: function (data) {
                location.reload();
            },
            401: function (data) {
                location.reload();
            }
        };
        // first {} for .extend is important to not override initial options (used in wrappers)
        options.statusCode = $.extend({}, statusCodeDefaults, options.statusCode);
        var opts = $.extend({}, options, defaults);

        // initialize delayed loading layer
        $.ajaxLoading.loadingStart();
        // start regular AJAX call
        $.ajax(opts);
        return this;
    };

    $.ajaxLoading.loadingStart = function () {
        // check if we are already started
        if ($.ajaxLoading.timer) return;

        // not started yet - start with delay
        $.ajaxLoading.timer = setTimeout(function () {
            $($.ajaxLoading.defaults.selector).addClass($.ajaxLoading.defaults.loadingClass);
            clearTimeout($.ajaxLoading.timer);
        }, $.ajaxLoading.defaults.timeout);
    };

    $.ajaxLoading.errorStart = function() {
        $($.ajaxLoading.defaults.selector).addClass($.ajaxLoading.defaults.errorClass);
        $.ajaxLoading.timer = setTimeout(function() {
            $.ajaxLoading.cleanup();
        }, $.ajaxLoading.defaults.timeout);
    };
    $.ajaxLoading.cleanup = function () {
        // clear timeout and hide layers
        clearTimeout($.ajaxLoading.timer);
        $.ajaxLoading.timer = null;
        $($.ajaxLoading.defaults.selector).removeClass($.ajaxLoading.defaults.loadingClass);
        $($.ajaxLoading.defaults.selector).removeClass($.ajaxLoading.defaults.errorClass);
    };

    $.ajaxLoading.timer = null;

    // plugin defaults
    $.ajaxLoading.defaults = {
        selector: 'body',
        loadingClass: 'loading',
        errorClass: 'error',
        timeout: 500 
    };
}(jQuery));