// home-index.js

var homeIndexModule = angular.module('homeIndex', ['ngRoute']);

homeIndexModule.config(["$routeProvider", function ($routeProvider)
{
    $routeProvider.when("/", {
        controller: "topicsController",
        templateUrl: "templates/topicsView.html"
    });

    $routeProvider.when("/newmessage", {
        controller: "newTopicController",
        templateUrl: "templates/newTopicView.html"
    });

    $routeProvider.when("/message/:id", {
        controller: "singleTopicController",
        templateUrl: "templates/singleTopicView.html"
    });

    $routeProvider.otherwise({ redirectTo: "/" });
}]);

homeIndexModule.factory("dataService", ["$http", "$q", function ($http, $q)
{
    var topics = [];
    var isInit = false;

    var isReady = function ()
    {
        return isInit;
    }

    var getTopics2 = function ()
    {
        var deferred = $q.defer();
        var EntityQuery = breeze.EntityQuery;
        var FilterQueryOp = breeze.FilterQueryOp;
        var Predicate = breeze.Predicate;

        // create a manager to execute queries
        var manager = new breeze.EntityManager("http://localhost/Playground/MessageBoard/breeze/breeze");

        var query1a = EntityQuery.from('Topics');

        manager.executeQuery(query1a) // returns a promise
             .then(function (result)
             {
                 // Successful
                 angular.copy(result.data, topics);
                 isInit = true;
                 deferred.resolve();
             })  // process results
             .fail(function ()
             {
                 // Error
                 deferred.reject("No topics loaded.");
             });    // handle error

        return deferred.promise;
    }

    var getTopics = function ()
    {
        var deferred = $q.defer();

        $http.get("api/v1/topics?includeReplies=true")
            .then(function (result)
            {
                // Successful
                angular.copy(result.data, topics);
                isInit = true;
                deferred.resolve();
            },
            function ()
            {
                // Error
                deferred.reject("No topics loaded.");
            })

        return deferred.promise;
    }

    var addTopic = function (newTopic)
    {
        var deferred = $q.defer();

        $http.post("api/v1/topics", newTopic)
        .then(function (result)
        {
            // successful
            var newlyCreatedTopic = result.data;

            // Merge with existing data
            topics.splice(0, 0, newlyCreatedTopic);

            deferred.resolve(newlyCreatedTopic);
        },
        function ()
        {
            // Error
            deferred.reject("Failed to save new topic.");
        });

        return deferred.promise;
    }
    
    function findTopic(id)
    {
        var topic = null;

        $.each(topics, function (i, item)
        {
            if (item.id == id)
            {
                topic = item;
                return false;
            }
        });

        return topic;
    }

    var getTopicById = function (id)
    {
        var deferred = $q.defer();

        if (isReady())
        {
            var topic = findTopic(id);

            if (topic)
            {
                deferred.resolve(topic);
            } else
            {
                deferred.reject("Can't find topic in previously loaded topics");
            }
        } else
        {
            getTopics()
                .then(function ()
                {
                    // success
                    var topic = findTopic(id);

                    if (topic)
                    {
                        deferred.resolve(topic);
                    } else
                    {
                        deferred.reject("Loaded all topics but can't find this one");
                    }
                },
                function ()
                {
                    // error
                    deferred.reject("Failed to load topics");
                });
        }

        return deferred.promise;
    }
    
    saveReply = function (topic, newReply)
    {
        var deferred = $q.defer();

        $http.post("api/v1/topics/" + topic.id + "/replies", newReply)
            .then(function (result)
            {
                // success
                if (topic.replies == null)
                {
                    topic.replies = [];
                }
                topic.replies.push(result.data);
                deferred.resolve(result.data);
            },
            function ()
            {
                // error
                deferred.reject("Failed to save reply");
            });

        return deferred.promise;
    }

    return {
        topics: topics,
        getTopics: getTopics,
        addTopic: addTopic,
        isReady: isReady,
        getTopicById: getTopicById,
        saveReply: saveReply
    };
}]);

var topicsController = ["$scope", "$http", "dataService",
    function ($scope, $http, dataService)
    {
        $scope.data = dataService;
        $scope.isBusy = false;

        if (!dataService.isReady())
        {
            $scope.isBusy = true;

            dataService.getTopics()
                .then(function (result)
                {
                    // Successful
                },
                function (err)
                {
                    // Error
                    if (err == undefined)
                        alert("Failed to load topics.");
                    else
                        alert(err);
                })
                .then(function ()
                {
                    // Like a finally
                    $scope.isBusy = false;
                });
        }
    }];

homeIndexModule.controller('topicsController', topicsController);

homeIndexModule.controller('newTopicController', ["$scope", "$http", "$window", "dataService", function ($scope, $http, $window, dataService)
{
    $scope.newTopic = {};

    $scope.save = function ()
    {
        dataService.addTopic($scope.newTopic)
        .then(function (result)
        {
            // Reload page;
            $window.location = "#/";
        },
        function (err)
        {
            // Error
            alert(err);
        });
    }
    
}]);

homeIndexModule.controller('singleTopicController', ["$scope", "$http", "$window", "dataService", "$routeParams", function ($scope, $http, $window, dataService, $routeParams)
{
    $scope.topic = null;
    $scope.newReply = {}

    dataService.getTopicById($routeParams.id)
        .then(function (topic)
        {
            //success
            $scope.topic = topic;
        },
        function ()
        {
            // error
            $window.location = "#/";
        });
    
    $scope.addReply = function ()
    {
        dataService.saveReply($scope.topic, $scope.newReply)
        .then(function (result)
        {
            // Success - clear out the body for next time
            $scope.newReply.body = "";
        },
        function (err)
        {
            // Error
            alert(err);
        });
    }

}]);