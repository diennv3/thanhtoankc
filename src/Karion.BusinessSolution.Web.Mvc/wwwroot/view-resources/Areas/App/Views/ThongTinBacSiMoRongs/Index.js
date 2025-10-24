(function () {
    $(function () {

        var _$thongTinBacSiMoRongsTable = $('#ThongTinBacSiMoRongsTable');
        var _thongTinBacSiMoRongsService = abp.services.app.thongTinBacSiMoRongs;
		var _entityTypeFullName = 'Karion.BusinessSolution.QuanLyDanhMuc.ThongTinBacSiMoRong';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.ThongTinBacSiMoRongs.Create'),
            edit: abp.auth.hasPermission('Pages.ThongTinBacSiMoRongs.Edit'),
            'delete': abp.auth.hasPermission('Pages.ThongTinBacSiMoRongs.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ThongTinBacSiMoRongs/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ThongTinBacSiMoRongs/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditThongTinBacSiMoRongModal'
        });       

		 var _viewThongTinBacSiMoRongModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ThongTinBacSiMoRongs/ViewthongTinBacSiMoRongModal',
            modalClass: 'ViewThongTinBacSiMoRongModal'
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

        var dataTable = _$thongTinBacSiMoRongsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _thongTinBacSiMoRongsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ThongTinBacSiMoRongsTableFilter').val(),
					imageFilter: $('#ImageFilterId').val(),
					tieuSuFilter: $('#TieuSuFilterId').val(),
					chucDanhFilter: $('#ChucDanhFilterId').val(),
					userNameFilter: $('#UserNameFilterId').val()
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
                                    _viewThongTinBacSiMoRongModal.open({ id: data.record.thongTinBacSiMoRong.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.thongTinBacSiMoRong.id });                                
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
                                    entityId: data.record.thongTinBacSiMoRong.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteThongTinBacSiMoRong(data.record.thongTinBacSiMoRong);
                            }
                        }]
                    }
                },
                {
                    targets: 1,
                    data: "userName",
                    name: "userFk.name"
                },
                {
                    targets: 2,
                    data: "thongTinBacSiMoRong.chucDanh",
                    name: "chucDanh"
                },
                {
                    targets: 3,
                    data: "thongTinBacSiMoRong.tieuSu",
                    name: "tieuSu",
                    render: function (displayName, type, row, meta) {
                        let btnTieuSu = '<button class="btn btn-primary btn-sm btn-brand xemTieuSu" data-index="' + row.thongTinBacSiMoRong.tieuSu + '">Xem Tiểu sử</button>';
                        return btnTieuSu
                    }
                },
                {
                    targets: 4,
                    data: "thongTinBacSiMoRong.image",
                    name: "image"
                },
                
                
					
            ]
        });

        function getThongTinBacSiMoRongs() {
            dataTable.ajax.reload();
        }

        function deleteThongTinBacSiMoRong(thongTinBacSiMoRong) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _thongTinBacSiMoRongsService.delete({
                            id: thongTinBacSiMoRong.id
                        }).done(function () {
                            getThongTinBacSiMoRongs(true);
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

        $('#CreateNewThongTinBacSiMoRongButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _thongTinBacSiMoRongsService
                .getThongTinBacSiMoRongsToExcel({
				filter : $('#ThongTinBacSiMoRongsTableFilter').val(),
					imageFilter: $('#ImageFilterId').val(),
					tieuSuFilter: $('#TieuSuFilterId').val(),
					chucDanhFilter: $('#ChucDanhFilterId').val(),
					userNameFilter: $('#UserNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        var TieuSuContainer = document.createElement("div")
        TieuSuContainer.className = "text-center"
        TieuSuContainer.id = "tieusucontainer"
        $(document).on('click', '.xemTieuSu', function () {
            var $self = $(this);
            let noidung = $self.attr('data-index');
            TieuSuContainer.textContent = noidung;
            sweetAlert({
                content: TieuSuContainer,
                button: {
                    Text: app.localize("OK"),
                    closeModal: true
                }
            });
        });

        abp.event.on('app.createOrEditThongTinBacSiMoRongModalSaved', function () {
            getThongTinBacSiMoRongs();
        });

		$('#GetThongTinBacSiMoRongsButton').click(function (e) {
            e.preventDefault();
            getThongTinBacSiMoRongs();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getThongTinBacSiMoRongs();
		  }
		});
    });
})();