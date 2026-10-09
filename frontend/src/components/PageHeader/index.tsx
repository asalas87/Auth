import { IoBagSharp } from 'react-icons/io5';
import Button from '@/components/Button';
import Heading from '@/components/Heading';

const Icons = {
    IoBagSharp,
};

interface PageHeaderProps {
    heading: string;
    headingIcon?: keyof typeof Icons;
    btnLabel?: string;
    btnEvent?: () => void;
}

const PageHeader = ({ heading, headingIcon, btnLabel, btnEvent }: PageHeaderProps) => {
    const Icon = headingIcon ? Icons[headingIcon] : undefined;

    return (
        <div className="d-flex align-items-center justify-content-between gap-3 mb-3">
            <Heading size="h4" className="mb-0">
                <span className="d-inline-flex align-items-center gap-2">
                    {Icon && <Icon size={20} className="text-warning" />}
                    <span>{heading}</span>
                </span>
            </Heading>

            {btnLabel && btnEvent && (
                <Button type="button" size="small" variant="primary" onClick={btnEvent}>
                    {btnLabel}
                </Button>
            )}
        </div>
    );
};

export default PageHeader;
