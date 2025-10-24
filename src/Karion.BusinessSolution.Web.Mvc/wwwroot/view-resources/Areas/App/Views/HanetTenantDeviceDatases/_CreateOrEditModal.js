(function ($) {
    app.modals.CreateOrEditHanetTenantDeviceDatasModal = function () {

        var _hanetTenantDeviceDatasesService = abp.services.app.hanetTenantDeviceDatases;

        var _modalManager;
        var _$hanetTenantDeviceDatasInformationForm = null;

		        var _HanetTenantDeviceDatashanetTenantPlaceDatasLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/HanetTenantDeviceDatases/HanetTenantPlaceDatasLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/HanetTenantDeviceDatases/_HanetTenantDeviceDatasHanetTenantPlaceDatasLookupTableModal.js',
            modalClass: 'HanetTenantPlaceDatasLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$hanetTenantDeviceDatasInformationForm = _modalManager.getModal().find('form[name=HanetTenantDeviceDatasInformationsForm]');
            _$hanetTenantDeviceDatasInformationForm.validate();
        };

		          $('#OpenHanetTenantPlaceDatasLookupTableButton').click(function () {

            var hanetTenantDeviceDatas = _$hanetTenantDeviceDatasInformationForm.serializeFormToObject();

            _HanetTenantDeviceDatashanetTenantPlaceDatasLookupTableModal.open({ id: hanetTenantDeviceDatas.hanetTenantPlaceDatasId, displayName: hanetTenantDeviceDatas.hanetTenantPlaceDatasplaceName }, function (data) {
                _$hanetTenantDeviceDatasInformationForm.find('input[name=hanetTenantPlaceDatasplaceName]').val(data.displayName); 
                _$hanetTenantDeviceDatasInformationForm.find('input[name=hanetTenantPlaceDatasId]').val(data.id); 
            });
        });
		
		$('#ClearHanetTenantPlaceDatasplaceNameButton').click(function () {
                _$hanetTenantDeviceDatasInformationForm.find('input[name=hanetTenantPlaceDatasplaceName]').val(''); 
                _$hanetTenantDeviceDatasInformationForm.find('input[name=hanetTenantPlaceDatasId]').val(''); 
        });
		


        this.save = function () {
            if (!_$hanetTenantDeviceDatasInformationForm.valid()) {
                return;
            }
            if ($('#HanetTenantDeviceDatas_HanetTenantPlaceDatasId').prop('required') && $('#HanetTenantDeviceDatas_HanetTenantPlaceDatasId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('HanetTenantPlaceDatas')));
                return;
            }

            var hanetTenantDeviceDatas = _$hanetTenantDeviceDatasInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _hanetTenantDeviceDatasesService.createOrEdit(
				hanetTenantDeviceDatas
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditHanetTenantDeviceDatasModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);