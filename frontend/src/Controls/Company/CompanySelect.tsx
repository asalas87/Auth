import React, { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import { crearOActualizarEmpresa, getEmpresaByCuit } from '../ControlService'; 
import { ICompanyDTO } from './ICompanyDTO';
import InputMask from 'react-input-mask';

interface CompanySelectProps {
  empresas: ICompanyDTO[];
  value?: number | string;
  onChange: (idEmpresa: string) => void;
  onEmpresasChange: (empresas: ICompanyDTO[]) => void;
  defaultNombre?: string;
}

// useEffect(() => {
//   if (defaultNombre) {
//     setNombreBusqueda(defaultNombre);
//   }
// }, [defaultNombre]);

export const CompanySelect: React.FC<CompanySelectProps> = ({
  empresas,
  value,
  onChange,
  onEmpresasChange
}) => {
  const [nombreBusqueda, setNombreBusqueda] = useState('');
  const [empresaNoEncontrada, setEmpresaNoEncontrada] = useState<string | null>(null);
  const [cuit, setCuit] = useState('');

  const empresaEncontrada = empresas.find(e =>
    nombreBusqueda && e.name.toLowerCase().includes(nombreBusqueda.toLowerCase())
  );

  useEffect(() => {
    if (empresaEncontrada) {
      onChange(empresaEncontrada.id);
      setEmpresaNoEncontrada(null);
    } else if (nombreBusqueda.trim()) {
      setEmpresaNoEncontrada(nombreBusqueda.trim());
    }
  }, [nombreBusqueda]);

  const handleCrearEmpresa = async () => {
    if (!cuit) {
      toast.error('Debe ingresar un CUIT válido');
      return;
    }

    try {
      const existente = await getEmpresaByCuit(cuit);
      if (existente) {
        const updated = await crearOActualizarEmpresa(existente.id, empresaNoEncontrada!, cuit);
        toast.info('Empresa actualizada por coincidencia de CUIT');

        const nuevasEmpresas = empresas.map(e =>
          e.id === existente.id ? updated : e
        );

        onEmpresasChange(nuevasEmpresas);
        onChange(updated.id);
      } else {
        const nueva = await crearOActualizarEmpresa(null, empresaNoEncontrada!, cuit);
        toast.success('Empresa creada con éxito');

        const nuevasEmpresas = [...empresas, nueva];
        onEmpresasChange(nuevasEmpresas);
        onChange(nueva.id);
      }

      setEmpresaNoEncontrada(null);
      setCuit('');
    } catch (e) {
      toast.error('Error al crear o actualizar la empresa');
    }
  };

  return (
    <>
      <input
        type="text"
        className="form-control mb-2"
        value={nombreBusqueda}
        onChange={(e) => setNombreBusqueda(e.target.value)}
        placeholder="Buscar empresa..."
      />

      {empresaNoEncontrada && (
        <div className="mt-2 border p-2 rounded">
          <div className="mb-2">Empresa <strong>{empresaNoEncontrada}</strong> no encontrada.</div>

          <label className="form-label">CUIT</label>
          <InputMask
            mask="99-99999999-9"
            maskPlaceholder={null}
            value={cuit}
            onChange={(e) => setCuit(e.target.value)}
          >
            {(inputProps: any) => (
              <input
                {...inputProps}
                type="text"
                className="form-control mb-2"
                placeholder="Ingresar CUIT"
              />
            )}
          </InputMask>

          <button className="btn btn-sm btn-primary" onClick={handleCrearEmpresa}>
            Crear empresa
          </button>
        </div>
      )}
    </>
  );
};
