using System.ComponentModel.DataAnnotations;

namespace MuseumApp.Models
{
    /// <summary>
    /// Музей (справочная таблица, сторона "один")
    /// </summary>
    public class Museum
    {
        [Key]
        public int Id { get; set; }
        
        /// <summary>Название музея</summary>
        public string Name { get; set; } = "";
        
        /// <summary>Навигационное свойство: экспонаты этого музея</summary>
        public ICollection<Exhibit> Exhibits { get; set; } = new List<Exhibit>();
        
        public override string ToString() => Name;
    }
}