(function () {
    $(function () {

        var _$shiftsTable = $('#ShiftsTable');
        var _shiftsService = abp.services.app.shifts;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Shifts.Create'),
            edit: abp.auth.hasPermission('Pages.Shifts.Edit'),
            'delete': abp.auth.hasPermission('Pages.Shifts.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Shifts/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Shifts/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditShiftModal'
        });       

		 var _viewShiftModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Shifts/ViewshiftModal',
            modalClass: 'ViewShiftModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$shiftsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _shiftsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ShiftsTableFilter').val(),
					codeFilter: $('#CodeFilterId').val(),
					nameFilter: $('#NameFilterId').val(),
					minStartTimeFilter:  getDateFilter($('#MinStartTimeFilterId')),
					maxStartTimeFilter:  getDateFilter($('#MaxStartTimeFilterId')),
					minEndTimeFilter:  getDateFilter($('#MinEndTimeFilterId')),
					maxEndTimeFilter:  getDateFilter($('#MaxEndTimeFilterId')),
					minWorkDaysMaskFilter: $('#MinWorkDaysMaskFilterId').val(),
					maxWorkDaysMaskFilter: $('#MaxWorkDaysMaskFilterId').val(),
					minToleranceMinutesFilter: $('#MinToleranceMinutesFilterId').val(),
					maxToleranceMinutesFilter: $('#MaxToleranceMinutesFilterId').val(),
					isActiveFilter: $('#IsActiveFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val()
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
                                    _viewShiftModal.open({ id: data.record.shift.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.shift.id });                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteShift(data.record.shift);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "shift.code",
						 name: "code"   
					},
					{
						targets: 2,
						 data: "shift.name",
						 name: "name"   
					},
                {
                    targets: 3,
                    data: "shift.startTime",
                    name: "startTime",
                    render: function (startTime) {
                        if (startTime) {
                            var time = moment(startTime, 'HH:mm:ss');
                            return time.format('HH:mm');
                        }
                        return "";
                    }
                },
                {
                    targets: 4,
                    data: "shift.endTime",
                    name: "endTime",
                    render: function (endTime) {
                        if (endTime) {
                            var time = moment(endTime, 'HH:mm:ss');
                            return time.format('HH:mm');
                        }
                        return "";
                    }
                },
					{
						targets: 5,
						 data: "shift.workDaysMask",
						 name: "workDaysMask"   
					},
					{
						targets: 6,
						 data: "shift.toleranceMinutes",
						 name: "toleranceMinutes"   
					},
					{
						targets: 7,
						 data: "shift.isActive",
						 name: "isActive"  ,
						render: function (isActive) {
							if (isActive) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 8,
						 data: "shift.description",
						 name: "description"   
					}
            ]
        });

        function getShifts() {
            dataTable.ajax.reload();
        }

        function deleteShift(shift) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _shiftsService.delete({
                            id: shift.id
                        }).done(function () {
                            getShifts(true);
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

        $('#CreateNewShiftButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _shiftsService
                .getShiftsToExcel({
				filter : $('#ShiftsTableFilter').val(),
					codeFilter: $('#CodeFilterId').val(),
					nameFilter: $('#NameFilterId').val(),
					minStartTimeFilter:  getDateFilter($('#MinStartTimeFilterId')),
					maxStartTimeFilter:  getDateFilter($('#MaxStartTimeFilterId')),
					minEndTimeFilter:  getDateFilter($('#MinEndTimeFilterId')),
					maxEndTimeFilter:  getDateFilter($('#MaxEndTimeFilterId')),
					minWorkDaysMaskFilter: $('#MinWorkDaysMaskFilterId').val(),
					maxWorkDaysMaskFilter: $('#MaxWorkDaysMaskFilterId').val(),
					minToleranceMinutesFilter: $('#MinToleranceMinutesFilterId').val(),
					maxToleranceMinutesFilter: $('#MaxToleranceMinutesFilterId').val(),
					isActiveFilter: $('#IsActiveFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditShiftModalSaved', function () {
            getShifts();
        });

		$('#GetShiftsButton').click(function (e) {
            e.preventDefault();
            getShifts();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getShifts();
		  }
		});
    });
})();