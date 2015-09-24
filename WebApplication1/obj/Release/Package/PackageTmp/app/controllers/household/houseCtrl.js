'use strict';
angular.module('budget_tracker').controller('houseCtrl', ['householdSvc', '$state', 'authSvc', function (houseSvc, $state, authSvc) {
    var self = this;
    this.sent = false;
    this.joinHouse = function (inviteEmail, inviteCode) {
        houseSvc.joinHousehold(inviteEmail, inviteCode).then(function (data) {
            $state.go('household.details');
        })
    }
    this.createHouse = function (name) {
        houseSvc.createHousehold(name).then(function (data) {
            authSvc.refresh().then(function () {
                $state.go('household.details');
            })
        })
    }
    this.leaveHouse = function () {
        houseSvc.leaveHousehold(self.understood).then(function () {
            self.refresh();
            $state.go('household.create');
        })
    }
    this.sendInvite = function () {
        houseSvc.sendInvite(self.email).then(function () {
            self.sent = true;
        });
    }

    this.refresh = function () {
        authSvc.refresh()
    }
}])