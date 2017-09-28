using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AWSNet.Dtos
{
    public class ProductDto : BaseDto
    {
        [StringLength(50, ErrorMessage = "Name max length is 50")]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "FirstName max length is 100")]
        public string FirstName { get; set; }

        [StringLength(100, ErrorMessage = "LastName max length is 100")]
        public string LastName { get; set; }

        public bool IsEnabled { get; set; }

    }


    public class DeleteProductDtoBindingModel : ProductDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id")]
        public override int Id { get; set; }
    }

}
