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
            viewUrl: abp.appPath + 'App/LeaveRequests/ViewLeaveRequestModal',
            modalClass: 'ViewLeaveRequestModal'
        });

        var getDateFilter = function (element) {
            if (!element.length || element.data("DateTimePicker").date() == null) {
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
                        minStartDateTimeFilter: getDateFilter($('#MinStartDateTimeFilterId')),
                        maxStartDateTimeFilter: getDateFilter($('#MaxStartDateTimeFilterId')),
                        minEndDateTimeFilter: getDateFilter($('#MinEndDateTimeFilterId')),
                        maxEndDateTimeFilter: getDateFilter($('#MaxEndDateTimeFilterId')),
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
            columns: [
                {
                    width: 120,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    render: function (data, type, row) {
                        var actions = [];

                        actions.push({
                            text: app.localize('View'),
                            action: function () {
                                _viewLeaveRequestModal.open({ id: row.leaveRequest.id });
                            }
                        });

                        if (_permissions.edit) {
                            actions.push({
                                text: app.localize('Edit'),
                                action: function () {
                                    _createOrEditModal.open({ id: row.leaveRequest.id });
                                }
                            });
                        }

                        if (_permissions.delete) {
                            actions.push({
                                text: app.localize('Delete'),
                                action: function () {
                                    deleteLeaveRequest(row.leaveRequest);
                                }
                            });
                        }

                        if (actions.length === 0) {
                            return '';
                        }

                        var dropdownHtml = '<div class="dropdown">' +
                            '<button class="btn btn-brand dropdown-toggle" type="button" data-toggle="dropdown">' +
                            '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>' +
                            '</button>' +
                            '<ul class="dropdown-menu">';

                        actions.forEach(function(action) {
                            dropdownHtml += '<li><a href="javascript:void(0);" class="dropdown-item">' + action.text + '</a></li>';
                        });

                        dropdownHtml += '</ul></div>';

                        return dropdownHtml;
                    }
                },
                {
                    data: "leaveRequest.startDateTime",
                    name: "startDateTime",
                    render: function (startDateTime) {
                        return startDateTime ? moment(startDateTime).format('L') : "";
                    }
                },
                {
                    data: "leaveRequest.endDateTime",
                    name: "endDateTime",
                    render: function (endDateTime) {
                        return endDateTime ? moment(endDateTime).format('L') : "";
                    }
                },
                {
                    data: "leaveRequest.type",
                    name: "type",
                    render: function (type) {
                        var typeMap = {
                            0: "Nguyên ngày",
                            1: "Nửa ngày",
                            2: "Về sớm",
                            3: "Đi muộn"
                        };

                        return type !== null && type !== undefined ?
                            (typeMap[type] || "Khác") : "";
                    }
                },
                {
                    data: "leaveRequest.reason",
                    name: "reason"
                },
                {
                    data: "leaveRequest.isForEarlyLeave",
                    name: "isForEarlyLeave",
                    render: function (isForEarlyLeave) {
                        return isForEarlyLeave ?
                            '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>' :
                            '<div class="text-center"><i class="fa fa-times-circle text-danger" title="False"></i></div>';
                    }
                },
                {
                    data: "leaveRequest.isForLateArrival",
                    name: "isForLateArrival",
                    render: function (isForLateArrival) {
                        return isForLateArrival ?
                            '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>' :
                            '<div class="text-center"><i class="fa fa-times-circle text-danger" title="False"></i></div>';
                    }
                },
                {
                    data: "nguoiBenhUserName",
                    name: "nguoiBenhFk.userName"
                }
            ],
        });

        function getLeaveRequests() {
            dataTable.ajax.reload();
        }

        function deleteLeaveRequest(leaveRequest) {
            abp.message.confirm(
                app.localize('DeleteConfirmMessage', leaveRequest.reason),
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _leaveRequestsService.delete({
                            id: leaveRequest.id
                        }).done(function () {
                            getLeaveRequests();
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
                    filter: $('#LeaveRequestsTableFilter').val(),
                    minStartDateTimeFilter: getDateFilter($('#MinStartDateTimeFilterId')),
                    maxStartDateTimeFilter: getDateFilter($('#MaxStartDateTimeFilterId')),
                    minEndDateTimeFilter: getDateFilter($('#MinEndDateTimeFilterId')),
                    maxEndDateTimeFilter: getDateFilter($('#MaxEndDateTimeFilterId')),
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

        $(document).on('click', '.dropdown-item', function() {
            var actionText = $(this).text();
            var rowData = dataTable.row($(this).closest('tr')).data();

            if (rowData) {
                if (actionText === app.localize('View')) {
                    _viewLeaveRequestModal.open({ id: rowData.leaveRequest.id });
                } else if (actionText === app.localize('Edit') && _permissions.edit) {
                    _createOrEditModal.open({ id: rowData.leaveRequest.id });
                } else if (actionText === app.localize('Delete') && _permissions.delete) {
                    deleteLeaveRequest(rowData.leaveRequest);
                }
            }
        });
    });
})();