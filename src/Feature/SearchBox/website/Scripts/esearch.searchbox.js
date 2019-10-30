document.addEventListener('DOMContentLoaded', function () {
    var $form = document.getElementById('search-box_form');
    var $keyword = document.getElementById('search-box_keyword');
    var $results = document.getElementById('search-box_results');
    var send_timeout_id = null
    var send_timeout_milliseconds = 200;

    function send() {
        var url = $form.getAttribute('ajax-url');
        var data = {
            Keyword: $keyword.value,
            SearchSettingsItemId: $form.getAttribute("search-settings-item-id")
        };
        var request = new XMLHttpRequest();
        request.open("POST", url, true);
        request.onload = function (event) {
            $results.innerHTML = request.responseText;
        };
        request.onerror = function (event) {
            $results.innerHTML = request.responseText;
        };
        request.setRequestHeader('Content-Type', 'application/json');
        request.send(JSON.stringify(data));
    };

    $keyword.addEventListener('keyup', function () {
        if (send_timeout_id) {
            clearTimeout(send_timeout_id);
        }
        send_timeout_id = setTimeout(send, send_timeout_milliseconds);
    });
});
