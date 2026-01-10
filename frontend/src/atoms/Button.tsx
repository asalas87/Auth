/* eslint-disable react/prop-types */

function Button({
  type = "button",
  size = "medium",
  variant,
  label,
  className,
  onClick,
  ...props
} : {
  type: "button" | "submit" | "reset";
  size?: "small" | "medium" | "large";
  variant: "Primary" | "Secondary" | "Tertiary";
  label: string | React.ReactNode;
  className?: string;
  onClick?: () => void;
}) {
  const baseStyle =
    "text-sm text-nowrap font-medium capitalize rounded-lg outline-none transition-all duration-300 ease-linear";

  const variantStyles = {
    Tertiary:
      "bg-transparent border-[1px] border-gray-400 hover:border-amber-500 hover:shadow-md focus:shadow-md hover:bg-amber-500 hover:active:bg-amber-500",
    Secondary:
      "bg-amber-400 hover:bg-transparent border-2 border-transparent hover:border-amber-500",
    Primary:
      "bg-transparent hover:bg-amber-400 border-2 border-amber-500 hover:border-transparent",
  };

  const sizeStyles = {
    medium: "py-2 px-4",
    small: "py-1 px-3",
    large: "py-3 px-5",
  };

  const finalClassName = `${baseStyle} ${variantStyles[variant]} ${sizeStyles[size]} ${className} `;

  return (
    <button
      type={type}
      className={finalClassName}
      aria-label={typeof label === "string" ? label : "button"}
      onClick={onClick}
      {...props}
    >
      {label}
    </button>
  );
}

export default Button