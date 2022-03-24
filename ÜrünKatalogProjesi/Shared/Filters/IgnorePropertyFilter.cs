using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MezuniyetProjesi.Shared.Filters
{
    public class IgnorePropertyFilter : IOperationFilter
    {


        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            context.ApiDescription.ParameterDescriptions
                .Where(d => d.Source.Id == "Query").ToList()
                .ForEach(param =>
                {
                    var toIgnore =
                        ((DefaultModelMetadata)param.ModelMetadata)
                        .Attributes.PropertyAttributes
                        ?.Any(x => x is JsonIgnoreAttribute);

                    var toRemove = operation.Parameters
                        .SingleOrDefault(p => p.Name == param.Name);

                    if (toIgnore ?? false)
                        operation.Parameters.Remove(toRemove);
                });
        }
    }
}
