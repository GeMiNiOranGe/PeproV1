using Svg;

namespace Pepro.Presentation.Utilities;

public static class IconProvider
{
    private static readonly string _iconFolderPath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "Assets",
        "Icons"
    );
    private static readonly string _imageFolderPath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "Assets",
        "Images"
    );
    private static readonly string _logoFolderPath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "Assets",
        "Logos"
    );
    private static readonly Dictionary<string, SvgDocument> _cache = [];

    private static void ProcessNodes(
        IEnumerable<SvgElement> nodes,
        SvgPaintServer paintServer
    )
    {
        foreach (SvgElement node in nodes)
        {
            if (node.Fill != SvgPaintServer.None)
            {
                node.Fill = paintServer;
            }

            if (node.Color != SvgPaintServer.None)
            {
                node.Color = paintServer;
            }

            if (node.Stroke != SvgPaintServer.None)
            {
                node.Stroke = paintServer;
            }

            ProcessNodes(node.Descendants(), paintServer);
        }
    }

    public static Image GetIcon(
        string name,
        string style = "Linear",
        int size = 24,
        Color? color = null
    )
    {
        string iconName = $"{name}-{style}-24px.svg";
        string iconPath = Path.Combine(_iconFolderPath, iconName);

        if (!File.Exists(iconPath))
        {
            throw new FileNotFoundException($"Icon not found: {iconPath}");
        }

        string iconId = $"{color?.Name}-{iconPath}";

        if (!_cache.TryGetValue(iconId, out SvgDocument? svgDoc))
        {
            svgDoc = SvgDocument.Open<SvgDocument>(iconPath);

            if (color.HasValue)
            {
                SvgPaintServer paintServer = new SvgColourServer(color.Value);
                ProcessNodes(svgDoc.Descendants(), paintServer);
            }

            _cache[iconId] = svgDoc;
        }

        svgDoc.Width = size;
        svgDoc.Height = size;

        return svgDoc.Draw();
    }

    public static Image GetLogo(int size = 24)
    {
        string logoName = $"Pepro.svg";
        string logoPath = Path.Combine(_logoFolderPath, logoName);

        if (!File.Exists(logoPath))
        {
            throw new FileNotFoundException($"Logo not found: {logoPath}");
        }

        if (!_cache.TryGetValue(logoPath, out SvgDocument? svgDoc))
        {
            svgDoc = SvgDocument.Open<SvgDocument>(logoPath);
            _cache[logoPath] = svgDoc;
        }

        svgDoc.Width = size;
        svgDoc.Height = size;

        return svgDoc.Draw();
    }

    public static Image GetImage(
        string name,
        string frameName = "frame",
        Color? frameColor = null
    )
    {
        string imageName = $"{name}.svg";
        string imagePath = Path.Combine(_imageFolderPath, imageName);

        if (!File.Exists(imagePath))
        {
            throw new FileNotFoundException($"Image not found: {imagePath}");
        }

        if (!_cache.TryGetValue(imagePath, out SvgDocument? svgDoc))
        {
            svgDoc = SvgDocument.Open<SvgDocument>(imagePath);

            if (frameColor.HasValue)
            {
                SvgPaintServer paintServer = new SvgColourServer(
                    frameColor.Value
                );
                SvgElement frame = svgDoc.GetElementById(frameName);
                frame.Fill = paintServer;
            }

            _cache[imagePath] = svgDoc;
        }

        return svgDoc.Draw();
    }
}
