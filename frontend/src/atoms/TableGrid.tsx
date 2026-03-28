import { DataGrid, DataGridProps, GridToolbar } from "@mui/x-data-grid";
import { esES } from '@mui/x-data-grid/locales';
import { useMemo } from "react";

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
      localeText={esES.components.MuiDataGrid.defaultProps.localeText}
      {...rest}
       sx={{
        '& .MuiTablePagination-root p': {
          margin: 0,
        },
        ...rest.sx
      }}
    />
  );
}

export default TableGrid