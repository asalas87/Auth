import { GridColDef, GridRenderCellParams } from "@mui/x-data-grid";
import Actions from "../../../molecules/Actions";
import { ICertificateDTO } from "@/Documents/Interfaces";

export const certificateColumns = (
  onEdit: (id: string) => void,
  onDelete: (id: string, name: string) => void
): GridColDef<ICertificateDTO>[] => [
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
    type: "string",
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
    field: "action",
    headerName: "Action",
    headerClassName: "super-app-theme--header",
    flex: 1,
    sortable: false,
    filterable: false,
    renderCell: (params: GridRenderCellParams<ICertificateDTO>) => (
      <Actions
        params={params}
        onEdit={() => onEdit(params.row.id)}
        onDelete={() => onDelete(params.row.id, params.row.name)}
      />
    ),
  },
];
