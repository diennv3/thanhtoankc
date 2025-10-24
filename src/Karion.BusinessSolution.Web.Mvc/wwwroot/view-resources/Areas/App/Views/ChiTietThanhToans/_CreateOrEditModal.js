(function ($) {
    app.modals.CreateOrEditChiTietThanhToanModal = function () {

        var _chiTietThanhToansService = abp.services.app.chiTietThanhToans;

        var _modalManager;
        var _$chiTietThanhToanInformationForm = null;

		        var _ChiTietThanhToanlichHenKhamLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ChiTietThanhToans/LichHenKhamLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ChiTietThanhToans/_ChiTietThanhToanLichHenKhamLookupTableModal.js',
            modalClass: 'LichHenKhamLookupTableModal'
        });        var _ChiTietThanhToannguoiBenhLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ChiTietThanhToans/NguoiBenhLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ChiTietThanhToans/_ChiTietThanhToanNguoiBenhLookupTableModal.js',
            modalClass: 'NguoiBenhLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$chiTietThanhToanInformationForm = _modalManager.getModal().find('form[name=ChiTietThanhToanInformationsForm]');
            _$chiTietThanhToanInformationForm.validate();
        };

		          $('#OpenLichHenKhamLookupTableButton').click(function () {

            var chiTietThanhToan = _$chiTietThanhToanInformationForm.serializeFormToObject();

            _ChiTietThanhToanlichHenKhamLookupTableModal.open({ id: chiTietThanhToan.lichHenKhamId, displayName: chiTietThanhToan.lichHenKhamMoTaTrieuChung }, function (data) {
                _$chiTietThanhToanInformationForm.find('input[name=lichHenKhamMoTaTrieuChung]').val(data.displayName); 
                _$chiTietThanhToanInformationForm.find('input[name=lichHenKhamId]').val(data.id); 
            });
        });
		
		$('#ClearLichHenKhamMoTaTrieuChungButton').click(function () {
                _$chiTietThanhToanInformationForm.find('input[name=lichHenKhamMoTaTrieuChung]').val(''); 
                _$chiTietThanhToanInformationForm.find('input[name=lichHenKhamId]').val(''); 
        });
		
        $('#OpenNguoiBenhLookupTableButton').click(function () {

            var chiTietThanhToan = _$chiTietThanhToanInformationForm.serializeFormToObject();

            _ChiTietThanhToannguoiBenhLookupTableModal.open({ id: chiTietThanhToan.nguoiBenhId, displayName: chiTietThanhToan.nguoiBenhUserName }, function (data) {
                _$chiTietThanhToanInformationForm.find('input[name=nguoiBenhUserName]').val(data.displayName); 
                _$chiTietThanhToanInformationForm.find('input[name=nguoiBenhId]').val(data.id); 
            });
        });
		
		$('#ClearNguoiBenhUserNameButton').click(function () {
                _$chiTietThanhToanInformationForm.find('input[name=nguoiBenhUserName]').val(''); 
                _$chiTietThanhToanInformationForm.find('input[name=nguoiBenhId]').val(''); 
        });
		


        this.save = function () {
            if (!_$chiTietThanhToanInformationForm.valid()) {
                return;
            }
            if ($('#ChiTietThanhToan_LichHenKhamId').prop('required') && $('#ChiTietThanhToan_LichHenKhamId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('LichHenKham')));
                return;
            }
            if ($('#ChiTietThanhToan_NguoiBenhId').prop('required') && $('#ChiTietThanhToan_NguoiBenhId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('NguoiBenh')));
                return;
            }

            var chiTietThanhToan = _$chiTietThanhToanInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _chiTietThanhToansService.createOrEdit(
				chiTietThanhToan
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditChiTietThanhToanModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);