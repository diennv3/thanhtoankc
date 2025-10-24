(function () {
    $(function () {

        var _$giaDichVusTable = $('#GiaDichVusTable');
        var _giaDichVusService = abp.services.app.giaDichVus;
		var _entityTypeFullName = 'Karion.BusinessSolution.QuanLyDanhMuc.GiaDichVu';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.GiaDichVus.Create'),
            edit: abp.auth.hasPermission('Pages.GiaDichVus.Edit'),
            'delete': abp.auth.hasPermission('Pages.GiaDichVus.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/GiaDichVus/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/GiaDichVus/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditGiaDichVuModal'
        });       

		 var _viewGiaDichVuModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/GiaDichVus/ViewgiaDichVuModal',
            modalClass: 'ViewGiaDichVuModal'
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

        var dataTable = _$giaDichVusTable.DataTable({

            //scroll
            scrollX: true,
            scrollY: '50vh',
            //end
            
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _giaDichVusService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#GiaDichVusTableFilter').val(),
					mucGiaFilter: $('#MucGiaFilterId').val(),
					moTaFilter: $('#MoTaFilterId').val(),
					minGiaFilter: $('#MinGiaFilterId').val(),
					maxGiaFilter: $('#MaxGiaFilterId').val(),
					minNgayApDungFilter:  getDateFilter($('#MinNgayApDungFilterId')),
					maxNgayApDungFilter:  getDateFilter($('#MaxNgayApDungFilterId')),
					dichVuTenFilter: $('#DichVuTenFilterId').val()
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
                                    _viewGiaDichVuModal.open({ id: data.record.giaDichVu.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.giaDichVu.id });                                
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
                                    entityId: data.record.giaDichVu.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteGiaDichVu(data.record.giaDichVu);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "giaDichVu.mucGia",
						 name: "mucGia"   
					},
					{
						targets: 2,
						 data: "giaDichVu.moTa",
						 name: "moTa"   
					},
					{
						targets: 3,
						 data: "giaDichVu.gia",
						 name: "gia"   
					},
					{
						targets: 4,
						 data: "giaDichVu.ngayApDung",
						 name: "ngayApDung" ,
					render: function (ngayApDung) {
						if (ngayApDung) {
							return moment(ngayApDung).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 5,
						 data: "dichVuTen" ,
						 name: "dichVuFk.ten" 
					}
            ]
        });

        function getGiaDichVus() {
            dataTable.ajax.reload();
        }

        function deleteGiaDichVu(giaDichVu) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _giaDichVusService.delete({
                            id: giaDichVu.id
                        }).done(function () {
                            getGiaDichVus(true);
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

        $('#CreateNewGiaDichVuButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _giaDichVusService
                .getGiaDichVusToExcel({
				filter : $('#GiaDichVusTableFilter').val(),
					mucGiaFilter: $('#MucGiaFilterId').val(),
					moTaFilter: $('#MoTaFilterId').val(),
					minGiaFilter: $('#MinGiaFilterId').val(),
					maxGiaFilter: $('#MaxGiaFilterId').val(),
					minNgayApDungFilter:  getDateFilter($('#MinNgayApDungFilterId')),
					maxNgayApDungFilter:  getDateFilter($('#MaxNgayApDungFilterId')),
					dichVuTenFilter: $('#DichVuTenFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditGiaDichVuModalSaved', function () {
            getGiaDichVus();
        });

		$('#GetGiaDichVusButton').click(function (e) {
            e.preventDefault();
            getGiaDichVus();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getGiaDichVus();
		  }
		});
    });
})();