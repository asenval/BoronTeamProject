/// <reference path="lib/_references.js" />
(function () {
    var dataPersister = persisters.get("http://localhost:4414/api");
    var userPersister = dataPersister.userPersister;

    var appLayout = new kendo.Layout('<div id="main-content"></div>');
    var router = new kendo.Router();

    router.route("/", function () {
        if (!userPersister.isUserLogged()) {
            router.navigate("/login");
        }
        else {
            viewsFactory.getLoggedView()
               .then(function (loggedViewHtml) {
                   var loggedVm = vmFactory.getLoggedViewModel();
                   var view = new kendo.View(loggedViewHtml, { model: loggedVm });
                   appLayout.showIn("#main-content", view);
               });
        }
    });

    router.route("/login", function () {
        if (userPersister.isUserLogged()) {
            router.navigate("/");
        }
        else {
            viewsFactory.getLoginView()
				.then(function (loginViewHtml) {
				    var loginVm = vmFactory.getLoginViewModel();
				    var view = new kendo.View(loginViewHtml, { model: loginVm });
				    appLayout.showIn("#main-content", view);
				});
        }
    });

    $(function () {
        appLayout.render("#application");
        router.start();
    });
}());