(function ($) {
    app.modals.CreateOrEditDichVuModal = function () {

        var _dichVusService = abp.services.app.dichVus;
        console.log(_dichVusService)
        var _modalManager;
        var _$dichVuInformationForm = null;

		        var _DichVuchuyenKhoaLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DichVus/ChuyenKhoaLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DichVus/_DichVuChuyenKhoaLookupTableModal.js',
            modalClass: 'ChuyenKhoaLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$dichVuInformationForm = _modalManager.getModal().find('form[name=DichVuInformationsForm]');
            _$dichVuInformationForm.validate();
        };

		          $('#OpenChuyenKhoaLookupTableButton').click(function () {

            var dichVu = _$dichVuInformationForm.serializeFormToObject();

            _DichVuchuyenKhoaLookupTableModal.open({ id: dichVu.chuyenKhoaId, displayName: dichVu.chuyenKhoaTen }, function (data) {
                _$dichVuInformationForm.find('input[name=chuyenKhoaTen]').val(data.displayName); 
                _$dichVuInformationForm.find('input[name=chuyenKhoaId]').val(data.id); 
            });
        });
		
		$('#ClearChuyenKhoaTenButton').click(function () {
                _$dichVuInformationForm.find('input[name=chuyenKhoaTen]').val(''); 
                _$dichVuInformationForm.find('input[name=chuyenKhoaId]').val(''); 
        });
		


        this.save = function () {
            if (!_$dichVuInformationForm.valid()) {
                return;
            }
            if ($('#DichVu_ChuyenKhoaId').prop('required') && $('#DichVu_ChuyenKhoaId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('ChuyenKhoa')));
                return;
            }

            var dichVu = _$dichVuInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _dichVusService.createOrEdit(
				dichVu
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditDichVuModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);