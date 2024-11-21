using KmlAPI.Models;

public static class FilterValidationHelper
{
    public static void ValidateFilters(FilterModel filters, List<CustomPlacemark> placemarks)
    {
        // Verifica se pelo menos um dos filtros está preenchido
        if (string.IsNullOrEmpty(filters.Cliente) &&
            string.IsNullOrEmpty(filters.Situacao) &&
            string.IsNullOrEmpty(filters.Bairro) &&
            string.IsNullOrEmpty(filters.Referencia) &&
            string.IsNullOrEmpty(filters.RuaCruzamento))
        {
            throw new BadHttpRequestException("At least one of the filters must be filled in.");
        }

        // Verifica se os campos de pré-seleção estão válidos (com insensibilidade ao case)
        var validClientes = placemarks.Select(p => p.Cliente).Distinct(StringComparer.OrdinalIgnoreCase);
        if (!string.IsNullOrEmpty(filters.Cliente) && !validClientes.Contains(filters.Cliente, StringComparer.OrdinalIgnoreCase))
        {
            throw new BadHttpRequestException($"The value for 'CLIENTE' '{filters.Cliente}' is not valid.");
        }

        var validSituacoes = placemarks.Select(p => p.Situacao).Distinct(StringComparer.OrdinalIgnoreCase);
        if (!string.IsNullOrEmpty(filters.Situacao) && !validSituacoes.Contains(filters.Situacao, StringComparer.OrdinalIgnoreCase))
        {
            throw new BadHttpRequestException($"The value for 'SITUAÇÃO' '{filters.Situacao}' is not valid.");
        }

        var validBairros = placemarks.Select(p => p.Bairro).Distinct(StringComparer.OrdinalIgnoreCase);
        if (!string.IsNullOrEmpty(filters.Bairro) && !validBairros.Contains(filters.Bairro, StringComparer.OrdinalIgnoreCase))
        {
            throw new BadHttpRequestException($"The value for 'BAIRRO' '{filters.Bairro}' is not valid.");
        }

        // Verifica se os filtros de texto têm pelo menos 3 caracteres
        if (!string.IsNullOrEmpty(filters.Referencia) && filters.Referencia.Length < 3)
        {
            throw new BadHttpRequestException("The 'REFERENCIA' filter must have at least 3 characters.");
        }

        if (!string.IsNullOrEmpty(filters.RuaCruzamento) && filters.RuaCruzamento.Length < 3)
        {
            throw new BadHttpRequestException("The 'RUA/CRUZAMENTO' filter must have at least 3 characters.");
        }
    }
}