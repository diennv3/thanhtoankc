(function ($) {
    app.modals.CreateOrEditHanetTenantLogModal = function () {

        var _hanetTenantLogsService = abp.services.app.hanetTenantLogs;

        var _modalManager;
        var _$hanetTenantLogInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$hanetTenantLogInformationForm = _modalManager.getModal().find('form[name=HanetTenantLogInformationsForm]');
            _$hanetTenantLogInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$hanetTenantLogInformationForm.valid()) {
                return;
            }

            var hanetTenantLog = _$hanetTenantLogInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _hanetTenantLogsService.createOrEdit(
				hanetTenantLog
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditHanetTenantLogModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);