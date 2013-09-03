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


ui = (function () {
    function unique(myString, by) {
        by = by || "";
        var sorted = myString.split(by).sort();
        var last = null;
        var ret = [];
        for(var ii = 0; ii < sorted.length; ++ii) {
            if (last != (last = sorted[ii])) {
                ret.push(last);
            }
        }
        return ret;
    }
    function cleanArray(actual){
        var newArray = new Array();
        for(var i = 0; i<actual.length; i++){
            if (actual[i]){
                newArray.push(actual[i]);
            }
        }
        return newArray;
    }
    function cleanListString(str, by) {
        by = by || ",";
        return cleanArray(unique(str, by)).join(",");
    }
    var ali = function (text, url) {
        return '<a href="' + url + '"><li>' + text + '</li></a>';
    };
    var addUsers = function (name) {
        window.alert(name);
    };
    return {
        aLi: ali,
        addUsers: addUsers,
        unique: unique,
        cleanListString: cleanListString
    };
}());
