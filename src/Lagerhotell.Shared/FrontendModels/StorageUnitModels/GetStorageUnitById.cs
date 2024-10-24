namespace LagerhotellAPI.Models.FrontendModels;

public class GetStorageUnitByIdRequest
{
    public required string StorageUnitId { get; set; }
}

public class GetStorageUnitByIdResponse
{
    public required StorageUnit StorageUnit { get; set; }
}
