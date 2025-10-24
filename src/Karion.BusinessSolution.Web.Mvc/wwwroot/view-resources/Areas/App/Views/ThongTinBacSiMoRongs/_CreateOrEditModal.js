(function ($) {
    app.modals.CreateOrEditThongTinBacSiMoRongModal = function () {

        var _thongTinBacSiMoRongsService = abp.services.app.thongTinBacSiMoRongs;

        var _modalManager;
        var _$thongTinBacSiMoRongInformationForm = null;

		        var _ThongTinBacSiMoRonguserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ThongTinBacSiMoRongs/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ThongTinBacSiMoRongs/_ThongTinBacSiMoRongUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$thongTinBacSiMoRongInformationForm = _modalManager.getModal().find('form[name=ThongTinBacSiMoRongInformationsForm]');
            _$thongTinBacSiMoRongInformationForm.validate();
        };

		          $('#OpenUserLookupTableButton').click(function () {

            var thongTinBacSiMoRong = _$thongTinBacSiMoRongInformationForm.serializeFormToObject();

            _ThongTinBacSiMoRonguserLookupTableModal.open({ id: thongTinBacSiMoRong.userId, displayName: thongTinBacSiMoRong.userName }, function (data) {
                _$thongTinBacSiMoRongInformationForm.find('input[name=userName]').val(data.displayName); 
                _$thongTinBacSiMoRongInformationForm.find('input[name=userId]').val(data.id); 
            });
        });
		
		$('#ClearUserNameButton').click(function () {
                _$thongTinBacSiMoRongInformationForm.find('input[name=userName]').val(''); 
                _$thongTinBacSiMoRongInformationForm.find('input[name=userId]').val(''); 
        });
		


        this.save = function () {
            if (!_$thongTinBacSiMoRongInformationForm.valid()) {
                return;
            }
            if ($('#ThongTinBacSiMoRong_UserId').prop('required') && $('#ThongTinBacSiMoRong_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }

            var thongTinBacSiMoRong = _$thongTinBacSiMoRongInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _thongTinBacSiMoRongsService.createOrEdit(
				thongTinBacSiMoRong
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditThongTinBacSiMoRongModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);