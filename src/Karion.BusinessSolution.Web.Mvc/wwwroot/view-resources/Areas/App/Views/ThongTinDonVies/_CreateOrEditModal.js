(function ($) {
    app.modals.CreateOrEditThongTinDonViModal = function () {

        var _thongTinDonViesService = abp.services.app.thongTinDonVies;

        var _modalManager;
        var _$thongTinDonViInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$thongTinDonViInformationForm = _modalManager.getModal().find('form[name=ThongTinDonViInformationsForm]');
            _$thongTinDonViInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$thongTinDonViInformationForm.valid()) {
                return;
            }

            var thongTinDonVi = _$thongTinDonViInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _thongTinDonViesService.createOrEdit(
				thongTinDonVi
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditThongTinDonViModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);