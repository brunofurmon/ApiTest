using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ApiTest.Models
{
    public interface IAbstractModel
    {
        int Id { get; set; }
    }
}