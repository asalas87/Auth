import { GridColDef, GridRenderCellParams } from "@mui/x-data-grid";
import Actions from "../../../molecules/Actions";
import { IUserDTO } from "@/Security/Interfaces";

export const userColumns = (
  onEdit: (id: string) => void,
  onDelete: (id: string, name: string) => void
): GridColDef<IUserDTO>[] => [
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
    field: "name",
    headerName: "Name",
    headerClassName: "super-app-theme--header",
    type: "string",
    flex: 1,
  },
  {
    field: "email",
    headerName: "Email",
    headerClassName: "super-app-theme--header",
    type: "string",
    flex: 1,
  },
  {
    field: "company",
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
    renderCell: (params: GridRenderCellParams<IUserDTO>) => (
      <Actions
        params={params}
        onEdit={() => onEdit(params.row.id)}
        onDelete={() => onDelete(params.row.id, params.row.name)}
      />
    ),
  },
];
