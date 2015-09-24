'use strict';
angular.module('budget_tracker').controller('houseCtrl', ['householdSvc', '$state', function (houseSvc, $state) {
    var self = this;
    this.sent = false;
    this.joinHouse = function (inviteEmail, inviteCode) {
        houseSvc.joinHousehold(inviteEmail, inviteCode).then(function (data) {
            $state.go('household.details');
        })
    }
    this.createHouse = function (name) {
        houseSvc.createHousehold(name).then(function (data) {
            $state.go('household.details');
        })
    }
    this.leaveHouse = function () {
        houseSvc.leaveHousehold(self.understood).then(function () {
            $state.go('household.create');
        })
    }
    this.sendInvite = function () {
        houseSvc.sendInvite(self.email).then(function () {
            self.sent = true;
        });
    }
}])