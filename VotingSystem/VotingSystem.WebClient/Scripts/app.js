/// <reference path="lib/_references.js" />
(function () {
    var dataPersister = persisters.get("http://votingsysyem.apphb.com/api");
    // var dataPersister = persisters.get("http://localhost:4414/api");
    var userPersister = dataPersister.userPersister;
    var electionsPersister = dataPersister.electionsPersister;
    
    var appLayout = new kendo.Layout('<div id="main-content"></div>');
    var router = new kendo.Router();

    function renderRouteIfLoggedIn(getView, getModel) {
        if (!userPersister.isUserLogged()) {
            router.navigate("/login");
        }
        else {
            renderRoute(getView, getModel);
        }
    };

    function renderRoute(getView, getModel) {
        var args = Array.prototype.slice.apply(arguments);
        args.splice(0, 2);
        getView.apply(null, args).then(function (viewHtml) {
            getModel.apply(null, args).then(function (model) {
                var template = kendo.template(viewHtml);
                var finalHtml = template(model);
                var view = new kendo.View(finalHtml, { model: model });
                
                appLayout.showIn("#main-content", view);
                //$("#my-elections").kendoMenu();
                //$("#logged-view").kendoMenu();
                
            }, function (modelErr) {
                console.log(modelErr);
            }).then(function () {
    
            }, function (err) {
                console.log(err);
            });
        }, function (viewError) {
            console.log(viewError);
        });
    }
    
    // rami
    router.route("/", function () { renderRouteIfLoggedIn(viewsFactory.getLoggedView, vmFactory.getLoggedViewModel) });
   
    // rami
    router.route("/login", function () { renderRoute(viewsFactory.getLoginView, vmFactory.getLoginViewModel) });

    // velko 
    router.route("/manage-election/:id", function (id) { renderRoute(viewsFactory.getManageElectionView, vmFactory.getManageElectionModel, id) });

    // todo: change view/model factories
    // asen
    router.route("/create-election", function (id) { renderRoute(viewsFactory.getCreateElectionView, vmFactory.getCreateElectionModel) });

    // not available in demo!
    // router.route("/own-votes/:id", function (id) { renderRoute(viewsFactory.getManageElectionView, vmFactory.getManageElectionModel, id) });

    router.route("/vote-election/:id", function (id) { renderRoute(viewsFactory.getInvitedElectionView, vmFactory.getInvitedElectionModel, id) });

    router.route("/see-results/:id", function (id) { renderRoute(viewsFactory.getSeeResultsView, vmFactory.getSeeResultsModel, id) });

    $(function () {
        appLayout.render("#application");
        router.start();
    });
}());
