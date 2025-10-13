using Svg;

namespace Pepro.Presentation.Utilities;

/// <summary>
/// Provides methods for loading and rendering SVG-based icons, logos, and images from predefined asset directories.
/// </summary>
/// <remarks>
/// The <see cref="IconProvider"/> caches loaded SVG documents to improve performance
/// and avoid redundant file reads. It also supports color customization and scaling.
/// </remarks>
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

    /// <summary>
    /// Recursively processes all SVG nodes to apply the specified paint color to their fill, color, and stroke properties.
    /// </summary>
    /// <param name="nodes">
    /// The collection of <see cref="SvgElement"/> nodes to be processed.
    /// </param>
    /// <param name="paintServer">
    /// The <see cref="SvgPaintServer"/> defining the new color to apply.
    /// </param>
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

    /// <summary>
    /// Loads and returns an SVG icon image from the predefined icon directory.
    /// </summary>
    /// <param name="name">
    /// The base name of the icon (without extension or style suffix).
    /// </param>
    /// <param name="style">
    /// The style category of the icon, such as "Linear" or "Bold". Defaults to "Linear".
    /// </param>
    /// <param name="size">
    /// The desired output size (in pixels) of the rendered image. Defaults to 24.
    /// </param>
    /// <param name="color">
    /// Optional color to apply to the icon. If omitted, the icon’s original color is used.
    /// </param>
    /// <returns>
    /// A rendered <see cref="Image"/> object representing the specified SVG icon.
    /// </returns>
    /// <exception cref="FileNotFoundException">
    /// Thrown when the icon file cannot be found at the expected path.
    /// </exception>
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

        // Retrieve the cached SVG document if available, otherwise load and process it.
        if (!_cache.TryGetValue(iconId, out SvgDocument? svgDoc))
        {
            svgDoc = SvgDocument.Open<SvgDocument>(iconPath);

            if (color.HasValue)
            {
                // Apply the specified color to all paintable elements within the SVG.
                SvgPaintServer paintServer = new SvgColourServer(color.Value);
                ProcessNodes(svgDoc.Descendants(), paintServer);
            }

            _cache[iconId] = svgDoc;
        }

        // Set the rendered dimensions of the SVG before drawing.
        svgDoc.Width = size;
        svgDoc.Height = size;

        // Render and return the SVG as a bitmap image.
        return svgDoc.Draw();
    }

    /// <summary>
    /// Loads and returns the application logo as an SVG-rendered image.
    /// </summary>
    /// <param name="size">
    /// The desired output size (in pixels) of the rendered logo. Defaults to 24.
    /// </param>
    /// <returns>
    /// A rendered <see cref="Image"/> object representing the logo.
    /// </returns>
    /// <exception cref="FileNotFoundException">
    /// Thrown when the logo file cannot be found in the "Assets/Logos" directory.
    /// </exception>
    public static Image GetLogo(int size = 24)
    {
        string logoName = $"Pepro.svg";
        string logoPath = Path.Combine(_logoFolderPath, logoName);

        if (!File.Exists(logoPath))
        {
            throw new FileNotFoundException($"Logo not found: {logoPath}");
        }

        // Retrieve the cached logo if available; otherwise, load and cache it.
        if (!_cache.TryGetValue(logoPath, out SvgDocument? svgDoc))
        {
            svgDoc = SvgDocument.Open<SvgDocument>(logoPath);
            _cache[logoPath] = svgDoc;
        }

        svgDoc.Width = size;
        svgDoc.Height = size;

        return svgDoc.Draw();
    }

    /// <summary>
    /// Loads and returns an SVG image from the predefined image directory, with optional frame color adjustment.
    /// </summary>
    /// <param name="name">
    /// The base name of the image (without extension).
    /// </param>
    /// <param name="frameName">
    /// The ID of the SVG element representing the frame whose fill color may be changed. Defaults to "frame".
    /// </param>
    /// <param name="frameColor">
    /// Optional color to apply to the specified frame element.
    /// </param>
    /// <returns>
    /// A rendered <see cref="Image"/> object representing the specified SVG image.
    /// </returns>
    /// <exception cref="FileNotFoundException">
    /// Thrown when the image file cannot be found at the expected path.
    /// </exception>
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

        // Retrieve cached image if available; otherwise, load and optionally recolor it.
        if (!_cache.TryGetValue(imagePath, out SvgDocument? svgDoc))
        {
            svgDoc = SvgDocument.Open<SvgDocument>(imagePath);

            if (frameColor.HasValue)
            {
                // Apply the specified color to the SVG element identified by "frameName".
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
