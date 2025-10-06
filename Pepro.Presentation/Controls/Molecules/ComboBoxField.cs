using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Pepro.Presentation.Controls.Templates;
using static System.Windows.Forms.ComboBox;

namespace Pepro.Presentation.Controls.Molecules;

public partial class ComboBoxField : InputFieldTemplate, INotifyPropertyChanged
{
    private static readonly object s_selectedIndexChangedEvent = new();
    public event PropertyChangedEventHandler? PropertyChanged;

    public ComboBoxField()
    {
        InitializeComponent();
    }

    protected override Size DefaultSize => new(240, 48);

    [DefaultValue(typeof(Size), "240, 48")]
    public new Size Size
    {
        get => base.Size;
        set => base.Size = value;
    }

    /// <summary>
    ///     Collection of the items contained in this ComboBox.
    /// </summary>
    [Category("Data")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public ObjectCollection Items => comboBoxField.Items;

    [AllowNull]
    [Browsable(true)]
    [Category("Appearance")]
    [DefaultValue("")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public override string Text
    {
        get => comboBoxField.Text;
        set => comboBoxField.Text = value;
    }

    [Browsable(true)]
    [Category("Behavior")]
    public event EventHandler? SelectedIndexChanged
    {
        add => Events.AddHandler(s_selectedIndexChangedEvent, value);
        remove => Events.RemoveHandler(s_selectedIndexChangedEvent, value);
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public new bool Enabled
    {
        get => comboBoxField.Enabled;
        set
        {
            comboBoxField.Enabled = value;
            TabStop = value;
        }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public object? DataSource
    {
        get => comboBoxField.DataSource;
        set => comboBoxField.DataSource = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string DisplayMember
    {
        get => comboBoxField.DisplayMember;
        set => comboBoxField.DisplayMember = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string ValueMember
    {
        get => comboBoxField.ValueMember;
        set => comboBoxField.ValueMember = value;
    }

    [Bindable(true)]
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [DisallowNull]
    public object? SelectedValue
    {
        get => comboBoxField.SelectedValue;
        set => comboBoxField.SelectedValue = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int SelectedIndex
    {
        get => comboBoxField.SelectedIndex;
        set => comboBoxField.SelectedIndex = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public object? SelectedItem
    {
        get => comboBoxField.SelectedItem;
        set => comboBoxField.SelectedItem = value;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public new ControlBindingsCollection DataBindings =>
        comboBoxField.DataBindings;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new BindingContext? BindingContext
    {
        get => comboBoxField.BindingContext;
        set => comboBoxField.BindingContext = value;
    }

    protected override void SetBoundsCore(
        int x,
        int y,
        int width,
        int height,
        BoundsSpecified specified
    )
    {
        base.SetBoundsCore(x, y, width, DefaultSize.Height, specified);
    }

    protected override void OnBackColorChanged(EventArgs e)
    {
        base.OnBackColorChanged(e);
        if (comboBoxField != null)
        {
            comboBoxField.BackColor = BackColor;
        }
    }

    protected override void OnForeColorChanged(EventArgs e)
    {
        base.OnForeColorChanged(e);
        if (comboBoxField != null)
        {
            comboBoxField.ForeColor = ForeColor;
        }
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName)
        );
    }

    private void InputFieldComboBox_SelectedIndexChanged(
        object sender,
        EventArgs e
    )
    {
        if (Events[s_selectedIndexChangedEvent] is EventHandler handler)
        {
            handler(this, e);
        }

        OnPropertyChanged(nameof(SelectedValue));
    }
}
