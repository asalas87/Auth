import { FaEdit } from "react-icons/fa";
import { IoEye } from "react-icons/io5";
import { MdDelete, MdFileDownload } from "react-icons/md";

function Actions({ params, onEdit, onDelete, onPreview, onDownload, onView }: { params: any; onEdit?: () => void; onDelete?: () => void; onPreview?: () => void; onDownload?: () => void; onView?: () => void }) {
  return (
    <div className="h-full flex items-center gap-3 cursor-pointer" role="button">
      {onPreview && (
        <IoEye
          size="22"
          key={`${params.id}-preview`}
          onClick={onPreview}
          title="preview"
          aria-label="preview"
        />
      )}
      {onEdit && (
        <FaEdit
          size="22"
          key={`${params.id}-edit`}
          onClick={onEdit}
          title="Editar"
          aria-label="Editar"
        />
      )}
      {onView && (
        <IoEye
          size="22"
          key={`${params.id}-view`}
          onClick={onView}
          title="Ver"
          aria-label="Ver"
        />
      )}
      {onDownload && (
        <MdFileDownload
          size="22"
          key={`${params.id}-download`}
          onClick={onDownload}
          title="Descargar"
          aria-label="Descargar"
        />
      )}
      {onDelete && (
        <MdDelete
          size="22"
          key={`${params.id}-delete`}
          onClick={onDelete}
          title="Borrar"
          aria-label="Borrar"
        />
      )}
    </div>
  );
}

export default Actions