using System;
using System.ComponentModel.DataAnnotations;

using Core.Entities;

namespace API.Dtos;

public class CreateProductDto : BaseEntity
{
    [Required]
    public string Name { get; set; } = default!;

    [Required]
    public string Description { get; set; } = default!;

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    [Required]
    public string PictureUrl { get; set; } = default!;

    [Required]
    public string Type { get; set; } = default!;

    [Required]
    public string Brand { get; set; } = default!;

    public int QuantityInStock { get; set; }

}
