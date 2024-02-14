var builder = WebApplication.CreateBuilder(args);

var proxyBuilder = builder.Services.AddReverseProxy();

proxyBuilder.LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapReverseProxy();
});


// https://github.com/microsoft/reverse-proxy/blob/2593b4c009f2c3a131cd4bf327115976aad2d7a6/samples/ReverseProxy.Direct.Sample/Startup.cs#L98-L99

//// Configure our own HttpMessageInvoker for outbound calls for proxy operations
//var httpClient = new HttpMessageInvoker(new SocketsHttpHandler()
//{
//    UseProxy = false,
//    AllowAutoRedirect = false,
//    AutomaticDecompression = DecompressionMethods.None,
//    UseCookies = false
//});

//// Setup our own request transform class
//var transformer = new CustomTransformer(); // or HttpTransformer.Default;
//var requestOptions = new RequestProxyOptions { Timeout = TimeSpan.FromSeconds(100) };

//app.UseEndpoints(endpoints =>
//{
//    // When using IHttpProxy for direct proxying you are responsible for routing, destination discovery, load balancing, affinity, etc..
//    // For an alternate example that includes those features see BasicYarpSample.
//    endpoints.Map("/{**catch-all}", async httpContext =>
//    {
//        await httpProxy.ProxyAsync(httpContext, "https://example.com", httpClient, requestOptions, transformer);
//        var errorFeature = httpContext.Features.Get<IProxyErrorFeature>();

//        // Check if the proxy operation was successful
//        if (errorFeature != null)
//        {
//            var error = errorFeature.Error;
//            var exception = errorFeature.Exception;
//        }
//    });
//});

app.Run();