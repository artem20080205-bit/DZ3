using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MuseumApp.Models
{
    /// <summary>
    /// Экспонат (основная таблица, сторона "много")
    /// </summary>
    public class Exhibit
    {
        [Key]
        public int Id { get; set; }

        /// <summary>Внешний ключ на музей</summary>
        public int MuseumId { get; set; }

        /// <summary>Название экспоната</summary>
        public string Name { get; set; } = "";

        private double _valueK;

        /// <summary>Оценочная стоимость (тыс. руб.) - не может быть отрицательной</summary>
        public double ValueK
        {
            get => _valueK;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Стоимость не может быть отрицательной");
                _valueK = value;
            }
        }

        /// <summary>Навигационное свойство: музей, где находится экспонат</summary>
        [ForeignKey("MuseumId")]
        public Museum? Museum { get; set; }

        public override string ToString() => $"{Name} ({(Museum?.Name ?? "нет музея")}) - {ValueK} тыс.руб.";
    }
}