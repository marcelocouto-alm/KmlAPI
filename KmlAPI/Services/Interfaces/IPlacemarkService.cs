using KmlAPI.Models;

namespace KmlAPI.Services.Interfaces
{
    public interface IPlacemarkService
    {
        IEnumerable<CustomPlacemark> FilterPlacemarks(FilterModel filters);
        byte[] ExportFilteredKML(IEnumerable<CustomPlacemark> placemarks);
        IEnumerable<string> GetUniqueValues(string field);
    }
}
