using SharpKml.Base;
using SharpKml.Dom;
using KmlAPI.Services.Interfaces;
using KmlAPI.Models;
using SharpKml.Engine;
using System.Reflection;

namespace KmlAPI.Services.Implementations
{
    public class KmlService : IKmlService
    {
        private readonly List<CustomPlacemark> _placemarks;

        public KmlService()
        {
            _placemarks = LoadKMLFile("Data/DIRECIONADORES1.kml");
        }

        public List<CustomPlacemark> GetPlacemarks()
        {
            return _placemarks;
        }

        public List<CustomPlacemark> LoadKMLFile(string filePath)
        {
            var placemarks = new List<CustomPlacemark>();

            var file = System.IO.File.OpenRead(filePath);
            var kmlDocument = KmlFile.Load(file);
            var root = kmlDocument.Root as Kml;
            var feature = root.Feature as Feature;
            var featureList = GetFeatureListFromContainer(feature);
            var firstFeatureList = featureList[0];
            var nestedFeatureList = GetFeatureListFromContainer(firstFeatureList);

            foreach (var featureItem in nestedFeatureList)
            {
                // Acessar cada placemark dentro da feature
                var placemarksInFeature = featureItem.Flatten().OfType<Placemark>().ToList();

                foreach (var placemark in placemarksInFeature)
                {
                    var customPlacemark = new CustomPlacemark
                    {
                        Nome = placemark.Name,
                        Cliente = ExtractExtendedDataValue(placemark.ExtendedData, "CLIENTE"),
                        Situacao = ExtractExtendedDataValue(placemark.ExtendedData, "SITUAÇÃO"),
                        Bairro = ExtractExtendedDataValue(placemark.ExtendedData, "BAIRRO"),
                        Referencia = ExtractExtendedDataValue(placemark.ExtendedData, "REFERENCIA"),
                        RuaCruzamento = ExtractExtendedDataValue(placemark.ExtendedData, "RUA/CRUZAMENTO")
                    };

                    // Adicionar o placemark à lista de resultados
                    placemarks.Add(customPlacemark);
                }
            }

            return placemarks;
        }

        // Função para acessar a FeatureList usando reflexão
        private List<Feature> GetFeatureListFromContainer(Feature feature)
        {
            var container = feature as Container;
            if (container != null)
            {
                var featureListProperty = container.GetType().GetProperty("FeatureList", BindingFlags.NonPublic | BindingFlags.Instance);
                if (featureListProperty != null)
                {
                    return featureListProperty.GetValue(container) as List<Feature>;
                }
            }
            return null;
        }

        // Função para extrair valor do ExtendedData
        private string ExtractExtendedDataValue(ExtendedData extendedData, string name)
        {
            if (extendedData == null) return null;

            var dataElement = extendedData.Data.FirstOrDefault(d => d.Name == name);
            return dataElement?.Value;
        }
    }
}