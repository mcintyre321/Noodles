$(function () {
    $.connection.hub.start().done(function () {
        $(document).ready(function () {
            var nodesUrlsOnPage = $(".noodles-object").map(function () {
                return $(this).data("noodles-node-url");
            }).get();
            $.connection.noodlesHub.server.subscribe(nodesUrlsOnPage);
        });
    });
    $.connection.noodlesHub.client.onChange = function (nodeUrls) {
        for (var i = 0; i < nodeUrls.length; i++) {
            var nodeUrl = nodeUrls[i];
            $.get(nodeUrl, function (nodeHtml) {
                var newFragments = {};
                var att = "data-noodles-fragment-id";
                $(nodeHtml).find("[" + att + "]").each(function () {
                    var fragmentId = $(this).attr(att);
                    var newFragment = $(this).html();
                    newFragments[fragmentId] = newFragment;
                    console.log(fragmentId + ":" + newFragment);
                });
                
                var nodeElement = $(".noodles-object[data-noodles-node-url='" + nodeUrl + "']");
                nodeElement.find("[" + att + "]").each(function () {
                    var fragmentId = $(this).attr(att);
                    var newFragment = newFragments[fragmentId];
                    if (!newFragment) {
                        console.log("removing fragment:" + fragmentId);
                        $(this).animate({ opacity: 0 }, function () { $(this).fadeOut(); });
                        delete newFragments[fragmentId];
                    }else {
                        if (newFragment != $(this).html()) {
                            console.log("updating fragment:" + fragmentId);
                            $(this).animate({ opacity: 0 }, function () {
                                $(this).html(newFragment);
                                $(this).animate({ opacity: 1 });
                            });
                            delete newFragments[fragmentId];
                        }else {
                            console.log("no change to fragment:" + fragmentId);
                        }
                    }
                    for (var f in newFragments) {
                        console.log("new fragment:" + f);
                    }
                    
                });
            });
        }
    };

});