/// <reference path="lib/_references.js" />
(function () {

    var dataPersister = persisters.get("http://localhost:4414/api");
    var userPersister = dataPersister.userPersister;
    var electionsPersister = dataPersister.electionsPersister;
    electionsPersister.getMyElections().then(function (data) {
        console.log(data);
    });

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
            var model = getModel();
            var view = new kendo.View(viewHtml, { model: model });
            appLayout.showIn("#main-content", view);
        });
   };


    router.route("/", function () { renderRouteIfLoggedIn(viewsFactory.getLoggedView, vmFactory.getLoggedViewModel) });
   
        router.route("/login", function() { renderRoute(viewsFactory.getLoginView, vmFactory.getLoginViewModel)});


    $(function () {
        appLayout.render("#application");
        router.start();
    });
}());
