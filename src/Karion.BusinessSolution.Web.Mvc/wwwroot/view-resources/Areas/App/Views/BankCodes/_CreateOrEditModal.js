(function ($) {
    app.modals.CreateOrEditBankCodeModal = function () {

        var _bankCodesService = abp.services.app.bankCodes;

        var _modalManager;
        var _$bankCodeInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$bankCodeInformationForm = _modalManager.getModal().find('form[name=BankCodeInformationsForm]');
            _$bankCodeInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$bankCodeInformationForm.valid()) {
                return;
            }

            var bankCode = _$bankCodeInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _bankCodesService.createOrEdit(
				bankCode
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditBankCodeModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);