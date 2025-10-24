(function () {
    $(function () {

        var _$leaveRequestsTable = $('#LeaveRequestsTable');
        var _leaveRequestsService = abp.services.app.leaveRequests;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.LeaveRequests.Create'),
            edit: abp.auth.hasPermission('Pages.LeaveRequests.Edit'),
            'delete': abp.auth.hasPermission('Pages.LeaveRequests.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LeaveRequests/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LeaveRequests/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditLeaveRequestModal'
        });       

		 var _viewLeaveRequestModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LeaveRequests/ViewleaveRequestModal',
            modalClass: 'ViewLeaveRequestModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$leaveRequestsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _leaveRequestsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#LeaveRequestsTableFilter').val(),
					minStartDateTimeFilter:  getDateFilter($('#MinStartDateTimeFilterId')),
					maxStartDateTimeFilter:  getDateFilter($('#MaxStartDateTimeFilterId')),
					minEndDateTimeFilter:  getDateFilter($('#MinEndDateTimeFilterId')),
					maxEndDateTimeFilter:  getDateFilter($('#MaxEndDateTimeFilterId')),
					leaveTypeFilter: $('#LeaveTypeFilterId').val(),
					statusFilter: $('#StatusFilterId').val(),
					reasonFilter: $('#ReasonFilterId').val(),
					isForEarlyLeaveFilter: $('#IsForEarlyLeaveFilterId').val(),
					isForLateArrivalFilter: $('#IsForLateArrivalFilterId').val(),
					nguoiBenhUserNameFilter: $('#NguoiBenhUserNameFilterId').val(),
					userNameFilter: $('#UserNameFilterId').val(),
					shiftNameFilter: $('#ShiftNameFilterId').val()
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
                                    _viewLeaveRequestModal.open({ id: data.record.leaveRequest.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.leaveRequest.id });                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteLeaveRequest(data.record.leaveRequest);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "leaveRequest.startDateTime",
						 name: "startDateTime" ,
					render: function (startDateTime) {
						if (startDateTime) {
							return moment(startDateTime).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 2,
						 data: "leaveRequest.endDateTime",
						 name: "endDateTime" ,
					render: function (endDateTime) {
						if (endDateTime) {
							return moment(endDateTime).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 3,
						 data: "leaveRequest.leaveType",
						 name: "leaveType"   
					},
					{
						targets: 4,
						 data: "leaveRequest.status",
						 name: "status"   ,
						render: function (status) {
							return app.localize('Enum_LeaveRequestStatus_' + status);
						}
			
					},
					{
						targets: 5,
						 data: "leaveRequest.reason",
						 name: "reason"   
					},
					{
						targets: 6,
						 data: "leaveRequest.isForEarlyLeave",
						 name: "isForEarlyLeave"  ,
						render: function (isForEarlyLeave) {
							if (isForEarlyLeave) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 7,
						 data: "leaveRequest.isForLateArrival",
						 name: "isForLateArrival"  ,
						render: function (isForLateArrival) {
							if (isForLateArrival) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 8,
						 data: "nguoiBenhUserName" ,
						 name: "nguoiBenhFk.userName" 
					},
					{
						targets: 9,
						 data: "userName" ,
						 name: "userFk.name" 
					},
					{
						targets: 10,
						 data: "shiftName" ,
						 name: "shiftFk.name" 
					}
            ]
        });

        function getLeaveRequests() {
            dataTable.ajax.reload();
        }

        function deleteLeaveRequest(leaveRequest) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _leaveRequestsService.delete({
                            id: leaveRequest.id
                        }).done(function () {
                            getLeaveRequests(true);
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

        $('#CreateNewLeaveRequestButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _leaveRequestsService
                .getLeaveRequestsToExcel({
				filter : $('#LeaveRequestsTableFilter').val(),
					minStartDateTimeFilter:  getDateFilter($('#MinStartDateTimeFilterId')),
					maxStartDateTimeFilter:  getDateFilter($('#MaxStartDateTimeFilterId')),
					minEndDateTimeFilter:  getDateFilter($('#MinEndDateTimeFilterId')),
					maxEndDateTimeFilter:  getDateFilter($('#MaxEndDateTimeFilterId')),
					leaveTypeFilter: $('#LeaveTypeFilterId').val(),
					statusFilter: $('#StatusFilterId').val(),
					reasonFilter: $('#ReasonFilterId').val(),
					isForEarlyLeaveFilter: $('#IsForEarlyLeaveFilterId').val(),
					isForLateArrivalFilter: $('#IsForLateArrivalFilterId').val(),
					nguoiBenhUserNameFilter: $('#NguoiBenhUserNameFilterId').val(),
					userNameFilter: $('#UserNameFilterId').val(),
					shiftNameFilter: $('#ShiftNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditLeaveRequestModalSaved', function () {
            getLeaveRequests();
        });

		$('#GetLeaveRequestsButton').click(function (e) {
            e.preventDefault();
            getLeaveRequests();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getLeaveRequests();
		  }
		});
    });
})();