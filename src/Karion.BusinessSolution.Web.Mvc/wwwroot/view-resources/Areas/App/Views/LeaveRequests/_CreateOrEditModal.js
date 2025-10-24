(function ($) {
    app.modals.CreateOrEditLeaveRequestModal = function () {

        var _leaveRequestsService = abp.services.app.leaveRequests;

        var _modalManager;
        var _$leaveRequestInformationForm = null;

		        var _LeaveRequestnguoiBenhLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LeaveRequests/NguoiBenhLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LeaveRequests/_LeaveRequestNguoiBenhLookupTableModal.js',
            modalClass: 'NguoiBenhLookupTableModal'
        });        var _LeaveRequestuserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LeaveRequests/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LeaveRequests/_LeaveRequestUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });        var _LeaveRequestshiftLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LeaveRequests/ShiftLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LeaveRequests/_LeaveRequestShiftLookupTableModal.js',
            modalClass: 'ShiftLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$leaveRequestInformationForm = _modalManager.getModal().find('form[name=LeaveRequestInformationsForm]');
            _$leaveRequestInformationForm.validate();
        };

		          $('#OpenNguoiBenhLookupTableButton').click(function () {

            var leaveRequest = _$leaveRequestInformationForm.serializeFormToObject();

            _LeaveRequestnguoiBenhLookupTableModal.open({ id: leaveRequest.nguoiBenhId, displayName: leaveRequest.nguoiBenhUserName }, function (data) {
                _$leaveRequestInformationForm.find('input[name=nguoiBenhUserName]').val(data.displayName); 
                _$leaveRequestInformationForm.find('input[name=nguoiBenhId]').val(data.id); 
            });
        });
		
		$('#ClearNguoiBenhUserNameButton').click(function () {
                _$leaveRequestInformationForm.find('input[name=nguoiBenhUserName]').val(''); 
                _$leaveRequestInformationForm.find('input[name=nguoiBenhId]').val(''); 
        });
		
        $('#OpenUserLookupTableButton').click(function () {

            var leaveRequest = _$leaveRequestInformationForm.serializeFormToObject();

            _LeaveRequestuserLookupTableModal.open({ id: leaveRequest.userId, displayName: leaveRequest.userName }, function (data) {
                _$leaveRequestInformationForm.find('input[name=userName]').val(data.displayName); 
                _$leaveRequestInformationForm.find('input[name=userId]').val(data.id); 
            });
        });
		
		$('#ClearUserNameButton').click(function () {
                _$leaveRequestInformationForm.find('input[name=userName]').val(''); 
                _$leaveRequestInformationForm.find('input[name=userId]').val(''); 
        });
		
        $('#OpenShiftLookupTableButton').click(function () {

            var leaveRequest = _$leaveRequestInformationForm.serializeFormToObject();

            _LeaveRequestshiftLookupTableModal.open({ id: leaveRequest.shiftId, displayName: leaveRequest.shiftName }, function (data) {
                _$leaveRequestInformationForm.find('input[name=shiftName]').val(data.displayName); 
                _$leaveRequestInformationForm.find('input[name=shiftId]').val(data.id); 
            });
        });
		
		$('#ClearShiftNameButton').click(function () {
                _$leaveRequestInformationForm.find('input[name=shiftName]').val(''); 
                _$leaveRequestInformationForm.find('input[name=shiftId]').val(''); 
        });
		


        this.save = function () {
            if (!_$leaveRequestInformationForm.valid()) {
                return;
            }
            if ($('#LeaveRequest_NguoiBenhId').prop('required') && $('#LeaveRequest_NguoiBenhId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('NguoiBenh')));
                return;
            }
            if ($('#LeaveRequest_UserId').prop('required') && $('#LeaveRequest_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }
            if ($('#LeaveRequest_ShiftId').prop('required') && $('#LeaveRequest_ShiftId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Shift')));
                return;
            }

            var leaveRequest = _$leaveRequestInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _leaveRequestsService.createOrEdit(
				leaveRequest
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditLeaveRequestModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);