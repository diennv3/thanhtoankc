(function ($) {
    app.modals.CreateOrEditDanhSachVersionModal = function () {

        var _danhSachVersionsService = abp.services.app.danhSachVersions;

        var _modalManager;
        var _$danhSachVersionInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$danhSachVersionInformationForm = _modalManager.getModal().find('form[name=DanhSachVersionInformationsForm]');
            _$danhSachVersionInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$danhSachVersionInformationForm.valid()) {
                return;
            }

            var danhSachVersion = _$danhSachVersionInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _danhSachVersionsService.createOrEdit(
				danhSachVersion
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditDanhSachVersionModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);