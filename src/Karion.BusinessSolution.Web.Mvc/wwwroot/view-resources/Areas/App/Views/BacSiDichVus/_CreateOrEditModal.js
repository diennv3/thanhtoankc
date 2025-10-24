(function ($) {
    app.modals.CreateOrEditBacSiDichVuModal = function () {

        var _bacSiDichVusService = abp.services.app.bacSiDichVus;

        var _modalManager;
        var _$bacSiDichVuInformationForm = null;

		        var _BacSiDichVuuserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/BacSiDichVus/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/BacSiDichVus/_BacSiDichVuUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });        var _BacSiDichVudichVuLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/BacSiDichVus/DichVuLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/BacSiDichVus/_BacSiDichVuDichVuLookupTableModal.js',
            modalClass: 'DichVuLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$bacSiDichVuInformationForm = _modalManager.getModal().find('form[name=BacSiDichVuInformationsForm]');
            _$bacSiDichVuInformationForm.validate();
        };

		          $('#OpenUserLookupTableButton').click(function () {

            var bacSiDichVu = _$bacSiDichVuInformationForm.serializeFormToObject();

            _BacSiDichVuuserLookupTableModal.open({ id: bacSiDichVu.userId, displayName: bacSiDichVu.userName }, function (data) {
                _$bacSiDichVuInformationForm.find('input[name=userName]').val(data.displayName); 
                _$bacSiDichVuInformationForm.find('input[name=userId]').val(data.id); 
            });
        });
		
		$('#ClearUserNameButton').click(function () {
                _$bacSiDichVuInformationForm.find('input[name=userName]').val(''); 
                _$bacSiDichVuInformationForm.find('input[name=userId]').val(''); 
        });
		
        $('#OpenDichVuLookupTableButton').click(function () {

            var bacSiDichVu = _$bacSiDichVuInformationForm.serializeFormToObject();

            _BacSiDichVudichVuLookupTableModal.open({ id: bacSiDichVu.dichVuId, displayName: bacSiDichVu.dichVuTen }, function (data) {
                _$bacSiDichVuInformationForm.find('input[name=dichVuTen]').val(data.displayName); 
                _$bacSiDichVuInformationForm.find('input[name=dichVuId]').val(data.id); 
            });
        });
		
		$('#ClearDichVuTenButton').click(function () {
                _$bacSiDichVuInformationForm.find('input[name=dichVuTen]').val(''); 
                _$bacSiDichVuInformationForm.find('input[name=dichVuId]').val(''); 
        });
		


        this.save = function () {
            if (!_$bacSiDichVuInformationForm.valid()) {
                return;
            }
            if ($('#BacSiDichVu_UserId').prop('required') && $('#BacSiDichVu_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }
            if ($('#BacSiDichVu_DichVuId').prop('required') && $('#BacSiDichVu_DichVuId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('DichVu')));
                return;
            }

            var bacSiDichVu = _$bacSiDichVuInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _bacSiDichVusService.createOrEdit(
				bacSiDichVu
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditBacSiDichVuModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);