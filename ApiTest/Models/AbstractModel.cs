using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace ApiTest.Models
{
    public class AbstractModel
    {
        [Key]
        public long Id { get; set; }

        [DefaultValue("getutcdate()")]
        public SqlDateTime CreationDate { get; set; }
    }
}