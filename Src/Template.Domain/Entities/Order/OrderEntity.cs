using Template.Domain.Enums.Order;

namespace Template.Domain.Entities.Order;

public class OrderEntity : ATAEntity
{
    public ReferenceCity CityId { get; private set; }

    public OrderStatus OrderStatus { get; private set; }

    public int GroupCalendarId { get; private set; }

    public int? UserId { get; private set; }

    public int UserPersonnelCode { get; private set; }

    public string? UserFullName { get; private set; }

    public string? Location { get; private set; }

    public bool IsActive { get; private set; } = false;

    public int? CreatedBy { get; private set; }

    public int? UpdatedBy { get; private set; }

    public DateTime? CreatedDate { get; private set; }

    public DateTime? UpdatedDate { get; private set; }

    public bool IsHalfServing { get; private set; } = false;

    // Nav Props

    // Constructors

    public OrderEntity()
    {
        // For EF
    }

    public OrderEntity(ReferenceCity cityId, OrderStatus orderStatus, int groupCalendarId, int? userId, int userPersonnelCode, string? userFullName, string? location, bool isActive, int? createdBy, int? updatedBy, DateTime? createdDate, DateTime? updatedDate, bool isHalfServing)
    {
        CityId = cityId;
        OrderStatus = orderStatus;
        GroupCalendarId = groupCalendarId;
        UserId = userId;
        UserPersonnelCode = userPersonnelCode;
        UserFullName = userFullName;
        Location = location;
        IsActive = isActive;
        CreatedBy = createdBy;
        UpdatedBy = updatedBy;
        CreatedDate = createdDate;
        UpdatedDate = updatedDate;
        IsHalfServing = isHalfServing;
    }

    // APIs
    public void ChangeOrderStatus(OrderStatus orderStatus)
    {
        OrderStatus = orderStatus;
    }
}