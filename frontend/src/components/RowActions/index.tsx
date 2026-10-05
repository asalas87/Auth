import { FaEdit } from 'react-icons/fa';
import { IoEye } from 'react-icons/io5';
import { MdDelete, MdFileDownload } from 'react-icons/md';

interface RowActionsProps {
    params: any;
    onEdit?: () => void;
    onDelete?: () => void;
    onDownload?: () => void;
    onView?: () => void;
}

const RowActions = ({ onEdit, onDelete, onDownload, onView }: RowActionsProps) => {
    return (
        <div className="d-flex align-items-center gap-3" role="button">
            {onView && (
                <button type="button" className="btn btn-link p-0 text-dark" onClick={onView} aria-label="Ver">
                    <IoEye size={20} />
                </button>
            )}
            {onEdit && (
                <button type="button" className="btn btn-link p-0 text-dark" onClick={onEdit} aria-label="Editar">
                    <FaEdit size={18} />
                </button>
            )}
            {onDownload && (
                <button type="button" className="btn btn-link p-0 text-dark" onClick={onDownload} aria-label="Descargar">
                    <MdFileDownload size={20} />
                </button>
            )}
            {onDelete && (
                <button type="button" className="btn btn-link p-0 text-danger" onClick={onDelete} aria-label="Borrar">
                    <MdDelete size={20} />
                </button>
            )}
        </div>
    );
};

export default RowActions;
