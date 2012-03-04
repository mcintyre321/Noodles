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

    function modalHtml(title, body, footer) {
        return '<div class="modal objectMethod" id="myModal"><div class="modal-header"><a class="close" data-dismiss="modal">×</a><h3>' + title + '</h3></div><div class="modal-body">' + body + '</div><div class="modal-footer">' + footer + '</div></div>';
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
        $.get(path, {}, function (data) {
            if (transform) {
                data = transform(data);
            }
            $('<div>').attr("id", id).html(data).hide().appendTo($("body"));
            callback();
        });
        return true;
    }

    $(".nodeMethodLink").live('click', function (e) {
        var $link = $(this);
        if (e.target != this) return false;
        var methodsPanelId = "method-" + $link.attr("data-nodeid");
        if ($("#" + methodsPanelId).length == false) {
            ensureLazyElement(
                methodsPanelId,
                $link.attr("data-nodepath"),
                function () { showMethodForm($link); },
                function (body) {
                    return modalHtml($link[0].innerHTML, body, "<button class='btn primary submitMethod'>Submit</button>");
                });
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

        var $form = $(this).closest(".objectMethod").find("form");
        $.ajax({
            url: $form.attr('action'),
            type: "POST",
            data: $form.serialize(),
            success: function (data) {
                window.location.reload();
            },

            error: function (jqXhr, textStatus, errorThrown) {
                if (errorThrown == "Conflict") {
                    $form.html(jqXhr.responseText);
                }
            },
            complete: function () {
                //$("#ProgressDialog").dialog("close");
            }
        });
        return false;
    });
});