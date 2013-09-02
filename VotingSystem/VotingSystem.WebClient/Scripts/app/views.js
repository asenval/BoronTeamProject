/// <reference path="../lib/_references.js" />
window.viewsFactory = (function () {
    var rootUrl = "Scripts/partials/";
    var templates = {};

    function getTemplate(name) {
        var promise = new RSVP.Promise(function (resolve, reject) {
            if (templates[name]) {
                resolve(templates[name])
            }
            else {
                $.ajax({
                    // GET /login-form.hml
                    url: rootUrl + name + ".html",
                    type: "GET",
                    success: function (templateHtml) {
                        templates[name] = templateHtml;
                        resolve(templateHtml);
                    },
                    error: function (err) {
                        reject(err)
                    }
                });
            }
        });
        return promise;
    }

<<<<<<< HEAD
    function getLoginView() {
        return getTemplate("login-form");
    }

    function getLoggedView() {
        return getTemplate("logged-form");
    }

    function getElectionView() {
        return getTemplate("election-form");
    }

    return {
        getLoginView: getLoginView,
        getLoggedView: getLoggedView,
        getElectionView: getElectionView,
=======
    return {
        getLoginView: function () { return getTemplate("login-form") },
        getLoggedView: function () { return getTemplate("logged-form") },
        getManageElectionView: function () { return getTemplate("manage-election") }
>>>>>>> 3459c38428fe553af041ea3bd96af36ceb5b28f1
    }
}());