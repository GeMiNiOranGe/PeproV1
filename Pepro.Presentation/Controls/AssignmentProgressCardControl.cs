﻿using Pepro.DTOs;
using System.ComponentModel;

namespace Pepro.Presentation.Controls;

public partial class AssignmentProgressCardControl : PeproUserControl {
    private AssignmentProgressView _item = null!;
    private Color _defaultBackColor;

    public AssignmentProgressCardControl() {
        InitializeComponent();
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color MouseOverBackColor {
        get;
        set;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color MouseDownBackColor {
        get;
        set;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public AssignmentProgressView Item {
        get => _item;
        set {
            _item = value ?? throw new ArgumentNullException(nameof(Item));
            assignmentIdLabel.Text = _item.AssignmentId.ToString();
            assignmentNameLabel.Text = _item.Name;
            assignmentPercentLabel.Text = _item.ProgressPercent.ToString() + "%";
        }
    }

    private void AssignmentProgressCardControl_MouseEnter(object sender, EventArgs e) {
        _defaultBackColor = BackColor;
        BackColor = MouseOverBackColor;
    }

    private void AssignmentProgressCardControl_MouseLeave(object sender, EventArgs e) {
        BackColor = _defaultBackColor;
    }

    private void AssignmentProgressCardControl_MouseDown(object sender, MouseEventArgs e) {
        BackColor = MouseDownBackColor;
    }

    private void AssignmentProgressCardControl_MouseUp(object sender, MouseEventArgs e) {
        BackColor = MouseOverBackColor;
    }

    private void AssignmentProgressCardControl_Click(object sender, EventArgs e) {
        OnClick(e);
    }
}
