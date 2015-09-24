﻿'use strict';
angular.module('budget_tracker').controller('signinCtrl', ['authSvc', '$state', function(authSvc, $state) {
    var self = this;

    this.name = '';
    this.username = '';
    this.password = '';
    this.confirmPass = '';
    this.errors = null;

    this.registration = {
        Email: '',
        Password: '',
        ConfirmPassword: '',
        DisplayName: ''
    }

    self.login = function () {
        authSvc.login(self.username, self.password).then(function(success) {
            $state.go('dashboard');
        }, function(error) {
            self.errors = error.data;
        });
    }
    self.register = function () {
        authSvc.saveRegistration(self.registration).then(function (success) {
            $state.go('dashboard');
        }, function (error) {
            self.errors = error.data;
        });
    }
}])