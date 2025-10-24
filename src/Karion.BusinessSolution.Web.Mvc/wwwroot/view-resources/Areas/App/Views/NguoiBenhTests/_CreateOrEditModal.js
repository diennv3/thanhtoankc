(function ($) {
    app.modals.CreateOrEditNguoiBenhTestModal = function () {

        var _nguoiBenhTestsService = abp.services.app.nguoiBenhTests;

        var _modalManager;
        var _$nguoiBenhTestInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$nguoiBenhTestInformationForm = _modalManager.getModal().find('form[name=NguoiBenhTestInformationsForm]');
            _$nguoiBenhTestInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$nguoiBenhTestInformationForm.valid()) {
                return;
            }

            var nguoiBenhTest = _$nguoiBenhTestInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _nguoiBenhTestsService.createOrEdit(
				nguoiBenhTest
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditNguoiBenhTestModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);