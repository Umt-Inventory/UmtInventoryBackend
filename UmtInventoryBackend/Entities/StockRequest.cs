using System.ComponentModel.DataAnnotations;
using UmtInventoryBackend.Enums;

namespace UmtInventoryBackend.Entities;

public class StockRequest : IEntity
{
    [Key] 
    public int Id { get; set; }

    public int RequestedQuantity { get; set; }
    
    public string Reason { get; set; }

    public DateTime RequestDate { get; set; }
    
    public RequestStatus Status { get; set; } // enum
    
    /* One-to-many relations */
    
    public int UserId { get; set; }
    
    public virtual User User { get; set; }
    
    public int ItemId { get; set; }
    
    public virtual Item Item { get; set; }

    
}
