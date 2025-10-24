(function ($) {
    app.modals.CreateOrEditNguoiThanModal = function () {

        var _nguoiThansService = abp.services.app.nguoiThans;

        var _modalManager;
        var _$nguoiThanInformationForm = null;

		        var _NguoiThannguoiBenhLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/NguoiThans/NguoiBenhLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/NguoiThans/_NguoiThanNguoiBenhLookupTableModal.js',
            modalClass: 'NguoiBenhLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$nguoiThanInformationForm = _modalManager.getModal().find('form[name=NguoiThanInformationsForm]');
            _$nguoiThanInformationForm.validate();
        };

		          $('#OpenNguoiBenhLookupTableButton').click(function () {

            var nguoiThan = _$nguoiThanInformationForm.serializeFormToObject();

            _NguoiThannguoiBenhLookupTableModal.open({ id: nguoiThan.nguoiBenhId, displayName: nguoiThan.nguoiBenhHoVaTen }, function (data) {
                _$nguoiThanInformationForm.find('input[name=nguoiBenhHoVaTen]').val(data.displayName); 
                _$nguoiThanInformationForm.find('input[name=nguoiBenhId]').val(data.id); 
            });
        });
		
		$('#ClearNguoiBenhHoVaTenButton').click(function () {
                _$nguoiThanInformationForm.find('input[name=nguoiBenhHoVaTen]').val(''); 
                _$nguoiThanInformationForm.find('input[name=nguoiBenhId]').val(''); 
        });
		


        this.save = function () {
            if (!_$nguoiThanInformationForm.valid()) {
                return;
            }
            if ($('#NguoiThan_NguoiBenhId').prop('required') && $('#NguoiThan_NguoiBenhId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('NguoiBenh')));
                return;
            }

            var nguoiThan = _$nguoiThanInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _nguoiThansService.createOrEdit(
				nguoiThan
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditNguoiThanModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);