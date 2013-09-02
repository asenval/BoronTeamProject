/// <reference path="../lib/_references.js" />
window.vmFactory = (function () {
    var dataPersister = persisters.get("http://localhost:4414/api");
    var userPersister = dataPersister.userPersister;
    var electionsPersister = dataPersister.electionsPersister;
    var router = new kendo.Router();

    function getElectionViewModel(id) {
        return electionsPersister.getElection(id).then(function (election) {
            var viewModel = {
                title: election.title,
                invitedDisplayname: "",
                //inviteUser: function () {
                //    userPersister.invite().then(function () {
                        
                //    });
                //},
            }
            var obs = kendo.observable(viewModel);
            return obs;
        });
    }
    
    function getLoggedViewModel() {
        return electionsPersister.getMyElections().then(function (elections) {
            var viewModel = {
                displayname: localStorage["displayname"],
                myElections: elections,
                log: function () {
                    userPersister.logout().then(function () {
                        router.navigate("/login");
                    }, function (errMsg) {
                        console.log(errMsg);
                    });
                },
                createElection: function () {
                    var electionData = {
                    };
                    electionsPersister.createElection(electionData).then(function (data) {
                        console.log(data);
                        router.navigate("/");
                    }, function (errMsg) {
                        console.log(errMsg);
                    })
                }
            }
            var obs = kendo.observable(viewModel);
            return obs;
        });
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
        return new RSVP.Promise(function (res, rej) {
            res(kendo.observable(viewModel));
        });
    };

    function getManageElectionModel() {
        return kendo.observable({ title: "My Election!", users: { name: "Gosho Goshev", name: "Pesho Peshev" } });
    };

    return {
        getLoginViewModel: getLoginViewModel,
        getLoggedViewModel: getLoggedViewModel,
        getElectionViewModel: getElectionViewModel,
        getManageElectionModel: getManageElectionModel
    }
}());