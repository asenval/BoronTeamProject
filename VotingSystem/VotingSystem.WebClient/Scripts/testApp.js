/// <reference path="lib/_references.js" />
(function () {
    var dataPersister = persisters.get("http://localhost:4414/api");
    var electionsPersister = dataPersister.electionsPersister;
    var myElections = electionsPersister.getMyElections();
    console.log(myElections);
}());