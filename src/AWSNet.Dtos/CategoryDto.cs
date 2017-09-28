using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AWSNet.Dtos
{
    public class CategoryDto : BaseDto
    {
        [StringLength(100, ErrorMessage = "Name max length is 100")]
        public virtual string Name { get; set; }

        public bool IsEnabled { get; set; }

        //public List<MediaDto> Media { get; set; }

        //public CategoryDto()
        //{
        //    Media = new List<MediaDto>();
        //}
    }

    public class CreateCategoryDtoBindingModel : CategoryDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name max length is 100")]
        public override string Name { get; set; }
    }

    public class UpdateCategoryDtoBindingModel : CreateCategoryDtoBindingModel
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id")]
        public override int Id { get; set; }
    }

    public class DeleteCategoryDtoBindingModel : CategoryDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id")]
        public override int Id { get; set; }
    }
}
