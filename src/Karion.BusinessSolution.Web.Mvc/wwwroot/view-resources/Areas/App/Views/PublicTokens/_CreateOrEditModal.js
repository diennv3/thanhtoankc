(function ($) {
    app.modals.CreateOrEditPublicTokenModal = function () {

        var _publicTokensService = abp.services.app.publicTokens;

        var _modalManager;
        var _$publicTokenInformationForm = null;

		        var _PublicTokennguoiBenhLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PublicTokens/NguoiBenhLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PublicTokens/_PublicTokenNguoiBenhLookupTableModal.js',
            modalClass: 'NguoiBenhLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$publicTokenInformationForm = _modalManager.getModal().find('form[name=PublicTokenInformationsForm]');
            _$publicTokenInformationForm.validate();
        };

		          $('#OpenNguoiBenhLookupTableButton').click(function () {

            var publicToken = _$publicTokenInformationForm.serializeFormToObject();

            _PublicTokennguoiBenhLookupTableModal.open({ id: publicToken.nguoiBenhId, displayName: publicToken.nguoiBenhUserName }, function (data) {
                _$publicTokenInformationForm.find('input[name=nguoiBenhUserName]').val(data.displayName); 
                _$publicTokenInformationForm.find('input[name=nguoiBenhId]').val(data.id); 
            });
        });
		
		$('#ClearNguoiBenhUserNameButton').click(function () {
                _$publicTokenInformationForm.find('input[name=nguoiBenhUserName]').val(''); 
                _$publicTokenInformationForm.find('input[name=nguoiBenhId]').val(''); 
        });
		


        this.save = function () {
            if (!_$publicTokenInformationForm.valid()) {
                return;
            }
            if ($('#PublicToken_NguoiBenhId').prop('required') && $('#PublicToken_NguoiBenhId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('NguoiBenh')));
                return;
            }

            var publicToken = _$publicTokenInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _publicTokensService.createOrEdit(
				publicToken
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditPublicTokenModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);