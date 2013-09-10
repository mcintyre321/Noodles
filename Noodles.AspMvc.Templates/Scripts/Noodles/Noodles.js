﻿(function () { 
    
    $(document).on("click", ".show-setter", function (e) {
        var $property = $(this).closest("[data-node-property]");
        var $setter = $property.find("> [data-node-setter]");
        var $getter = $property.find("> [data-node-getter]");
        $getter.fadeOut(function () {
            $setter.fadeIn(function () {
                $setter.find('input,textarea,select').filter(':enabled:visible:not([readonly="readonly"]):not([type="hidden"])').first().focus();

                $.validator.unobtrusive.parseDynamicContent($setter);
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

    $(document).on("click", ".popover .close", function (e) { $(this).closest(".popover").hide(); });

    $(document).on("click", ".nodeMethodsMenuLink", function (e) {
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

    $(document).on('click', ".noodles-popup", function (e) {
        e.preventDefault();
        var $link = $(this);
        if ($link.attr("data-custom-method-handler")) {
            return false;
        }
        $link.closest(".popover").hide();


        var methodsPanelId = "method-" + $link.attr("href").replace(/\//g, "_");
        if ($("#" + methodsPanelId).length == false) {
            ensureLazyElement(
                methodsPanelId,
                $link.attr("href"),
                function() { showMethodForm($link); },
                function(body) {
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
        if ($link.hasClass("auto-submit")) {
            $method.find("form").submit();
        } else {
            $method.modal({ show: true, backdrop: true });
            $method.find(":input:visible:enabled:first").focus();

            $.validator.unobtrusive.parseDynamicContent($method.find("form"));
        }

    };
    $(document).on("submit", "form.node-form", function(e) {
        var $submitButton = $(this).find("> input[type=submit]");
        //if (!submitButton[0]) {
        //    submitButton = $(this).parents('div.nodeMethod').find('.submitMethod');
        //}
        

        if (!$(this).valid()) {
            e.preventDefault();
            //$submitButton.trigger("click");
        }
    });
    $(document).on("click", "form.node-form > input[type=submit]", function (e) {
        var $container = $(this).closest(".form.node-form");
        var $form = $container.is("form") ? $container : $container.find("form");
        if (!$form.valid()) return false;
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
                    $.validator.unobtrusive.parseDynamicContent($form);

                }
            },
            complete: function () {
                //$("#ProgressDialog").dialog("close");
            }
        };
        $.ajax(ajaxOptions);
        return false;
    });

    $(document).on("click", "a[data-modal=true]", function(e) {
        var $link = $(this);
        $.ajax($link.attr("href"), {
            success: function(data, textStatus) {
                var $modal = $(modalHtml());
                $modal.find(".title").append($link.html());
                //$modal.find(".modal-footer").append($formHtml.find("button").remove());
                $modal.find(".modal-body").append($(data));
                $modal.appendTo($("body")).modal();
            }
        });

        return false;
    });
})();