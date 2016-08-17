'use strict';

var ArchiverController = angular.module('ArchiverController', []);

ArchiverController.controller('FolderController', ['$scope', '$http',
    function ($scope, $http) {
        $http.get('/Home/Init').success(function (data) {
            $scope.categories = $.parseJSON(data);
        });
        $scope.folderItems = [];
        $scope.fileItems = [];

        $scope.getCategory = function (dir) {
            $http.get('/Home/GetCategory?name=' + dir).success(function (data) {
                setItems(data);
            });
        };

        $scope.getFolder = function (id) {
            $http.get('/Home/GetFolder?id=' + id).success(function (data) {
                setItems(data);
            });
        };

        function setItems(data) {
            var items = $.parseJSON(data);
            $scope.fileItems.length = 0;
            $scope.folderItems.length = 0;
            for (var i = 0; i < items.length; i++) {
                if (items[i].IsFolder === false) {
                    $scope.fileItems.push(items[i]);
                }
                else {
                    $scope.folderItems.push(items[i]);
                }
            }
        }
    }]);

ArchiverController.controller('ItemController', ['$scope', '$http', '$routeParams',
    function ($scope, $http, $routeParams) {
        $http.get('/Home/GetFolder?id=' + $routeParams.id).success(function (data) {
            $scope.items = $.parseJSON(data);
        });
    }
]);

ArchiverController.controller('ItemListController', ['$scope', '$http', '$routeParams',
    function ($scope, $http, $routeParams) {
        $http.get('/Home/GetItem?id=' + $routeParams.id).success(function (data) {
            $scope.Items = $.parseJSON(data);
        });
    }
]);
