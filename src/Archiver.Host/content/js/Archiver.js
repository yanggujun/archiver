﻿'use strict';

var archiver = angular.module('Archiver', [
    'ngRoute',
    'ArchiverController'
]);
//archiver.controller('ArchiverController', ArchiverController);
//archiver.controller('ItemListController', ItemListController);

archiver.config(['$routeProvider',
    function ($routeProvider ) {
        $routeProvider
            .when('/', {
                templateUrl: 'html/Folders.html',
                controller: 'FolderController'
            })
            .when('/folder/:name', {
                templateUrl: 'html/ItemList.html',
                controller: 'ItemController'
            })
            .when('/item/:id', {
                templateUrl: 'html/SubItemList.html',
                controller: 'ItemListController'
            })
            .otherwise({
                redirectto: '/'
            });
    }]);
