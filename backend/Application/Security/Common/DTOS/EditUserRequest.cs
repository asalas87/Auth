using System.ComponentModel.DataAnnotations;
using Application.Security.Common.DTOS;

namespace Application.Security.Common.DTOs;

public class EditUserRequest : CreateUserRequest
{
    [Required]
    public Guid Id { get; set; }
}
