using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SkillSheetManager.Models
{
    public class EmployeeDetailsModel
    {
        /// <summary>
        /// User ID.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        [DisplayName("Employee Name")]
        public string Name { get; set; }

        /// <summary>
        /// Designation.
        /// </summary>
        public string Designation { get; set; }

        /// <summary>
        /// Date of birth.
        /// </summary>
        [DisplayName("Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Sex.
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// Joining date.
        /// </summary>
        [DisplayName("Joining date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime JoiningDate { get; set; }

        /// <summary>
        /// Worked in Japan.
        /// </summary>
        [DisplayName("Worked in Japan")]
        public string WorkedInJapan { get; set; }

        /// <summary>
        /// Qualification.
        /// </summary>
        public string Qualification { get; set; }

        /// <summary>
        /// Photo.
        /// </summary>
        public byte[] Photo { get; set; }

        // Computer skills.

        /// <summary>
        /// Languages.
        /// </summary>
        public string Languages { get; set; }

        /// <summary>
        /// Database.
        /// </summary>
        public string Database { get; set; }
    }
}