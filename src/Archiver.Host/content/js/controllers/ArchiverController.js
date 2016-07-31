'use strict'

var ArchiverController = angular.module('ArchiverController', []);

ArchiverController.controller('FolderController', ['$scope', '$http', 
    function ($scope, $http) {
        $http.get('/Home/Init').success(function (data) {
            $scope.folders = $.parseJSON(data);
        });

        $scope.getFolder = function (dir) {
            $http.get('/Home/GetFolder?name=' + dir).success(function (data) {
                var items = $.parseJSON(data);
                var folderItems = [];
                var fileItems = [];
                for (var i = 0; i < items.length; i++) {
                    if (items[i].File === "True") {
                        fileItems.push(items[i]);
                    }
                    else {
                        folderItems.push(items[i]);
                    }
                }

                $scope.folderItems = folderItems;
                $scope.fileItems = fileItems;
            });
        }
    }]);

ArchiverController.controller('ItemController', ['$scope', '$http', '$routeParams',
    function ($scope, $http, $routeParams) {
        $http.get('/Home/GetFolder?name=' + $routeParams.name).success(function (data) {
            $scope.items = $.parseJSON(data);
        })
    }
]);

ArchiverController.controller('ItemListController', ['$scope', '$http', '$routeParams',
    function ($scope, $http, $routeParams) {
        $http.get('/Home/GetItem?id=' + $routeParams.id).success(function (data) {
            $scope.Items = $.parseJSON(data);
        })
    }
])
