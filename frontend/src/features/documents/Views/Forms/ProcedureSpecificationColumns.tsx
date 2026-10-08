import { GridColDef, GridRenderCellParams } from "@mui/x-data-grid";
import Actions from "@/components/RowActions";
import { IProcedureSpecificationDTO } from "@/features/documents/Interfaces";
import { formatDate } from "date-fns";

export const procedureSpecificationColumns = (
  onEdit: (id: string) => void,
  onDelete: (id: string, name: string) => void
): GridColDef<IProcedureSpecificationDTO>[] => [
  {
    field: "id",
    headerName: "ID",
    headerClassName: "super-app-theme--header",
    type: "number",
    width: 90,
    sortable: false,
    editable: false,
    filterable: false,
  },
  {
    field: "procedureNumber",
    headerName: "N° Procedimiento",
    headerClassName: "super-app-theme--header",
    type: "string",
    flex: 1,
  },
  {
    field: "standardCode",
    headerName: "Norma o Código",
    headerClassName: "super-app-theme--header",
    type: "string",
    flex: 1
  },
  {
    field: "assignedTo",
    headerName: "Empresa",
    headerClassName: "super-app-theme--header",
    type: "string",
    flex: 1
  },
  {
    field: "action",
    headerName: "Action",
    headerClassName: "super-app-theme--header",
    width: 90,
    sortable: false,
    filterable: false,
    renderCell: (params: GridRenderCellParams<IProcedureSpecificationDTO>) => (
      <Actions
        params={params}
        onEdit={() => onEdit(params.row.id)}
        onDelete={() => onDelete(params.row.id, params.row.name)}
      />
    ),
  },
];