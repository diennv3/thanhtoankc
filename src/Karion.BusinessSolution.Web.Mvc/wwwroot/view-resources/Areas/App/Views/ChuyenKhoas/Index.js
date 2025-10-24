(function () {
    $(function () {

        var _$chuyenKhoasTable = $('#ChuyenKhoasTable');
        var _chuyenKhoasService = abp.services.app.chuyenKhoas;
		var _entityTypeFullName = 'Karion.BusinessSolution.QuanLyDanhMuc.ChuyenKhoa';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.ChuyenKhoas.Create'),
            edit: abp.auth.hasPermission('Pages.ChuyenKhoas.Edit'),
            'delete': abp.auth.hasPermission('Pages.ChuyenKhoas.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ChuyenKhoas/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ChuyenKhoas/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditChuyenKhoaModal'
        });       

		 var _viewChuyenKhoaModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ChuyenKhoas/ViewchuyenKhoaModal',
            modalClass: 'ViewChuyenKhoaModal'
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

        var dataTable = _$chuyenKhoasTable.DataTable({

            //scroll
            scrollX: true,
            scrollY: '50vh',
            //end
            
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _chuyenKhoasService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ChuyenKhoasTableFilter').val(),
					tenFilter: $('#TenFilterId').val(),
					moTaFilter: $('#MoTaFilterId').val()
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
                                    _viewChuyenKhoaModal.open({ id: data.record.chuyenKhoa.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.chuyenKhoa.id });                                
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
                                    entityId: data.record.chuyenKhoa.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteChuyenKhoa(data.record.chuyenKhoa);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "chuyenKhoa.ten",
						 name: "ten"   
					},
					{
						targets: 2,
						 data: "chuyenKhoa.moTa",
						 name: "moTa"   
					}
            ]
        });

        function getChuyenKhoas() {
            dataTable.ajax.reload();
        }

        function deleteChuyenKhoa(chuyenKhoa) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _chuyenKhoasService.delete({
                            id: chuyenKhoa.id
                        }).done(function () {
                            getChuyenKhoas(true);
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

        $('#CreateNewChuyenKhoaButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _chuyenKhoasService
                .getChuyenKhoasToExcel({
				filter : $('#ChuyenKhoasTableFilter').val(),
					tenFilter: $('#TenFilterId').val(),
					moTaFilter: $('#MoTaFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditChuyenKhoaModalSaved', function () {
            getChuyenKhoas();
        });

		$('#GetChuyenKhoasButton').click(function (e) {
            e.preventDefault();
            getChuyenKhoas();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getChuyenKhoas();
		  }
		});
    });
})();