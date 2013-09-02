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
		    .then(function (data) {
		        clearUserData(data);
		        return data;
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

        getMyElections: function () {
            return this.getAllElections().then(function (elections) {
                var myElections = [];
                for (var i = 0; i < elections.length; i++) {
                    var election = elections[i];
                    if (election.ownerNickname == localStorage["displayname"]) {
                        myElections.push(election);
                    }
                }
                return myElections;
            });
        },

        getElection: function (id) {
            var url = this.rootUrl + "/" + id;
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
