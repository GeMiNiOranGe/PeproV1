﻿using Pepro.Presentation.Controls.Atoms;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Pepro.Presentation.Controls.Templates;

public partial class InputFieldTemplate : PeproUserControl {
    public InputFieldTemplate() {
        InitializeComponent();
    }

    [Browsable(true)]
    [Category("Appearance")]
    [DefaultValue("")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public virtual string LabelText {
        get => inputFieldLabel.Text;
        set => inputFieldLabel.Text = value;
    }

    [AllowNull]
    [Category("Appearance")]
    [DefaultValue(typeof(Font), "Segoe UI, 14px")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public virtual Font LabelFont {
        get => inputFieldLabel.Font;
        set => inputFieldLabel.Font = value;
    }

    protected override Size DefaultSize => new(240, 128);

    [DefaultValue(typeof(Size), "240, 128")]
    public new Size Size {
        get => base.Size;
        set => base.Size = value;
    }
}
