(function ($) {
    app.modals.CreateOrEditChuyenKhoaModal = function () {

        var _chuyenKhoasService = abp.services.app.chuyenKhoas;

        var _modalManager;
        var _$chuyenKhoaInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$chuyenKhoaInformationForm = _modalManager.getModal().find('form[name=ChuyenKhoaInformationsForm]');
            _$chuyenKhoaInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$chuyenKhoaInformationForm.valid()) {
                return;
            }

            var chuyenKhoa = _$chuyenKhoaInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _chuyenKhoasService.createOrEdit(
				chuyenKhoa
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditChuyenKhoaModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);