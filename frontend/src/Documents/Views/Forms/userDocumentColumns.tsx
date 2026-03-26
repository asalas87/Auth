import { GridColDef, GridRenderCellParams } from "@mui/x-data-grid";
import Actions from "../../../molecules/Actions";
import { IDocumentResponseDTO } from "@/Documents/Interfaces";

export const userDocumentsColumns = (
  onView: (id: IDocumentResponseDTO) => void,
  onDownload: (id: IDocumentResponseDTO) => void
): GridColDef<IDocumentResponseDTO>[] => [
  {
    field: "id",
    headerName: "ID",
    headerClassName: "super-app-theme--header",
    type: "string",
    sortable: false,
    editable: false,
    filterable: false,
  },
  {
    field: "certificateNumber",
    headerName: "Nro Certificado",
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
    field: "standardCode",
    headerName: "Norma",
    headerClassName: "super-app-theme--header",
    type: "string",
    flex: 1
  }, 
  {
    field: "type",
    headerName: "Tipo",
    headerClassName: "super-app-theme--header",
    type: "string",
    flex: 1
  },
  {
    field: "validity",
    headerName: "Vigencia",
    headerClassName: "super-app-theme--header",
    type: "date",
    flex: 1
  }, 
  {
    field: "action",
    headerName: "Action",
    headerClassName: "super-app-theme--header",
    width: 90,
    sortable: false,
    filterable: false,
    renderCell: (params: GridRenderCellParams<IDocumentResponseDTO>) => (
      <Actions
        params={params}
        onView={() => onView(params.row)}
        onDownload={() => onDownload(params.row)}
      />
    ),
  },
];
