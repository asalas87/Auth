import { GridColDef, GridRenderCellParams } from "@mui/x-data-grid";
import Actions from "../../../molecules/Actions";
import { IRenovationDTO } from "@/Documents/Interfaces";
import { formatDate } from "date-fns";

export const renovationColumns = (
  onEdit: (id: string) => void,
  onDelete: (id: string, name: string) => void
): GridColDef<IRenovationDTO>[] => [
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
    field: "certificateNumber",
    headerName: "N° Certificado",
    headerClassName: "super-app-theme--header",
    type: "string",
    flex: 1,
  },
  {
    field: "renovationNumber",
    headerName: "N° Ren.",
    headerClassName: "super-app-theme--header",
    type: "string",
    width: 70,
  },
  {
    field: "employerFullName",
    headerName: "Soldador",
    headerClassName: "super-app-theme--header",
    type: "string",
    flex: 1,
  },
  {
    field: "validity",
    headerName: "Vigencia",
    headerClassName: "super-app-theme--header",
    type: "date",
    renderCell: (params) => formatDate(params.value, "dd/MM/yyyy"),
    flex: 1
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
    renderCell: (params: GridRenderCellParams<IRenovationDTO>) => (
      <Actions
        params={params}
        onEdit={() => onEdit(params.row.id)}
        onDelete={() => onDelete(params.row.id, params.row.name)}
      />
    ),
  },
];
