using FluentSheets;
using FluentSheets.Test;
using Mapster;
using MapsterMapper;
using Newtonsoft.Json;

DotnetEnvironment.Load();

Console.WriteLine("Hello, World!");

var context = new GoogleSheetsContext("Test");

var adapter = TypeAdapterConfig<IList<object>, Model>
              .NewConfig()
              .Map(model => model.Text, source => source[0].ToString())
              .Map(model => model.Source, source => source[1].ToString());

string? SHEET = Environment.GetEnvironmentVariable("SHEET");

var model = await GoogleSheets.Context(context).Map(new Mapper(adapter.Config)).Sheet(SHEET).Tab("Sheet1").First<Model>();

Console.WriteLine(JsonConvert.SerializeObject(model, Formatting.Indented));

var models = await GoogleSheets.Context(context).Map(new Mapper(adapter.Config)).Sheet(SHEET).Tab("Sheet2").GetRecords<Model>();

Console.WriteLine(JsonConvert.SerializeObject(models, Formatting.Indented));

Console.WriteLine("Press any key to continue...");

Console.ReadKey();