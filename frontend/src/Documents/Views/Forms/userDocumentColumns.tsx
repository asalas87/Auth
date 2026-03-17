import { GridColDef, GridRenderCellParams } from "@mui/x-data-grid";
import Actions from "../../../molecules/Actions";
import { IDocumentResponseDTO } from "@/Documents/Interfaces";

export const userDocumentsColumns = (
  onView: (id: IDocumentResponseDTO) => void,
  onDelete: (id: string) => void,
  onDownload: (id: IDocumentResponseDTO) => void
): GridColDef<IDocumentResponseDTO>[] => [
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
    headerName: "Nombre",
    headerClassName: "super-app-theme--header",
    type: "string",
    flex: 1,
  },
  {
    field: "description",
    headerName: "Descripcion",
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
    field: "action",
    headerName: "Action",
    headerClassName: "super-app-theme--header",
    flex: 1,
    sortable: false,
    filterable: false,
    renderCell: (params: GridRenderCellParams<IDocumentResponseDTO>) => (
      <Actions
        params={params}
        onView={() => onView(params.row)}
        onDelete={() => onDelete(params.row.id)}
        onDownload={() => onDownload(params.row)}
      />
    ),
  },
];
