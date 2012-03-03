﻿$(document).ready(function() {
    $(".popover .close").live('click', function(e) { $(this).closest(".popover").hide(); });
    $(".nodeActionsLink").live('click', function(e) {
        var $link = $(this);
        if (e.target != this) return false;
        var nodePath = $link.attr("data-nodepath");
        var nodeId = $link.attr("data-nodeid");
        if ($('#actions-' + nodeId).length) {
            return false;
        }
        if ($link.attr("data-content") === undefined) {
            $link.attr("data-content", "");
            var actionUrl = nodePath + "?action=getNodeActions";
            $.get(actionUrl, { }, function(data) {
                $link.attr("data-content", data)
                    .popover({
                        trigger: "manual",
                        placement: "right",
                        offset: 0,
                        html: true,
                        delayOut: 500,
                        title: function() { return "Actions<a class='close' href='#'>×</a>"; }
                    })
                    .click(function() {
                        $(".popover").hide();
                        $link.popover('show');

                        console.log("showing");
                        var actionName = $link.attr("data-actionname");
                        if (actionName) {
                            $("#" + nodeId + "_" + actionName + "_actionlink").click();

                        }
                    })
                    .click();

            });
        }
        return false;
    });
    $(".nodeActionsPanelLink").live('click', function(e) {
        var panelId = $(this).attr("id") + "panel";
        $("#" + panelId).modal({ show: true, backdrop: true });
        $("#" + panelId + " :input:visible:enabled:first").focus();
        e.stopPropagation();
    });
    $(".post-via-ajax").live('click', function(e) {
        var $form = $(this).closest("form");
        $.ajax({
            url: $form.attr('action'),
            type: "POST",
            data: $form.serialize(),
            success: function(data) {
                if (data === "OK") {
                    window.location.reload();
                } else {
                    $form.parent().html(data);
                }
            },
            error: function(jqXhr, textStatus, errorThrown) {

                console.log("Error '" + jqXhr.status + "' (textStatus: '" + textStatus + "', errorThrown: '" + errorThrown + "')");
            },
            complete: function() {
                //$("#ProgressDialog").dialog("close");
            }
        });
        return false;
    });
});