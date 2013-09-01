/// <reference path="../lib/_references.js" />
window.vmFactory = (function () {
    var dataPersister = persisters.get("http://localhost:4414/api");
    var userPersister = dataPersister.userPersister;
    var router = new kendo.Router();

    function getLoggedViewModel() {
        var viewModel = {
            displayname: userPersister.currentUser.displayname,
            logout: function () {
                userPersister.logout().then(function () {
                    router.navigate("/login");
                });
            }
        };

        return kendo.observable(viewModel);
    }

    function getLoginViewModel() {
        var viewModel = {
            username: "",
            displayname: "",
            password: "",
            login: function () {
                var userData = {
                    username: this.get("username"),
                    password: this.get("password")
                };
                userPersister.login(userData).then(function () {
                    router.navigate("/");
                });
            },
            register: function () {
                var userData = {
                    username: this.get("username"),
                    displayname: this.get("displayname"),
                    password: this.get("password")
                };
                userPersister.register(userData).then(function () {
                    router.navigate("/");
                });;
            }
        };

        return kendo.observable(viewModel);
    };

    return {
        getLoginViewModel: getLoginViewModel,
        getLoggedViewModel: getLoggedViewModel
    }
}());