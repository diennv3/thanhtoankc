(function ($) {
    app.modals.CreateOrEditShiftModal = function () {

        var _shiftsService = abp.services.app.shifts;

        var _modalManager;
        var _$shiftInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$shiftInformationForm = _modalManager.getModal().find('form[name=ShiftInformationsForm]');
            _$shiftInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$shiftInformationForm.valid()) {
                return;
            }

            var shift = _$shiftInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _shiftsService.createOrEdit(
				shift
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditShiftModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);