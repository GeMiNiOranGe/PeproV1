﻿using Pepro.Business;
using Pepro.DTOs;
using Pepro.Presentation.Controls.Molecules;
using Pepro.Presentation.Controls.Templates;
using System.ComponentModel;

namespace Pepro.Presentation.Controls.Pages;

public partial class AssignmentDetailControl : MediatedTemplate {
    private int _projectId;

    public AssignmentDetailControl() {
        InitializeComponent();
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public required int ProjectId {
        get => _projectId;
        set {
            _projectId = value;
            projectIdLabel.Text = _projectId.ToString();
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public required string ProjectName {
        get => projectNameLabel.Text;
        set => projectNameLabel.Text = value;
    }

    private void AssignmentDetailControl_Load(object sender, EventArgs e) {
        ShowAssignmentsByProject();
    }

    private void ShowAssignmentsByProject() {
        if (assignmentsOfProjectFlowLayoutPanel.Controls.Count > 0) {
            assignmentsOfProjectFlowLayoutPanel.Controls.Clear();
        }

        List<AssignmentProgressView> assignmentsProgress = AssignmentBusiness.Instance.GetAssignmentProgressViewsByProjectId(ProjectId);

        for (int i = 0; i < assignmentsProgress.Count; i++) {
            AssignmentProgressView item = assignmentsProgress[i];

            AssignmentProgressCard assignmentCard = new() {
                Item = item,
                Margin = i != assignmentsProgress.Count - 1 ? new Padding(0, 0, 0, 8) : new Padding(0),
                Width = assignmentsOfProjectFlowLayoutPanel.ClientSize.Width - assignmentsOfProjectFlowLayoutPanel.Padding.Horizontal,
                Cursor = Cursors.Hand,
                ForeColor = ThemeColors.Text,
                BackColor = Color.FromArgb(29, 29, 29),
                MouseOverBackColor = ThemeColors.Accent.Base,
                MouseDownBackColor = ThemeColors.Accent.Dark
            };

            assignmentCard.Click += (sender, e) => {
                RetrieveInfomation(item.AssignmentId);
            };

            assignmentsOfProjectFlowLayoutPanel.Controls.Add(assignmentCard);
        }
    }

    public void RetrieveInfomation(int assignmentId) {
        if (otherAssignmentsOfManagerFlowLayoutLabel.Controls.Count > 0) {
            otherAssignmentsOfManagerFlowLayoutLabel.Controls.Clear();
        }

        EmployeeDto? employee = AssignmentBusiness.Instance.GetAssignmentManager(assignmentId);
        if (employee == null)
        {
            MessageBox.Show("Assignment manager not found");
            return;
        }

        assignmentManagerCard.Item = employee;

        List<AssignmentProgressView> assignmentsProgress = AssignmentBusiness.Instance.GetAssignmentProgressViewsByEmployeeId(employee.EmployeeId);

        for (int i = 0; i < assignmentsProgress.Count; i++) {
            AssignmentProgressView item = assignmentsProgress[i];

            AssignmentProgressCard assignmentCard = new() {
                Item = item,
                Margin = i != assignmentsProgress.Count - 1 ? new Padding(0, 0, 0, 8) : new Padding(0),
                Width = otherAssignmentsOfManagerFlowLayoutLabel.ClientSize.Width - otherAssignmentsOfManagerFlowLayoutLabel.Padding.Horizontal,
                ForeColor = ThemeColors.Text,
                BackColor = Color.FromArgb(29, 29, 29),
                MouseOverBackColor = ThemeColors.Accent.Base,
                MouseDownBackColor = ThemeColors.Accent.Dark
            };

            otherAssignmentsOfManagerFlowLayoutLabel.Controls.Add(assignmentCard);
        }
    }
}
