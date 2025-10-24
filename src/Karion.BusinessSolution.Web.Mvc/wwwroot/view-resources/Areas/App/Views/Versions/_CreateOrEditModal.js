(function ($) {
    app.modals.CreateOrEditVersionModal = function () {

        var _versionsService = abp.services.app.versions;

        var _modalManager;
        var _$versionInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$versionInformationForm = _modalManager.getModal().find('form[name=VersionInformationsForm]');
            _$versionInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$versionInformationForm.valid()) {
                return;
            }

            var version = _$versionInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _versionsService.createOrEdit(
				version
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditVersionModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);