(function () {
    $(function () {

        var _$nguoiBenhNotificationsTable = $('#NguoiBenhNotificationsTable');
        var _nguoiBenhNotificationsService = abp.services.app.nguoiBenhNotifications;
		var _entityTypeFullName = 'Karion.BusinessSolution.QuanLyDanhMuc.NguoiBenhNotification';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.NguoiBenhNotifications.Create'),
            edit: abp.auth.hasPermission('Pages.NguoiBenhNotifications.Edit'),
            'delete': abp.auth.hasPermission('Pages.NguoiBenhNotifications.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/NguoiBenhNotifications/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/NguoiBenhNotifications/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditNguoiBenhNotificationModal'
        });       

		 var _viewNguoiBenhNotificationModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/NguoiBenhNotifications/ViewnguoiBenhNotificationModal',
            modalClass: 'ViewNguoiBenhNotificationModal'
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

        var dataTable = _$nguoiBenhNotificationsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _nguoiBenhNotificationsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#NguoiBenhNotificationsTableFilter').val(),
					noiDungTinNhanFilter: $('#NoiDungTinNhanFilterId').val(),
					minTrangThaiFilter: $('#MinTrangThaiFilterId').val(),
					maxTrangThaiFilter: $('#MaxTrangThaiFilterId').val(),
					tieuDeFilter: $('#TieuDeFilterId').val(),
					minThoiGianGuiFilter:  getDateFilter($('#MinThoiGianGuiFilterId')),
					maxThoiGianGuiFilter:  getDateFilter($('#MaxThoiGianGuiFilterId')),
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
                                    _viewNguoiBenhNotificationModal.open({ id: data.record.nguoiBenhNotification.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.nguoiBenhNotification.id });                                
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
                                    entityId: data.record.nguoiBenhNotification.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteNguoiBenhNotification(data.record.nguoiBenhNotification);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "nguoiBenhNotification.noiDungTinNhan",
						 name: "noiDungTinNhan"   
					},
					{
						targets: 2,
						 data: "nguoiBenhNotification.trangThai",
						 name: "trangThai"   
					},
					{
						targets: 3,
						 data: "nguoiBenhNotification.tieuDe",
						 name: "tieuDe"   
					},
					{
						targets: 4,
						 data: "nguoiBenhNotification.thoiGianGui",
						 name: "thoiGianGui" ,
					render: function (thoiGianGui) {
						if (thoiGianGui) {
							return moment(thoiGianGui).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 5,
						 data: "nguoiBenhUserName" ,
						 name: "nguoiBenhFk.userName" 
					}
            ]
        });

        function getNguoiBenhNotifications() {
            dataTable.ajax.reload();
        }

        function deleteNguoiBenhNotification(nguoiBenhNotification) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _nguoiBenhNotificationsService.delete({
                            id: nguoiBenhNotification.id
                        }).done(function () {
                            getNguoiBenhNotifications(true);
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

        $('#CreateNewNguoiBenhNotificationButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _nguoiBenhNotificationsService
                .getNguoiBenhNotificationsToExcel({
				filter : $('#NguoiBenhNotificationsTableFilter').val(),
					noiDungTinNhanFilter: $('#NoiDungTinNhanFilterId').val(),
					minTrangThaiFilter: $('#MinTrangThaiFilterId').val(),
					maxTrangThaiFilter: $('#MaxTrangThaiFilterId').val(),
					tieuDeFilter: $('#TieuDeFilterId').val(),
					minThoiGianGuiFilter:  getDateFilter($('#MinThoiGianGuiFilterId')),
					maxThoiGianGuiFilter:  getDateFilter($('#MaxThoiGianGuiFilterId')),
					nguoiBenhUserNameFilter: $('#NguoiBenhUserNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditNguoiBenhNotificationModalSaved', function () {
            getNguoiBenhNotifications();
        });

		$('#GetNguoiBenhNotificationsButton').click(function (e) {
            e.preventDefault();
            getNguoiBenhNotifications();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getNguoiBenhNotifications();
		  }
		});
    });
})();