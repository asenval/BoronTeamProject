﻿/// <reference path="../lib/_references.js" />
window.persisters = (function () {
    var currentUsername = null;
    function saveUserData(userData) {
        localStorage.setItem("displayname", userData.displayName);
        localStorage.setItem("sessionKey", userData.sessionKey);
    }

    function clearUserData() {
        localStorage.removeItem("displayname");
        localStorage.removeItem("sessionKey");
        currentUsername = null;
    }

    var MainPersister = Class.create({
        init: function (url) {
            this.rootUrl = url;
            this.userPersister = new UserPersister(this.rootUrl);
            this.electionsPersister = new ElectionsPersister(this.rootUrl);
            this.clearUserData = clearUserData;
        }
    });

    var UserPersister = Class.create({
        init: function (rootUrl) {
            this.rootUrl = rootUrl + "/users";
            this.currentUser = {
                displayname: localStorage["displayname"],
                sessioneKey: localStorage["sessionKey"]
            };
        },

        login: function (user) {
            var url = this.rootUrl + "/login";
            var userData = {
                username: user.username,
                authCode: CryptoJS.SHA1(user.username + user.password).toString()
            };

            return httpRequester.postJSON(url, userData)
				.then(function (data) {
				    currentUsername = userData.username;
				    saveUserData(data);
				    return data;
				},
                function (errMsg) {
                    console.log(errMsg);
                });
        },

        register: function (user) {
            var url = this.rootUrl + "/register";
            var userData = {
                username: user.username,
                displayname: user.displayname,
                authCode: CryptoJS.SHA1(user.username + user.password).toString()
            }

            return httpRequester.postJSON(url, userData)
				.then(function (data) {
				    currentUsername = userData.username;
				    saveUserData(data);
				    return data;
				},
                function (errMsg) {
                    console.log(errMsg);
                });
        },

        logout: function () {
            var sessionKey = localStorage["sessionKey"];
            var url = this.rootUrl + "/logout";
            var headers = {
                "X-sessionKey": sessionKey
            };

            return httpRequester.putJSON(url, sessionKey, headers)
		    .then(function () {
		        clearUserData();
		    },
            function (errMsg) {
                console.log(errMsg);
            });
        },

        isUserLogged: function () {
            if (localStorage["sessionKey"] != null) {
                return true;
            }

            return false;
        }
    });

    var ElectionsPersister = Class.create({
        init: function (rootUrl) {
            this.rootUrl = rootUrl + "/elections";
        },

        getAllElections: function () {
            var url = this.rootUrl;
            return httpRequester.getJSON(url)
		    .then(function (data) {
		        return data;
		    },
            function (errMsg) {
                console.log(errMsg);
            });
        },

        getFilteredElections: function () {
            return this.getAllElections().then(function (elections) {

                var displayname = localStorage["displayname"];
                var myElections = [];
                var votedElections = [];
                var invitedElections = [];
                var closedElections = [];

                for (var i = 0; i < elections.length; i++) {
                    var election = elections[i];

                    if (election.status.toLowerCase() != "open") {
                        closedElections.push(election);
                    }
                    else if (election.ownerNickname == displayname) {
                        myElections.push(election);
                    }
                    else if ((election.invitedUsersDisplayNameString || "").indexOf(displayname) != -1) {
                        invitedElections.push(election);
                    }
                    else if ((election.votedUsersDisplayNamesString || "").indexOf(displayname) != -1) {
                        votedElections.push(election);
                    }
                }

                return [myElections, invitedElections, votedElections, closedElections];
            });
        },

        getElection: function (id) {
            var url = this.rootUrl + "/" + id + "/GetById";
            return httpRequester.getJSON(url)
            .then(function (data) {
                return data;
            },
            function (errMsg) {
                console.log(errMsg);
            });
        },

        createElection: function (electionData) {
            var sessionKey = localStorage["sessionKey"];
            var url = this.rootUrl;
            debugger
            var headers = {
                "X-sessionKey": sessionKey
            };
            
            return httpRequester.postJSON(url, electionData, headers)
        },

        putElection: function (election) {
            var url = this.rootUrl + "/" + election.id + "/Update";
            return httpRequester.putJSON(url, election)
            .then(function (data) {
                return data;
            },
            function (errMsg) {
                console.log(errMsg);
            });
        },

        closeElection: function (election) {
            var url = this.rootUrl + "/" + election.id + "/close";
            return httpRequester.putJSON(url)
            .then(function (data) {
                return data;
            },
            function (errMsg) {
                console.log(errMsg);
            });
        },

        getElectionResults: function (id) {
            var url = this.rootUrl + "/" + id + "/results";
            return httpRequester.getJSON(url)
            .then(function (data) {
                return data;
            },
            function (errMsg) {
                console.log(errMsg);
            });
        }
    });

    return {
        get: function (url) {
            return new MainPersister(url);
        }
    }
}());
