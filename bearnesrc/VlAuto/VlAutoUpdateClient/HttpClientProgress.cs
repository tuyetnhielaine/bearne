namespace VlAutoUpdateClient;
public static class HttpClientProgressExtensions
{
    public static async Task DownloadDataAsync(this IHttpClient client, string requestUrl, Stream destination,
        ProgressBar? progress, CancellationToken cancellationToken = default(CancellationToken))
    {
        var httpClient = client.GetHttpClient();
        using (var response = await httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead))
        {
            var contentLength = response.Content.Headers.ContentLength;
            using (var download = await response.Content.ReadAsStreamAsync())
            {
                if (progress is null || !contentLength.HasValue)
                {
                    await download.CopyToAsync(destination);
                    return;
                }
                progress.Invoke(delegate { progress.Maximum = (int)(contentLength.GetValueOrDefault()); });
                await download.CopyToAsync(destination, 81920, progress, cancellationToken);
            }
        }
    }

    static async Task CopyToAsync(this Stream source, Stream destination, int bufferSize, ProgressBar? progress = null, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (bufferSize < 0)
            throw new ArgumentOutOfRangeException(nameof(bufferSize));
        if (source is null)
            throw new ArgumentNullException(nameof(source));
        if (!source.CanRead)
            throw new InvalidOperationException($"'{nameof(source)}' is not readable.");
        if (destination == null)
            throw new ArgumentNullException(nameof(destination));
        if (!destination.CanWrite)
            throw new InvalidOperationException($"'{nameof(destination)}' is not writable.");

        var buffer = new byte[bufferSize];
        long totalBytesRead = 0;
        int bytesRead;
        while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) != 0)
        {
            await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);
            totalBytesRead += bytesRead;
            if (progress != null)
            {
                progress.Invoke(delegate { progress.Value = (int)(totalBytesRead); });
            }
        }
    }
}
