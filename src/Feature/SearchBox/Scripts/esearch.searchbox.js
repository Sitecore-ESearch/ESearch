document.addEventListener('DOMContentLoaded', function () {
    var $form = document.getElementById('search-box_form');
    var $keyword = document.getElementById('search-box_keyword');
    var $results = document.getElementById('search-box_results');
    var $button = document.getElementById('search-box_button');
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
            if (request.readyState === 4 && request.status === 200) {
                $results.innerHTML = request.responseText;
            } else {
                $results.innerHTML = request.responseText;
            }
        };
        request.onerror = function (event) {
            $results.innerHTML = request.responseText;
        };
        request.setRequestHeader('Content-Type', 'application/json');
        request.send(JSON.stringify(data));
    };

    // 検索ボックス入力時のイベントハンドラ
    $keyword.addEventListener('keyup', function () {
        // keyupされた時点で既にsendの実行が予約されていたら一旦削除
        if (send_timeout_id) {
            clearTimeout(send_timeout_id);
        }
        // send_timeout_milliseconds後にsendを実行するように予約
        send_timeout_id = setTimeout(send, send_timeout_milliseconds);
    });

    //検索ボタンのイベントハンドラ
    $button.addEventListener("click", function (e) {
        e.preventDefault();
        send();
    });
});
