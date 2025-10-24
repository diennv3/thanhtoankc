(function () {
    $(function () {

        var _$attendancesTable = $('#AttendancesTable');
        var _attendancesService = abp.services.app.attendances;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Attendances.Create'),
            edit: abp.auth.hasPermission('Pages.Attendances.Edit'),
            'delete': abp.auth.hasPermission('Pages.Attendances.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Attendances/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Attendances/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditAttendanceModal'
        });       

		 var _viewAttendanceModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Attendances/ViewattendanceModal',
            modalClass: 'ViewAttendanceModal'
        });
         
        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$attendancesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _attendancesService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#AttendancesTableFilter').val(),
                        
                        minCheckInFilter:  getDateFilter($('#MinCheckInFilterId')),
                        maxCheckInFilter:  getDateFilter($('#MaxCheckInFilterId')),
                        nguoiBenhUserNameFilter: $('#NguoiBenhUserNameFilterId').val()
                    };
                }
            },
            columnDefs: [
                {
                    width: 120,
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
                            {
                                    text: app.localize('View'),
                                    action: function (data) {
                                        _viewAttendanceModal.open({ id: data.record.attendance.id });
                                    }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    // return _permissions.edit;
                                    return false;
                                },
                                action: function (data) {
                                _createOrEditModal.open({ id: data.record.attendance.id });                                
                                }
                            }, 
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return false;
                                    // return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteAttendance(data.record.attendance);
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 1,
                    data: "attendance.photoPath",
                    name: "photoPath",
                    render: function (photoPath) {
                        if (!photoPath) {
                            return '';
                        }

                        return '<img src="data:image/jpeg;base64,' + photoPath + '" style="width: 75px; height: 100px; object-fit: cover;" />';
                    }
                },
				{
					targets: 2,
					data: "nguoiBenhName" ,
					name: "nguoiBenhName"
				},
				{
					targets: 3,
					data: "attendance.checkIn",
					name: "checkIn" ,
					render: function (checkIn) {
						if (checkIn) {
							return moment(checkIn).format('L');
						}
						return "";
					}

				},
				
					{
						targets: 4,
						 data: "attendance.checkInLatitude",
						 name: "checkInLatitude"   
					},
					{
						targets: 5,
						 data: "attendance.checkInLongitude",
						 name: "checkInLongitude"   
					},
					{
						targets: 6,
						data: "attendance.checkInDeviceInfo",
						name: "checkInDeviceInfo"
					},
					{
						targets: 7,
						 data: "attendance.isCheckInFaceMatched",
						 name: "isCheckInFaceMatched"  ,
						render: function (isCheckInFaceMatched) {
							if (isCheckInFaceMatched) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 8,
						 data: "attendance.isWithinLocation",
						 name: "isWithinLocation"  ,
						render: function (isWithinLocation) {
							if (isWithinLocation) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					}
					
					
            ]
        });

        function getAttendances() {
            dataTable.ajax.reload();
        }

        function deleteAttendance(attendance) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _attendancesService.delete({
                            id: attendance.id
                        }).done(function () {
                            getAttendances(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

		$('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewAttendanceButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _attendancesService
                .getAttendancesToExcel({
				filter : $('#AttendancesTableFilter').val(),
					minCheckInLatitudeFilter: $('#MinCheckInLatitudeFilterId').val(),
					maxCheckInLatitudeFilter: $('#MaxCheckInLatitudeFilterId').val(),
					minCheckInLongitudeFilter: $('#MinCheckInLongitudeFilterId').val(),
					maxCheckInLongitudeFilter: $('#MaxCheckInLongitudeFilterId').val(),
					isCheckInFaceMatchedFilter: $('#IsCheckInFaceMatchedFilterId').val(),
					isWithinLocationFilter: $('#IsWithinLocationFilterId').val(),
					checkInDeviceInfoFilter: $('#CheckInDeviceInfoFilterId').val(),
					photoPathFilter: $('#PhotoPathFilterId').val(),
					minCheckInFaceMatchPercentageFilter: $('#MinCheckInFaceMatchPercentageFilterId').val(),
					maxCheckInFaceMatchPercentageFilter: $('#MaxCheckInFaceMatchPercentageFilterId').val(),
					minCheckInFilter:  getDateFilter($('#MinCheckInFilterId')),
					maxCheckInFilter:  getDateFilter($('#MaxCheckInFilterId')),
					isLateCheckInFilter: $('#IsLateCheckInFilterId').val(),
					isOvertimeFilter: $('#IsOvertimeFilterId').val(),
					minOvertimeStartFilter:  getDateFilter($('#MinOvertimeStartFilterId')),
					maxOvertimeStartFilter:  getDateFilter($('#MaxOvertimeStartFilterId')),
					minOvertimeEndFilter:  getDateFilter($('#MinOvertimeEndFilterId')),
					maxOvertimeEndFilter:  getDateFilter($('#MaxOvertimeEndFilterId')),
					nguoiBenhUserNameFilter: $('#NguoiBenhUserNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditAttendanceModalSaved', function () {
            getAttendances();
        });

		$('#GetAttendancesButton').click(function (e) {
            e.preventDefault();
            getAttendances();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getAttendances();
		  }
		});
    });
})();