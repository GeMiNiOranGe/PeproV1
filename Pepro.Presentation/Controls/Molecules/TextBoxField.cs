using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Pepro.Presentation.Controls.Templates;

namespace Pepro.Presentation.Controls.Molecules;

public partial class TextBoxField : InputFieldTemplate
{
    private static readonly object s_textChangedEventKey = new();

    public TextBoxField()
    {
        InitializeComponent();

        FocusColor = Color.Gray;
    }

    protected override Size DefaultSize => new(240, 48);

    [DefaultValue(typeof(Size), "240, 48")]
    public new Size Size
    {
        get => base.Size;
        set => base.Size = value;
    }

    [Browsable(true)]
    [Category("Appearance")]
    [DefaultValue(typeof(Color), "Gray")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color FocusColor { get; set; }

    [AllowNull]
    [DefaultValue("")]
    public virtual string PlaceholderText
    {
        get => textBoxField.PlaceholderText;
        set => textBoxField.PlaceholderText = value;
    }

    [AllowNull]
    [Browsable(true)]
    [Category("Appearance")]
    [DefaultValue("")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public override string Text
    {
        get => textBoxField.Text;
        set => textBoxField.Text = value;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public new bool Enabled
    {
        get => textBoxField.Enabled;
        set
        {
            textBoxField.Enabled = value;
            TabStop = value;
        }
    }

    [Browsable(true)]
    [Category("Property Changed")]
    public new event EventHandler TextChanged
    {
        add => Events.AddHandler(s_textChangedEventKey, value);
        remove => Events.RemoveHandler(s_textChangedEventKey, value);
    }

    /// <summary>
    ///     Retrieves the bindings for this control.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public new ControlBindingsCollection DataBindings
    {
        get => textBoxField.DataBindings;
    }

    /// <summary>
    ///     Clears all text from the text box control.
    /// </summary>
    public void Clear()
    {
        textBoxField.Clear();
    }

    /// <summary>
    ///     Attempts to set focus to this control.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public new bool Focus()
    {
        return textBoxField.Focus();
    }

    protected override void OnEnter(EventArgs e)
    {
        base.OnEnter(e);
        if (Enabled)
        {
            underlinePanel.BackColor = FocusColor.IsEmpty
                ? Color.Gray
                : FocusColor;
        }
    }

    protected override void OnLeave(EventArgs e)
    {
        base.OnLeave(e);
        if (Enabled)
        {
            underlinePanel.BackColor = ForeColor;
        }
    }

    protected override void OnBackColorChanged(EventArgs e)
    {
        base.OnBackColorChanged(e);
        if (textBoxField != null)
        {
            textBoxField.BackColor = BackColor;
        }
    }

    protected override void OnForeColorChanged(EventArgs e)
    {
        base.OnForeColorChanged(e);
        if (textBoxField != null)
        {
            textBoxField.ForeColor = ForeColor;
        }

        // Make the BackColor of the panel the same color as the text color
        if (underlinePanel != null)
        {
            underlinePanel.BackColor = ForeColor;
        }
    }

    private void InputFieldTextBox_TextChanged(object sender, EventArgs e)
    {
        if (Events[s_textChangedEventKey] is EventHandler handler)
        {
            handler(this, e);
        }
    }
}
