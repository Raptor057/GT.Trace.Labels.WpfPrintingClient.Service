using GT.Trace.Labels.WpfPrintingClient.Service;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Open_GT_Trace_Labels_WpfPrintingClient_Service>();

var host = builder.Build();
host.Run();
