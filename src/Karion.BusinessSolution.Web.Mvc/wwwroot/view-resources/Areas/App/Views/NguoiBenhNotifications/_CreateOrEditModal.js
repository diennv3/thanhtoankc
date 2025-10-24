(function ($) {
    app.modals.CreateOrEditNguoiBenhNotificationModal = function () {

        var _nguoiBenhNotificationsService = abp.services.app.nguoiBenhNotifications;

        var _modalManager;
        var _$nguoiBenhNotificationInformationForm = null;

		        var _NguoiBenhNotificationnguoiBenhLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/NguoiBenhNotifications/NguoiBenhLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/NguoiBenhNotifications/_NguoiBenhNotificationNguoiBenhLookupTableModal.js',
            modalClass: 'NguoiBenhLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$nguoiBenhNotificationInformationForm = _modalManager.getModal().find('form[name=NguoiBenhNotificationInformationsForm]');
            _$nguoiBenhNotificationInformationForm.validate();
        };

		          $('#OpenNguoiBenhLookupTableButton').click(function () {

            var nguoiBenhNotification = _$nguoiBenhNotificationInformationForm.serializeFormToObject();

            _NguoiBenhNotificationnguoiBenhLookupTableModal.open({ id: nguoiBenhNotification.nguoiBenhId, displayName: nguoiBenhNotification.nguoiBenhUserName }, function (data) {
                _$nguoiBenhNotificationInformationForm.find('input[name=nguoiBenhUserName]').val(data.displayName); 
                _$nguoiBenhNotificationInformationForm.find('input[name=nguoiBenhId]').val(data.id); 
            });
        });
		
		$('#ClearNguoiBenhUserNameButton').click(function () {
                _$nguoiBenhNotificationInformationForm.find('input[name=nguoiBenhUserName]').val(''); 
                _$nguoiBenhNotificationInformationForm.find('input[name=nguoiBenhId]').val(''); 
        });
		


        this.save = function () {
            if (!_$nguoiBenhNotificationInformationForm.valid()) {
                return;
            }
            if ($('#NguoiBenhNotification_NguoiBenhId').prop('required') && $('#NguoiBenhNotification_NguoiBenhId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('NguoiBenh')));
                return;
            }

            var nguoiBenhNotification = _$nguoiBenhNotificationInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _nguoiBenhNotificationsService.createOrEdit(
				nguoiBenhNotification
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditNguoiBenhNotificationModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);