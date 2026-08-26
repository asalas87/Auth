import { GridColDef, GridRenderCellParams } from "@mui/x-data-grid";
import Actions from "../../../molecules/Actions";
import { ICompanyDTO } from "@/Controls/Company/ICompanyDTO";

export const companyColumns = (
    onEdit: (id: string) => void,
    onDelete: (id: string, name: string) => void
): GridColDef<ICompanyDTO>[] => [
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
        field: "cuitCuil",
        headerName: "CUIT/CUIL",
        headerClassName: "super-app-theme--header",
        type: "string",
        flex: 1,
    },
    {
        field: "action",
        headerName: "Acciones",
        headerClassName: "super-app-theme--header",
        width: 90,
        sortable: false,
        filterable: false,
        renderCell: (params: GridRenderCellParams<ICompanyDTO>) => (
            <Actions
                params={params}
                onEdit={() => onEdit(params.row.id)}
                onDelete={() => onDelete(params.row.id, params.row.name)}
            />
        ),
    },
];
