//amaze
$(document).ready(function () {
    
    $(document).on("click", ".show-setter", function (e) {
        var $property = $(this).closest("[data-node-property]");
        var $setter = $property.find("> [data-node-setter]");
        var $getter = $property.find("> [data-node-getter]");
        $getter.fadeOut(function () {
            $setter.fadeIn(function () {
                $setter.find('input,textarea,select').filter(':enabled:visible:not([readonly="readonly"]):not([type="hidden"])').first().focus();

                $.validator.unobtrusive.parseDynamicContent($setter.find("input,textarea,select"));
            });
        });
        e.preventDefault();
        return false;
    });
    $(document).on("click", ".hide-setter", function (e) {
        var $property = $(this).closest("[data-node-property]");
        var $setter = $property.find("> [data-node-setter]");
        var $getter = $property.find("> [data-node-getter]");
        $setter.fadeOut(function () {
            $getter.fadeIn();
        });

        e.preventDefault();
        return false;
    });

    $(".popover .close").live('click', function (e) { $(this).closest(".popover").hide(); });

    $(".nodeMethodsMenuLink").live('click', function (e) {
        var $link = $(this);
        if (e.target != this) return false;
        var methodsPanelId = "methods-" + $link.attr("href").replace(/\//g, "_");
        if ($("#" + methodsPanelId).length == false) {
            ensureLazyElement(
                methodsPanelId,
                $link.attr("href"),
                function () { showMethodsMenu($link); }
            );
        }
        return false;
    });

    function modalHtml() {
        return '<div class="modal nodeMethod" id="myModal"><div class="modal-header"><a class="close" data-dismiss="modal">×</a><h3 class="title"></h3></div><div class="modal-body"></div><div class="modal-footer"></div></div>';
    }

    var showMethodsMenu = function ($link) {
        var methodsNodeId = "methods-" + $link.attr("href").replace(/\//g, "_");
        $link.attr("data-content", $("#" + methodsNodeId).html());
        $link.popover({
            trigger: "manual",
            placement: ($link.offset().left > ($(window).width() / 2)) ? "left" : "right",
            offset: 0,
            html: true,
            delayOut: 500,
            title: function () { return "Actions<a class='close' href='#'>×</a>"; }
        }).click(function () {
            $(".popover").hide();
            $link.popover('show');
        }).click();
    };

    function ensureLazyElement(id, path, callback, transform) {
        if ($("#" + id).length) {
            callback();
            return true;
        }
        $.get(path, {}, function (html) {
            var $html;
            if (transform) {
                $html = transform(html);
            } else {
                $html = $(html);
            }
            $('<div>').attr("id", id).css("display", "none").append($html).appendTo($("body"));
            callback();
        });
        return true;
    }

    $(".nodeMethodLink").live('click', function (e) {
        e.preventDefault();
        var $link = $(this);
        if ($link.attr("data-custom-method-handler")) {
            return false;
        }
        $link.closest(".popover").hide();
        //if (e.target != this) return false; //why???
        var methodsPanelId = "method-" + $link.attr("href").replace(/\//g, "_");
        if ($("#" + methodsPanelId).length == false) {
            ensureLazyElement(
                methodsPanelId,
                $link.attr("href"),
                function () { showMethodForm($link); },
                function (body) {
                    var $formHtml = $(body);
                    var $modal = $(modalHtml());
                    $modal.find(".title").append($link.html());
                    $modal.find(".modal-footer").append($formHtml.find("button").remove());
                    $modal.find(".modal-body").append($formHtml);
                    return $modal;
                });
        } else {
            showMethodForm($link);
        }
        return false;
    });
    var showMethodForm = function ($link) {

        var methodsPanelId = "method-" + $link.attr("href").replace(/\//g, "_");
        var $method = $("#" + methodsPanelId);
        $method.modal({ show: true, backdrop: true });
        $method.find(":input:visible:enabled:first").focus();

        $.validator.unobtrusive.parseDynamicContent($method.find("form").find("input,textarea,select").first());


    };
    $(document).on("submit", "form.nodeMethod", function(e) {
        var submitButton = $(this).find(".submitMethod");
        if (!submitButton[0]) {
            submitButton = $(this).parents('div.nodeMethod').find('.submitMethod');
        }
        if (submitButton[0]) {
            e.preventDefault();
            submitButton.click();
        }
    });
    $(document).on("click", ".submitMethod", function (e) {
        var $container = $(this).closest(".nodeMethod");
        var $form = $container.is("form") ? $container : $container.find("form");
        var formdata = false;
        if (window.FormData) {
            formdata = new FormData($form[0]);
        };

        var formAction = $form.attr('action');
        var ajaxOptions = {
            url: formAction,
            data: formdata ? formdata : $form.serialize(),
            cache: false,
            contentType: false,
            processData: false,
            type: 'POST',
            success: function (data, textStatus, jqXHR) {
                if (jqXHR.getResponseHeader('IsAjaxRedirect') === 'true') {
                    window.location = jqXHR.getResponseHeader('Location');
                    return;
                };
                //if ($link) {
                //    var $table = $link.closest(".dataTable");
                //    if ($table.length) {
                //        $table.dataTable().fnDraw(false);
                //        $method.modal("hide");
                //        $method.remove();
                //        return;
                //    }
                //}
                window.location.reload();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                if (errorThrown == "Bad Request") {
                    var $html = $(jqXhr.responseText);
                    if ($container.hasClass("modal")) {
                        var $buttons = $html.find("button").remove();
                        $container.find(".modal-footer").empty().append($buttons);
                    } else if ($form.hasClass("form-inline")) {
                        $html.find("div.controls").css("display", "inline");
                        $html.find("div.control-group").css("display", "inline");
                        $html.addClass("form-inline");
                    }
                    $form.replaceWith($html);
                    $.validator.unobtrusive.parseDynamicContent($form.find("input,textarea,select").first());

                }
            },
            complete: function () {
                //$("#ProgressDialog").dialog("close");
            }
        };
        $.ajax(ajaxOptions);
        return false;
    });
});