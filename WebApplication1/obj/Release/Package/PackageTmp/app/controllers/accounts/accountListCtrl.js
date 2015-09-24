'use strict';
angular.module('budget_tracker').controller('accountListCtrl', ['accountSvc', '$state', 'accounts', function (accountSvc, $state, accounts) {
    var self = this;

    this.display = accounts;
    this.managePanel = 'c';
    this.model = {};
    this.beginNew = function () {
        self.model = {};
        self.managePanel = 'c';
    }
    this.beginEdit = function (id) {
        accountSvc.getAccount(id).then(function (data) {
            self.model = data;
            self.managePanel = 'e';
        })
    }
}])