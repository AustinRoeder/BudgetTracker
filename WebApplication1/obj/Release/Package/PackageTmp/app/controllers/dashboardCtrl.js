'use strict';
angular.module('budget_tracker').controller('dashboardCtrl', ['householdSvc', 'accountSvc', 'transactionSvc', '$state', '$http', function(householdSvc, accountSvc, transactionSvc, $state, $http) {
    var self = this;

    this.user = {};
    this.getUser = function () {
        householdSvc.getUser().then(function (data) {
            self.user = data
        });
    }
    self.getUser();

    this.accounts = {};
    this.getAccounts = function () {
        accountSvc.getAccounts().then(function (data) {
            self.accounts = data;
        })
    }
    self.getAccounts();

    this.trans = {};
    this.recentTrans = function () {
        transactionSvc.recent().then(function (data) {
            self.trans = data;
        })
    }
    self.recentTrans();


    this.options = {
        chart: {
            type: 'multiBarChart',
            height: 500,
            transitionDuration: 500,
        }
    };
    this.values = [];
    self.getValues = function () {
        $http.get('/api/values/month').then(function (response) {
            self.values = response.data;
        });
    }
    self.getValues();
    this.valuesMonthly = [];
    self.getValuesMonthly = function () {
        $http.get('/api/values/monthly').then(function (response) {
            self.valuesMonthly = response.data;
        });
    }
    self.getValuesMonthly();
}])