/// <reference path="../lib/_references.js" />
window.vmFactory = (function () {
    var dataPersister;
    var userPersister;
    var electionsPersister;
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
                    router.navigate("/create-election");
                   /* var electionData = {
                    };
                    electionsPersister.createElection(electionData).then(function (data) {
                        console.log(data);
                        router.navigate("/");
                    }, function (errMsg) {
                        console.log(errMsg);
                    })*/
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

    function getSeeResultsModel(id) {
        return electionsPersister.getElectionResults(id).then(kendo.observable, console.log);
    };

    function getManageElectionModel(id) {
        var election = electionsPersister.getElection(id).then(function(election) {
            election.inviteUsers = function () {
                election.invitedUsersDisplayNameString += "," + $("#tb-invite-user").val();
                election.invitedUsersDisplayNameString = ui.cleanListString(election.invitedUsersDisplayNameString);
                electionsPersister.putElection(election).then(function () {
                    document.location.reload(true);
                });
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

    function getCreateElectionModel() {
        var viewModel = {
            title: "",
            status: "Open",
            state: "Public",
            startdate: "",
            enddate: "",
            questions: [],
            curentQuestionContent: "",
            curentQuestionType: "Boolean",
            answersForCurrQuestion: [],
            currentAnswerContent: "",
            
            addQuestion: function () {
                var questionsGathered = this.get("questions");

                questionsGathered.push(
                    { 
                        type: this.get("currentQuestionType"),
                        content: this.get("curentQuestionContent"),
                        answers: this.get("answersForCurrQuestion")
                    });

                this.set("questions", questionsGathered);

                this.set("curentQuestionContent", ""); 
                this.set("currentQuestionType", "Boolean"); //za4istva6
                this.set("answersForCurrQuestion",""); 
                                
                //return questions
            },

            addAnswer: function (numberOfQuestions) {
                var answersGathered = this.get("answersForCurrQuestion");

                answersGathered.push({ type: this.get("currentAnswerContent") });

                this.set("answersForCurrQuestion", answersGathered);

                this.set("currentAnswerContent", "");

                //return addAnswer
            },

            submit: function () {
                var electionData = {
                    title: this.get("title"),
                    status: this.get("status"),
                    state: this.get("state"),
                    startdate: this.get("startdate"),
                    enddate: this.get("enddate"),
                    questions: this.get("questions")
                };

                electionsPersister.createElection(electionData).then(function () {
                    router.navigate("/");
                });
            }
        };
        return new RSVP.Promise(function (res, rej) {
            res(kendo.observable(viewModel));
        });
        //return new RSVP.Promise(function (res, rej) { res({}); });
    };

    return {
        init: function(url)
        {
            dataPersister = persisters.get(url);
            userPersister = dataPersister.userPersister;
            electionsPersister = dataPersister.electionsPersister;
        },
        getLoginViewModel: getLoginViewModel,
        getLoggedViewModel: getLoggedViewModel,
        getManageElectionModel: getManageElectionModel,
        getCreateElectionModel: getCreateElectionModel,
        getInvitedElectionModel: getInvitedElectionModel,
        getSeeResultsModel: getSeeResultsModel
    }
}());
