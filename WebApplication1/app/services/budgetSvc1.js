'use strict';
angular.module('budget_tracker').factory('budgetSvc1', ['authSvc', '$state', '$http', function (authSvc, $state, $http) {
    var factory = {};
    factory.getBudgetItems = function () {
        return $http.post('/api/BudgetItems/HouseBudgetItems').then(function (response) {
            return response.data;
        });
    }
    factory.getBudgetItem = function (id) {
        return $http.post('/api/BudgetItems/Find?id='+ id).then(function (response) {
            return response.data;
        });
    }
    factory.editBudgetItem = function (id, budgetItem) {
        return $http.post('/api/BudgetItems/Edit?id='+id , budgetItem).then(function (response) {
            return response.data;
        });
    }
    factory.createBudgetItem = function (budgetItem) {
        return $http.post('/api/BudgetItems/Create', budgetItem).then(function (response) {
            return response.data;
        });
    }
    factory.deleteBudgetItem = function (id) {
        return $http.post('/api/BudgetItems/Delete?id='+ id).then(function (response) {
            return response.data;
        });
    }
    return factory;
}]);