(function ($) {
    app.modals.CreateOrEditHanetTenantPlaceDatasModal = function () {

        var _hanetTenantPlaceDatasesService = abp.services.app.hanetTenantPlaceDatases;

        var _modalManager;
        var _$hanetTenantPlaceDatasInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$hanetTenantPlaceDatasInformationForm = _modalManager.getModal().find('form[name=HanetTenantPlaceDatasInformationsForm]');
            _$hanetTenantPlaceDatasInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$hanetTenantPlaceDatasInformationForm.valid()) {
                return;
            }

            var hanetTenantPlaceDatas = _$hanetTenantPlaceDatasInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _hanetTenantPlaceDatasesService.createOrEdit(
				hanetTenantPlaceDatas
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditHanetTenantPlaceDatasModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);