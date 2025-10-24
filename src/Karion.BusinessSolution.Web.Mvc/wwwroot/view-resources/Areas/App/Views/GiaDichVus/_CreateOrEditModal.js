(function ($) {
    app.modals.CreateOrEditGiaDichVuModal = function () {

        var _giaDichVusService = abp.services.app.giaDichVus;

        var _modalManager;
        var _$giaDichVuInformationForm = null;

		        var _GiaDichVudichVuLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/GiaDichVus/DichVuLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/GiaDichVus/_GiaDichVuDichVuLookupTableModal.js',
            modalClass: 'DichVuLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$giaDichVuInformationForm = _modalManager.getModal().find('form[name=GiaDichVuInformationsForm]');
            _$giaDichVuInformationForm.validate();
        };

		          $('#OpenDichVuLookupTableButton').click(function () {

            var giaDichVu = _$giaDichVuInformationForm.serializeFormToObject();

            _GiaDichVudichVuLookupTableModal.open({ id: giaDichVu.dichVuId, displayName: giaDichVu.dichVuTen }, function (data) {
                _$giaDichVuInformationForm.find('input[name=dichVuTen]').val(data.displayName); 
                _$giaDichVuInformationForm.find('input[name=dichVuId]').val(data.id); 
            });
        });
		
		$('#ClearDichVuTenButton').click(function () {
                _$giaDichVuInformationForm.find('input[name=dichVuTen]').val(''); 
                _$giaDichVuInformationForm.find('input[name=dichVuId]').val(''); 
        });
		


        this.save = function () {
            if (!_$giaDichVuInformationForm.valid()) {
                return;
            }
            if ($('#GiaDichVu_DichVuId').prop('required') && $('#GiaDichVu_DichVuId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('DichVu')));
                return;
            }

            var giaDichVu = _$giaDichVuInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _giaDichVusService.createOrEdit(
				giaDichVu
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditGiaDichVuModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);