(function () {
    $(function () {

        var _$hanetTenantPlaceDatasesTable = $('#HanetTenantPlaceDatasesTable');
        var _hanetTenantPlaceDatasesService = abp.services.app.hanetTenantPlaceDatases;
		var _entityTypeFullName = 'Karion.BusinessSolution.HanetTenant.HanetTenantPlaceDatas';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.HanetTenantPlaceDatases.Create'),
            edit: abp.auth.hasPermission('Pages.HanetTenantPlaceDatases.Edit'),
            'delete': abp.auth.hasPermission('Pages.HanetTenantPlaceDatases.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/HanetTenantPlaceDatases/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/HanetTenantPlaceDatases/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditHanetTenantPlaceDatasModal'
        });       

		 var _viewHanetTenantPlaceDatasModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/HanetTenantPlaceDatases/ViewhanetTenantPlaceDatasModal',
            modalClass: 'ViewHanetTenantPlaceDatasModal'
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

        var dataTable = _$hanetTenantPlaceDatasesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _hanetTenantPlaceDatasesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#HanetTenantPlaceDatasesTableFilter').val(),
					placeNameFilter: $('#placeNameFilterId').val(),
					placeAddressFilter: $('#placeAddressFilterId').val(),
					placeIdFilter: $('#placeIdFilterId').val(),
					minuserIdFilter: $('#MinuserIdFilterId').val(),
					maxuserIdFilter: $('#MaxuserIdFilterId').val(),
					mintenantIdFilter: $('#MintenantIdFilterId').val(),
					maxtenantIdFilter: $('#MaxtenantIdFilterId').val()
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
                                    _viewHanetTenantPlaceDatasModal.open({ id: data.record.hanetTenantPlaceDatas.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.hanetTenantPlaceDatas.id });                                
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
                                    entityId: data.record.hanetTenantPlaceDatas.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteHanetTenantPlaceDatas(data.record.hanetTenantPlaceDatas);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "hanetTenantPlaceDatas.placeName",
						 name: "placeName"   
					},
					{
						targets: 2,
						 data: "hanetTenantPlaceDatas.placeAddress",
						 name: "placeAddress"   
					},
					{
						targets: 3,
						 data: "hanetTenantPlaceDatas.placeId",
						 name: "placeId"   
					},
					{
						targets: 4,
						 data: "hanetTenantPlaceDatas.userId",
						 name: "userId"   
					},
					{
						targets: 5,
						 data: "hanetTenantPlaceDatas.tenantId",
						 name: "tenantId"   
					}
            ]
        });

        function getHanetTenantPlaceDatases() {
            dataTable.ajax.reload();
        }

        function deleteHanetTenantPlaceDatas(hanetTenantPlaceDatas) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _hanetTenantPlaceDatasesService.delete({
                            id: hanetTenantPlaceDatas.id
                        }).done(function () {
                            getHanetTenantPlaceDatases(true);
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

        $('#CreateNewHanetTenantPlaceDatasButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _hanetTenantPlaceDatasesService
                .getHanetTenantPlaceDatasesToExcel({
				filter : $('#HanetTenantPlaceDatasesTableFilter').val(),
					placeNameFilter: $('#placeNameFilterId').val(),
					placeAddressFilter: $('#placeAddressFilterId').val(),
					placeIdFilter: $('#placeIdFilterId').val(),
					minuserIdFilter: $('#MinuserIdFilterId').val(),
					maxuserIdFilter: $('#MaxuserIdFilterId').val(),
					mintenantIdFilter: $('#MintenantIdFilterId').val(),
					maxtenantIdFilter: $('#MaxtenantIdFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditHanetTenantPlaceDatasModalSaved', function () {
            getHanetTenantPlaceDatases();
        });

		$('#GetHanetTenantPlaceDatasesButton').click(function (e) {
            e.preventDefault();
            getHanetTenantPlaceDatases();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getHanetTenantPlaceDatases();
		  }
		});
    });
})();