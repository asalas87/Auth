import { DataGrid, GridToolbar } from "@mui/x-data-grid";

function TableGrid({rows, columns} : {rows: any[], columns: any[]}) {
  return (
    <DataGrid
      rows={rows}
      columns={columns}
      autoHeight={true}
      showToolbar
      initialState={{
        columns: {
          columnVisibilityModel: {
            id: false,
          },
        },
        pagination: {
          paginationModel: { page: 0, pageSize: 5 },
        },
      }}
      getRowClassName={(params) =>
        params.indexRelativeToCurrentPage % 2 === 0 ? "even" : "odd"
      }
      getRowId={(row) => row.id}
      pageSizeOptions={[5, 10, 15, 20]}
    />
  );
}

export default TableGrid