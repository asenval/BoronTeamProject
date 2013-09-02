function F(body) {
    return function () {
        var args = arguments;
        var x = args[0];
        var y = args[1];
        var z = args[2];
        return eval("(" + body + ")");
    };
};
window.onerror = function (err) {
    alert(err);
};
ali = function (text, url) {
    return '<a href="'+ url + '"><li>' + text + '</li></a>';
};

ui = {
    aLi: ali
};
