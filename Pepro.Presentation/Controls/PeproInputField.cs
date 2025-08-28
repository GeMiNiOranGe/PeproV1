﻿using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Pepro.Presentation.Controls;

public partial class PeproInputField : PeproInputFieldBase {
    private static readonly object s_textChangedEventKey = new();

    public PeproInputField() {
        InitializeComponent();

        FocusColor = Color.Gray;
    }

    protected override Size DefaultSize => new(240, 48);

    [DefaultValue(typeof(Size), "240, 48")]
    public new Size Size {
        get => base.Size;
        set => base.Size = value;
    }

    [Browsable(true)]
    [Category("Appearance")]
    [DefaultValue(typeof(Color), "Gray")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color FocusColor {
        get;
        set;
    }

    [AllowNull]
    [DefaultValue("")]
    public virtual string PlaceholderText {
        get => inputFieldTextBox.PlaceholderText;
        set => inputFieldTextBox.PlaceholderText = value;
    }

    [AllowNull]
    [Browsable(true)]
    [Category("Appearance")]
    [DefaultValue("")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public override string Text {
        get => inputFieldTextBox.Text;
        set => inputFieldTextBox.Text = value;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public new bool Enabled {
        get => inputFieldTextBox.Enabled;
        set => inputFieldTextBox.Enabled = value;
    }

    [Browsable(true)]
    [Category("Property Changed")]
    public new event EventHandler TextChanged {
        add => Events.AddHandler(s_textChangedEventKey, value);
        remove => Events.RemoveHandler(s_textChangedEventKey, value);
    }

    /// <summary>
    ///     Retrieves the bindings for this control.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public new ControlBindingsCollection DataBindings {
        get => inputFieldTextBox.DataBindings;
    }

    /// <summary>
    ///     Clears all text from the text box control.
    /// </summary>
    public void Clear() {
        inputFieldTextBox.Clear();
    }

    /// <summary>
    ///     Attempts to set focus to this control.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public new bool Focus() {
        return inputFieldTextBox.Focus();
    }

    protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified) {
        base.SetBoundsCore(x, y, width, DefaultSize.Height, specified);
    }

    protected override void OnEnter(EventArgs e) {
        base.OnEnter(e);
        if (Enabled) {
            inputFieldPanel.BackColor = FocusColor.IsEmpty ? Color.Gray : FocusColor;
        }
    }

    protected override void OnLeave(EventArgs e) {
        base.OnLeave(e);
        if (Enabled) {
            inputFieldPanel.BackColor = ForeColor;
        }
    }

    protected override void OnBackColorChanged(EventArgs e) {
        base.OnBackColorChanged(e);
        if (inputFieldTextBox != null) {
            inputFieldTextBox.BackColor = BackColor;
        }
    }

    protected override void OnForeColorChanged(EventArgs e) {
        base.OnForeColorChanged(e);
        if (inputFieldTextBox != null) {
            inputFieldTextBox.ForeColor = ForeColor;
        }

        // Make the BackColor of the panel the same color as the text color
        if (inputFieldPanel != null) {
            inputFieldPanel.BackColor = ForeColor;
        }
    }

    private void InputFieldTextBox_TextChanged(object sender, EventArgs e) {
        if (Events[s_textChangedEventKey] is EventHandler handler) {
            handler(this, e);
        }
    }
}
