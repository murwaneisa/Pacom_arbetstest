using System.ComponentModel.DataAnnotations;

namespace arbetstest_murwan.Models
{
    /// <summary>
    /// Represents a Modbus register value that can be toggled on/off
    /// </summary>
    public class ModbusValue
    {
        /// <summary>
        /// Unique identifier for the Modbus value
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Name or description of this Modbus value
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Modbus register address
        /// </summary>
        [Required]
        public ushort RegisterAddress { get; set; }

        /// <summary>
        /// The current state of the value (true = on, false = off)
        /// </summary>
        public bool IsOn { get; set; }

        /// <summary>
        /// Last time this value was updated
        /// </summary>
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
