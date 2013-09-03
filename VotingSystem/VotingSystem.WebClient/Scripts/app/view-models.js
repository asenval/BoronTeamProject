﻿/// <reference path="../lib/_references.js" />
window.vmFactory = (function () {
    var dataPersister = persisters.get("http://votingsysyem.apphb.com/api");
    var userPersister = dataPersister.userPersister;
    var electionsPersister = dataPersister.electionsPersister;
    var router = new kendo.Router();

    function getLoggedViewModel() {
        return electionsPersister.getFilteredElections().then(function (elections) {
            var viewModel = {
                displayname: localStorage["displayname"],
                myElections: elections[0],
                invitedElections: elections[1],
                votedElections: elections[2],
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

    function getInvitedElectionModel(id) {
        return electionsPersister.getElection(id).then(function (election) {
            var viewModel = {
                displayname: localStorage["displayname"],
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
                },
                electionTitle: election.title,
                questions: election.questions,
                voteValue: "",
                submit: function () {
                    console.log(this.voteValue);
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

    function getManageElectionModel(id) {
        var election = electionsPersister.getElection(id).then(function(election) {
            election.inviteUsers = function () {
                election.invitedUsersDisplayNameString += "," + $("#tb-invite-user").val();
                election.invitedUsersDisplayNameString = ui.cleanListString(election.invitedUsersDisplayNameString);
                electionsPersister.putElection(election);
                router.navigate("/manage-election/" + election.id);
            };
            election.seeResults = function () {
                router.navigate("/see-results/" + election.id);
            };
            election.closeElection = function () {
                electionsPersister.closeElection(election);
                router.navigate("/see-results/" + election.id);
            };
            return election;
        });
        
        return election.then(kendo.observable, console.log);
    };

    return {
        getLoginViewModel: getLoginViewModel,
        getLoggedViewModel: getLoggedViewModel,
        getManageElectionModel: getManageElectionModel,
        getInvitedElectionModel: getInvitedElectionModel,
    }
}());
