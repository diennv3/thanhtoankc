(function ($) {
    app.modals.CreateOrEditAttendanceModal = function () {

        var _attendancesService = abp.services.app.attendances;

        var _modalManager;
        var _$attendanceInformationForm = null;

		        var _AttendancenguoiBenhLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Attendances/NguoiBenhLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Attendances/_AttendanceNguoiBenhLookupTableModal.js',
            modalClass: 'NguoiBenhLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$attendanceInformationForm = _modalManager.getModal().find('form[name=AttendanceInformationsForm]');
            _$attendanceInformationForm.validate();
        };

		          $('#OpenNguoiBenhLookupTableButton').click(function () {

            var attendance = _$attendanceInformationForm.serializeFormToObject();

            _AttendancenguoiBenhLookupTableModal.open({ id: attendance.nguoiBenhId, displayName: attendance.nguoiBenhUserName }, function (data) {
                _$attendanceInformationForm.find('input[name=nguoiBenhUserName]').val(data.displayName); 
                _$attendanceInformationForm.find('input[name=nguoiBenhId]').val(data.id); 
            });
        });
		
		$('#ClearNguoiBenhUserNameButton').click(function () {
                _$attendanceInformationForm.find('input[name=nguoiBenhUserName]').val(''); 
                _$attendanceInformationForm.find('input[name=nguoiBenhId]').val(''); 
        });
		


        this.save = function () {
            if (!_$attendanceInformationForm.valid()) {
                return;
            }
            if ($('#Attendance_NguoiBenhId').prop('required') && $('#Attendance_NguoiBenhId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('NguoiBenh')));
                return;
            }

            var attendance = _$attendanceInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _attendancesService.createOrEdit(
				attendance
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditAttendanceModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);