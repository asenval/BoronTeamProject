/// <reference path="lib/_references.js" />
(function () {
    var dataPersister = persisters.get("http://localhost:4414/api");
    var userPersister = dataPersister.userPersister;

    var appLayout = new kendo.Layout('<div id="main-content"></div>');
    var router = new kendo.Router();

    function renderRouteIfLoggedIn(getView, getModel) {
        if (!userPersister.isUserLogged()) {
            router.navigate("/login");
        }
        else {
            renderRoute(getView, getModel);
        }
    }

    function renderRoute(getView, getModel) {
        getView().then(function (viewHtml) {
            getModel().then(function(model) {
                var template = kendo.template(viewHtml);
                var finalHtml = template(model);
                var view = new kendo.View(finalHtml, { model: model });
                appLayout.showIn("#main-content", view);
            });
        }).then(function () { }, function (err) {
            console.log(err);
        });
    };

    router.route("/", function () { renderRouteIfLoggedIn(viewsFactory.getLoggedView, vmFactory.getLoggedViewModel); });
   
    router.route("/login", function () { renderRoute(viewsFactory.getLoginView, vmFactory.getLoginViewModel) });

    router.route("/manage-election", function () { renderRoute(viewsFactory.getManageElectionView, vmFactory.getManageElectionModel) });

    $(function () {
        appLayout.render("#application");
        router.start();
    });
}());