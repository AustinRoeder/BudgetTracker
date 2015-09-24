'use strict';
angular.module('budget_tracker').controller('accountCtrl', ['accountSvc', '$state', function (accountSvc, $state) {
    var self = this;

    this.getAccount = function (id) {
        accountSvc.getAccount(id)
    }
    this.archiveAccount = function (id) {
        accountSvc.archiveAccount(id).then(function (data) {
            $state.go($state.current, {}, { reload: true }); //second parameter is for $stateParams
        })
    }
    this.editAccount = function (id,model) {
        accountSvc.editAccount(id,model).then(function (data) {
            $state.go($state.current, {}, { reload: true });
        })
    }
    this.createAccount = function (model) {
        accountSvc.createAccount(model).then(function (data) {
            $state.go($state.current, {}, { reload: true }); //second parameter is for $stateParams
        })
    }
}])