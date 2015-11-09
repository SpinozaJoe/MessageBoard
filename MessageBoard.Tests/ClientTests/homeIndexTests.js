/// <reference path="../scripts/jasmine.js" />
/// <reference path="../../messageboard/scripts/angular.min.js" />
/// <reference path="../../messageboard/scripts/angular-mocks.js" />
/// <reference path="../../messageboard/js/home-index.js" />
/// <reference path="../../messageboard/scripts/angular-route.js" />

describe("home-index tests->", function ()
{
    beforeEach(function ()
    {
        module("homeIndex");
    });

    var $httpBackend;

    beforeEach(inject(function ($injector)
    {
        $httpBackend = $injector.get("$httpBackend");

        $httpBackend.when("GET", "/Playground/MessageBoard/api/v1/topics?includeReplies=true")
        .respond([{
            title: "some title",
            body: "a body",
            id: 1,
            created: "20050401"
        }]);
    }));

    afterEach(function ()
    {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    describe("dataService->", function ()
    {
        it("can do nothing", function ()
        {
            expect(true).toEqual(true);
        });
    });

    describe("breeze->", function ()
    {
        'use strict';

        var fns = window.testFns;
        var EntityQuery = breeze.EntityQuery;
        var em, tester;
        var failed = fns.failed;

        it('works', function (done)
        {
            EntityQuery.from("Topics")
            .using(em)
            .execute()
            .then(success)
            .catch(failed)
            .finally(done);

            function success(data)
            {
                var results = data.results;

                expect(results.length).toBeGreaterThan(0);
            }
        });
    });

    
    describe("dataService->", function ()
    {
        it("can load topics", inject(function (dataService)
        {
            expect(dataService.topics).toEqual([]);
            $httpBackend.expectGET("/Playground/MessageBoard/api/v1/topics?includeReplies=true")
            dataService.getTopics();
            $httpBackend.flush();
            expect(dataService.topics.length).toBeGreaterThan(0);
            expect(dataService.topics.length).toEqual(1);

        }));
    });

    describe("topicsController->", function ()
    {
        it("can load data", inject(function ($controller, $http, dataService)
        {
            var theScope = {};

            $httpBackend.expectGET("/Playground/MessageBoard/api/v1/topics?includeReplies=true")

            var controller = $controller("topicsController", {
                $scope: theScope,
                $http: $http,
                dataService: dataService
            });

            $httpBackend.flush();

            expect(controller).not.toBeNull();
            expect(theScope.data).toBeDefined();
        }));
    });

});