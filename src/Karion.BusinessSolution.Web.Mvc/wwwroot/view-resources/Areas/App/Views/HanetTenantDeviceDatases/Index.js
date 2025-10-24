(function () {
    $(function () {

        var _$hanetTenantDeviceDatasesTable = $('#HanetTenantDeviceDatasesTable');
        var _hanetTenantDeviceDatasesService = abp.services.app.hanetTenantDeviceDatases;
		var _entityTypeFullName = 'Karion.BusinessSolution.HanetTenant.HanetTenantDeviceDatas';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.HanetTenantDeviceDatases.Create'),
            edit: abp.auth.hasPermission('Pages.HanetTenantDeviceDatases.Edit'),
            'delete': abp.auth.hasPermission('Pages.HanetTenantDeviceDatases.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/HanetTenantDeviceDatases/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/HanetTenantDeviceDatases/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditHanetTenantDeviceDatasModal'
        });       

		 var _viewHanetTenantDeviceDatasModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/HanetTenantDeviceDatases/ViewhanetTenantDeviceDatasModal',
            modalClass: 'ViewHanetTenantDeviceDatasModal'
        });

		        var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();
		        function entityHistoryIsEnabled() {
            return abp.auth.hasPermission('Pages.Administration.AuditLogs') &&
                abp.custom.EntityHistory &&
                abp.custom.EntityHistory.IsEnabled &&
                _.filter(abp.custom.EntityHistory.EnabledEntities, entityType => entityType === _entityTypeFullName).length === 1;
        }

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$hanetTenantDeviceDatasesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _hanetTenantDeviceDatasesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#HanetTenantDeviceDatasesTableFilter').val(),
					deviceIdFilter: $('#deviceIdFilterId').val(),
					deviceNameFilter: $('#deviceNameFilterId').val(),
					deviceStatusFilter: $('#deviceStatusFilterId').val(),
					minlastCheckFilter:  getDateFilter($('#MinlastCheckFilterId')),
					maxlastCheckFilter:  getDateFilter($('#MaxlastCheckFilterId')),
					mintenantIdFilter: $('#MintenantIdFilterId').val(),
					maxtenantIdFilter: $('#MaxtenantIdFilterId').val(),
					hanetTenantPlaceDatasplaceNameFilter: $('#HanetTenantPlaceDatasplaceNameFilterId').val()
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
                                    _viewHanetTenantDeviceDatasModal.open({ id: data.record.hanetTenantDeviceDatas.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.hanetTenantDeviceDatas.id });                                
                            }
                        },
                        {
                            text: app.localize('History'),
                            visible: function () {
                                return entityHistoryIsEnabled();
                            },
                            action: function (data) {
                                _entityTypeHistoryModal.open({
                                    entityTypeFullName: _entityTypeFullName,
                                    entityId: data.record.hanetTenantDeviceDatas.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteHanetTenantDeviceDatas(data.record.hanetTenantDeviceDatas);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "hanetTenantDeviceDatas.deviceId",
						 name: "deviceId"   
					},
					{
						targets: 2,
						 data: "hanetTenantDeviceDatas.deviceName",
						 name: "deviceName"   
					},
					{
						targets: 3,
						 data: "hanetTenantDeviceDatas.deviceStatus",
						 name: "deviceStatus"  ,
						render: function (deviceStatus) {
							if (deviceStatus) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 4,
						 data: "hanetTenantDeviceDatas.lastCheck",
						 name: "lastCheck" ,
					render: function (lastCheck) {
						if (lastCheck) {
							return moment(lastCheck).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 5,
						 data: "hanetTenantDeviceDatas.tenantId",
						 name: "tenantId"   
					},
					{
						targets: 6,
						 data: "hanetTenantPlaceDatasplaceName" ,
						 name: "hanetTenantPlaceDatasFk.placeName" 
					}
            ]
        });

        function getHanetTenantDeviceDatases() {
            dataTable.ajax.reload();
        }

        function deleteHanetTenantDeviceDatas(hanetTenantDeviceDatas) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _hanetTenantDeviceDatasesService.delete({
                            id: hanetTenantDeviceDatas.id
                        }).done(function () {
                            getHanetTenantDeviceDatases(true);
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

        $('#CreateNewHanetTenantDeviceDatasButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _hanetTenantDeviceDatasesService
                .getHanetTenantDeviceDatasesToExcel({
				filter : $('#HanetTenantDeviceDatasesTableFilter').val(),
					deviceIdFilter: $('#deviceIdFilterId').val(),
					deviceNameFilter: $('#deviceNameFilterId').val(),
					deviceStatusFilter: $('#deviceStatusFilterId').val(),
					minlastCheckFilter:  getDateFilter($('#MinlastCheckFilterId')),
					maxlastCheckFilter:  getDateFilter($('#MaxlastCheckFilterId')),
					mintenantIdFilter: $('#MintenantIdFilterId').val(),
					maxtenantIdFilter: $('#MaxtenantIdFilterId').val(),
					hanetTenantPlaceDatasplaceNameFilter: $('#HanetTenantPlaceDatasplaceNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditHanetTenantDeviceDatasModalSaved', function () {
            getHanetTenantDeviceDatases();
        });

		$('#GetHanetTenantDeviceDatasesButton').click(function (e) {
            e.preventDefault();
            getHanetTenantDeviceDatases();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getHanetTenantDeviceDatases();
		  }
		});
    });
})();