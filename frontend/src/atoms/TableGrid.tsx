import { DataGrid, DataGridProps, GridToolbar } from "@mui/x-data-grid";
type TableGridProps = DataGridProps;

function TableGrid({
  rows,
  columns,
  initialState,
  ...rest
}: TableGridProps) {
  return (
    <DataGrid
      rows={rows}
      columns={columns}
      autoHeight
      pageSizeOptions={[5, 10, 15, 20]}
      initialState={{
        columns: {
          columnVisibilityModel: { id: false },
        },
        pagination: {
          paginationModel: { page: 0, pageSize: 5 },
        },
        ...initialState,
      }}
      showToolbar
      {...rest}
    />
  );
}

export default TableGrid