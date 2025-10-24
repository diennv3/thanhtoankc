(function ($) {
    app.modals.CreateOrEditHanetFaceDetectedModal = function () {

        var _hanetFaceDetectedsService = abp.services.app.hanetFaceDetecteds;

        var _modalManager;
        var _$hanetFaceDetectedInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$hanetFaceDetectedInformationForm = _modalManager.getModal().find('form[name=HanetFaceDetectedInformationsForm]');
            _$hanetFaceDetectedInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$hanetFaceDetectedInformationForm.valid()) {
                return;
            }

            var hanetFaceDetected = _$hanetFaceDetectedInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _hanetFaceDetectedsService.createOrEdit(
				hanetFaceDetected
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditHanetFaceDetectedModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);