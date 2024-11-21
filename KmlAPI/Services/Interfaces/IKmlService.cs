using KmlAPI.Models;

namespace KmlAPI.Services.Interfaces
{
    public interface IKmlService
    {
        /// <summary>
        /// Carrega os dados do arquivo KML no momento em que o serviço for iniciado.
        /// </summary>
        List<CustomPlacemark> LoadKMLFile(string filePath);

        List<CustomPlacemark> GetPlacemarks();
    }
}