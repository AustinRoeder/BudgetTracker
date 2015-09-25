'use strict';
angular.module('budget_tracker').controller('profileCtrl', ['profileSvc', '$state', 'authSvc', function (profileSvc, $state, authSvc) {
    var self = this;

    this.user = {}

    profileSvc.getUser().then(function (data) { self.user =  data; });

    this.model = {
        OldPassword: '',
        NewPassword: '',
        ConfirmPassword: '',
    }

    this.managePanel = 'd';
    this.beginEdit = function () {
        self.managePanel = 'e';
    }
    this.doneEdit = function () {
        self.managePanel = 'd';
    }

    this.edit = function () {
        profileSvc.editProfile(self.model).then(function () {
            self.managePanel = 'd';
            $state.go($state.current, {}, { reload: true })
        })
    }
}])