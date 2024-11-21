using KmlAPI.Models;
using KmlAPI.Services.Interfaces;

public class PlacemarkService : IPlacemarkService
{
    private readonly List<CustomPlacemark> _placemarks;

    public PlacemarkService(IKmlService kmlService)
    {
        // Obtém os placemarks diretamente do KmlService
        _placemarks = kmlService.GetPlacemarks();
    }

    public IEnumerable<CustomPlacemark> FilterPlacemarks(FilterModel filters)
    {
        // Valida os filtros antes de aplicar
        FilterValidationHelper.ValidateFilters(filters, _placemarks);

        return _placemarks.Where(p =>
            (string.IsNullOrEmpty(filters.Cliente) ||
             string.Equals(p.Cliente, filters.Cliente, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrEmpty(filters.Situacao) ||
             string.Equals(p.Situacao, filters.Situacao, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrEmpty(filters.Bairro) ||
             string.Equals(p.Bairro, filters.Bairro, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrEmpty(filters.Referencia) ||
                (!string.IsNullOrEmpty(filters.Referencia) &&
                 p.Referencia.Contains(filters.Referencia, StringComparison.OrdinalIgnoreCase))) &&
            (string.IsNullOrEmpty(filters.RuaCruzamento) ||
                (!string.IsNullOrEmpty(filters.RuaCruzamento) &&
                 p.RuaCruzamento.Contains(filters.RuaCruzamento, StringComparison.OrdinalIgnoreCase)))
        );
    }

    public byte[] ExportFilteredKML(IEnumerable<CustomPlacemark> placemarks)
    {
        using var stream = new MemoryStream();
        var writer = new StreamWriter(stream);

        writer.WriteLine("<kml xmlns=\"http://www.opengis.net/kml/2.2\">");
        writer.WriteLine("<Document>");

        foreach (var placemark in placemarks)
        {
            writer.WriteLine("<Placemark>");
            writer.WriteLine($"<name>{placemark.Nome}</name>");
            writer.WriteLine("<ExtendedData>");
            writer.WriteLine($"<Data name=\"CLIENTE\"><value>{placemark.Cliente}</value></Data>");
            writer.WriteLine($"<Data name=\"SITUAÇÃO\"><value>{placemark.Situacao}</value></Data>");
            writer.WriteLine($"<Data name=\"BAIRRO\"><value>{placemark.Bairro}</value></Data>");
            writer.WriteLine($"<Data name=\"REFERENCIA\"><value>{placemark.Referencia}</value></Data>");
            writer.WriteLine($"<Data name=\"RUA/CRUZAMENTO\"><value>{placemark.RuaCruzamento}</value></Data>");
            writer.WriteLine("</ExtendedData>");
            writer.WriteLine("</Placemark>");
        }

        writer.WriteLine("</Document>");
        writer.WriteLine("</kml>");
        writer.Flush();

        return stream.ToArray();
    }

    public IEnumerable<string> GetUniqueValues(string field)
    {
        return field.ToUpper() switch
        {
            "CLIENTE" => _placemarks.Select(p => p.Cliente).Distinct(),
            "SITUAÇÃO" => _placemarks.Select(p => p.Situacao).Distinct(),
            "BAIRRO" => _placemarks.Select(p => p.Bairro).Distinct(),
            _ => Enumerable.Empty<string>()
        };
    }
}
