$(document).ready(function () {


    $(".popover .close").live('click', function (e) { $(this).closest(".popover").hide(); });

    $(".nodeMethodsMenuLink").live('click', function (e) {
        var $link = $(this);
        if (e.target != this) return false;
        var methodsPanelId = "methods-" + $link.attr("data-nodeid");
        if ($("#" + methodsPanelId).length == false) {
            ensureLazyElement(
                methodsPanelId,
                $link.attr("data-nodemethodspath"),
                function () { showMethodsMenu($link); }
            );
        }
        return false;
    });

    function modalHtml() {
        return '<div class="modal objectMethod" id="myModal"><div class="modal-header"><a class="close" data-dismiss="modal">×</a><h3 class="title"></h3></div><div class="modal-body"></div><div class="modal-footer"></div></div>';
    }

    var showMethodsMenu = function ($link) {
        var methodsNodeId = "methods-" + $link.attr("data-nodeid");
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
            //            var actionName = $link.attr("data-actionname");
            //            if (actionName) {
            //                $("#" + nodeId + "_" + actionName + "_actionlink").click();
            //            }
        }).click();
    };

    function ensureLazyElement(id, path, callback, transform) {
        if ($("#" + id).length) {
            callback();
            return true;
        }
        $.get(path, {}, function (html) {
            if (transform) {
                html = transform(html)[0].outerHTML;
            }
            $('<div>').attr("id", id).css("display", "none").html(html).appendTo($("body"));
            callback();
        });
        return true;
    }

    $(".nodeMethodLink").live('click', function (e) {
        var $link = $(this);
        $link.closest(".popover").hide();
        if (e.target != this) return false;
        var methodsPanelId = "method-" + $link.attr("data-nodeid");
        if ($("#" + methodsPanelId).length == false) {
            ensureLazyElement(
                methodsPanelId,
                $link.attr("data-nodepath"),
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

        //var panelId = $(this).attr("id") + "panel";
        var methodsPanelId = "method-" + $link.attr("data-nodeid");
        $("#" + methodsPanelId).modal({ show: true, backdrop: true });
        $("#" + methodsPanelId + " :input:visible:enabled:first").focus();
    };

    $(".submitMethod").live('click', function (e) {

        var $container = $(this).closest(".objectMethod");
        var $form = $container.find("form");
        $.ajax({
            url: $form.attr('action'),
            type: "POST",
            data: $form.serialize(),
            success: function (data) {
                window.location.reload();
            },

            error: function (jqXhr, textStatus, errorThrown) {
                if (errorThrown == "Conflict") {
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
                }
            },
            complete: function () {
                //$("#ProgressDialog").dialog("close");
            }
        });
        return false;
    });
});